#!/usr/bin/env python3
"""Validate documentation structure and exports, NOT gameplay behavior."""
import json
import re
import sys
from build_exports import ROOT, digest, safe_path, section

def load_json(root, path):
    return json.loads(safe_path(root,path).read_text(encoding="utf-8"))

def check(root=ROOT):
    errors = []
    def require(ok, message):
        if not ok:
            errors.append(message)
    manifest = load_json(root,"data/manifest.json")
    docs = manifest["documents"]
    ids = [d["id"] for d in docs]
    paths = [d["path"] for d in docs]
    require(set(ids)=={f"ASK-{i:03d}" for i in range(1,81)} and len(ids)==80,"Expected unique ASK-001..080")
    require(len(paths)==len(set(paths)),"Duplicate document path")
    require({p.relative_to(root).as_posix() for p in root.rglob("*.md")}==set(paths),"MD count/manifest mismatch")
    links, words = 0, 0
    for doc in docs:
        p = safe_path(root,doc["path"])
        require(p.is_file(),f"Missing {doc['path']}")
        if not p.is_file():
            continue
        raw = p.read_bytes()
        text = raw.decode("utf-8")
        words += len(text.split())
        require(digest(raw)==doc["sha256"],f"Stale hash {doc['id']}")
        require(text.startswith("---\n"),f"Missing frontmatter {doc['id']}")
        front = text.split("---\n",2)[1] if text.startswith("---\n") else ""
        require(f"doc_id: {doc['id']}\n" in front,f"Wrong frontmatter ID {doc['id']}")
        require("status: proposed\n" in front,f"Unexpected approval {doc['id']}")
        require(f"version: {doc['version']}\n" in front,f"Version mismatch {doc['id']}")
        dep = re.search(r"^dependencies: (.+)$",front,re.M)
        require(bool(dep),f"Missing dependencies {doc['id']}")
        if dep:
            require(json.loads(dep.group(1))==doc["dependencies"],f"Dependencies differ {doc['id']}")
        require(set(doc["dependencies"])<=set(ids),f"Unknown dependency {doc['id']}")
        require(not re.search(r"\[\[ASK-\d+\]\]",text),f"Unresolved placeholder {doc['id']}")
        require(text.count(chr(96)*3)%2==0,f"Unbalanced fences {doc['id']}")
        require(len(text.split())>=180,f"Unexpectedly short {doc['id']}")
        for m in re.finditer(r"\[[^\]]+\]\(([^)]+)\)",text):
            target = m.group(1)
            if re.match(r"^[a-zA-Z]+:",target) or target.startswith("#"):
                continue
            links += 1
            dest = (p.parent/target.split("#",1)[0]).resolve()
            require(root.resolve() in dest.parents,f"Escaping link {doc['id']}")
            require(dest.exists(),f"Broken link {doc['id']}: {target}")
    reqs = load_json(root,"data/requirements.json")
    tests = load_json(root,"data/test_cases.json")
    work = load_json(root,"data/backlog.json")
    rids,tids,wids = ({r["id"] for r in reqs},{t["id"] for t in tests},{w["id"] for w in work})
    require(len(rids)==len(reqs)==40,"Expected 40 unique requirements")
    require(len(tids)==len(tests)==40,"Expected 40 unique tests")
    require(len(wids)==len(work)==32,"Expected 32 unique work items")
    for r in reqs:
        require(bool(r["acceptance_tests"]) and set(r["acceptance_tests"])<=tids,f"Missing test {r['id']}")
        require(bool(r["source_docs"]) and set(r["source_docs"])<=set(ids),f"Bad source {r['id']}")
        require(r["status"]=="PROPOSED",f"Unverified req status {r['id']}")
        require(any(r["id"] in w["requirements"] for w in work),f"No work for {r['id']}")
    for t in tests:
        require(bool(t["requirements"]) and set(t["requirements"])<=rids,f"Bad requirement {t['id']}")
        require(t["status"]=="NOT_RUN" and t["result"] is None,f"Unverified test result {t['id']}")
        require(bool(t["steps"]) and bool(t["expected"]),f"Incomplete test {t['id']}")
        for rid in t["requirements"]:
            require(t["id"] in next(r for r in reqs if r["id"]==rid)["acceptance_tests"],f"Asymmetric trace {t['id']}")
    for w in work:
        require(set(w["dependencies"])<=wids,f"Bad work dependency {w['id']}")
        require(set(w["requirements"])<=rids,f"Bad work req {w['id']}")
        require(set(w["acceptance_tests"])<=tids,f"Bad work test {w['id']}")
        require(0<w["estimate_hours"]["low"]<=w["estimate_hours"]["high"],f"Invalid estimate {w['id']}")
        require(w["status"]=="PLANNED",f"Unverified work status {w['id']}")
    by_w = {w["id"]:w for w in work}
    visited, visiting = set(),set()
    def visit(wid):
        if wid in visiting:
            errors.append(f"Backlog cycle {wid}")
            return
        if wid in visited:
            return
        visiting.add(wid)
        for dep in by_w[wid]["dependencies"]:
            if dep in by_w:
                visit(dep)
        visiting.remove(wid)
        visited.add(wid)
    for wid in by_w:
        visit(wid)
    require(sum(w["estimate_hours"]["high"] for w in work[:8])==100,"First sprint must total 100h upper bound")
    params = load_json(root,"data/parameters.json")
    require(len({p["id"] for p in params})==len(params),"Duplicate parameters")
    glossary = safe_path(root,next(d["path"] for d in docs if d["id"]=="ASK-010")).read_text(encoding="utf-8")
    for parameter in params:
        require(parameter["id"] in glossary,f"Missing parameter {parameter['id']}")
        require(parameter["owner_doc"] in ids,f"Bad parameter owner {parameter['id']}")
        line = next((l for l in glossary.splitlines() if f"| {parameter['id']} |" in l),"")
        cells = [c.strip() for c in line.split("|")]
        if len(cells)>2:
            try:
                require(float(cells[2].replace(",","."))==parameter["value"],f"Parameter mismatch {parameter['id']}")
            except ValueError:
                errors.append(f"Bad parameter number {parameter['id']}")
    for a in load_json(root,"data/assets.json"):
        require(a["spec_doc"] in ids,f"Bad asset doc {a['id']}")
        require(not a["engine_approved"] and not a["license_approved"],f"Unverified asset {a['id']}")
    sources = load_json(root,"data/sources.json")
    for s in sources:
        require(s["url"].startswith("https://"),f"Bad source URL {s['id']}")
        require(set(s["referenced_by"])<=set(ids),f"Bad source trace {s['id']}")
    snapshot = digest("\n".join(f"{d['id']}:{d['sha256']}" for d in docs).encode())
    require(snapshot==manifest["snapshot_sha256"],"Snapshot mismatch")
    index = load_json(root,"notebook_exports/index.json")
    require(index["snapshot_sha256"]==snapshot,"Export snapshot mismatch")
    require(len(index["volumes"])==10,"Expected 10 volumes")
    exported = []
    by_id = {d["id"]:d for d in docs}
    for volume in index["volumes"]:
        raw = safe_path(root,volume["path"]).read_bytes()
        require(digest(raw)==volume["sha256"],f"Stale export {volume['path']}")
        content = raw.decode("utf-8")
        require(f"SNAPSHOT: {snapshot}" in content,f"Wrong volume snapshot {volume['path']}")
        for did in volume["documents"]:
            exported.append(did)
            require(section(root,by_id[did]) in content,f"Export/source difference {did}")
    require(exported==ids,"Export coverage/order mismatch")
    return {"scope":"DOCUMENT_PACKAGE_ONLY","game_tests_executed":False,
            "status":"PASS" if not errors else "FAIL","errors":errors,
            "stats":{"markdown_documents":len(docs),"canonical_words":words,"internal_links_checked":links,
                     "requirements":len(reqs),"planned_game_tests":len(tests),"planned_work_items":len(work),
                     "notebook_volumes":len(index["volumes"]),"sources":len(sources)}}

def main():
    try:
        report = check()
    except (ValueError,KeyError,OSError,UnicodeError) as exc:
        report = {"scope":"DOCUMENT_PACKAGE_ONLY","status":"FAIL","game_tests_executed":False,"errors":[str(exc)]}
    (ROOT/"validation_report.json").write_text(json.dumps(report,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print(json.dumps(report,ensure_ascii=False,indent=2))
    return 0 if report["status"]=="PASS" else 1

if __name__=="__main__":
    sys.exit(main())


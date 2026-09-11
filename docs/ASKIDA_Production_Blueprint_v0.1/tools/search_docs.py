#!/usr/bin/env python3
"""Local read-only lexical search. This is NOT an MCP server."""
import argparse
import json
import re
import unicodedata
from build_exports import ROOT, safe_path

def normalize(text):
    text = text.casefold().replace("ı", "i")
    return "".join(c for c in unicodedata.normalize("NFKD", text) if not unicodedata.combining(c))

def search(root, query="", doc_id=None, limit=5):
    if not 1 <= limit <= 10:
        raise ValueError("limit must be between 1 and 10")
    manifest = json.loads((root / "data/manifest.json").read_text(encoding="utf-8"))
    if doc_id is not None and not re.fullmatch(r"ASK-\d{3}", doc_id):
        raise ValueError("Invalid document ID")
    tokens = normalize(query).split()
    if not doc_id and not tokens:
        raise ValueError("Provide query or --id")
    results = []
    for doc in manifest["documents"]:
        if doc_id and doc["id"] != doc_id:
            continue
        raw = safe_path(root, doc["path"]).read_text(encoding="utf-8")
        plain = normalize(raw)
        if not doc_id and not all(t in plain for t in tokens):
            continue
        score = 100 if doc_id else sum(plain.count(t) + 5 * normalize(doc["title"]).count(t) for t in tokens)
        pos = plain.find(tokens[0]) if tokens else 0
        start = max(0, pos - 80)
        results.append({"id":doc["id"], "title":doc["title"], "path":doc["path"],
                        "version":doc["version"], "status":doc["status"], "sha256":doc["sha256"],
                        "score":score, "excerpt":raw[start:start+700], "excerpt_truncated":len(raw)>start+700})
    return sorted(results, key=lambda r:(-r["score"],r["id"]))[:limit]

def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("query", nargs="*")
    parser.add_argument("--id", dest="doc_id")
    parser.add_argument("--limit", type=int, default=5)
    args = parser.parse_args()
    try:
        results = search(ROOT, " ".join(args.query), args.doc_id, args.limit)
    except ValueError as exc:
        parser.error(str(exc))
    print(json.dumps({"mode":"READ_ONLY_LOCAL_SEARCH","results":results},ensure_ascii=False,indent=2))

if __name__ == "__main__":
    main()


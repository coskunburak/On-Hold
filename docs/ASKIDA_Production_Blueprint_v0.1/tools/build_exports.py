#!/usr/bin/env python3
"""Regenerate source hashes and 10 notebook volumes. Standard library only."""
import hashlib
import json
from pathlib import Path
ROOT = Path(__file__).resolve().parents[1]

def digest(raw):
    return hashlib.sha256(raw).hexdigest()

def safe_path(root, relative):
    root = root.resolve()
    candidate = (root / relative).resolve()
    if candidate != root and root not in candidate.parents:
        raise ValueError("Path outside package root")
    return candidate

def section(root, document):
    raw = safe_path(root, document["path"]).read_bytes()
    return (f"DOCUMENT: {document['id']}\nPATH: {document['path']}\n"
            f"VERSION: {document['version']}\nSTATUS: {document['status']}\n"
            f"SHA256: {digest(raw)}\n\n" + raw.decode("utf-8") + "\n\n")

def build(root=ROOT):
    manifest_path = root / "data/manifest.json"
    manifest = json.loads(manifest_path.read_text(encoding="utf-8"))
    documents = manifest["documents"]
    if len(documents) != 80:
        raise ValueError("This export profile expects exactly 80 documents")
    for doc in documents:
        raw = safe_path(root, doc["path"]).read_bytes()
        doc.update(sha256=digest(raw), word_count=len(raw.decode("utf-8").split()), byte_count=len(raw))
    snapshot = digest("\n".join(f"{d['id']}:{d['sha256']}" for d in documents).encode())
    manifest["snapshot_sha256"] = snapshot
    manifest["total_source_words"] = sum(d["word_count"] for d in documents)
    manifest_path.write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    output = root / "notebook_exports"
    output.mkdir(exist_ok=True)
    volumes = []
    for i in range(10):
        subset = documents[i*8:(i+1)*8]
        content = (f"ASKIDA KNOWLEDGE SNAPSHOT v{manifest['package_version']}\n"
                   f"VOLUME: {i+1:02d}/10\nSNAPSHOT: {snapshot}\n"
                   "STATUS: PROPOSED; GAME TESTS NOT RUN; READ-ONLY DERIVED COPY.\n"
                   "Canonical Markdown documents remain the source of truth.\n\n"
                   + "".join(section(root, d) for d in subset))
        path = output / f"volume_{i+1:02d}.txt"
        path.write_text(content, encoding="utf-8")
        volumes.append({"path": path.relative_to(root).as_posix(),
                        "documents": [d["id"] for d in subset], "sha256": digest(path.read_bytes())})
    (output / "index.json").write_text(json.dumps(
        {"snapshot_sha256": snapshot, "volumes": volumes}, ensure_ascii=False, indent=2)+"\n", encoding="utf-8")
    print(json.dumps({"volumes":10, "documents":len(documents),
                      "source_words":manifest["total_source_words"], "snapshot_sha256":snapshot}))

if __name__ == "__main__":
    build()


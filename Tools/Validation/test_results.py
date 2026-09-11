#!/usr/bin/env python3
"""Summarize actual Unity/NUnit XML; never infer PASS from missing evidence."""
import json
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

for name in sys.argv[1:]:
    path = Path(name)
    if not path.is_file():
        print(json.dumps({"path": str(path), "result": "NOT_RUN"}))
        continue
    root = ET.parse(path).getroot()
    cases = list(root.iter("test-case"))
    failures = []
    for case in cases:
        if case.get("result") == "Failed":
            failures.append({"name": case.get("fullname"), "message": case.findtext("failure/message"), "stack": case.findtext("failure/stack-trace")})
    print(json.dumps({"path": str(path), "result": root.get("result"),
        "total": len(cases), "passed": sum(c.get("result") == "Passed" for c in cases),
        "failed": len(failures), "skipped": sum(c.get("result") == "Skipped" for c in cases),
        "duration_seconds": root.get("duration"), "failures": failures}, ensure_ascii=False, indent=2))

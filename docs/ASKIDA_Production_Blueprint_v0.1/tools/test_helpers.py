#!/usr/bin/env python3
"""Tests only for document helpers, NOT Unity, transport or MCP."""
import tempfile
import unittest
from pathlib import Path
from build_exports import ROOT,safe_path
from search_docs import normalize,search
from validate import check

class HelperTests(unittest.TestCase):
    def test_turkish_normalization(self):
        self.assertEqual(normalize("IŞIK İÇİN"),"isik icin")
    def test_id_lookup(self):
        result=search(ROOT,doc_id="ASK-018")
        self.assertEqual(len(result),1)
        self.assertEqual(result[0]["id"],"ASK-018")
    def test_query_search(self):
        result=search(ROOT,"platform",limit=3)
        self.assertEqual(len(result),3)
        self.assertTrue(all(r["status"]=="proposed" for r in result))
    def test_invalid_id(self):
        with self.assertRaises(ValueError):
            search(ROOT,doc_id="../../etc/passwd")
    def test_limit_bounds(self):
        for limit in (0,11,-1):
            with self.assertRaises(ValueError):
                search(ROOT,"platform",limit=limit)
    def test_path_traversal(self):
        with self.assertRaises(ValueError):
            safe_path(ROOT,"../../etc/passwd")
    def test_symlink_escape(self):
        with tempfile.TemporaryDirectory(prefix="askida_doc_test_") as tmp:
            base=Path(tmp)
            allowed,outside=base/"allowed",base/"outside"
            allowed.mkdir()
            outside.mkdir()
            (allowed/"escape").symlink_to(outside,target_is_directory=True)
            with self.assertRaises(ValueError):
                safe_path(allowed,"escape/secret.txt")
    def test_full_package(self):
        report=check(ROOT)
        self.assertEqual(report["status"],"PASS",report["errors"])

if __name__=="__main__":
    unittest.main(verbosity=2)


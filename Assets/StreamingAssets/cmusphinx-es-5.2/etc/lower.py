#!/usr/bin/python

import sys

for line in sys.stdin:
    items = line.split()
    print items[0], " ".join(items[1:]).upper()

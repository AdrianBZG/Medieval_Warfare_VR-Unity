#!/usr/bin/python

import sys
file = open(sys.argv[1], "r")

for l in file:
	line = l.split()[-1][1:-1]
	print line.strip()


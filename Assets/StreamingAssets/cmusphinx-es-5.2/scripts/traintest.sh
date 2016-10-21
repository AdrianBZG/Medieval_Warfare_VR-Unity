#!/bin/sh
#
#  splits train and test data
#

LANG=C; export LANG

cat -n $1 |
awk '{if ($1 ~ /.*9$/)
      {
       for (i=2; i<NF; i++)
          printf("%s ",$i);
       printf("%s\n", $NF);
      }
     }' >$1.test
cat -n $1 |
awk '{if ($1 ~ /.*[^9]$/)
      {
       for (i=2; i<NF; i++)
          printf("%s ",$i);
       printf("%s\n", $NF);
      }
     }' > $1.train

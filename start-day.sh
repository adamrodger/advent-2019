#!/usr/bin/env bash
set -e

if [[ -z $AOC_COOKIE ]]; then
    echo "AOC_COOKIE is not defined" >&2
    exit 1
fi

if [[ -z $1 ]]; then
    echo "No day argument supplied" >&2
    exit 2
fi

# create solution
cp src/AdventOfCode/DayXX.cs src/AdventOfCode/Day$1.cs
sed -i "s/XX/$1/g" src/AdventOfCode/Day$1.cs

# create tests
cp tests/AdventOfCode.Tests/DayXXTests.cs tests/AdventOfCode.Tests/Day$1Tests.cs
sed -i "s/XX/$1/g" tests/AdventOfCode.Tests/Day$1Tests.cs
sed -i "s/using FactAttribute = System.Runtime.CompilerServices.CompilerGeneratedAttribute;//g" tests/AdventOfCode.Tests/Day$1Tests.cs

# download input file
curl -L "https://adventofcode.com/2019/day/$1/input" -H "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:65.0) Gecko/20100101 Firefox/65.0" -H "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8" -H "Accept-Language: en-US,en;q=0.5" --compressed -H "Referer: https://adventofcode.com/2019/day/$1" -H "Connection: keep-alive" -H "Cookie: session=$AOC_COOKIE" -H "Upgrade-Insecure-Requests: 1" -H "Cache-Control: max-age=0" -o src/AdventOfCode/inputs/day$1.txt


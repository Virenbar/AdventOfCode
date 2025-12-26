using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Days
{
    public interface IDay
    {
        (string PartOne, string PartTwo) Solve();
    }
}
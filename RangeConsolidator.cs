using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode2023
{
    internal class RangeConsolidator : List<RangeC>
    {

        public bool NeedUpdate { get; set; }
        public RangeConsolidator()
        {
        }

        internal void Add(ulong min, ulong max)
        {
            foreach (var item in this)
            {
                if (min > item.Max || max < item.Min)
                    continue; //mimo

                // spojenie
                item.Min = Math.Min(min, item.Min);
                item.Max = Math.Max(max, item.Max);
                NeedUpdate = true;
                return;
            }
            Add(new RangeC { Min = min, Max = max });
        }
    }

    [DebuggerDisplay("{Min/1000000m}-{Max/1000000m} ({Max-Min/1000000m})")]
    internal class RangeC
    {
        public ulong Min { get; set; }
        public ulong Max { get; set; }
    }
}
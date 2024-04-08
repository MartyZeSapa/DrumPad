using System.Collections.Generic;
using UnityEngine;

public class SampleDataComparer : IComparer<SampleData>
{
    public int Compare(SampleData x, SampleData y)
    {
        // Handle null cases first
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        // Compare by sampleIndex
        return x.sampleIndex.CompareTo(y.sampleIndex);
    }
}

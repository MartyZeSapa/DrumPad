using System.Collections.Generic;
using UnityEngine;

public class SampleDataComparer : IEqualityComparer<SampleData>
{
    public bool Equals(SampleData x, SampleData y)
    {
        // Check if x and y are references to the same object
        if (ReferenceEquals(x, y)) return true;

        // Check if one of the objects is null, while the other is not
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        // Check if the audioClips have the same name
        return x.audioClip.name == y.audioClip.name;
    }

    public int GetHashCode(SampleData obj)
    {
        // Check if the object is null
        if (ReferenceEquals(obj, null)) return 0;

        // Use the hash code of the audioClip name
        // If audioClip is null, then return 0
        return obj.audioClip != null ? obj.audioClip.name.GetHashCode() : 0;
    }
}

// Ensure the SampleData class is in the same namespace or included with a using directive


using System;
using System.Collections.Generic;
using System.Linq;

public static class HistogrammHelper
{
    public static int[] Bucketize(this IEnumerable<float> source, int totalBuckets, float noDataValue)
    {
        var sourceArray = source.Where(i => i != noDataValue).ToArray(); 
        var min = sourceArray.Min();
        var max = sourceArray.Max();
        int[] bucketsArray = new int[totalBuckets];

        var bucketSize = (max - min) / totalBuckets;
        foreach (var value in sourceArray)
        {
            int bucketIndex = 0;
            if (bucketSize > 0.0)
            {
                bucketIndex = (int)((value - min) / bucketSize);
                if (bucketIndex == totalBuckets)
                {
                    bucketIndex--;
                }
            }
            bucketsArray[bucketIndex]++;
        }
        return bucketsArray;
    }
    
}

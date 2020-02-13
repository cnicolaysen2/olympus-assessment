using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDependancy
{
    private Dictionary<int, int[]> PrimaryGroups = new Dictionary<int, int[]>();
    private Dictionary<int, int[]> SecondaryGroups = new Dictionary<int, int[]>();

    private Dictionary<int, int[]> UnusedGroups = new Dictionary<int, int[]>();

    private int[] Empty = new int[0];

    public ColorDependancy()
    {
        PrimaryGroups.Add(0, new int[] { 2, 7 });
        PrimaryGroups.Add(1, new int[] { 6 });
        PrimaryGroups.Add(2, new int[] { 6, 0 });
        PrimaryGroups.Add(3, new int[] { 4, 5 });
        PrimaryGroups.Add(4, new int[] { 0, 5 });
        PrimaryGroups.Add(5, new int[] { 8, 4 });
        PrimaryGroups.Add(6, new int[] { 2 });
        PrimaryGroups.Add(7, new int[] { 2, 4 });
        PrimaryGroups.Add(8, new int[] { 11, 5 });
        PrimaryGroups.Add(9, new int[] { 8, 4 });
        PrimaryGroups.Add(10, new int[] { 11 });
        PrimaryGroups.Add(11, new int[] { 8, 9 });

        SecondaryGroups.Add(0, new int[] { 1 });
        SecondaryGroups.Add(1, new int[] { 2 });
        SecondaryGroups.Add(2, new int[] { 5 });
        SecondaryGroups.Add(3, new int[] { 9, 10 });
        SecondaryGroups.Add(4, new int[] { 2, 7 });
        SecondaryGroups.Add(5, new int[] { 2 });
        SecondaryGroups.Add(6, new int[] { 8 });
        SecondaryGroups.Add(7, new int[] { 1 });
        SecondaryGroups.Add(8, new int[] { 10, 6 });
        SecondaryGroups.Add(9, new int[] { 10 });
        SecondaryGroups.Add(10, new int[] { 8 });
        SecondaryGroups.Add(11, new int[] { 5 });

        List<int> all = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        List<int> temp;

        for (int i = 0; i < 12; i++)
        {
            temp = new List<int>(all);

            if (PrimaryGroups.ContainsKey(i))
            {
                foreach (int groupIndex in PrimaryGroups[i])
                {
                    temp.Remove(groupIndex);
                }
            }

            if (SecondaryGroups.ContainsKey(i))
            {
                foreach (int groupIndex in SecondaryGroups[i])
                {
                    temp.Remove(groupIndex);
                }
            }

            temp.Remove(i);

            UnusedGroups.Add(i, temp.ToArray());
        }
    }

    public int[] GetPrimaryGroupIndex(int baseGroupIndex)
    {
        if (PrimaryGroups.ContainsKey(baseGroupIndex))
        {
            return PrimaryGroups[baseGroupIndex];
        }
        else
        {
            return Empty;
        }
    }

    public int[] GetSecondaryGroupIndex(int baseGroupIndex)
    {
        if (SecondaryGroups.ContainsKey(baseGroupIndex))
        {
            return SecondaryGroups[baseGroupIndex];
        }
        else
        {
            return Empty;
        }
    }

    public int[] GetUnusedGroups(int baseGroupIndex)
    {
        if (UnusedGroups.ContainsKey(baseGroupIndex))
        {
            return UnusedGroups[baseGroupIndex];
        }
        else
        {
            return Empty;
        }
    }
}

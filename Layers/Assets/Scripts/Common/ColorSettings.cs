using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ColorSettings", order = 1)]
public class ColorSettings : ScriptableObject
{
    public Color DefaultColor;

    public Color AgeSelectionColor;

    public ColorSettings()
    {
        DefaultColor = Color.white;
        AgeSelectionColor = new Color(1, 0.4720202f, 0, 1);
    }
}

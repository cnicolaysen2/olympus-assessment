using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mapbox/Modifiers/WellModifier")]
public class WellModifier : GameObjectModifier
{
    public GameObject WellPrefab;

    private AbstractMap map;

    public override void Run(VectorEntity ve, UnityTile tile)
    {
        if (map == null)
        {
            map = FindObjectOfType<AbstractMap>();
        };

        if (ve != null && ve.GameObject != null)
        {
            Instantiate(WellPrefab).transform.SetParent(ve.GameObject.transform, false);
        }

        OnPoolItem(ve);
    }

    public override void OnPoolItem(VectorEntity vectorEntity)
    {
        float ratio = 1;

        if (map != null)
        {
            ratio = map.Zoom;
        }

        if(vectorEntity != null && vectorEntity.GameObject != null)
        {
            Well WRandomizer = vectorEntity.GameObject.GetComponentInChildren<Well>();

            if (WRandomizer != null)
            {
                WRandomizer.SetNewScale(ratio);
            }
        }

        //Color ConvertToGradientColor(float rValue, Gradient gradient)
        //{
        //    Color result = Color.black;

        //    for (int i = 0; i < gradient.colorKeys.Length - 1; i++)
        //    {
        //        if (rValue >= gradient.colorKeys[i].time && rValue <= gradient.colorKeys[i + 1].time)
        //        {
        //            rValue = remap(rValue, gradient.colorKeys[i].time, gradient.colorKeys[i + 1].time, 0, 1);
        //            result = gradient.colorKeys[i].color + (gradient.colorKeys[i + 1].color - gradient.colorKeys[i].color) * rValue;
        //            result.a = 1;
        //            break;
        //        }
        //    }

        //    return result;
        //}

        //float remap(float s, float a1, float a2, float b1, float b2)
        //{
        //    return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        //}
    }

    public override void ClearCaches()
    {

    }
}

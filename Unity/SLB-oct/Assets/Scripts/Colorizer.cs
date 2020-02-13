using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mapbox/Modifiers/Colorizer")]
public class Colorizer : GameObjectModifier
{
    public Gradient Gradient;

    private Color TileColor;

    private AbstractMap map;

    public override void Run(VectorEntity ve, UnityTile tile)
    {
        if (map == null)
        {
            map = FindObjectOfType<AbstractMap>();
        }

        OnPoolItem(ve);
    }

    public override void OnPoolItem(VectorEntity vectorEntity)
    {
        if (vectorEntity != null && vectorEntity.GameObject != null)
        {
            Vector3 position = vectorEntity.GameObject.transform.position;
            float ratio = 1;
            if (map != null)
            {

                ratio = map.Zoom / 8f;
            }
            
            position.y = -16.5f * ratio *1.4f;
            vectorEntity.GameObject.transform.position = position;

            if (float.TryParse(vectorEntity.Feature.Properties["ELEV"].ToString(), out float value))
            {
                //if (value > Shader.GetGlobalFloat("_BottomElevation") * 255 && value < Shader.GetGlobalFloat("_TopElevation") * 255)
                //{
                //    vectorEntity.GameObject.SetActive(true);
                //}
                //else
                //{
                //    vectorEntity.GameObject.SetActive(false);
                //}

                float remappedValue = remap(value, 0, 255, 0, 1);

                foreach (Material m in vectorEntity.MeshRenderer.materials)
                {
                    TileColor = ConvertToGradientColor(remappedValue, Gradient);
                    TileColor.a = 1;

                    m.color = TileColor;
                }
            }
        }

        Color ConvertToGradientColor(float rValue, Gradient gradient)
        {
            Color result = Color.black;

            for (int i = 0; i < gradient.colorKeys.Length - 1; i++)
            {
                if (rValue >= gradient.colorKeys[i].time && rValue <= gradient.colorKeys[i + 1].time)
                {
                    rValue = remap(rValue, gradient.colorKeys[i].time, gradient.colorKeys[i + 1].time, 0, 1);
                    result = gradient.colorKeys[i].color + (gradient.colorKeys[i + 1].color - gradient.colorKeys[i].color) * rValue;
                    result.a = 1;
                    break;
                }
            }

            return result;
        }

        float remap(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }
    }

    public override void ClearCaches()
    {

    }
}

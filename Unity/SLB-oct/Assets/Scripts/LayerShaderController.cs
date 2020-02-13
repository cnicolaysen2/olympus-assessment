using UnityEngine;
using System;

public enum Layers
{
    Elevation,
    Porosity,
    Thickness,
    TOC,
    VShale,
}

public class LayerShaderController : MonoBehaviour
{
    private static string ShowPrefix = "_Show";
    private static string TopPrefix = "_Top";
    private static string BottomPrefix = "_Bottom";

    public static void EnableLayer(Layers layer)
    {
        Shader.SetGlobalFloat(ShowPrefix + layer, 1);
    }

    public static void DisableLayer(Layers layer)
    {
        Shader.SetGlobalFloat(ShowPrefix + layer, 0);
    }

    public static void SetTop(float value, Layers layer)
    {
        Shader.SetGlobalFloat(TopPrefix + layer, value);
    }

    public static void SetBottom(float value, Layers layer)
    {
        Shader.SetGlobalFloat(BottomPrefix + layer, value);
    }

    private void Awake()
    {
        foreach (Layers layer in (Layers[])Enum.GetValues(typeof(Layers)))
        {
            DisableLayer(layer);
            SetTop(1, layer);
            SetBottom(0, layer);
        }
    }
}

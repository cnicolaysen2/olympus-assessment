using System;
using UnityEngine;

public class LayerController
{
    public float ElevationMin = -9025.51f;
    public float ElevationMax = -5645.38f;

    public float PorosityMin = -0.0203f;
    public float PorosityMax = 0.0916f;

    public float ThicknessMin = 4.96f;
    public float ThicknessMax = 82.9f;

    public float TOCMin = -0.0011f;
    public float TOCMax = 0.0372f;

    public float VShaleMin = 0f;
    public float VShaleMax = 0.2119f;

    public LayerController()
    {
        JSBridge.SetLineChartData("Elevation", ElevationMin, ElevationMax);
        JSBridge.SetLineChartData("Porosity", PorosityMin, PorosityMax);
        JSBridge.SetLineChartData("Thickness", ThicknessMin, ThicknessMax);
        JSBridge.SetLineChartData("TOC", TOCMin, TOCMax);
        JSBridge.SetLineChartData("Vshale", VShaleMin, VShaleMax);
    }

    public void LineChartFiler(string message)
    {
        try
        {
            FilterPOCO filterData = JsonUtility.FromJson<FilterPOCO>(message);

            if (filterData == null)
            {
                Console.Log("Invalid data");
            }

            if (filterData.LayerID == "Vshale")
            {
                filterData.LayerID = "VShale";
            }

            if (Enum.TryParse(filterData.LayerID, out Layers layer))
            {
                Console.Log("Data received: " + layer.ToString() + " : " + filterData.IsEnabled + " : " + filterData.Min + " : " + filterData.Max);

                if (filterData.IsEnabled)
                {
                    LayerShaderController.EnableLayer(layer);
                }
                else
                {
                    LayerShaderController.DisableLayer(layer);
                }

                switch (layer)
                {
                    case Layers.Elevation:
                        filterData.Min = Remap(filterData.Min, ElevationMin, ElevationMax, 0, 1);
                        filterData.Max = Remap(filterData.Max, ElevationMin, ElevationMax, 0, 1);
                        break;

                    case Layers.Porosity:
                        filterData.Min = Remap(filterData.Min, PorosityMin, PorosityMax, 0, 1);
                        filterData.Max = Remap(filterData.Max, PorosityMin, PorosityMax, 0, 1);
                        break;

                    case Layers.Thickness:
                        filterData.Min = Remap(filterData.Min, ThicknessMin, ThicknessMax, 0, 1);
                        filterData.Max = Remap(filterData.Max, ThicknessMin, ThicknessMax, 0, 1);
                        break;

                    case Layers.TOC:
                        filterData.Min = Remap(filterData.Min, TOCMin, TOCMax, 0, 1);
                        filterData.Max = Remap(filterData.Max, TOCMin, TOCMax, 0, 1);
                        break;

                    case Layers.VShale:
                        filterData.Min = Remap(filterData.Min, VShaleMin, VShaleMax, 0, 1);
                        filterData.Max = Remap(filterData.Max, VShaleMin, VShaleMax, 0, 1);
                        break;
                }

                LayerShaderController.SetTop(filterData.Max, layer);
                LayerShaderController.SetBottom(filterData.Min, layer);
            }
            else
            {
                Console.Log($"NOT SUPPORTER LAYER TYPE {filterData.LayerID}");
            }
        }
        catch (Exception ex)
        {
            Console.Log(ex.Message);
        }
    }

    private float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
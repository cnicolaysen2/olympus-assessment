using UnityEngine;
using System.Runtime.InteropServices;
using System;
using AOT;
using System.Collections.Generic;
using System.Linq;

public class FilterPOCO
{
    public string LayerID;
    public float Min;
    public float Max;
    public bool IsEnabled;
}

public class JSBridge : MonoBehaviour
{
    [SerializeField]
    private LayerController layerController;

    [DllImport("__Internal")]
    private static extern void SetConsoleStringCallback(Action<string> action);

    [DllImport("__Internal")]
    private static extern void LogMessageUnity(string message);
    
    [DllImport("__Internal")]
    private static extern void LineChart(string message);
    
    [DllImport("__Internal")]
    private static extern void PieChart(string message);

    [DllImport("__Internal")]
    private static extern void LogMessageBrowser(string message);

    // -- Layers --

    //[DllImport("__Internal")]
    //public static extern void SetLayersCallbacks(Action<string> enableLayerCallback, 
    //                                             Action<string> disableLayerCallback, 
    //                                             Action<float, string> setLayerTopCallback,
    //                                             Action<float, string> setLayerBottomCallback);

    //[DllImport("__Internal")]
    //public static extern void EnableLayer(string layerName);

    //[DllImport("__Internal")]
    //public static extern void DisableLayer(string layerName);

    //[DllImport("__Internal")]
    //public static extern void SetLayerTop(float value, string layerName);

    //[DllImport("__Internal")]
    //public static extern void SetLayerBottom(float value, string layerName);

    private void Awake()
    {
        layerController = new LayerController();
    }

    private void Start()
    {
        if (Application.isEditor == false)
        {
            SetConsoleStringCallback(ConsoleStringCallback);
        }

        //SetLayersCallbacks(EnableLayerCallback, DisableLayerCallback, SetLayerTopCallback, SetLayerBottomCallback);
    }

    public static void WellClicked(GameObject go)
    {
        Debug.Log(go + " : " + go.transform.position);
    }

    [MonoPInvokeCallback(typeof(Action))]
    public static void ConsoleStringCallback(string message)
    {
        Console.Log(message);
    }

    /// <summary>
    /// CALLED FROM FE
    /// </summary>
    /// <param name="filter"></param>
    public void LineChartFilter(string filter)
    {
        layerController.LineChartFiler(filter);
    }


    //[MonoPInvokeCallback(typeof(Action))]
    //public static void EnableLayerCallback(string layerName)
    //{
    //    if (Enum.TryParse(layerName, out Layers layer))
    //    {
    //        LayerShaderController.EnableLayer(layer);
    //    }
    //    else
    //    {
    //        Console.Log("Wrong layer name received for enabling: " + layerName);
    //    }
    //}


    //[MonoPInvokeCallback(typeof(Action))]
    //public static void DisableLayerCallback(string layerName)
    //{
    //    if (Enum.TryParse(layerName, out Layers layer))
    //    {
    //        LayerShaderController.DisableLayer(layer);
    //    }
    //    else
    //    {
    //        Console.Log("Wrong layer name received for disabling: " + layerName);
    //    }
    //}


    //[MonoPInvokeCallback(typeof(Action))]
    //public static void SetLayerTopCallback(float value, string layerName)
    //{
    //    if (Enum.TryParse(layerName, out Layers layer))
    //    {
    //        LayerShaderController.SetTop(value, layer);
    //    }
    //    else
    //    {
    //        Console.Log("Wrong layer name received for top setting: " + layerName);
    //    }
    //}


    //[MonoPInvokeCallback(typeof(Action))]
    //public static void SetLayerBottomCallback(float value, string layerName)
    //{
    //    if (Enum.TryParse(layerName, out Layers layer))
    //    {
    //        LayerShaderController.SetBottom(value, layer);
    //    }
    //    else
    //    {
    //        Console.Log("Wrong layer name received for bottom setting: " + layerName);
    //    }
    //}


    public static void SetLineChartData(string layerName, float min, float max)
    {
        if (Application.isEditor == false)
        {
            LineChart(GetLineFilterJSON(layerName, min, max));
        }
    }

    private static string GetLineFilterJSON(string layerName, float min, float max)
    {
        var thiknessLineData = new LineData();
        thiknessLineData.LayerID = layerName;

        List<Vector2> dataDist = new List<Vector2>();
        dataDist = new List<Vector2>();

        thiknessLineData.DataDistribution = new Vector2[100];
        dataDist.Add(new Vector2(min, 34));

        for (int i = 1; i <= 98; i++)
        {
            float X = UnityEngine.Random.Range(min, max);
            float Y = UnityEngine.Random.Range(0, 255);
            var vect = new Vector2(X, Y);
            dataDist.Add(vect);
        };

        dataDist.Add(new Vector2(max, 34));
        thiknessLineData.DataDistribution = dataDist.OrderBy(i => i.x).ToArray();


        string lineChartJSON = JsonUtility.ToJson(thiknessLineData);
        return lineChartJSON;
    }
}

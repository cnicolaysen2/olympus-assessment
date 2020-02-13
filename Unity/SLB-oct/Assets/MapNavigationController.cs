using Mapbox.Unity.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class LineData
{
    public string LayerID;

    public Vector2[] DataDistribution;
}

public class MapNavigationController : MonoBehaviour
{

    [SerializeField]
    private AbstractMap Map;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ZoomOut();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ZoomIn();
        }

        if (Input.GetMouseButtonDown(0))
        {
            string result = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                var pair = UnityEngine.Random.insideUnitCircle * 5;
                result += pair.x +"," + pair.y + ";";
            }

#if !UNITY_EDITOR && UNITY_WEBGL
          //  JSBridge.LineChart(result);

            string message = (UnityEngine.Random.value * 100).ToString();
        
         //   JSBridge.PieChart(message);
#endif
        }

        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //  PanLeft();
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //  PanRight();
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //  PanUp();
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //  PanDown();
        //}
    }

    public void ZoomIn()
    {
        Map.UpdateMap(Map.CenterLatitudeLongitude, Map.Zoom + 0.5f);
    }
    public void ZoomOut()
    {
        Map.UpdateMap(Map.CenterLatitudeLongitude, Map.Zoom - 0.5f);
    }

    public void PanLeft()
    {
        Map.UpdateMap(Map.CenterLatitudeLongitude, Map.Zoom + 0.5f);
    }
    public void PanRight()
    {
        Map.UpdateMap(Map.CenterLatitudeLongitude, Map.Zoom - 0.5f);
    }
    public void PanUp()
    {
        Map.UpdateMap(Map.CenterLatitudeLongitude, Map.Zoom + 0.5f);
    }
    public void PanDown()
    {
        Map.UpdateMap(Map.CenterLatitudeLongitude, Map.Zoom - 0.5f);
    }
}

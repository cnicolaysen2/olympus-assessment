using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Mapbox.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

// https://wiki.openstreetmap.org/wiki/Slippy_map_tilenames

public enum MapSource
{
    OpenStreetMap,
    MapLarge,
    MapLargeF32,
    MapHack
}

[ExecuteInEditMode]
public class EnlargeMapExample: MonoBehaviour
{
    public MapSource Source;
    public int X;
    public int Y;
    public int Z;

    private MapSource currentSource;
    private int currentX;
    private int currentY;
    private int currentZ;

    public EnlargeMapExample()
    {
        Source = MapSource.OpenStreetMap;
        X = 0;
        Y = 1;
        Z = 1;

        currentSource = MapSource.OpenStreetMap;
        currentX = -1;
        currentY = -1;
        currentZ = -1;
    }

    private static Dictionary<MapSource, string> templates = new Dictionary<MapSource, string>() {
        { MapSource.OpenStreetMap, "https://tile.openstreetmap.org/${Z}/${X}/${Y}.png" },
        { MapSource.MapLarge, "https://energy.maplarge.com/Api/ProcessDirect?request={\"action\":\"tile/getmultitile\",\"hash\":\"6c37276a93586c8cc06c64aded835a19\"}&uParams=x:${X};y:${Y};z:${Z};w:1;h:1;layeronly:true;debug:false" },
        { MapSource.MapLargeF32, "https://em-dev-mincurvature-wip.maplarge.net/api/processdirect?request={\"action\":\"tile/getrawtiledata\",\"hash\":\"a755f40726075818a98d56b932df19f6\"}&uparams=x:${X};y:${Y};z:${Z};band:Vshale" },
        { MapSource.MapHack, "" }
    };

    public string URL
    {
        get
        {
            return templates[Source]
                .Replace("${X}", X.ToString())
                .Replace("${Y}", Y.ToString())
                .Replace("${Z}", Z.ToString());
        }
    }

    void Start()
    {
        SetVertices();
        Update();
    }

    void Update()
    {
        if (currentX != X || currentY != Y || currentZ != Z || currentSource != Source)
        {
            currentX = X;
            currentY = Y;
            currentZ = Z;
            currentSource = Source;
            if (Application.isPlaying)
            {
                StartCoroutine(GetTileTexture());
            }
            SetTransform();
        }
    }

    void SetVertices()
    {
        Vector3[] vertices = new Vector3[] {
            new Vector3(0, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0)
        };
        int[] triangles = new int[] {
            0, 2, 1,
            2, 3, 1
        };
        Vector3[] normals = new Vector3[] {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 0, 1)
        };
        Vector2[] uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    void SetTransform()
    {
        double n = Math.Pow(2.0, Z);
        double lng = X / n * 360 - 180;
        double lat = Math.Atan(Math.Sinh(Math.PI - 2 * Math.PI * Y / n)) * (180 / Math.PI);
        double width = 360 / n;
        double height = 180 / n;

        double mercator_lat = Math.Log(Math.Tan((lat / 90 + 1) * (Math.PI / 4))) * (90 / Math.PI);
        double layer = -0.001 * Z; // Smaller tiles closer to camera
        layer += Source == MapSource.OpenStreetMap ? 0.0 : -0.0005; // OpenStreetMap further away
        transform.localPosition = new Vector3((float)lng, (float)mercator_lat, (float)layer);
        transform.localScale = new Vector3((float)width, (float)height, 1.0f);
    }

    IEnumerator LoadPng()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(URL))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(uwr);
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.shader = Shader.Find("Sprites/Default");
                renderer.material.mainTexture = texture;
            }
        }
    }

    IEnumerator LoadRaw()
    {
        Debug.Log("Raw... ");
        Debug.Log(URL);
        using (UnityWebRequest uwr = UnityWebRequest.Post(URL,""))
        {
            uwr.downloadHandler =  new DownloadHandlerTexture(true);
          //  uwr.SetRequestHeader("User-Agent", "PostmanRuntime/7.11.0");
            //uwr.SetRequestHeader("Content-Type", "application/octet-stream");

            //uwr.SetRequestHeader("Content-Encoding", "gzip");
            //uwr.SetRequestHeader("Strict-Transport-Security", "max-age=2592000");

            Debug.Log("waiting...");
            //uwr.downloadHandler = new DownloadHandlerBuffer();
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
                GetComponent<Renderer>().material.mainTexture = null;
            }
            else
            {
                Debug.Log("Ok... go for it!");
                byte[] bytesDl = uwr.downloadHandler.data;

                byte[] decodedBytes;
                using (var gzipStream = new GZipStream(new MemoryStream(bytesDl), CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        decodedBytes = ms.ToArray();
                    }
                }


                Debug.Log($"response is {decodedBytes.Length}  {uwr.downloadHandler.data.Length}");

                //for(int i = 0; i< 20000;i+=4)
                //{
                //    float myFloat = BitConverter.ToSingle(decodedBytes, i);
                //    Debug.Log($"float value is { myFloat}");
                //}

                int size = 256;
                Texture2D tex = new Texture2D(size, size, TextureFormat.RFloat, false);

                
                tex.LoadRawTextureData(decodedBytes);
                //tex.wrapMode = TextureWrapMode.Clamp;
               // tex.filterMode = FilterMode.Point;
                tex.Apply();
  
                GetComponent<Renderer>().material.mainTexture = tex;
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.shader = Shader.Find("Sprites/Default");
            }
        }
    }

    IEnumerator LoadHack()
    {
        int size = 256;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        byte[] bytes = new byte[size * size * 3];
        for (var i = 0; i < size * size; i++)
        {
            bytes[i * 3] = (byte)(i % 256);
        }
        tex.LoadRawTextureData(bytes);
        tex.Apply();
        GetComponent<Renderer>().material.mainTexture = tex;
        return (new object[0]).GetEnumerator();
    }

    IEnumerator GetTileTexture()
    {
        switch (Source)
        {
            case MapSource.MapLargeF32: return LoadRaw();
            case MapSource.MapHack: return LoadHack();
            default: return LoadPng();
        }
    }

    [ContextMenu("Split")]
    void Split()
    {

    }

    [ContextMenu("Zoom In")]
    void ZoomIn()
    {
        X *= 2;
        Y *= 2;
        Z++;
    }

    [ContextMenu("Zoom Out")]
    void ZoomOut()
    {
        X /= 2;
        Y /= 2;
        Z--;
    }
}

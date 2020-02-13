using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using UnityEditor;
using UnityEngine;

public static class BinaryConverterEditor
{
    [MenuItem("Schlumberger/Binary Converter")]
    public static void Init()
    {
        BinaryConverter window = EditorWindow.GetWindow<BinaryConverter>("Binary Converter");
        window.Show();
    }
}

public class BinaryConverter : EditorWindow
{
    private bool IsReceiving;

    private string Layer = "Vshale";

    private int X_From = 107;
    private int X_To = 113;

    private int Y_From = 175;
    private int Y_To = 182;

    private int Z = 9;

    private ReceivingStatus[,] Status;
    private Dictionary<string, byte[]> Data = new Dictionary<string, byte[]>();

    private string DirectoryPath = "Assets/BIN/";

    private TextureVisualizer VisualizerWindow;

    private enum ReceivingStatus
    {
        Receiving,
        Error,
        Received,
        NoData,
        OK,
    }

    private GUIStyle ReceivingStyle;
    private GUIStyle ErrorStyle;
    private GUIStyle ReceivedStyle;
    private GUIStyle CorruptionStyle;
    private GUIStyle OKStyle;

    private void Awake()
    {
        ReceivingStyle = new GUIStyle();
        ErrorStyle = new GUIStyle();
        ReceivedStyle = new GUIStyle();
        CorruptionStyle = new GUIStyle();
        OKStyle = new GUIStyle();

        ReceivingStyle.normal.textColor = Color.black;
        ErrorStyle.normal.textColor = Color.red;
        ReceivedStyle.normal.textColor = Color.yellow;
        CorruptionStyle.normal.textColor = Color.red;
        OKStyle.normal.textColor = Color.green;
    }   

    private void OnGUI()
    {
        EditorGUILayout.Separator();

        Layer = EditorGUILayout.TextField("Layer: ", Layer);

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        X_From = EditorGUILayout.IntField("X From:", X_From);
        EditorGUILayout.Separator();
        X_To = EditorGUILayout.IntField("X To:", X_To);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        Y_From = EditorGUILayout.IntField("Y From:", Y_From);
        EditorGUILayout.Separator();
        Y_To = EditorGUILayout.IntField("Y To:", Y_To);
        EditorGUILayout.EndHorizontal();

        Z = EditorGUILayout.IntField("Zoom:", Z);

        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Status: " + (IsReceiving ? "Processing": "Stopped"));

        EditorGUILayout.Separator();

        int X_delta = X_To - X_From;
        int Y_delta = Y_To - Y_From;

        if (IsReceiving)
        {
            if (Status != null)
            {
                for (int y = 0; y < Y_delta; y++)
                {
                    GUILayout.BeginHorizontal();

                    for (int x = 0; x < X_delta; x++)
                    {
                        ReceivingStatus status = Status[x, y];

                        switch (status)
                        {
                            case ReceivingStatus.Receiving:
                                GUILayout.Label(status.ToString(), ReceivingStyle);
                                break;
                            case ReceivingStatus.Error:
                                GUILayout.Label(status.ToString(), ErrorStyle);
                                break;
                            case ReceivingStatus.Received:
                                GUILayout.Label(status.ToString(), ReceivedStyle);
                                break;
                            case ReceivingStatus.NoData:
                                GUILayout.Label(status.ToString(), CorruptionStyle);
                                break;
                            case ReceivingStatus.OK:
                                GUILayout.Label(status.ToString(), OKStyle);
                                break;
                        }
                    }

                    GUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.Separator();

            if (GUILayout.Button("Stop"))
            {
                BreakReceiving();
            }
        }
        else
        {
            if (GUILayout.Button("Receive data"))
            {
                IsReceiving = true;

                Status = new ReceivingStatus[X_delta, Y_delta];

                for (int x = 0; x < X_delta; x++)
                {
                    for (int y = 0; y < Y_delta; y++)
                    {
                        Status[x, y] = ReceivingStatus.Receiving;

                        int x_int = x;
                        int y_int = y;

                        LoadData(X_From + x_int, Y_From + y_int, Z, Layer, (d) => Callback(x_int, y_int, d));                        
                    }
                }                
            }
        }
    }

    private void BreakReceiving()
    {
        if (VisualizerWindow != null)
        {
            VisualizerWindow.Close();
            DestroyImmediate(VisualizerWindow);
            VisualizerWindow = null;
        }

        IsReceiving = false;
        Status = null;
        Data.Clear();
    }

    private void Callback(int x, int y, string filePath)
    {
        if (Status == null) return;

        Status[x, y] = ReceivingStatus.Received;

        try
        {
            if (File.ReadAllBytes(filePath).Length == 0)
            {
                Status[x, y] = ReceivingStatus.NoData;
                File.Delete(filePath);
                return;
            }
        }
        catch (Exception e)
        {
            Status[x, y] = ReceivingStatus.Error;
            return;
        }

        FileInfo fileToDecompress = new FileInfo(filePath);

        using (FileStream originalFileStream = fileToDecompress.OpenRead())
        {
            string currentFileName = fileToDecompress.FullName;

            using (FileStream decompressedFileStream = File.Create(currentFileName.Replace("_compressed", "")))
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    decompressionStream.CopyTo(decompressedFileStream);
                }
            }
        }

        File.Delete(filePath);
        Status[x, y] = ReceivingStatus.OK;

        CheckStatus();
    }

    private void CheckStatus()
    {
        foreach (ReceivingStatus status in Status)
        {
            if (status == ReceivingStatus.NoData || status == ReceivingStatus.OK) continue;
            return;
        }

        Debug.Log("Check success");

        VisualizerWindow = EditorWindow.GetWindow<TextureVisualizer>("Texture Visualizer");
               
        DirectoryInfo binaryDirectory = new DirectoryInfo(DirectoryPath);

        int x_min = int.MaxValue;
        int x_max = int.MinValue;

        int y_min = int.MaxValue;
        int y_max = int.MinValue;

        foreach (FileInfo fileToDecompress in binaryDirectory.GetFiles("*.bin"))
        {
            string[] name = fileToDecompress.Name.Split('_');

            if (int.TryParse(name[0], out int coord_x))
            {
                if (coord_x > x_max) x_max = coord_x;
                if (coord_x < x_min) x_min = coord_x;
            }

            if (int.TryParse(name[1], out int coord_y))
            {
                if (coord_y > y_max) y_max = coord_y;
                if (coord_y < y_min) y_min = coord_y;
            }
        }

        int x_delta = x_max - x_min;
        int y_delta = y_max - y_min;

        byte[,][] data = new byte[x_delta + 1, y_delta + 1][];

        foreach (FileInfo fileToDecompress in binaryDirectory.GetFiles("*.bin"))
        {
            string[] name = fileToDecompress.Name.Split('_');

            if (int.TryParse(name[0], out int coord_x) && int.TryParse(name[1], out int coord_y))
            {
                int x = x_max - coord_x;
                int y = y_max - coord_y;

                data[x, y] = (File.ReadAllBytes(fileToDecompress.FullName));
            }            
        }

        VisualizerWindow.Initialize(x_delta, y_delta, data);
        VisualizerWindow.Show();
    }

    private void LoadData(int x_coord, int y_coord, int z_coord, string layer, Action<string> callback)
    {
        using (WebClient client = new WebClient())
        {
            string address = @"https://em-dev-mincurvature-wip.maplarge.net/api/processdirect?request={""action"":""tile/getrawtiledata"",""hash"":""a755f40726075818a98d56b932df19f6""}&uparams=x:" + x_coord + ";y:" + y_coord + ";z:" + z_coord + ";band:" + layer;

            try
            {
                string filePath = DirectoryPath + (x_coord + "_" + y_coord + "_" + z_coord) + "_compressed.bin";

                client.DownloadFileCompleted += (s, e) => callback(filePath); 

                client.DownloadFileAsync(new Uri(address), filePath);
            }
            catch (Exception e)
            {
                callback(null);
                Debug.Log(e.Message);
            }
        }
    }
}

using BojkoSoft.Transformations;
using BojkoSoft.Transformations.Constants;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public static class GridConverterEditor
{
    [MenuItem("Schlumberger/Grid Converter")]
    public static void Init()
    {
        GridConverter window = EditorWindow.GetWindow<GridConverter>("Grid Converter");
        window.Show();
    }
}

public struct DataHeader
{
    public string DataType; //1
    //3
    public float NoDataValue; //4
    //5
    //6
    //7
    public int ColumnCount; //8
    public int RowCount; //9
    public float RowMin; //10
    public float RowMax; //11
    public float ColumnMin; //12
    public float ColumnMax; //13
    //14
    public float ColumnSize; //15
    public float RowSize; //16

    private static XmlSerializer DataSerializer = new XmlSerializer(typeof(DataHeader));

    public static DataHeader Convert(string path)
    {
        using (StreamReader xmlStream = new StreamReader(path))
        {
            return (DataHeader)DataSerializer.Deserialize(xmlStream);
        }
    }

    public static void Convert(DataHeader header, string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            DataSerializer.Serialize(fs, header);
        }
    }
}

public class GridConverter : EditorWindow
{
    private static Transformations tr = new Transformations();

    private TextAsset TextAsset;
    private int HeaderMaxLines = 25;

    private bool IsProcessing;

    private float Foot = 0.3048f;

    private Color[] Pixels;

    private int ImageWidth;
    private int ImageHeight;

    private bool UseCustomRange;
    private float CustomBottom;
    private float CustomTop;

    private bool UseCustomColors;
    private enum CustomColorType { ColorRange, Gradient, Elevation, };
    private int SelectedCustomColorOption;
    private Color CustomColorFrom;
    private Color CustomColorTo;
    private Gradient CustomGradient;
    private LayerDataAssets layerDataAsset;
    private Gradient DefaultGradient;

    private bool IsElevationData;

    private bool IsNewImageDataAvailable;

    private int GeoJSONEntryCount = 0;

    private bool GenerateBinary;
    private bool GenerateImage;
    private bool GenerateGeoJSON;

    private bool GenerateHistogram;
    private Layers selectedHistogramLayer;

    private bool NewAssetAvailable;

    private void Awake()
    {
        layerDataAsset = AssetDatabase.LoadAssetAtPath<LayerDataAssets>("Assets/TextAsset/LayerDataAssets.asset");
        DefaultGradient = new Gradient();
        DefaultGradient.mode = GradientMode.Blend;
        DefaultGradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };
        DefaultGradient.colorKeys = new GradientColorKey[] { new GradientColorKey(new Color(0, 0, 0), 0), new GradientColorKey(new Color(0, 0, 1), 1 / 3f), new GradientColorKey(new Color(0, 1, 0), 2 / 3f), new GradientColorKey(new Color(1, 0, 0), 1) };

        CustomGradient = new Gradient();
        CustomGradient.mode = GradientMode.Blend;
        CustomGradient.alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) };
        CustomGradient.colorKeys = new GradientColorKey[] { new GradientColorKey(new Color(0, 0, 0), 0), new GradientColorKey(new Color(0, 0, 1), 1 / 3f), new GradientColorKey(new Color(0, 1, 0), 2 / 3f), new GradientColorKey(new Color(1, 0, 0), 1) };

    }

    private void OnGUI()
    {
        EditorGUILayout.Separator();

        TextAsset = EditorGUILayout.ObjectField(TextAsset, typeof(TextAsset), false) as TextAsset;

        EditorGUILayout.Separator();

        if (TextAsset != null)
        {
            GenerateBinary = EditorGUILayout.ToggleLeft("Generate Binary", GenerateBinary);

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();

            GenerateImage = EditorGUILayout.ToggleLeft("Generate Image", GenerateImage);

            if (GenerateImage)
            {
                UseCustomRange = EditorGUILayout.ToggleLeft("Override range", UseCustomRange);
                UseCustomColors = EditorGUILayout.ToggleLeft("Override colors", UseCustomColors);
            }
            else
            {
                UseCustomRange = false;
                UseCustomColors = false;
            }

            EditorGUILayout.EndHorizontal();

            if (UseCustomRange)
            {
                CustomBottom = EditorGUILayout.FloatField("Bottom: ", CustomBottom);
                CustomTop = EditorGUILayout.FloatField("Top: ", CustomTop);
            }

            if (UseCustomColors)
            {
                SelectedCustomColorOption = EditorGUILayout.Popup("Custom color type", SelectedCustomColorOption, Enum.GetNames(typeof(CustomColorType)));

                switch ((CustomColorType)SelectedCustomColorOption)
                {
                    case CustomColorType.ColorRange:
                        EditorGUILayout.BeginHorizontal();
                        CustomColorFrom = EditorGUILayout.ColorField("Color from:", CustomColorFrom);
                        CustomColorTo = EditorGUILayout.ColorField("Color from:", CustomColorTo);
                        EditorGUILayout.EndHorizontal();
                        break;
                    case CustomColorType.Gradient:
                        CustomGradient = EditorGUILayout.GradientField(CustomGradient);
                        break;
                }
            }

            EditorGUILayout.Separator();

            GenerateGeoJSON = EditorGUILayout.ToggleLeft("Generate GeoJSON", GenerateGeoJSON);

            if (GenerateGeoJSON)
            {
                GeoJSONEntryCount = Math.Abs(EditorGUILayout.IntField(@"Count (""0"" for all): ", GeoJSONEntryCount));
            }

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();

            GenerateHistogram = EditorGUILayout.ToggleLeft("Generate Histogram", GenerateHistogram);

            if (GenerateHistogram)
            {
                selectedHistogramLayer = (Layers)EditorGUILayout.EnumFlagsField(selectedHistogramLayer);
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Separator();

            if (IsProcessing == false && GUILayout.Button("Convert"))
            {
                string assetPath = AssetDatabase.GetAssetPath(TextAsset);
                Task.Run(() => StartConverting(assetPath)).ContinueWith( t => SaveLayerAsset(), TaskContinuationOptions.ExecuteSynchronously);
            }

            if (IsProcessing == true && GUILayout.Button("Break"))
            {
                StopConverting();
            }
        }

        if (IsNewImageDataAvailable)
        {
            if (GenerateImage)
            {
                Texture2D newAsset = new Texture2D(ImageWidth, ImageHeight, TextureFormat.RGBA32, false);
                newAsset.SetPixels(Pixels);
                newAsset.Apply(false);

                byte[] bytes = newAsset.EncodeToPNG();

                try
                {
                    File.WriteAllBytes(AssetDatabase.GetAssetPath(TextAsset).Split('.')[0] + ".png", bytes);
                }
                catch (IOException e)
                {
                    Debug.Log(e.Message);
                }
            }

            IsNewImageDataAvailable = false;
            NewAssetAvailable = true;
            Debug.Log("Image generated");
        }

        if (NewAssetAvailable)
        {
            AssetDatabase.Refresh();
            NewAssetAvailable = false;
        }
    }

    private void SaveLayerAsset()
    {
        if (GenerateHistogram)
        {
            Debug.Log("SAVED");
            EditorUtility.SetDirty(layerDataAsset);
            AssetDatabase.SaveAssets();
        }
    }

    private void StartConverting(string assetPath)
    {
        if (GenerateImage == false && GenerateBinary == false && GenerateGeoJSON == false && GenerateHistogram == false)
        {
            Debug.Log("Nothing to do");
            return;
        }

        IsProcessing = true;

        try
        {
            using (StreamReader sr = new StreamReader(assetPath))
            {
                bool isReadingHeader = false;
                string headerString = "";

                string[] allLines = File.ReadAllLines(assetPath);
                int currentLineIndex = 0;

                for (int i = 0; i < HeaderMaxLines; i++)
                {
                    string line = allLines[currentLineIndex];
                    currentLineIndex++;

                    if (line.Length == 0)
                    {
                        continue;
                    }

                    if (line[0] == '!')
                    {
                        continue;
                    }

                    if (line[0] == '@')
                    {
                        if (isReadingHeader)
                        {
                            break;
                        }

                        isReadingHeader = true;
                    }

                    if (isReadingHeader)
                    {
                        headerString += line;
                        headerString += ',';
                    }
                }

                // 0       1        2           3   4           5   6   7   8           9        10     11     12        13        14  15         16         
                // Header  DataType ColumnCount ?   NoDataValue ?   ?   ?   ColumnCount RowCount RowMin RowMax ColumnMin ColumnMax ?   ColumnSize RowSize

                string[] splittedHeader = headerString.Split(',');

                DataHeader header = new DataHeader();
                header.DataType = splittedHeader[1]; //1
                header.NoDataValue = float.Parse(splittedHeader[4].Replace('.', ',')); //4
                header.ColumnCount = int.Parse(splittedHeader[8]); //8
                header.RowCount = int.Parse(splittedHeader[9]); //9
                header.RowMin = float.Parse(splittedHeader[10].Replace('.', ',')); //10
                header.RowMax = float.Parse(splittedHeader[11].Replace('.', ',')); //11
                header.ColumnMin = float.Parse(splittedHeader[12].Replace('.', ',')); //12
                header.ColumnMax = float.Parse(splittedHeader[13].Replace('.', ',')); //13
                header.ColumnSize = float.Parse(splittedHeader[15].Replace('.', ',')); //15
                header.RowSize = float.Parse(splittedHeader[16].Replace('.', ',')); //16

                char[] separator = new char[] { ' ' };

                float[] floatArray = new float[header.ColumnCount * header.RowCount];
                int currentElement = 0;

                DateTime startTime = DateTime.UtcNow;
                DateTime time = startTime;
                float step = 100000;

                float minValue = float.MaxValue;
                float maxValue = float.MinValue;

                while (currentElement < floatArray.Length && IsProcessing)
                {
                    string line = allLines[currentLineIndex];
                    currentLineIndex++;

                    string[] lineArray = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string str in lineArray)
                    {
                        float value = float.Parse(str.Replace('.', ','));

                        if (value != header.NoDataValue)
                        {
                            if (value < minValue)
                            {
                                minValue = value;
                            }

                            if (value > maxValue)
                            {
                                maxValue = value;
                            }
                        }

                        floatArray[currentElement] = value;
                        currentElement++;
                    }

                    if (currentElement % step == 0)
                    {
                        TimeSpan timeDiff = DateTime.UtcNow - time;
                        time = DateTime.UtcNow;

                        Debug.Log("Converting progress: " + currentElement + "/" + floatArray.Length);
                        Debug.Log("Converting speed: " + (step / timeDiff.TotalMinutes).ToString("########") + " entries per minute");
                        Debug.Log("End in " + (((floatArray.Length - currentElement) / step) * timeDiff.TotalSeconds).ToString("###.##") + " seconds");
                    }
                }

                Debug.Log("Converted: " + currentElement + "/" + floatArray.Length + " entries in " + (DateTime.UtcNow - startTime).TotalSeconds.ToString("####.#") + " seconds");
                Debug.Log("Min value: " + minValue);
                Debug.Log("Max value: " + maxValue);

                Debug.Log(" - - - - - - - - - - - - - - - - - - - - - - ");

                if (GenerateBinary)
                {
                    Debug.Log("Binary generating started");

                    DataHeader.Convert(header, assetPath.Split('.')[0] + ".header");

                    byte[] array = new byte[4 * header.ColumnCount * header.RowCount];
                    int index = 0;

                    foreach (float value in floatArray)
                    {
                        byte[] bytes = BitConverter.GetBytes(value);

                        for (int k = 0; k < bytes.Length; k++)
                        {
                            array[index] = bytes[k];
                            index++;
                        }
                    }

                    using (FileStream fs = File.Create(assetPath.Split('.')[0] + ".bin", 2048, FileOptions.None))
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        bw.Write(array);
                    }

                    Debug.Log("Binary generated");
                    NewAssetAvailable = true;
                }

                if (this.GenerateHistogram)
                {

                    Debug.Log("Histogram generating started");

                    int bucketSize = 100;

                    var histogramResult = HistogrammHelper.Bucketize(floatArray, bucketSize, header.NoDataValue);



                    switch (this.selectedHistogramLayer)
                    {
                        case Layers.Elevation:
                            layerDataAsset.elevationData = histogramResult;
                            layerDataAsset.elevationbucketSize = bucketSize;
                            break;
                        case Layers.Porosity:
                            layerDataAsset.porosityData = histogramResult;
                            layerDataAsset.porositybucketSize = bucketSize;
                            break;
                        case Layers.Thickness:
                            layerDataAsset.thicknessnData = histogramResult;
                            layerDataAsset.thicknessBucketSize = bucketSize;
                            break;
                        case Layers.TOC:
                            layerDataAsset.tocData = histogramResult;
                            layerDataAsset.tocBucketSize = bucketSize;
                            break;
                        case Layers.VShale:
                            layerDataAsset.vshaleData = histogramResult;
                            layerDataAsset.vshaleBucketSize = bucketSize;
                            break;
                    }
                    Debug.Log($"{histogramResult} {bucketSize}");
                }

                if (GenerateImage)
                {
                    Debug.Log("Image generating started");

                    ImageWidth = header.ColumnCount;
                    ImageHeight = header.RowCount;

                    Pixels = new Color[ImageWidth * ImageHeight];

                    for (int i = 0; i < floatArray.Length; i++)
                    {
                        if (UseCustomRange)
                        {
                            Pixels[i] = ConvertFloatToRGB(floatArray[i], CustomBottom, CustomTop, header.NoDataValue);
                        }
                        else
                        {
                            Pixels[i] = ConvertFloatToRGB(floatArray[i], minValue, maxValue, header.NoDataValue);
                        }
                    }

                    IsNewImageDataAvailable = true;
                }

                if (GenerateGeoJSON)
                {
                    Debug.Log("GeoJSON generating started");

                    int current = 0;
                    int currentColumn = 0;
                    int currentRow = 0;

                    GeoJSON gJSON = new GeoJSON();

                    foreach (float value in floatArray)
                    {
                        if (GeoJSONEntryCount != 0 && current >= GeoJSONEntryCount)
                        {
                            break;
                        }

                        currentColumn = (int)Mathf.Floor(((current / (float)header.ColumnCount) % 1) * header.ColumnCount);
                        currentRow = (int)Mathf.Floor(current / (float)header.ColumnCount);
                        current++;

                        if (value != header.NoDataValue)
                        {
                            GeoPoint geogr = tr.TransformUTMToGeographic(new GeoPoint((header.ColumnMin + currentColumn * header.ColumnSize) * Foot, (header.RowMin + currentRow * header.RowSize) * Foot), enumProjection.UTM13N, enumEllipsoid.WGS84);
                            gJSON.AddPoint(geogr.Y, geogr.X, value);
                        }
                    }

                    string geoJSONString = gJSON.GetJSON();
                    File.WriteAllText(assetPath.Split('.')[0] + ".geojson", geoJSONString);

                    Debug.Log("GeoJSON generated");
                    NewAssetAvailable = true;
                }
            }
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be read: " + e.Message);
        }
        catch (FormatException e)
        {
            Debug.Log("Parsing error: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.Log("Unknown exception: " + e.Message);
        }
        finally
        {
            IsProcessing = false;
        }
    }


    private void StopConverting()
    {
        IsProcessing = false;
    }

    private Color ConvertFloatToRGB(float value, float min, float max, float noDataValue)
    {
        if (value == noDataValue)
        {
            return Color.clear;
        }

        float remappedValue = remap(value, min, max, 0, 1);

        if (UseCustomColors)
        {
            switch ((CustomColorType)SelectedCustomColorOption)
            {
                case CustomColorType.ColorRange:
                    Color result = CustomColorFrom + (CustomColorTo - CustomColorFrom) * remappedValue;
                    result.a = 1;
                    return result;

                case CustomColorType.Gradient:
                    return ConvertToGradientColor(ref remappedValue, CustomGradient);

                case CustomColorType.Elevation:
                    return ConvertToElevationColor(remap(value, min, max, 0, 256 * 256 * 256 - 1));

                default: throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            return ConvertToGradientColor(ref remappedValue, DefaultGradient);
        }

        float remap(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        Color ConvertToGradientColor(ref float rValue, Gradient gradient)
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

        Color ConvertToElevationColor(float rValue)
        {
            byte r = (byte)(rValue / (256 * 256));
            byte g = (byte)((rValue - r * 256 * 256) / 256);
            byte b = (byte)(rValue - r * 256 * 256 - g * 256);
            byte a = 255;

            return new Color32(r, g, b, a);
        }
    }

    private class GeoJSON
    {
        private StringBuilder jsonBuilder;

        private string GeoJSON_Start = @"{""type"": ""FeatureCollection"",""features"": [";
        private string GeoJSON_End = @"]}";

        private string GeoJSON_PointTemplate = @"{""geometry"": {""coordinates"": [LONGITUDE,LATITUDE],""type"": ""Point""},""type"": ""Feature"",""properties"": {""value"": VALUE}},";
        private string GeoJSON_PolygonTemplate = @"{""geometry"":{""coordinates"": [[[LO1,LA1],[LO2,LA2],[LO3,LA3],[LO4,LA4],[LO1, LA1]]],""type"": ""Polygon""},""type"": ""Feature"",""properties"": {""value"": VALUE}},";

        public int Count { get; private set; }

        public GeoJSON()
        {
            jsonBuilder = new StringBuilder();
            jsonBuilder.Append(GeoJSON_Start);
        }

        public void AddPolygon(double la1, double lo1, double la2, double lo2, double la3, double lo3, double la4, double lo4, float value)
        {
            string jsonString = GeoJSON_PolygonTemplate.Replace("LA1", la1.ToString().Replace(',', '.'))
                                                       .Replace("LO1", lo1.ToString().Replace(',', '.'))
                                                       .Replace("LA2", la2.ToString().Replace(',', '.'))
                                                       .Replace("LO2", lo2.ToString().Replace(',', '.'))
                                                       .Replace("LA3", la3.ToString().Replace(',', '.'))
                                                       .Replace("LO3", lo3.ToString().Replace(',', '.'))
                                                       .Replace("LA4", la4.ToString().Replace(',', '.'))
                                                       .Replace("LO4", lo4.ToString().Replace(',', '.'))
                                                       .Replace("VALUE", value.ToString().Replace(',', '.'));
            jsonBuilder.Append(jsonString);

            Count++;
        }

        public void AddPoint(double longitude, double latitude, float value)
        {
            string jsonString = GeoJSON_PointTemplate.Replace("LONGITUDE", longitude.ToString().Replace(',', '.'))
                                                     .Replace("LATITUDE", latitude.ToString().Replace(',', '.'))
                                                     .Replace("VALUE", value.ToString().Replace(',', '.'));
            jsonBuilder.Append(jsonString);

            Count++;
        }

        public string GetJSON()
        {
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append(GeoJSON_End);
            return jsonBuilder.ToString();
        }
    }
}

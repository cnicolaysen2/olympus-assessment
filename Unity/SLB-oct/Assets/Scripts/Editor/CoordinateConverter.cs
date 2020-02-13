using BojkoSoft.Transformations;
using BojkoSoft.Transformations.Constants;
using UnityEditor;
public static class CoordinateConverterEditor
{
    [MenuItem("Schlumberger/Coordinate Converter")]
    public static void Init()
    {
        CoordinateConverter window = EditorWindow.GetWindow<CoordinateConverter>("Coordinate Converter");
        window.Show();
    }
}

public class CoordinateConverter : EditorWindow
{
    private static Transformations tr = new Transformations();

    private string NorthingFT = string.Empty;
    private string EastingFT = string.Empty;

    private string Latitude;
    private string Longitude;

    private float Foot = 0.3048f;

    private void OnGUI()
    {
        EastingFT = EditorGUILayout.TextField("Easting (ft): ", EastingFT);
        NorthingFT = EditorGUILayout.TextField("Northing (ft): ", NorthingFT);

        if (double.TryParse(EastingFT.Replace(".", ","), out double eastingFloatFT) && double.TryParse(NorthingFT.Replace(".", ","), out double northingFloatFT))
        {
            EditorGUILayout.Separator();

            EditorGUILayout.TextField("Easting (m): ", (eastingFloatFT * Foot).ToString("#########.####").Replace(",", "."));
            EditorGUILayout.TextField("Northing (m): ", (northingFloatFT * Foot).ToString("#########.####").Replace(",", "."));

            GeoPoint point = new GeoPoint(northingFloatFT * Foot, eastingFloatFT * Foot);
            point = tr.TransformUTMToGeographic(point, enumProjection.UTM13N, enumEllipsoid.WGS84);

            EditorGUILayout.Separator();

            EditorGUILayout.TextField("Longtitude: ", point.Y.ToString("####.#######").Replace(",", "."));
            EditorGUILayout.TextField("Latitude: ", point.X.ToString("####.#######").Replace(",", "."));
        }     
    }       
}
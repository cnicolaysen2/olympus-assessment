using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.UI;

public class MeshEnabler : MonoBehaviour
{
    public AbstractMap Map;

    public Toggle Toggle;
    
    [SerializeField]
    private Vector3 box = Vector3.one*10;
    
    [SerializeField]
    private Vector3 origin;

    private void Awake()
    {
        Toggle.onValueChanged.AddListener(ToggleChanged);
    }

    private void ToggleChanged(bool arg0)
    {
        if (arg0)
        {
            Map.Terrain.SetElevationType(ElevationLayerType.TerrainWithElevation);
        }
        else
        {
            Map.Terrain.SetElevationType(ElevationLayerType.FlatTerrain);
        }

        //Map.VectorData.FindFeatureSubLayerWithName("ElevationMesh").SetActive(arg0);
    }

    private void Update()
    {

        Shader.SetGlobalVector("_BoxSize", new Vector4(
            box.x,
            box.y,
            box.z,
            0f));
        Shader.SetGlobalVector("_Origin", new Vector4(
            origin.x,
            origin.y,
            origin.z,
            0f));

    }

    private void OnDestroy()
    {
        Toggle.onValueChanged.RemoveAllListeners();
    }
}

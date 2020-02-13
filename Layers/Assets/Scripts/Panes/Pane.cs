using UnityEngine;
using UnityEngine.UI;

public enum EPaneType
{
    TreeDimentional,
    Upper,
    Chart,
    LineChart,
}

public class Pane : MonoBehaviour
{
    [Header("Common")]
    public EPaneType Type;
    public string PaneName;
    public string Caption;
    public Color CornerColor;

    [Header("Camera settings")]
    public Vector3 CameraPosition;
    public Vector3 CameraAngles;
    public Rect ViewportRect;
    public int CullingMask = -1;

    [Space]
    public bool Orthographic = false;
    public float OrthographicSize = 5;

    [Header("Canvas settings")]
    public GameObject ControlPanelPrefab;
    public GameObject CaptionPrefab;
    public GameObject CornerPrefab;

    [Header("Dynamic")]
    public GameObject CameraGameObject;
    public Camera PaneCamera;

    [Space]
    public GameObject CanvasGameObject;
    public Canvas PaneCanvas;
    public IControlPanel ControlPanel;

    [Space]
    public GameObject CaptionGameObject;
    public GameObject CornerGameObject;

    private void Awake()
    {
        gameObject.name = PaneName;

        CameraGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/DefaultCamera"));
        CameraGameObject.transform.SetParent(transform, false);
        CameraGameObject.name = PaneName + "_Camera";
        CameraGameObject.transform.position = CameraPosition;
        CameraGameObject.transform.localEulerAngles = CameraAngles;

        CanvasGameObject = Instantiate(Resources.Load<GameObject>("Prefabs/UI/DefaultCanvas"));
        CanvasGameObject.transform.SetParent(transform, false);
        CanvasGameObject.name = PaneName + "_Canvas";

        ControlPanel = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ControlPanel")).GetComponent<IControlPanel>();
        ControlPanel.SetParent(CanvasGameObject.transform, false);
        ControlPanel.SetName(CanvasGameObject.name + "_ControlPanel");

        PaneCamera = CameraGameObject.GetComponent<Camera>();

        if (Orthographic)
        {
            PaneCamera.orthographic = Orthographic;
            PaneCamera.orthographicSize = OrthographicSize;

            foreach (Transform tr in CameraGameObject.transform)
            {
                Camera cam = tr.gameObject.GetComponent<Camera>();

                if(cam != null)
                {
                    cam.orthographic = Orthographic;
                    cam.orthographicSize = OrthographicSize;
                }
            }
        }

        PaneCamera.cullingMask = CullingMask;
        PaneCamera.rect = ViewportRect;

        PaneCanvas = CanvasGameObject.GetComponent<Canvas>();
        PaneCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        PaneCanvas.worldCamera = PaneCamera;

        CaptionGameObject = Instantiate(CaptionPrefab);
        CaptionGameObject.name = Caption;
        CaptionGameObject.transform.SetParent(PaneCanvas.transform, false);

        float paneWidth = 0;
        float paneHeight = 0;

        switch (Type)
        {
            case EPaneType.TreeDimentional:
                paneWidth = Screen.width * 0.5f;
                paneHeight = Screen.height;
                break;


            case EPaneType.Upper:
                paneWidth = Screen.width * 0.5f;
                paneHeight = Screen.height * 0.5f;
                break;

            case EPaneType.Chart:
            case EPaneType.LineChart:
                paneWidth = Screen.width * 0.25f;
                paneHeight = Screen.height * 0.5f;
                break;
        }

        float horizontalOffset = paneWidth * 0.5f - 20f;
        float verticalOffset = paneHeight * 0.5f - 20f;

        CaptionGameObject.transform.localPosition = new Vector3(-horizontalOffset, verticalOffset, CaptionGameObject.transform.localPosition.z);
        Text t = CaptionGameObject.GetComponent<Text>();

        if (t != null)
        {
            t.text = Caption;
        }

        CornerGameObject = Instantiate(CornerPrefab);
        CornerGameObject.name = "PaneCorner";
        CornerGameObject.transform.SetParent(PaneCanvas.transform, false);

        switch (Type)
        {
            case EPaneType.TreeDimentional:
                paneWidth = Screen.width * 0.5f;
                paneHeight = Screen.height;
                break;


            case EPaneType.Upper:
                paneWidth = Screen.width * 0.5f;
                paneHeight = Screen.height * 0.5f;
                break;

            case EPaneType.Chart:
            case EPaneType.LineChart:
                paneWidth = Screen.width * 0.25f;
                paneHeight = Screen.height * 0.5f;
                break;
        }

        horizontalOffset = paneWidth * 0.5f - 10f;
        verticalOffset = paneHeight * 0.5f;

        CornerGameObject.transform.localPosition = new Vector3(horizontalOffset, verticalOffset, CornerGameObject.transform.localPosition.z);
        Image img = CornerGameObject.GetComponent<Image>();

        if (img != null)
        {
            img.color = CornerColor;
        }
    }

    private void Update()
    {
        if (PaneCamera.rect.Contains(new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, Input.mousePosition.z)))
        {
            if (ControlPanel.IsActive == false)
            {
                ControlPanel.IsActive = true;
            }
        }
        else
        {
            if (ControlPanel.IsActive == true)
            {
                ControlPanel.IsActive = false;
            }
        }
    }

    public void SetParent(Transform parent, bool worldPositionStays = true)
    {
        gameObject.transform.SetParent(parent, worldPositionStays);
    }
}

using DG.Tweening;
using System;
using UnityEngine;



[RequireComponent(typeof(Pane))]
public class CameraRotator : MonoBehaviour
{
    public float HSensitivity;
    public float VSensitivity;
    
    [Space]
    public GameObject CubePrefab;
    public GameObject CubeObject;

    public string CubeLayer;

    private Vector3 PreviousMousePosition;
    private Vector3 PreviousMouseClickPosition;
    private Pane Pane;

    private RaycastHit[] HitResults = new RaycastHit[5];
    
    private Tweener RotationTweener;
    private Tweener MovingTweener;

    private float TransitionTime = 1;

    private float StartingDistance;
    private float OrthographicSize;

    private GizmoCubeSide GCS;

    private void Awake()
    {
        Pane = GetComponent<Pane>();

        CubeObject = Instantiate(CubePrefab);
        CubeObject.name = "GizmoCube";
        CubeObject.transform.SetParent(Pane.PaneCanvas.transform, false);

        int cubeLayer = LayerMask.NameToLayer(CubeLayer);

        CubeObject.layer = cubeLayer;

        foreach (Transform tr in CubeObject.transform)
        {
            tr.gameObject.layer = cubeLayer;
        }

        float paneWidth = 0;
        float paneHeight = 0;

        switch (Pane.Type)
        {
            case EPaneType.TreeDimentional:

                paneWidth = Screen.width * 0.5f;
                paneHeight = Screen.height;

                break;


            case EPaneType.Upper:

                paneWidth = Screen.width * 0.5f;
                paneHeight = Screen.height * 0.5f;

                break;
        }

        float horizontalOffset = paneWidth * 0.4f;
        float verticalOffset = paneHeight * 0.4f;

        CubeObject.transform.localPosition = new Vector3(-horizontalOffset, -verticalOffset, CubeObject.transform.localPosition.z);
    }

    private void Start()
    {
        StartingDistance = Vector3.Distance(LayerController.Instance.transform.position, Pane.CameraGameObject.transform.position);
        OrthographicSize = Pane.PaneCamera.orthographicSize;
    }

    private void LateUpdate()
    {
        CubeObject.transform.rotation = Quaternion.identity;

        if (Input.mouseScrollDelta != Vector2.zero && Pane.PaneCamera.rect.Contains(new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, Input.mousePosition.z)))
        {
            if (Pane.PaneCamera.orthographic)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    if (Pane.PaneCamera.orthographicSize > OrthographicSize / 2f)
                    {
                        Pane.PaneCamera.orthographicSize -= Input.mouseScrollDelta.y * OrthographicSize / 20;
                    }
                }
                else
                {
                    if (Pane.PaneCamera.orthographicSize < OrthographicSize * 2f)
                    {
                        Pane.PaneCamera.orthographicSize -= Input.mouseScrollDelta.y * OrthographicSize / 20;
                    }
                }
            }
            else
            {
                float currentDistance = Vector3.Distance(LayerController.Instance.transform.position, Pane.CameraGameObject.transform.position);

                if (Input.mouseScrollDelta.y > 0)
                {
                    if (currentDistance > StartingDistance / 1.2f)
                    {
                        Pane.CameraGameObject.transform.localPosition += Input.mouseScrollDelta.y * Pane.CameraGameObject.transform.forward;
                    }
                }
                else
                {
                    if (currentDistance < StartingDistance * 2f)
                    {
                        Pane.CameraGameObject.transform.localPosition += Input.mouseScrollDelta.y * Pane.CameraGameObject.transform.forward;
                    }
                }
            }
        }

        if (Input.GetMouseButton(0) && RotationTweener == null && MovingTweener == null && Pane.PaneCamera.rect.Contains(new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, Input.mousePosition.z)))
        {
            Pane.CameraGameObject.transform.RotateAround(LayerController.Instance.transform.position, LayerController.Instance.transform.up, (Input.mousePosition - PreviousMousePosition).x * HSensitivity);
            Pane.CameraGameObject.transform.RotateAround(LayerController.Instance.transform.position, -Pane.CameraGameObject.transform.right, (Input.mousePosition - PreviousMousePosition).y * VSensitivity);
        }

        PreviousMousePosition = Input.mousePosition;

        GizmoCubeSide gcs = null;

        if (Pane.Type == EPaneType.TreeDimentional
            && Pane.PaneCamera.rect.Contains(new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, Input.mousePosition.z)))
        {
            int hitCount = Physics.RaycastNonAlloc(Pane.PaneCamera.ScreenPointToRay(Input.mousePosition), HitResults, 150, 1 << LayerMask.NameToLayer("GizmoCube3D"));

            if (hitCount > 0)
            {
                float nearestDistance = float.MaxValue;
                float tempDistance;

                GameObject nearestGO = null;

                for (int i = 0; i < hitCount; i++)
                {
                    tempDistance = (HitResults[i].point - Pane.PaneCamera.transform.position).magnitude;

                    if (tempDistance < nearestDistance)
                    {
                        nearestDistance = tempDistance;
                        nearestGO = HitResults[i].collider.gameObject;
                    }
                }

                if (nearestGO != null)
                {
                    gcs = nearestGO.GetComponent<GizmoCubeSide>();
                }
            }
        }

        if (GCS != gcs)
        {
            if (GCS != null)
            {
                GCS.StopHighlight();
            }

            GCS = gcs;

            if (gcs != null)
            {
                gcs.Highlight();
            }
        }

        if (RotationTweener != null || MovingTweener != null) return;

        if (Input.GetMouseButtonDown(0))
        {
            PreviousMouseClickPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && gcs != null)
        {
            Vector3 endValue;
            Vector3 direction;

            switch (gcs.GizmoSide)
            {
                case GizmoSide.Top:
                    endValue = new Vector3(90, 270, 90); //
                    direction = LayerController.Instance.transform.up;
                    break;
                case GizmoSide.Bottom:
                    endValue = new Vector3(270, 0, 0); //
                    direction = -LayerController.Instance.transform.up;
                    break;
                case GizmoSide.Left:
                    endValue = new Vector3(0, 270, 0); //
                    direction = LayerController.Instance.transform.right;
                    break;
                case GizmoSide.Front:
                    endValue = new Vector3(0, 180, 0); //
                    direction = LayerController.Instance.transform.forward;
                    break;
                case GizmoSide.Right:
                    endValue = new Vector3(0, 90, 0); //
                    direction = -LayerController.Instance.transform.right;
                    break;
                case GizmoSide.Back:
                    endValue = new Vector3(0, 0, 0); // 
                    direction = -LayerController.Instance.transform.forward;
                    break;

                default: throw new ArgumentOutOfRangeException();
            }

            RotationTweener = Pane.CameraGameObject.transform.DORotate(endValue, TransitionTime, RotateMode.Fast)
                                                             .SetEase(Ease.Linear)
                                                             .OnComplete(() => RotationTweener = null);

            Vector3 initPosition = Pane.CameraGameObject.transform.position;
            float distance = Vector3.Distance(initPosition, LayerController.Instance.transform.position);

            MovingTweener = DOTween.To(() => 0, (v) => Pane.CameraGameObject.transform.position = Vector3.Slerp(initPosition, direction * distance, v), 1f, TransitionTime)
                .SetEase(Ease.Linear)
                .OnComplete(() => MovingTweener = null);
        }
    }
}

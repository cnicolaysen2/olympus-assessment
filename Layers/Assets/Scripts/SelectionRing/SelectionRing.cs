using System;
using UnityEngine;

public class SelectionRing : MonoBehaviour
{
    public event Action<ESectorName> ButtonPressed = (s) => { };

    public GameObject RingGameObject;
    public RingSector[] Sectors;

    private RectTransform RectTransform;

    private void Awake()
    {
        RectTransform = (RectTransform)transform;

        if (Sectors != null)
        {
            foreach (RingSector rs in Sectors)
            {
                if (rs == null) continue;

                rs.SectorSelected += ProcessRingSectorSelection;
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            if (RingGameObject.activeSelf == false) RingGameObject.SetActive(true);
        }
        else
        {
            if (RingGameObject.activeSelf == true) RingGameObject.SetActive(false);

            RectTransform.position = Input.mousePosition;
        }
    }

    private void ProcessRingSectorSelection(ESectorName obj)
    {
        switch(obj)
        {
            case ESectorName.Explode_All_Directions:
                LayerController.Instance.ExpandAll();
                break;

            case ESectorName.Explode_Horizontally:
                LayerController.Instance.ExpandHorizontally();
                break;

            case ESectorName.Explode_Vertically:
                LayerController.Instance.ExpandVertically();
                break;

            case ESectorName.Display_Zones:
                LayerController.Instance.SwitchBlock();
                break;

            case ESectorName.Display_Horizons:
                LayerController.Instance.SwitchPlane();
                break;
        }
    }

    private void OnDestroy()
    {
        if (Sectors != null)
        {
            foreach (RingSector rs in Sectors)
            {
                if (rs == null) continue;

                rs.SectorSelected -= ProcessRingSectorSelection;
            }

            Sectors = null;
        }
    }
}

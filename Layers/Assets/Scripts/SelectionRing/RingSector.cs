using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class RingSector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<ESectorName> SectorSelected = (s) => { };

    public ESectorName SectorName;

    public Color BaseColor;

    private Image SectorImage;

    private Color Opaque;
    private Color Transparent;

    private bool IsFocused = false;

    private void Awake()
    {
        Opaque = BaseColor;
        Opaque.a = 1;

        Transparent = BaseColor;
        Transparent.a = 0;

        SectorImage = GetComponent<Image>();
        SectorImage.alphaHitTestMinimumThreshold = 1f;
        SectorImage.color = Transparent;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");

        SectorImage.color = Opaque;
        IsFocused = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");

        SectorImage.color = Transparent;
        IsFocused = false;
    }

    private void OnDisable()
    {
        //Debug.Log("OnDisable");

        if (IsFocused) SectorSelected(SectorName);
        IsFocused = false;

        SectorImage.color = Transparent;
    }
}

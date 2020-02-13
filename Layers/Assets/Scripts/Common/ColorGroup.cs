using System;
using UnityEngine;
using UnityEngine.UI;


public class ColorGroup : MonoBehaviour
{
    public static Action<int, Color> GroupColorUpdated = (i, c) => { };
    public static Action<int, float> GroupOpacityUpdated = (i, o) => { };

    public static Action _ResetSelection = () => { };

    public static void SetGroupColor(int groupIndex, Color groupColor)
    {
        GroupColorUpdated(groupIndex, groupColor);
    }

    public static void SetGroupOpacity(int groupIndex, float opacity)
    {
        GroupOpacityUpdated(groupIndex, opacity);
    }

    public static void ResetSelection()
    {
        _ResetSelection();
    }

    public int GroupIndex;

    public SpriteRenderer SpriteRenderer;
    public MeshRenderer MeshRenderer;
    public Image Image;

    //private Color SpriteDefaultColor;
    //private Color MeshDefaultColor;
    //private Color ImageDefaultColor;


    private ColorSettings colors;

    private RaycastHit[] HitResults = new RaycastHit[5];

    private void Awake()
    {
        this.colors = Resources.Load<ColorSettings>("ColorSettings");
        if (SpriteRenderer == null)
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            if (SpriteRenderer != null)
            {
                SpriteRenderer.color = colors.DefaultColor;
                //SpriteDefaultColor = SpriteRenderer.color;
            }
        }

        if (MeshRenderer == null)
        {
            MeshRenderer = GetComponent<MeshRenderer>();

            if (MeshRenderer != null)
            {
                MeshRenderer.material.color = colors.DefaultColor;
             //   MeshDefaultColor = MeshRenderer.material.color;
            }
        }

        if (Image == null)
        {
            Image = GetComponent<Image>();

            if (Image != null)
            {
                Image.color = colors.DefaultColor;
                //ImageDefaultColor = Image.color;
            }
        }

        GroupColorUpdated += UpdateGroupColor;
        GroupOpacityUpdated += UpdateGroupOpacity;
        _ResetSelection += ResetSelectionInternal;
    }

    private void UpdateGroupOpacity(int index, float opacity)
    {
        if (index != GroupIndex) return;

        Color temp = colors.DefaultColor;
        temp.a = opacity;

        if (SpriteRenderer != null) SpriteRenderer.color = temp;
        if (MeshRenderer != null)
        {
            MeshRenderer.material.color = temp;
            this.MeshRenderer.material.SetFloat("_Opacity",opacity);
        };
        if (Image != null) Image.color = temp;
    }

    private void UpdateGroupColor(int index, Color color)
    {
        if (index != GroupIndex) return;

        if (SpriteRenderer != null) SpriteRenderer.color = color;
        if (MeshRenderer != null)
        {
            MeshRenderer.material.color = color;
            this.MeshRenderer.material.SetFloat("_Opacity", 1);
        };
        if (Image != null) Image.color = color;
    }

    public void SetGroupColor(Color groupColor)
    {
        SetGroupColor(GroupIndex, groupColor);
    }
    
    private void ResetSelectionInternal()
    {       
        if (SpriteRenderer != null) SpriteRenderer.color = colors.DefaultColor;
        if (MeshRenderer != null)
        {
            MeshRenderer.material.color = colors.DefaultColor;

            this.MeshRenderer.material.SetFloat("_Opacity", 1);
        }
        if (Image != null) Image.color = colors.DefaultColor;
    }

    private void OnDestroy()
    {
        GroupColorUpdated -= UpdateGroupColor;
        GroupOpacityUpdated += UpdateGroupOpacity;
        _ResetSelection -= ResetSelectionInternal;
    }
}

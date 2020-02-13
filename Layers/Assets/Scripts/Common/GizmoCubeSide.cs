using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GizmoSide
{
    Top,
    Bottom,
    Left,
    Right,
    Front,
    Back,
}

public class GizmoCubeSide : MonoBehaviour
{
    public GizmoSide GizmoSide;

    public SpriteRenderer Side;

    private Color Default;
    private Color Highlighted;

    private Tweener HighlightTweener;

    private float HighlightingTime = 0.5f;

    private void Awake()
    {
        Default = Side.color;
        Default.a = 0f;

        Highlighted = Default;
        Highlighted.a = 0.5f;

        Side.color = Default;
    }

    public void Highlight()
    {
        if (HighlightTweener != null)
        {
            HighlightTweener.Kill();
            HighlightTweener = null;
        };

        HighlightTweener = DOTween.To(() => Side.color, (c) => Side.color = c, Highlighted, HighlightingTime)
            .OnComplete(() => HighlightTweener = null);
    }

    public void StopHighlight()
    {
        if (HighlightTweener != null)
        {
            HighlightTweener.Kill();
            HighlightTweener = null;
        };

        HighlightTweener = DOTween.To(() => Side.color, (c) => Side.color = c, Default, HighlightingTime)
            .OnComplete(() => HighlightTweener = null);
    }
}


using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LayerExpander : MonoBehaviour
{

    [SerializeField]
    private GameObject LayerRoot;

    [SerializeField]
    private Color DefaultColor;

    [SerializeField]
    private ColorSettings ColorSettings;


    public void ExpandAll(float direction)
    {
        if(this.CurrentTween != null)
          return;
      
        ExpandVert(direction).onComplete += () =>
        {
            ExpandHoriz(direction);
        };
    }

    private Tween _currentTween;
    private Tween CurrentTween {
        get
        {
            return _currentTween;
        }
        set
        {
            void resetTween()
            {
                this._currentTween = null;
            }

            if (this._currentTween != null)
            {

                _currentTween.onComplete -= resetTween;
                _currentTween.onKill -= resetTween;
            }

            _currentTween = value;
            if (_currentTween != null)
            {
                _currentTween.onComplete += resetTween;
                _currentTween.onKill += resetTween;
            }
        }
    }

    public  bool IsExpanding
    {
        get { return CurrentTween != null; }
    }

    public Tween ExpandVert(float direction)
    {
        if (CurrentTween != null)
            return CurrentTween;
      
        var wholeTween = DOTween.Sequence();
        var level0 = GetChildsWithPattern("1");
        wholeTween.Join(TweenGroup(level0, LayerRoot.transform.up * direction));

        var level1 = GetChildsWithPattern("2");
        wholeTween.Join(TweenGroup(level1, LayerRoot.transform.up * direction * 0.5f));

        var level2 = GetChildsWithPattern("3");
        wholeTween.Join(TweenGroup(level2, LayerRoot.transform.up * direction * -0.5f));

        var level3 = GetChildsWithPattern("4");
        wholeTween.Join(TweenGroup(level3, LayerRoot.transform.up * direction * -1f));
        CurrentTween = wholeTween;
        return wholeTween;
    }

    public void ExpandHoriz(float direction)
    {
        if (this.CurrentTween != null)
            return;

        var wholeTween = DOTween.Sequence();
        var front = GetChildsWithPattern("A", "B", "C", "D");
        wholeTween.Join(TweenGroup(front, LayerRoot.transform.forward * direction));

        var forwardLK = GetChildsWithPattern("J", "K");
        wholeTween.Join(TweenGroup(forwardLK, LayerRoot.transform.forward * direction * -0.8f));

        var back = GetChildsWithPattern( "L", "I", "H");
        wholeTween.Join(TweenGroup(back, LayerRoot.transform.forward * direction * -1));



        CurrentTween = wholeTween;
        wholeTween.onComplete += () =>
        {   
            var wholeTween2 = DOTween.Sequence();

            var rightB = GetChildsWithPattern("B");
            wholeTween2.Join(TweenGroup(rightB, LayerRoot.transform.right * direction * 0.2f));

            var left = GetChildsWithPattern("A", "E", "H");
            wholeTween2.Join(TweenGroup(left, LayerRoot.transform.right * direction));

            var rightLK = GetChildsWithPattern("J");
            wholeTween2.Join(TweenGroup(rightLK, LayerRoot.transform.right * direction * -0.8f));

            var right = GetChildsWithPattern("D", "K", "G", "L");
            wholeTween2.Join(TweenGroup(right, LayerRoot.transform.right * direction * -1));

            CurrentTween = wholeTween2;
        };
    }

    public void AgeFilter(int age)
    {
        switch (age)
        {
            case 0:
                SetActive(GetChildsWithPattern("p"), true, false);
                break;
            case 2:
                SetActive(GetChildsWithPattern("p"), false);
                SetActive(GetChildsWithPattern("3","4"), true);
                break;
            case 3:
                SetActive(GetChildsWithPattern("p"), false);
                SetActive(GetChildsWithPattern("2"), true);
                break;
            case 4:
                SetActive(GetChildsWithPattern("p"), false, true);
                SetActive(GetChildsWithPattern("1"), true);
                break;
        }
    }

    private const float OPACITY_RATE = 0.3f;
    private void SetActive(List<Transform> elements,bool state, bool colorSelection = true, bool disableGO = false)
    {
        foreach (Transform elem in elements)
        {
            elem.gameObject.SetActive(state || !disableGO);
            MeshRenderer renderer = elem.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                if (colorSelection || !state)
                {
                    Color resultColor = state ? ColorSettings.AgeSelectionColor : ColorSettings.DefaultColor;
                    renderer.material.color = resultColor;
                    renderer.material.SetFloat("_Opacity", state ? 1 : OPACITY_RATE);
                }
            }
        }
    }

    //TODO go to regular expressions.
    private List<Transform> GetChildsWithPattern(params string[] pattern)
    {
        var res = new List<Transform>();
        foreach (Transform layer in LayerRoot.transform)
        {
            if (layer.name.ContainsAny(pattern))
                res.Add(layer);
        }
        return res;
    }

    private Tween TweenGroup(List<Transform> elements, Vector3 delta)
    {
        var seq = DOTween.Sequence();
        foreach (Transform elem in elements)
        {
            seq.Join( elem.DOMove(elem.position + delta, 1f));
        }
        return seq;
    }

}

public static class Extensions
{
    public static bool ContainsAny(this string haystack, params string[] needles)
    {
        foreach (string needle in needles)
        {
            if (haystack.Contains(needle))
                return true;
        }

        return false;
    }
}

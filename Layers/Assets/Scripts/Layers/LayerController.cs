using System;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public static LayerController Instance;

    private float ExpandingCoefficient = 0.25f;

    [Flags]
    enum ExpandedState
    {
        None = 0x0,
        Vertically = 0x1,
        Horizontall = 0x2,
    }

    [SerializeField]
    private LayerExpander blocks;

    [SerializeField]
    private LayerExpander planes;

    [SerializeField]
    private StratigraphyView stratigraphyView; 

    private ExpandedState expandedState;

    private void Awake()
    {
        Instance = this;
        stratigraphyView.AgeSelection += AgeFilter;
    }

    public void ExpandAll()
    {
        if (this.blocks.IsExpanding || this.planes.IsExpanding)
            return;

        if ( expandedState == (ExpandedState.Horizontall | ExpandedState.Vertically))
        {
            blocks.ExpandAll(-ExpandingCoefficient);
            planes.ExpandAll(-ExpandingCoefficient);
            expandedState = ExpandedState.None;
            return;
        }
        if(expandedState == ExpandedState.None)
        {
            blocks.ExpandAll(ExpandingCoefficient);
            planes.ExpandAll(ExpandingCoefficient);
            expandedState = ExpandedState.Horizontall | ExpandedState.Vertically;
            return;
        }
        if( (expandedState & ExpandedState.Horizontall) == ExpandedState.Horizontall)
        {
            blocks.ExpandVert(ExpandingCoefficient);
            planes.ExpandVert(ExpandingCoefficient);
            expandedState |= ExpandedState.Vertically;
        }
        if ((expandedState & ExpandedState.Vertically) == ExpandedState.Vertically)
        {
            blocks.ExpandHoriz(ExpandingCoefficient);
            planes.ExpandHoriz(ExpandingCoefficient);
            expandedState |= ExpandedState.Horizontall;
        }
    }
    public void ExpandHorizontally()
    {
        if (this.blocks.IsExpanding || this.planes.IsExpanding)
            return;

        bool isExpandedH = (expandedState & ExpandedState.Horizontall) == ExpandedState.Horizontall;
        blocks.ExpandHoriz(isExpandedH ? -ExpandingCoefficient : ExpandingCoefficient);
        planes.ExpandHoriz(isExpandedH ? -ExpandingCoefficient : ExpandingCoefficient);

        if (isExpandedH)
            expandedState ^= ExpandedState.Horizontall;
        else
            expandedState |= ExpandedState.Horizontall;
    }

    public void ExpandVertically()
    {
        if (this.blocks.IsExpanding || this.planes.IsExpanding)
            return;

        bool isExpandedV = (expandedState & ExpandedState.Vertically) == ExpandedState.Vertically;
        blocks.ExpandVert(isExpandedV ? -ExpandingCoefficient : ExpandingCoefficient);
        planes.ExpandVert(isExpandedV ? -ExpandingCoefficient : ExpandingCoefficient);

        if (isExpandedV)
            expandedState ^= ExpandedState.Vertically;
        else
            expandedState |= ExpandedState.Vertically;

    }

    public void AgeFilter(int age )
    {
        planes.AgeFilter(age);
        blocks.AgeFilter(age);
    }

    public void SwitchBlock()
    {
        planes.gameObject.SetActive(false);
        blocks.gameObject.SetActive(true);
    }

    public void SwitchPlane()
    {
        planes.gameObject.SetActive(true);
        blocks.gameObject.SetActive(false);
    }
}

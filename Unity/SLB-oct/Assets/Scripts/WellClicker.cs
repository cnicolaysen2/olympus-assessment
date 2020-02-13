using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellClicker : MonoBehaviour
{
    private Camera Cam;

    private Ray R;
    private RaycastHit[] RResult = new RaycastHit[5];

    private void Awake()
    {
        Cam = Camera.main;
    }

    Well prevWell;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            R = Cam.ScreenPointToRay(Input.mousePosition);

            int rCount = Physics.RaycastNonAlloc(R, RResult, 1000, 1 << LayerMask.NameToLayer("Well"));

            if (rCount != 0)
            {
                JSBridge.WellClicked(RResult[0].collider.gameObject);
                var well = RResult[0].collider.GetComponentInParent<Well>();
                if (well != null)
                {
                    well.SetNewColor(Color.cyan);
                    if (prevWell != null)
                    {
                        prevWell.SetDefault();
                    }
                    prevWell = well;
                }
            }
            else if (prevWell != null) prevWell.SetDefault();
        }
    }
}

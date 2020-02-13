using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pane))]
public class ColorGroupSelector : MonoBehaviour
{
    private Pane Pane;

    private Vector3 PreviousMousePosition;

    private RaycastHit[] HitResults = new RaycastHit[5];

    private Color SelectionColor = new Color32(82, 165, 0, 255);
    private Color PrimaryColor = new Color32(165, 206, 165, 255);
    private Color SecondaryColor = new Color32(230, 230, 91, 255);

    private ColorDependancy ColorDependancy = new ColorDependancy();
    private const float OPACITY_RATE = 0.3f;

    private void Awake()
    {
        Pane = GetComponent<Pane>();
    }

    private void Update()
    {
        
        var stratRect = new Rect(0.8f, 0, 0.2f, 1);

        var mousePos = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, Input.mousePosition.z);
        if (stratRect.Contains(mousePos))
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            PreviousMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) 
            && (PreviousMousePosition - Input.mousePosition).magnitude < 2
            && Pane.PaneCamera.rect.Contains(mousePos))
        {
            int hitCount = Physics.RaycastNonAlloc(Pane.PaneCamera.ScreenPointToRay(Input.mousePosition), HitResults, 150);

            ColorGroup.ResetSelection();

            if (hitCount > 0)
            {
                float distance = float.MaxValue;
                float tempDistance;

                GameObject nearest = null;

                for (int i = 0; i < hitCount; i++)
                {
                    tempDistance = (HitResults[i].point - Pane.PaneCamera.transform.position).magnitude;

                    if (tempDistance < distance)
                    {
                        distance = tempDistance;
                        nearest = HitResults[i].collider.gameObject;
                    }
                }

                if (nearest != null)
                {
                    ColorGroup cg = nearest.GetComponent<ColorGroup>();

                    if (cg != null)
                    {
                        cg.SetGroupColor(SelectionColor);

                        foreach (int groupIndex in ColorDependancy.GetPrimaryGroupIndex(cg.GroupIndex))
                        {
                            ColorGroup.SetGroupColor(groupIndex, PrimaryColor);
                        }

                        foreach (int groupIndex in ColorDependancy.GetSecondaryGroupIndex(cg.GroupIndex))
                        {
                            ColorGroup.SetGroupColor(groupIndex, SecondaryColor);
                        }

                        foreach (int groupIndex in ColorDependancy.GetUnusedGroups(cg.GroupIndex))
                        {
                            ColorGroup.SetGroupOpacity(groupIndex, OPACITY_RATE);
                        }
                    }
                }
            }
        }        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaneRoot : MonoBehaviour
{
    public List<GameObject> PanesPrefabs;

    [Header("Dynamic")]
    public List<Pane> InstantiatedPanes = new List<Pane>();

    private void Awake()
    {
        if(PanesPrefabs != null)
        {
            Pane pane;

            foreach (GameObject panePrefab in PanesPrefabs)
            {
                if (panePrefab == null) continue;

                pane = Instantiate(panePrefab).GetComponent<Pane>();

                if (pane != null)
                {
                    pane.SetParent(transform, false);
                    InstantiatedPanes.Add(pane);
                }
            }
        }
    }
}

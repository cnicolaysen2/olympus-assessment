using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour, IControlPanel
{
    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }

    public void SetName(string name)
    {
        gameObject.name = name;
    }

    public void SetParent(Transform parent, bool worldPositionStays)
    {
        gameObject.transform.SetParent(parent, worldPositionStays);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControlPanel
{
    void SetName(string name);
    void SetParent(Transform parent, bool worldPositionStays);
    bool IsActive { get; set; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineChartView : MonoBehaviour
{

    [SerializeField]
    private  Sprite[] lineChartStates;


    [SerializeField]
    private SpriteRenderer renderer;

    private int currentstate = 0;

    // Start is called before the first frame update
    void Start()
    {
        ColorGroup.GroupColorUpdated += OnColorGroupChange;
        ColorGroup._ResetSelection += OnResetSelection;
        renderer.sprite = lineChartStates[currentstate % lineChartStates.Length];
    }

    private void OnResetSelection()
    {
        Unselect();
    }

    private void OnColorGroupChange(int arg1, Color arg2)
    {
        Select();
    }

    public void Select()
    {
        renderer.sprite = lineChartStates[1];
    }

    public void Unselect()
    {
        renderer.sprite = lineChartStates[0];
    }
}

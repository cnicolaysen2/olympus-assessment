using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayerUIControl : MonoBehaviour
{
    public Text Name;

    public Toggle Toggle;

    public Text TopText;
    public Text BottomText;

    public Slider TopSlider;
    public Text TopSliderText;

    public Slider BottomSlider;
    public Text BottomSliderText;

    [Space]
    public Layers Layer;
    
    public float Top;
    public float Bottom;

    private void Awake()
    {
        if (Application.isEditor)
        {
            Name.text = Layer.ToString();

            TopText.text = Top.ToString();
            BottomText.text = Bottom.ToString();

            Toggle.onValueChanged.AddListener(SetLayerState);

            TopSlider.onValueChanged.AddListener(TopValueChanged);
            BottomSlider.onValueChanged.AddListener(BottomValueChanged);

            TopValueChanged(1);
            BottomValueChanged(0);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void BottomValueChanged(float value)
    {
        BottomSliderText.text = (Bottom + value * (Top - Bottom)).ToString("#######.####");

        if (value > TopSlider.value)
        {
            TopSlider.value = value;
        }

        LayerShaderController.SetBottom(value, Layer);
    }

    public void TopValueChanged(float value)
    {
        TopSliderText.text = (Top - (1 - value) * (Top - Bottom)).ToString("#######.####");

        if (value < BottomSlider.value)
        {
            BottomSlider.value = value;
        }

        LayerShaderController.SetTop(value, Layer);
    }

    public void SetLayerState(bool condition)
    {
        if (condition)
        {
            LayerShaderController.EnableLayer(Layer);
        }
        else
        {
            LayerShaderController.DisableLayer(Layer);
        }
    }
}

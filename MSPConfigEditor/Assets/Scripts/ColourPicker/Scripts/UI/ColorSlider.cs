using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays one of the color values of aColorPicker
/// </summary>
[RequireComponent(typeof(Slider))]
public class ColorSlider : MonoBehaviour
{
    public ColorPicker hsvpicker;

    public ColorValues type;
    
    private Slider slider;    

    private bool ignoreCallback;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        hsvpicker.onValueChanged.AddListener(ColorChanged);
        hsvpicker.onHSVChanged.AddListener(HSVChanged);
        slider.onValueChanged.AddListener(SliderChanged);
        ColorChanged(hsvpicker.CurrentColor);
        HSVChanged(hsvpicker.H, hsvpicker.S, hsvpicker.V);
    }

    private void OnDestroy()
    {
        hsvpicker.onValueChanged.RemoveListener(ColorChanged);
        hsvpicker.onHSVChanged.RemoveListener(HSVChanged);
        slider.onValueChanged.RemoveListener(SliderChanged);
    }

    private void ColorChanged(Color newColor)
    {
        ignoreCallback = true;
        switch (type)
        {
            case ColorValues.R:
                slider.normalizedValue = newColor.r;
                break;
            case ColorValues.G:
                slider.normalizedValue = newColor.g;
                break;
            case ColorValues.B:
                slider.normalizedValue = newColor.b;
                break;
            case ColorValues.A:
                slider.normalizedValue = newColor.a;
                break;
            default:
                break;
        }
        ignoreCallback = false;
    }

    private void HSVChanged(float hue, float saturation, float value)
    {
        ignoreCallback = true;
        switch (type)
        {
            case ColorValues.Hue:
                slider.normalizedValue = hue;
                break;
            case ColorValues.Saturation:
                slider.normalizedValue = saturation;
                break;
            case ColorValues.Value:
                slider.normalizedValue = value;
                break;
            default:
                break;
        }
        ignoreCallback = false;
    }

    private void SliderChanged(float newValue)
    {
        if (ignoreCallback)
            return;

        newValue = slider.normalizedValue;
        hsvpicker.AssignColor(type, newValue);
    }   
}

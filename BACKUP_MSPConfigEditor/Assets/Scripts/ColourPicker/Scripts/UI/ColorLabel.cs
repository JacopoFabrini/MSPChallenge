using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CustomInputField))]
public class ColorLabel : MonoBehaviour
{
    public ColorPicker picker;

    public ColorValues type;

    public string prefix = "R: ";
    public float minValue = 0;
    public float maxValue = 255;

    public int precision = 0;

    private CustomInputField label;
    bool ignoreCallback;

    private void Awake()
    {
        label = GetComponent<CustomInputField>();
        picker.onValueChanged.AddListener(ColorChanged);
        picker.onHSVChanged.AddListener(HSVChanged);
        label.onEndEdit.AddListener(OnInputFieldChanged);
        UpdateValue();
    }

    private void OnDestroy()
    {
        if (picker != null)
        {
            picker.onValueChanged.RemoveListener(ColorChanged);
            picker.onHSVChanged.RemoveListener(HSVChanged);
            label.onEndEdit.RemoveListener(OnInputFieldChanged);
        }
    }

    private void ColorChanged(Color color)
    {
        UpdateValue();
    }

    private void HSVChanged(float hue, float sateration, float value)
    {
        UpdateValue();
    }

    private void UpdateValue()
    {
        ignoreCallback = true;
        if (picker == null)
        {
            label.text = prefix + "-";
        }
        else
        {
            float value = minValue + (picker.GetValue(type) * (maxValue - minValue));

            label.text = prefix + ConvertToDisplayString(value);
        }
        ignoreCallback = false;
    }

    private string ConvertToDisplayString(float value)
    {
        if (precision > 0)
            return value.ToString("f " + precision);
        else
            return Mathf.FloorToInt(value).ToString();
    }

    private void OnInputFieldChanged(string newValue)
    {
        if (ignoreCallback)
            return;

        float newColorValue = 0;
        if (float.TryParse(newValue, out newColorValue))
        {
            newColorValue = Mathf.Clamp(newColorValue, minValue, maxValue);
            newColorValue = (newColorValue - minValue) / (maxValue - minValue);
            picker.AssignColor(type, newColorValue);
        }
        else
        {
            UpdateValue();
            return;
        }
    }
}

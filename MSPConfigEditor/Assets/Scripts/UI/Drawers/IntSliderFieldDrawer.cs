using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntSliderFieldDrawer : InputFieldDrawer<IntSliderFieldDrawerAttribute, FieldData>
{
    [SerializeField]
    Slider m_slider;
    [SerializeField]
    TextMeshProUGUI m_minText;
    [SerializeField]
    TextMeshProUGUI m_maxText;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_slider.onValueChanged.AddListener(s => { if (!m_ignoreFieldChange) SliderCallback(s); });
    }

    public override void SetToFieldData(FieldData a_fieldData)
    {
        base.SetToFieldData(a_fieldData);
        m_ignoreFieldChange = true;
        m_minText.text = m_attribute.Min.ToString();
        m_maxText.text = m_attribute.Max.ToString();
        m_slider.wholeNumbers = true;
        m_slider.minValue = m_attribute.Min;
        m_slider.maxValue = m_attribute.Max;
        m_ignoreFieldChange = false;
    }

    protected override void InputFieldCallback(string a_newValue)
    {
        if (String.IsNullOrEmpty(a_newValue))
        {
            m_fieldData?.SetValue(m_attribute.Min);
        }
        else
        {
            try
            {
                int newValue = Math.Min(Math.Max(m_attribute.Min, int.Parse(a_newValue, DrawerManager.CultureInfo)), m_attribute.Max);
                m_fieldData?.SetValue(newValue, true);
            }
            catch (Exception e)
            {
                Debug.Log("Exception occured when parsing int input: " + e);
                m_fieldData?.SetValue(m_attribute.Min);
            }
        }
    }

    protected virtual void SliderCallback(float a_newValue)
    {
        m_fieldData?.SetValue((int)a_newValue);
    }

    public override void UpdateDisplayedValue(object a_value)
    {
        base.UpdateDisplayedValue(a_value);
        m_ignoreFieldChange = true;
        if (a_value == null)
        {
            m_slider.interactable = false;
        }
        else
        {
            m_slider.interactable = true;
            m_slider.value = (int)a_value;
        }
        m_ignoreFieldChange = false;
    }

    protected override string GetDisplayText(object a_value)
    {
        return ((int)a_value).ToString(DrawerManager.CultureInfo);
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<IntSliderFieldDrawer>(this);
    }
}

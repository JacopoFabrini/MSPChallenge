using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourFieldDrawer : FieldDrawer<ColourFieldDrawerAttribute, FieldData>
{
    [SerializeField]
    protected CustomButton m_colourSelectButton;
    [SerializeField]
    protected Image m_colourImage;
    [SerializeField]
    protected GameObject m_nullIndicator;

    protected override void Initialise()
    {
        if (m_initialised)
            return;
        base.Initialise();

        m_colourSelectButton.onClick.AddListener(OpenColourPicker);
        m_colourSelectButton.m_onRightClick.AddListener(OnRightClicked);
    }

    public override void UpdateDisplayedValue(object a_value)
    {
        base.UpdateDisplayedValue(a_value);
        if (a_value == null)
        {
            m_nullIndicator.SetActive(true);
            m_colourSelectButton.interactable = false;
            m_colourImage.color = Color.white;
        }
        else
        {
            m_nullIndicator.SetActive(false);
            m_colourSelectButton.interactable = true;
            m_colourImage.color = (Color)a_value;
        }
    }

    protected override void ReleaseObject()
    {
        DrawerManager.Instance.DrawerPool.ReleaseObject<ColourFieldDrawer>(this);
        DrawerManager.Instance.ColourPicker.Close();
    }

    void OpenColourPicker()
    {
        DrawerManager.Instance.ColourPicker.SetToDrawer(this);
        DrawerManager.Instance.ColourPicker.SetColourAndCallback(m_colourImage.color, ColourPicked);
    }

    void ColourPicked(Color a_newColour)
    {
        m_fieldData?.SetValue(a_newColour);
    }
}

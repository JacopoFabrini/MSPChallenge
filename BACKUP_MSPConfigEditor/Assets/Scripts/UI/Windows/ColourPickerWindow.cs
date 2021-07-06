using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourPickerWindow : DrawerAlignedWindow
{
    [SerializeField]
    ColorPicker m_colourPicker;

    [SerializeField]
    Image m_oldColourImage;

    public delegate void ColourCallback(Color a_newColour);
    ColourCallback m_acceptCallback;

    public override void SetToDrawer(AbstractFieldDrawer a_drawer)
    {
        if (!m_initialised)
            Initialise();
        if (a_drawer == null)
            return;

        base.SetToDrawer(a_drawer);
        PositionWindowImmediate(Input.mousePosition);
    }

    public void SetColourAndCallback(Color a_colour, ColourCallback a_callback)
    {
        m_colourPicker.CurrentColor = a_colour;
        m_oldColourImage.color = a_colour;
        m_acceptCallback = a_callback;
    }

    public void AcceptChange()
    {
        m_acceptCallback?.Invoke(m_colourPicker.CurrentColor);
        Close();
    }

    public override void Close()
    {
        base.Close();
        m_acceptCallback = null;
    }
}

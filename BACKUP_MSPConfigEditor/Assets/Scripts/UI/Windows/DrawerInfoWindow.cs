using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawerInfoWindow : DrawerAlignedWindow
{
    [SerializeField]
    TextMeshProUGUI m_text;
    
    public override void SetToDrawer(AbstractFieldDrawer a_drawer)
    {
        if (!m_initialised)
            Initialise();
        if (a_drawer == null)
            return;

        m_text.text = a_drawer.InfoText;
        base.SetToDrawer(a_drawer);
        PositionWindowImmediate(a_drawer);
    }
}


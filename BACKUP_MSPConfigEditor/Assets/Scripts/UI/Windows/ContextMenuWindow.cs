using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuWindow : DrawerAlignedWindow
{
    [SerializeField]
    GameObject m_buttonPrefab;

    [SerializeField]
    Transform m_childContainer;

    GameObjectPool<UIContextMenuButton> m_objectPool;
    AbstractFieldDrawer m_drawer;

    protected override void Initialise()
    {
        base.Initialise();
        m_objectPool = new GameObjectPool<UIContextMenuButton>(m_buttonPrefab, m_childContainer);
    }

    public override void SetToDrawer(AbstractFieldDrawer a_drawer)
    {
        if (!m_initialised)
            Initialise();
        if (a_drawer == null)
            return;

        m_drawer = a_drawer;

        //Create buttons
        m_objectPool.ResetIndex();
        foreach (ContextMenuButton button in a_drawer.GetContextMenuButtons())
            m_objectPool.GetNext().SetToButton(button, this);
        m_objectPool.DisableUnused();

        base.SetToDrawer(a_drawer);
        PositionWindowImmediate(Input.mousePosition);
        a_drawer.LockRimColour(Color.white);
    }

    public override void Close()
    {
        base.Close();
        m_drawer?.UnlockRimColour();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ContextMenuSprite { Null, Instantiate, Info, Copy, Paste }

public class ContextMenuButton
{
    [SerializeField]
    Sprite m_iconSprite;
    [SerializeField]
    string m_name;
    [SerializeField]
    Action m_callback;

    public Sprite IconSprite { get => m_iconSprite;  }
    public string Name { get => m_name;  }
    public Action Callback { get => m_callback; }

    public ContextMenuButton(string a_name, ContextMenuSprite a_sprite, Action a_callback)
    {
        m_name = a_name;
        m_callback = a_callback;
        SetSprite(a_sprite);
    }

    public void SetSprite(ContextMenuSprite a_newSprite)
    {
        m_iconSprite = DrawerManager.Instance.DrawerOptions.GetContextMenuSprite(a_newSprite);
    }
}

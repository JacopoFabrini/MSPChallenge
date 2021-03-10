using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIContextMenuButton : MonoBehaviour
{
    [SerializeField]
    Button m_button;
    [SerializeField]
    Image m_iconImage;
    [SerializeField]
    TextMeshProUGUI m_nameText;

    ContextMenuWindow m_menu;
    Action m_callback;

    void Awake()
    {
        m_button.onClick.AddListener(ButtonClicked);
    }

    public void SetToButton(ContextMenuButton a_button, ContextMenuWindow a_menu)
    {
        m_menu = a_menu;
        m_iconImage.sprite = a_button.IconSprite;
        m_nameText.text = a_button.Name;

        m_callback = a_button.Callback;
    }

    public void ButtonClicked()
    {
        m_callback();
        m_menu.Close();
    }
}


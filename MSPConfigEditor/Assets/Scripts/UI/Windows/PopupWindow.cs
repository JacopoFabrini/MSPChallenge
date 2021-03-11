using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupWindow : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_message;
    [SerializeField]
    TextMeshProUGUI m_title;

    [SerializeField]
    Button m_confirmationButton;
    [SerializeField]
    Button m_cancelButton;

    [SerializeField]
    TextMeshProUGUI m_confirmationButtonText;
    [SerializeField]
    TextMeshProUGUI m_cancelButtonText;

    Action m_confirmAction;
    Action m_cancelAction;

    private void Awake()
    {
        m_confirmationButton.onClick.AddListener(ConfirmButtonPressed);
        m_cancelButton.onClick.AddListener(CancelButtonPressed);
    }

    public void OpenConfirmationWindow(string a_title, string a_message, string a_confirmationText, string a_cancelText, Action a_confirmAction, Action a_cancelAction)
    {
        //If a window is opened while another was open, cancel it first
        if(gameObject.activeSelf)
            m_cancelAction?.Invoke();

        m_message.text = a_message;
        m_title.text = a_title;
        m_confirmationButtonText.text = a_confirmationText;
        m_cancelButtonText.text = a_cancelText;
        m_confirmAction = a_confirmAction;
        m_cancelAction = a_cancelAction;
        m_cancelButton.gameObject.SetActive(true);
        m_confirmationButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OpenNotificationWindow(string a_title, string a_message, string a_confirmationText, Action a_confirmAction)
    {
        //If a window is opened while another was open, cancel it first
        if (gameObject.activeSelf)
            m_cancelAction?.Invoke();

        m_message.text = a_message;
        m_title.text = a_title;
        m_confirmationButtonText.text = a_confirmationText;
        m_confirmAction = a_confirmAction;
        m_cancelButton.gameObject.SetActive(false);
        m_confirmationButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void OpenMessage(string a_title, string a_message)
    {
        //If a window is opened while another was open, cancel it first
        if (gameObject.activeSelf)
            m_cancelAction?.Invoke();

        m_cancelAction = null;
        m_message.text = a_message;
        m_title.text = a_title;
        m_confirmationButton.gameObject.SetActive(false);
        m_cancelButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    void ConfirmButtonPressed()
    {
        m_confirmAction?.Invoke();
        CloseWindow();
    }

    void CancelButtonPressed()
    {
        m_cancelAction?.Invoke();
        CloseWindow();
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
		DrawerManager.Instance.PopupWindowManager.AddWindowToPool(this);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindowManager : MonoBehaviour
{
	[SerializeField] private GameObject m_popupPrefab;

	private Stack<PopupWindow> m_unusedPopupWindows;

	void Awake()
	{
		m_unusedPopupWindows = new Stack<PopupWindow>();
	}

	public PopupWindow OpenConfirmationWindow(string a_title, string a_message, string a_confirmationText, string a_cancelText, Action a_confirmAction, Action a_cancelAction)
	{
		PopupWindow window = GetWindow();
		window.OpenConfirmationWindow(a_title, a_message, a_confirmationText, a_cancelText, a_confirmAction, a_cancelAction);
		return window;
	}

	public PopupWindow OpenNotificationWindow(string a_title, string a_message, string a_confirmationText, Action a_confirmAction)
	{
		PopupWindow window = GetWindow();
		window.OpenNotificationWindow(a_title, a_message, a_confirmationText, a_confirmAction);
		return window;
	}

	public PopupWindow OpenMessage(string a_title, string a_message)
	{
		PopupWindow window = GetWindow();
		window.OpenMessage(a_title, a_message);
		return window;
	}

	PopupWindow GetWindow()
	{
		if (m_unusedPopupWindows.Count > 0)
		{
			PopupWindow result = m_unusedPopupWindows.Pop();
			result.gameObject.SetActive(true);
			result.transform.SetAsLastSibling();
			return result;
		}
		else
			return GameObject.Instantiate(m_popupPrefab, transform).GetComponent<PopupWindow>();
	}

	public void AddWindowToPool(PopupWindow a_window)
	{
		m_unusedPopupWindows.Push(a_window);
	}
}

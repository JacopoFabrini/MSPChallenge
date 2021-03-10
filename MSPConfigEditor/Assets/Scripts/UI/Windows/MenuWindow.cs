using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;

class MenuWindow : MonoBehaviour
{
    [SerializeField]
    CustomButton m_loadFileButton;
    [SerializeField]
    CustomButton m_saveButton;
    [SerializeField]
    CustomButton m_saveAsButton;
    [SerializeField]
    CustomButton m_loadFromServerButton;
    [SerializeField]
    CustomButton m_saveToServerButton;
    [SerializeField]
    DataManager m_dataManager;
    [SerializeField]
    Toggle m_menuWindowToggle;
	[SerializeField]
	CustomButton m_exitButton;

	private void Awake()
    {
        m_loadFileButton.onClick.AddListener(LoadFile);
        m_saveButton.onClick.AddListener(Save);
        m_saveAsButton.onClick.AddListener(SaveAs);
        m_loadFromServerButton?.onClick.AddListener(LoadFromServer);
        m_saveToServerButton?.onClick.AddListener(SaveToServer);
		m_exitButton.onClick.AddListener(Application.Quit);
	}

    public void LoadFile()
    {
        m_menuWindowToggle.isOn = false;
        if (m_dataManager.UnsavedChanges)
        {
            DrawerManager.Instance.PopupWindowManager.OpenConfirmationWindow(
                "Unsaved changes",
                "Loading a file will discard all unsaved changes. Are you sure you want to continue?",
                "Continue",
                "Cancel",
                LoadFileConfirmed,
                null);
        }
        else
            LoadFileConfirmed();
    }

    void LoadFileConfirmed()
    {
        string path = FileBrowser.OpenSingleFile("json");
        if (!string.IsNullOrEmpty(path))
        {
            m_dataManager.LoadFile(path);
        }
    }

    public void Save()
    {
        m_menuWindowToggle.isOn = false;
        if (DrawerManager.Instance.ConstraintWindow.HasFatalErrors)
        {
            DrawerManager.Instance.PopupWindowManager.OpenNotificationWindow(
                    "Unresolved fatal errors",
                    "The config has unresolved fatal errors that prevent saving. Resolve all fatal errors (found in the issue window) and try again.",
                    "Continue",
                    null);
        }
        else if (DrawerManager.Instance.ConstraintWindow.HasErrors)
        {
            DrawerManager.Instance.PopupWindowManager.OpenConfirmationWindow(
                    "Unresolved errors",
                    "The config has unresolved errors, these will prevent the file from being used but it can still be reopened for editing later. Are you sure you want to save?",
                    "Continue",
                    "Cancel",
                    () => m_dataManager.Save(),
                    null);
        }
        else
            m_dataManager.Save();
    }

    public void SaveAs()
    {
        m_menuWindowToggle.isOn = false;
        if (DrawerManager.Instance.ConstraintWindow.HasFatalErrors)
        {
            DrawerManager.Instance.PopupWindowManager.OpenNotificationWindow(
                    "Unresolved fatal errors",
                    "The config has unresolved fatal errors that prevent saving. Resolve all fatal errors (found in the issue window) and try again.",
                    "Continue",
                    null);
        }
        else if (DrawerManager.Instance.ConstraintWindow.HasErrors)
        {
            DrawerManager.Instance.PopupWindowManager.OpenConfirmationWindow(
                    "Unresolved errors",
                    "The config has unresolved errors, these will prevent the file from being used but it can still be reopened for editing later. Are you sure you want to save?",
                    "Continue",
                    "Cancel",
                    SaveAsConfirmed,
                    null);
        }
        else
        {
            SaveAsConfirmed();
        }
    }

    void SaveAsConfirmed()
    {
        string path = FileBrowser.SaveFile("NewConfigFile", "json");
        if (!string.IsNullOrEmpty(path))
        {
            if (File.Exists(path))
                DrawerManager.Instance.PopupWindowManager.OpenConfirmationWindow(
                    "Overwrite file",
                    "A file with this name already exists in the target directory. Are you sure you want to overwrite it?",
                    "Overwrite",
                    "Cancel",
                    () => { m_dataManager.SaveAs(path); },
                    null);
            else
                m_dataManager.SaveAs(path);
        }
    }

    public void LoadFromServer()
    { }

    public void SaveToServer()
    { }
}


using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;

public class DataManager : MonoBehaviour
{
    static DataManager m_instance;
    public static DataManager Instance
    {
        get
        {
            if (m_instance == null)
                Debug.LogError("DataManager instance needed before initialisation.");
            return m_instance;
        }
    }

    DataModel m_data;
    bool m_unsavedChanges;
    string m_openedPath;
    private PopupWindow m_loadingMessageWindow;

    [SerializeField] private SavedChangesIcon m_savedChangesIcon;
    [SerializeField] private TextMeshProUGUI m_openFilePathText;
	[SerializeField] private ConfigEditorSettings m_confidEditorSettings;

	public bool UnsavedChanges  => m_unsavedChanges; 
    public DataModel Data { get => m_data; }

    void Awake()
    {
        m_instance = this;
        Application.wantsToQuit += () =>
        {
            if(UnsavedChanges)
            {
                DrawerManager.Instance.PopupWindowManager.OpenConfirmationWindow(
                                "Unsaved changes",
                                "You have unsaved changes. Are you sure you want to quit the application?",
                                "Quit",
                                "Cancel",
                                ForceQuit,
                                null);
                return false;
            }
            return true;
        };
    }

    public void LoadFile(string a_path)
    {
        StartCoroutine("OpenMessageAndLoad", a_path);
        //DrawerManager.Instance.PopupWindow.OpenMessage("Loading", "Loading the file and preparing the workspace");     
        //Canvas.ForceUpdateCanvases();
        //LoadFileImmediate(a_path);
    }

    IEnumerator OpenMessageAndLoad(string a_path)
    {
	    m_loadingMessageWindow = DrawerManager.Instance.PopupWindowManager.OpenMessage("Loading", "Loading the file and preparing the workspace.");
        Canvas.ForceUpdateCanvases();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        LoadFileImmediate(a_path);
    }

    public void LoadFileImmediate(string a_path)
    { 
        m_openedPath = a_path;
        m_openFilePathText.text = a_path;

		string dataString;
        using (StreamReader stream = new StreamReader(a_path))
        {
            dataString = stream.ReadToEnd();
        }
        StartCoroutine("ParseJSON", dataString);
    }

    public void Save()
    {
        if (!UnsavedChanges)
            return;

        SaveAs(m_openedPath);
    }

    public void SaveAs(string a_path)
    {
        try
        {
            string jsonData = JsonConvert.SerializeObject(new JsonObject(m_data, m_confidEditorSettings), Formatting.Indented);
            using (StreamWriter stream = new StreamWriter(a_path, false))
                stream.Write(jsonData);

            Debug.Log("Data saved successfully");
            m_unsavedChanges = false;
            m_savedChangesIcon.SetSaved(true);
            m_openedPath = a_path;
            m_openFilePathText.text = a_path;
		}
        catch (Exception e)
        {
            Debug.LogError("Failed to save config file: " + e.Message);
        }
    }

    public void LoadFromServer()
    { }

    public void SaveToServer()
    { }

    IEnumerator ParseJSON(string a_json)
    {
        DrawerManager.Instance.ClearRoot();
        m_unsavedChanges = false;
        m_savedChangesIcon.SetSaved(true);
        try
        {
            JsonObject saveObj = JsonConvert.DeserializeObject<JsonObject>(a_json);
            m_data = saveObj.datamodel;
            Debug.Log("Successfully parsed config file");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse config file: " + e.Message);
            DrawerManager.Instance.PopupWindowManager.OpenNotificationWindow(
                    "Invalid file format",
                    "The file could not be loaded because it does not match the expected format. Error message: " + e.Message,
                    "Continue",
                    null);
            m_openedPath = "";
            m_openFilePathText.text = "None";
		}
        yield return null;
        if (m_data != null)
        {
            DrawerManager.Instance.CreateRootDataHolder(m_data);
        }
        m_loadingMessageWindow.CloseWindow();
    }

    public void DataChanged()
    {
        m_unsavedChanges = true;
        m_savedChangesIcon.SetSaved(false);
    }

void ForceQuit()
    {
        m_unsavedChanges = false;
        Application.Quit();
    }
}

[Serializable]
public class JsonObject
{
    public ConfigMetaData metadata;
    public DataModel datamodel;

	[JsonConstructor]
	public JsonObject()
	{ }

	//public JsonObject(DataModel a_dataModel)
	//{
	//	metadata = new ConfigMetaData(JsonConvert.SerializeObject(a_dataModel, Formatting.Indented));
	//	datamodel = a_dataModel;
	//}

	public JsonObject(DataModel a_dataModel, ConfigEditorSettings a_settings)
    {
        metadata = new ConfigMetaData(JsonConvert.SerializeObject(a_dataModel, Formatting.Indented), a_settings.m_minSupportedClientVersion, a_settings.m_maxSupportedClientVersion);
        datamodel = a_dataModel;
    }
}

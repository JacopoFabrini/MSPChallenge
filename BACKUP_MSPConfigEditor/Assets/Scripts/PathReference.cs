using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class PathReference
{
    string[] m_path;
    int m_levelsUp; //-1 is global path


    //Events
    Action<object, DrawerEventData> m_valueChangeCallback;
    Action m_invalidatedCallback;
    Action<object, DrawerEventData> m_validatedCallback;

    bool m_valid = true;
    FieldData m_fieldData;
    FieldData m_subscribedFieldData;
    
    string m_subSelector;
    List<object> m_subSelectorResult;

    public bool Valid { get { return m_valid; } }
    public bool Subselecting { get { return !string.IsNullOrEmpty(m_subSelector); } }
    public FieldData FieldData { get => m_fieldData; }

    public PathReference(string a_path, FieldData a_fieldData, Action<object, DrawerEventData> a_valueChangeCallback, Action a_invalidatedCallback, Action<object, DrawerEventData> a_validatedCallback)
    {
        if (string.IsNullOrEmpty(a_path))
        {
            Debug.Log("Empty path specified.");
            return;
        }
        m_path = a_path.Split('/');
        if (int.TryParse(m_path[0], out m_levelsUp))
        {
            string[] newPath = new string[m_path.Length - 1];
            for (int i = 1; i < m_path.Length; i++)
                newPath[i-1] = m_path[i];
            m_path = newPath;
        }
        else
            m_levelsUp = -1;

        string[] subpath = m_path[m_path.Length - 1].Split('.');
        if (subpath.Length > 2)
        {
            Debug.LogError("Subpath reference specified with invalid subselection: " + m_path[m_path.Length - 1]);
            return;
        }
        else if (subpath.Length == 2)
        {
            m_subSelector = subpath[1];
            m_path[m_path.Length - 1] = subpath[0];
        }

        m_fieldData = a_fieldData;
        m_validatedCallback = a_validatedCallback;
        m_valueChangeCallback = a_valueChangeCallback;
        m_invalidatedCallback = a_invalidatedCallback;
        ReevaluatePath(null, true);
    }

    void ReevaluatePath(DrawerEventData a_eventData, bool a_forceSubscribe = false)
    {
        FieldData target;
        if (GetDrawerAtPathFor(FieldData, out target))
        {
            if (Subselecting)
            {
                if (!m_valid)
                {
                    SetSubscribedFieldData(target);
                    m_subSelectorResult = GetSubFieldData(target, m_subSelector);
                    m_valid = true;
                    m_validatedCallback?.Invoke(m_subSelectorResult, a_eventData);
                }
                else
                {
                    m_subSelectorResult = GetSubFieldData(target, m_subSelector);
                    if (a_forceSubscribe)
                        SetSubscribedFieldData(target);
                    m_valueChangeCallback?.Invoke(m_subSelectorResult, a_eventData);
                }
            }
            else
            {
                object value = target.GetDataObject();
                //if (target.Parent != null)
                //{
                //    object parentObject = target.Parent.GetDataObject();
                //    value = parentObject.GetType().GetField(m_path[m_path.Length - 1]).GetValue(parentObject);
                //}
                if (!m_valid)
                {
                    SetSubscribedFieldData(target);
                    m_valid = true;
                    m_validatedCallback?.Invoke(value, a_eventData);
                }
                else
                {
                    //Subscribed target shouldnt change, so only subscribe if forced
                    if (a_forceSubscribe)
                        SetSubscribedFieldData(target);
                    m_valueChangeCallback?.Invoke(value, a_eventData);
                }
            }          
        }
        else if (target != null)
        {
            if (m_valid)
                m_invalidatedCallback?.Invoke();          
            SetSubscribedFieldData(target);
            m_valid = false;
        }
        else
        {
            Debug.LogError("Invalid path reference specified");
        }
    }

    void SetSubscribedFieldData(FieldData a_newFieldData)
    {
        if (m_subscribedFieldData != null)
            m_subscribedFieldData.UnSubscribeEventHandler(SubscribedFieldDataUpdate);
        m_subscribedFieldData = a_newFieldData;
        m_subscribedFieldData.SubscribeEventHandler(SubscribedFieldDataUpdate);
    }

    void SubscribedFieldDataUpdate(DrawerEventData a_eventData)
    {
        if ((a_eventData.EventType == DrawerEventType.Destroyed && !a_eventData.PassedFromChild) || !m_valid)
            ReevaluatePath(a_eventData);
        else if (Subselecting)
        {
            m_subSelectorResult = GetSubFieldData(m_subscribedFieldData, m_subSelector);
            m_valueChangeCallback?.Invoke(m_subSelectorResult, a_eventData);
        }
        else
            m_valueChangeCallback?.Invoke(a_eventData.TargetObject, a_eventData);
    }

    public void RemoveAllReferences()
    {
        m_validatedCallback = null;
        m_valueChangeCallback = null;
        m_invalidatedCallback = null;
        m_fieldData = null;
        m_subscribedFieldData?.UnSubscribeEventHandler(SubscribedFieldDataUpdate);
        m_subscribedFieldData = null;
    }

    public object GetDataAtPathFor(FieldData a_fieldData)
    {
        if(m_levelsUp < 0)
            return GetObject(m_path, DataManager.Instance.Data);
        else
            return GetObject(m_path, a_fieldData, m_levelsUp);
    }

    public bool GetDrawerAtPathFor(FieldData a_fieldData, out FieldData a_result)
    {
        if (m_levelsUp < 0)
            return GetFieldData(m_path, DrawerManager.Instance.FieldDataRoot, out a_result);
        else
            return GetFieldData(m_path, a_fieldData, m_levelsUp, out a_result);
    }

    public static object GetLocalObject(string a_localPath, FieldData a_root)
    {
        //If the first part of the path is an int, it indicates the amount of levels to move up
        string[] path = a_localPath.Split('/');
        int levelsUp = 0;
        if (int.TryParse(path[0], out levelsUp))
        {
            FieldData currentObject;
            if (levelsUp > 0)
            {
                currentObject = a_root.Parent;
                for (int i = 1; i < levelsUp; i++)
                {
                    if (currentObject == null || currentObject.Parent == null)
                    {
                        Debug.LogError("Trying to get non-existent parent object in local reference path: " + a_localPath);
                        return null;
                    }
                    currentObject = currentObject.Parent;
                }
            }
            else
                currentObject = a_root;

            if (currentObject == null)
            {
                Debug.LogError("Trying to get non-existent parent object in local reference path: " + a_localPath);
                return null;
            }

            string[] newPath = new string[path.Length - 1];
            for (int i = 1; i < path.Length; i++)
                newPath[i-1] = path[i];
            return GetObject(newPath, currentObject.GetDataObject());
        }
        else
        {
            return GetObject(path, a_root.Parent);
        }
    }

    public static object GetGlobalObject(string a_globalPath)
    {
        return GetObject(a_globalPath.Split('/'), DataManager.Instance.Data);
    }

    public static object GetObject(string[] a_localPath, FieldData a_root, int a_levelsUp)
    {
        FieldData currentObject;
        if (a_levelsUp > 0)
        {
            currentObject = a_root.Parent;
            for (int i = 1; i < a_levelsUp; i++)
            {
                if (currentObject == null || currentObject.Parent == null)
                {
                    Debug.LogError("Trying to get non-existent parent object in local reference path: " + a_localPath);
                    return null;
                }
                currentObject = currentObject.Parent;
            }
        }
        else
            currentObject = a_root;

        if (currentObject == null)
        {
            Debug.LogError("Trying to get non-existent parent object in local reference path: " + a_localPath);
            return null;
        }

        return GetObject(a_localPath, currentObject.GetDataObject());
    }

    public static object GetObject(string[] a_localPath, object a_root)
    {
        object currentObject = a_root;
        for (int i = 0; i < a_localPath.Length; i++)
        {
            if (currentObject == null)
                return null;

            FieldInfo field = currentObject.GetType().GetField(a_localPath[i]);
            if (field == null)
            {
                Debug.LogError("Trying to get object from invalid path: " + a_localPath[i]);
                return null;
            }

            currentObject = field.GetValue(currentObject);
        }
        return currentObject;
    }

    public static bool GetFieldData(string[] a_localPath, FieldData a_root, int a_levelsUp, out FieldData a_result)
    {
        a_result = null;
        FieldData currentObject;
        if (a_levelsUp > 0)
        {
            currentObject = a_root.Parent;
            for (int i = 1; i < a_levelsUp; i++)
            {
                if (currentObject == null || currentObject.Parent == null)
                {
                    Debug.LogError("Trying to get non-existent parent object in local reference path: " + string.Concat(a_localPath, "/"));
                    return false;
                }
                currentObject = currentObject.Parent;
            }
        }
        else
            currentObject = a_root;

        if (currentObject == null)
        {
            Debug.LogError("Trying to get non-existent parent object in local reference path: " + string.Concat(a_localPath, "/"));
            return false;
        }

        return GetFieldData(a_localPath, currentObject, out a_result);
    }

    public static bool GetFieldData(string[] a_localPath, FieldData a_root, out FieldData a_result)
    {
        FieldData currentObject = a_root;
        for (int i = 0; i < a_localPath.Length; i++)
        {
            FieldData child = currentObject.GetChild(a_localPath[i]);
            if (child == null || child.IsBeingDestroyed)
            {
                a_result = currentObject;
                return false;
            }

            currentObject = child;
        }
        a_result = currentObject;
        return true;
    }

    List<object> GetSubFieldData(FieldData a_targetDrawer, string a_subselector)
    {
        List<object> result = new List<object>();
        foreach (FieldData child in a_targetDrawer.GetAllChildren())
        {
            if (!child.IsNull)
            {
                object dataObject = child.GetDataObject();
                result.Add(dataObject.GetType().GetField(m_subSelector).GetValue(dataObject));
            }
        }
        return result;
    }
}


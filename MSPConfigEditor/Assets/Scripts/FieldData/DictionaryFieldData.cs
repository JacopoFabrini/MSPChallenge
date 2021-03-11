using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DictionaryFieldData : ReferenceFieldData
{
    private List<DictionaryEntryFieldData> m_children;
    List<DictionaryData> m_targetDictionaryData;
    IDictionary m_targetDictionary;
    bool m_hasDuplicateKeys;
    Type m_keyType;
    Type m_valueType;
    DuplicateKeyConstraint m_duplicateKeyConstraint;
    float m_keyTovalueSizeRatio = 0.5f;
    bool m_autoIncrement = false;
    int m_autoIncrementMin = 0;
    bool m_autoIncrementFillGaps = false;

	AbstractFieldDrawerAttribute[] m_subDrawerAttributes;
    Dictionary<int, List<IConstraintDefinition>> m_subConstraints;
    
    public override void PostCreateInitialise()
    {
        if (m_postCreateInitialised)
            return;
        base.PostCreateInitialise();
        if (m_children != null)
            foreach (var child in m_children)
                child.PostCreateInitialise();
    }

    protected override void Initialise(int a_depth, FieldData a_parent, object a_target, AbstractFieldDrawerAttribute[] a_attributes, Type a_contentType, Dictionary<int, List<IConstraintDefinition>> a_constraints, AbstractSpacerAttribute[] a_spacerAttributes, AHideIfAttribute a_hideIfAttribute)
    {
        if (a_attributes.Length < 3)
        {
            Debug.LogError("No subdrawers specified for either keys or values: " + a_attributes[0].Name);
            return;
        }

        if (!typeof(IDictionary).IsAssignableFrom(a_contentType))
        {
            Debug.LogError("DictionaryDrawerAttribute assigned to field that is not a dictionary: " + a_contentType);
            return;
        }

        DictionaryFieldDrawerAttribute dictAttr = (DictionaryFieldDrawerAttribute) a_attributes[0];
        m_autoIncrement = dictAttr.AutoIncrement;
        m_autoIncrementMin = dictAttr.AutoIncrementMin;
        m_autoIncrementFillGaps = dictAttr.AutoIncrementFillGaps;

		m_keyType = a_contentType.GetGenericArguments()[0];
        m_valueType = a_contentType.GetGenericArguments()[1];
        m_subDrawerAttributes = new AbstractFieldDrawerAttribute[a_attributes.Length];
        m_subDrawerAttributes[0] = new DictionaryEntryDrawerAttribute();
        for (int i = 1; i < m_subDrawerAttributes.Length; i++)
            m_subDrawerAttributes[i] = a_attributes[i];
        m_subConstraints = a_constraints;
        m_drawerSupportElements = new List<Type> { typeof(ListElementIndexer) };

        base.Initialise(a_depth, a_parent, a_target, a_attributes, a_contentType, a_constraints, a_spacerAttributes, a_hideIfAttribute);
    }
	
    public override IEnumerable<FieldData> GetAllChildren()
    {
        return m_children;
    }

    public override void CreateDrawer(Transform a_drawerParent = null)
    {
        base.CreateDrawer(a_drawerParent);
        if (m_children != null)
            foreach (DictionaryEntryFieldData child in m_children)
                child.CreateDrawer(m_drawer.GetChildContainer());
        m_drawer.ChildIndicesUpdated();
    }

    public override void ReleaseDrawer()
    {
        base.ReleaseDrawer();
        if (m_children != null)
            foreach (DictionaryEntryFieldData child in m_children)
                child.ReleaseDrawer();
    }

    protected override void PreValueInitialise()
    {
        base.PreValueInitialise();
        if (m_constraints == null)
        {
            m_constraints = new List<Constraint>(1);
            m_constraintViolations = new List<bool>(1);
        }
        m_constraintViolations.Add(false);
        m_duplicateKeyConstraint = new DuplicateKeyConstraint(m_constraints.Count);
        m_constraints.Add(m_duplicateKeyConstraint);
    }

    public override FieldData GetChild(string a_index)
    {
        if (m_children == null)
            return null;
        if (int.TryParse(a_index, out int index))
        {
            return m_children[index];
        }
        return null;
    }

    public override FieldData GetChild(int a_index)
    {
        return m_children?[a_index];
    }

    protected override void ChildChanged(DrawerEventData a_eventData)
    {
        if (m_isBeingDestroyed || m_ignoreChildChange)
            return;
        if (!a_eventData.PassedFromChild && a_eventData.EventType == DrawerEventType.ValueChanged)
        {
            if (a_eventData.ChildIndex == 1)
            {
                //Value change, no checks
                m_targetDictionaryData[a_eventData.TargetIndex].Value = a_eventData.ChildObject;
                if (!m_hasDuplicateKeys)
                    m_targetDictionary[m_targetDictionaryData[a_eventData.TargetIndex].Key] = ((DictionaryData)a_eventData.TargetObject).Value;
            }
            else
            {
                //Key changed, check duplicates
                object oldKey = m_targetDictionaryData[a_eventData.TargetIndex].Key;
                bool hadDuplicatesBefore = m_hasDuplicateKeys;
                m_targetDictionaryData[a_eventData.TargetIndex].Key = a_eventData.ChildObject;
                CheckForDuplicateKeys();
                if (!m_hasDuplicateKeys)
                {
                    if (hadDuplicatesBefore)
                    {
                        //Re-do the entire dictionary
                        SetDictionaryToLocalData();
                    }
                    else
                    {
                        //Only re-add the changed element
                        m_targetDictionary.Remove(oldKey);
                        m_targetDictionary.Add(m_targetDictionaryData[a_eventData.TargetIndex].Key, m_targetDictionaryData[a_eventData.TargetIndex].Value);
                    }
                }
            }
        }
        InvokeEvent(a_eventData.EventType, GetDataObject(), Index, a_eventData.TargetIndex, a_eventData.TargetObject, true);
    }

    public override void ChangeChildIndex(int a_oldChildIndex, int a_newChildIndex)
    {
        if(m_children[a_oldChildIndex].Drawer != null)
            m_children[a_oldChildIndex].Drawer.transform.SetSiblingIndex(a_newChildIndex);

		//Move in data object
		var movedData = m_targetDictionaryData[a_oldChildIndex];
        m_targetDictionaryData.RemoveAt(a_oldChildIndex);
        m_targetDictionaryData.Insert(a_newChildIndex, movedData);
        InvokeEvent(DrawerEventType.IndexChanged, GetDataObject(), Index, a_oldChildIndex, movedData, true);
        DataManager.Instance.DataChanged();
    }

    public override void DestroyChild(int a_childIndex)
    {
        DictionaryData removedData = m_targetDictionaryData[a_childIndex];
        object childvalue = m_targetDictionaryData[a_childIndex];
        m_targetDictionaryData.RemoveAt(a_childIndex);
        if (m_hasDuplicateKeys)
        {
            CheckForDuplicateKeys();
            if (!m_hasDuplicateKeys)
                SetDictionaryToLocalData();
        }
        else
            m_targetDictionary.Remove(removedData.Key);
        FieldData destroyedChild = m_children[a_childIndex];
        m_children.RemoveAt(a_childIndex);
        destroyedChild.DestroyFieldData(childvalue);
        ReindexChildrenFrom(a_childIndex);
        DataManager.Instance.DataChanged();
    }

    public void AddNewBaseValueChild()
    {
	    DictionaryFieldDrawerAttribute dictAttr = (DictionaryFieldDrawerAttribute)DrawerAttribute;

		object key;
		if (dictAttr.AutoIncrement && m_keyType == typeof(int))
		{
			if (m_targetDictionary == null || m_targetDictionary.Count == 0)
				key = dictAttr.AutoIncrementMin;
			else if (dictAttr.AutoIncrementFillGaps)
			{
				for (int i = dictAttr.AutoIncrementMin; true; i++)
				{
					if (!m_targetDictionary.Contains(i))
					{
						key = i;
						break;
					}
				}
			}
			else
			{
				List<int> keys = m_targetDictionary.Keys.Cast<int>().ToList();
				keys.Sort();
				key = keys[keys.Count-1];
			}
		}
		else
			key = m_keyType.GetNewBaseObject();
        object value = m_valueType.GetNewBaseObject();
        DictionaryData newDictionaryData = new DictionaryData(key, value);
        int childIndex = m_targetDictionaryData.Count;
        m_targetDictionaryData.Add(newDictionaryData);
        CreateChild(childIndex, newDictionaryData, true, true);

        //Add to target dictionary if there are no duplicates
        if (!m_hasDuplicateKeys)
        {
            CheckForDuplicateKeys();
            if (!m_hasDuplicateKeys)
                m_targetDictionary.Add(key, value);
        }
        InvokeEvent(DrawerEventType.Added, GetDataObject(), Index, childIndex, newDictionaryData);
        DataManager.Instance.DataChanged();
    }

    void CheckForDuplicateKeys()
    {
        bool hadDuplicateKeys = m_hasDuplicateKeys;
        if (m_targetDictionaryData == null)
            m_hasDuplicateKeys = false;
        m_hasDuplicateKeys = m_targetDictionaryData.GroupBy(n => n.Key).Any(c => c.Count() > 1);
        if (m_hasDuplicateKeys != hadDuplicateKeys)
        {
            m_duplicateKeyConstraint.HasDuplicateKey = m_hasDuplicateKeys;
            ConstraintUpdated(m_hasDuplicateKeys, m_duplicateKeyConstraint.ConstraintIndex);
        }
    }

    void SetDictionaryToLocalData()
    {
        m_targetDictionary.Clear();
        foreach (DictionaryData data in m_targetDictionaryData)
            m_targetDictionary.Add(data.Key, data.Value);
    }

    void ReindexChildrenFrom(int a_fromIndex)
    {
        for (int i = a_fromIndex; i < m_children.Count; i++)
        {
            m_children[i].Index = i;
        }
    }

    protected override void CreateChildren()
    {
        m_children = new List<DictionaryEntryFieldData>();
        m_targetDictionaryData = new List<DictionaryData>();
        if (GetDataObject() != null)
        {
            m_targetDictionary = (IDictionary)GetDataObject();
            int i = 0;
            foreach (DictionaryEntry entry in m_targetDictionary)
            {
                DictionaryData newDictionaryData = new DictionaryData(entry.Key, entry.Value);
                m_targetDictionaryData.Add(newDictionaryData);
                CreateChild(i, newDictionaryData, false, m_postCreateInitialised);
                i++;
            }
        }
        CheckForDuplicateKeys();
    }

    private void CreateChild(int a_index, object a_targetData, bool a_createDrawer, bool a_postCreateInitialise)
    {
        DictionaryEntryFieldData newChild = new DictionaryEntryFieldData();
        newChild.SetField(m_depth, this, a_targetData, m_subDrawerAttributes, m_objectType, m_subConstraints, null, null);
        m_children.Add(newChild);
        if(a_postCreateInitialise)
            newChild.PostCreateInitialise();
        newChild.Index = a_index;
        if(a_createDrawer)
            newChild.CreateDrawer(m_drawer.GetChildContainer());
    }

    protected override void ClearChildren()
    {
        if (m_children != null)
            foreach (DictionaryEntryFieldData fieldData in m_children)
                fieldData.DestroyFieldData();
        m_children = null;
    }

    public override object GetValueOfChild(int a_childIndex)
    {
        return m_targetDictionaryData[a_childIndex];
    }

    public void SetKeyToValueSizeRatio(float a_keyRatio)
    {
        m_keyTovalueSizeRatio = a_keyRatio;

        foreach (DictionaryEntryFieldData child in m_children)
        {
            child.SetKeyToValueSizeRatio(a_keyRatio);
        }
    }

    public override void SetExpanded(bool a_expanded)
    {
        if (m_drawer != null)
            ((DictionaryFieldDrawer)m_drawer).SetExpanded(a_expanded);
    }
}

class DictionaryData
{
    object m_key;
    object m_value;

    public DictionaryData(object a_key, object a_value)
    {
        m_key = a_key;
        m_value = a_value;
    }

    public object Key { get => m_key; set => m_key = value; }
    public object Value { get => m_value; set => m_value = value; }
}


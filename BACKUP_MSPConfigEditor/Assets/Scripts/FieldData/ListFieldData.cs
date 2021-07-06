using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using UnityEngine;

public class ListFieldData : ReferenceFieldData
{
    private List<FieldData> m_children;
    IList m_targetList;
    Type m_subContentType;
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
        m_subContentType = a_contentType.GetGenericArguments()[0];
        if (a_attributes.Length < 2)
        {
            Debug.LogError("No subdrawers specified for list elements: " + Name);
            return;
        }

        if (!typeof(IList).IsAssignableFrom(a_contentType))
        {
            Debug.LogError("ListDrawerAttribute assigned to field that is not a list or array: " + a_contentType);
            return;
        }

        m_subConstraints = a_constraints;
        m_subDrawerAttributes = new AbstractFieldDrawerAttribute[a_attributes.Length - 1];
        for (int i = 1; i < a_attributes.Length; i++)
            m_subDrawerAttributes[i - 1] = a_attributes[i];
        m_drawerSupportElements = new List<Type> { typeof(ListElementIndexer) };

        base.Initialise(a_depth, a_parent, a_target, a_attributes, a_contentType, a_constraints, a_spacerAttributes, a_hideIfAttribute);
    }
    
    public override IEnumerable<FieldData> GetAllChildren()
    {
        return m_children;
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

    public override object GetValueOfChild(int a_childIndex)
    {
        return m_targetList[a_childIndex];
    }

    protected override void CreateChildren()
    {
        m_children = new List<FieldData>();
        if (GetDataObject() != null)
        {
            m_targetList = (IList) GetDataObject();
            for (int i = 0; i < m_targetList.Count; i++)
            {
                FieldData newChild = DrawerManager.CreateObjectFieldData(m_depth, this, m_targetList[i], m_subDrawerAttributes, m_subContentType, m_subConstraints, null, m_postCreateInitialised);
                newChild.Index = i;
                m_children.Add(newChild);
            }
        }
    }

    protected override void ClearChildren()
    {
        if (m_children != null)
            foreach (FieldData fieldData in m_children)
                fieldData.DestroyFieldData();
        m_children = null;
    }

    public override void CreateDrawer(Transform a_drawerParent = null)
    {
        base.CreateDrawer(a_drawerParent);
        if(m_children != null)
            foreach(FieldData child in m_children)
                child.CreateDrawer(m_drawer.GetChildContainer());
        m_drawer.ChildIndicesUpdated();
    }

    public override void ReleaseDrawer()
    {
        base.ReleaseDrawer();
        if (m_children != null)
            foreach (FieldData child in m_children)
                child.ReleaseDrawer();
    }

    public void AddChild(FieldData a_newChild)
    {
        m_children.Add(a_newChild);
    }

    public void AddNewBaseValueChild()
    {
        ListDrawerAttribute listAttr = (ListDrawerAttribute)DrawerAttribute;

        object newInstance = null;
        if (listAttr.AutoIncrement && m_subContentType == typeof(int))
        {
	        if (m_targetList == null || m_targetList.Count == 0)
		        newInstance = listAttr.AutoIncrementMin;
	        else
	        {
		        List<int> objects = m_targetList.Cast<int>().ToList();
		        if (listAttr.AutoIncrementFillGaps)
		        {
			        if (objects[0] < listAttr.AutoIncrementMin)
				        newInstance = listAttr.AutoIncrementMin;
					else if (objects.Count == 1)
				        newInstance = objects[0] + 1;
			        else
			        {
				        int lastValue = objects[0];
				        bool found = false;
				        for (int i = 1; i < objects.Count; i++)
				        {
					        if (objects[i] != lastValue + 1)
					        {
						        newInstance = lastValue + 1;
						        found = true;
						        break;
					        }
					        lastValue = objects[i];
				        }
						if(!found)
							newInstance = objects[objects.Count - 1] + 1;
					}
				}
		        else
		        {
			        objects.Sort();
					newInstance = objects[objects.Count - 1]+1;
		        }
	        }
        }
        else
	        newInstance = m_subContentType.GetNewBaseObject();

		int childIndex = m_targetList.Add(newInstance);
        FieldData newChild = DrawerManager.CreateObjectFieldData(m_depth, this, m_targetList[childIndex], m_subDrawerAttributes, m_subContentType, m_subConstraints, null, m_postCreateInitialised);
        m_children.Add(newChild);
        newChild.Index = m_children.Count-1;
        newChild.CreateDrawer(m_drawer.GetChildContainer());
        InvokeEvent(DrawerEventType.Added, GetDataObject(), Index, childIndex, newInstance);
        DataManager.Instance.DataChanged();
    }

    public override void DestroyChild(int a_childIndex)
    {
        FieldData destroyedChild = m_children[a_childIndex];
        m_children.RemoveAt(a_childIndex);
        object childValue = m_targetList[a_childIndex];
        m_targetList.RemoveAt(a_childIndex);
        destroyedChild.DestroyFieldData(childValue);
        ReindexChildrenFrom(a_childIndex);
        DataManager.Instance.DataChanged();
    }

    public override void ChangeChildIndex(int a_oldChildIndex, int a_newChildIndex)
    {
        if (m_children[a_oldChildIndex].Drawer != null)
            m_children[a_oldChildIndex].Drawer.transform.SetSiblingIndex(a_newChildIndex);

		//Reorder fielddata children
        FieldData movedElement = m_children[a_oldChildIndex];
        m_children.RemoveAt(a_oldChildIndex);
        m_children.Insert(a_newChildIndex, movedElement);

		//Reorder data
		m_targetList.RemoveAt(a_oldChildIndex);
		m_targetList.Insert(a_newChildIndex, movedElement.GetDataObject());

		ReindexChildrenFrom(Math.Min(a_oldChildIndex, a_newChildIndex));
        InvokeEvent(DrawerEventType.IndexChanged, GetDataObject(), Index, a_oldChildIndex, movedElement, true);
        DataManager.Instance.DataChanged();
    }

    void ReindexChildrenFrom(int a_fromIndex)
    {
        for (int i = a_fromIndex; i < m_children.Count; i++)
        {
            m_children[i].Index = i;
        }
    }

    protected override void ChildChanged(DrawerEventData a_eventData)
    {
        if (m_isBeingDestroyed || m_ignoreChildChange)
            return;
        if (!a_eventData.PassedFromChild && a_eventData.EventType == DrawerEventType.ValueChanged)
        {
            m_targetList[a_eventData.TargetIndex] = a_eventData.TargetObject;
        }
        InvokeEvent(a_eventData.EventType, GetDataObject(), Index, a_eventData.TargetIndex, a_eventData.TargetObject, true);
    }

    public override void SetExpanded(bool a_expanded)
    {
        if (m_drawer != null)
            ((ListFieldDrawer)m_drawer).SetExpanded(a_expanded);
    }
}


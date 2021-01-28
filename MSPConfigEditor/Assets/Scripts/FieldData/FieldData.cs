using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class FieldData
{
    private string m_name;

    //Event fired on value change
    public delegate void ObjectChanged(DrawerEventData eventData);
    protected event ObjectChanged m_valueChangedEvent;

    //Function to get the object this fielddata contains
    public delegate object ValueGetFunction();
    protected ValueGetFunction m_valueGetFunction;
    
    public IndexChangedEvent m_indexChangeCallback;
    
    protected int m_depth; //Menucolumn this drawer is in
    protected bool m_isNull;
    protected Type m_objectType;
    protected EConstraintType m_worstContraintType;
    protected FieldData m_parent;
    protected bool m_postCreateInitialised;
    protected bool m_isBeingDestroyed;
    protected bool m_ignoreChildChange;

    private AbstractFieldDrawerAttribute m_drawerAttribute;
    private int m_index;
    private List<AbstractSpacerAttribute> m_spacerAttributes;
    protected List<Type> m_drawerSupportElements;
    private AHideIfInstance m_hideIfInstance;

    //Null unless drawn
    private List<AbstractSpacer> m_spacers;
    protected AbstractFieldDrawer m_drawer;
    public AbstractFieldDrawer Drawer => m_drawer;

    //Constraints
    protected List<Constraint> m_constraints;
    protected List<bool> m_constraintViolations;

    //Properties
    public FieldData Parent { get => m_parent; }
    public bool IsBeingDestroyed { get => m_isBeingDestroyed; }
    public bool IsNull { get => m_isNull; }
    public List<bool> ConstraintViolations { get => m_constraintViolations; }
    public List<Constraint> Constraints { get => m_constraints; }
    public int Depth { get => m_depth; }
    public List<AbstractSpacerAttribute> SpacerAttributes { get => m_spacerAttributes; }
    public string Name { get => m_name; }
    public AbstractFieldDrawerAttribute DrawerAttribute { get => m_drawerAttribute; }
    public virtual string InfoText { get; }
    public Type ObjectType { get => m_objectType; }

    public EConstraintType WorstConstraintType
    {
        get => m_worstContraintType;
        set
        {
            m_worstContraintType = value;
            if(m_drawer != null)
                m_drawer.SetConstraintRim(m_worstContraintType);
        }
    }

    protected virtual void PreValueInitialise()
    { }

    public virtual void PostCreateInitialise()
    {
        m_postCreateInitialised = true;
    }

    public void SetField(int a_depth, FieldData a_parent, FieldInfo a_field, object a_fieldContainer, AbstractFieldDrawerAttribute[] a_attributes, Dictionary<int, List<IConstraintDefinition>> a_constraints, AbstractSpacerAttribute[] a_spacerAttributes, AHideIfAttribute a_hideIfAttribute)
    {
        m_parent = a_parent;
        m_valueChangedEvent += (data) =>
        {
            if (data.EventType == DrawerEventType.ValueChanged && !data.PassedFromChild)
                a_field.SetValue(a_fieldContainer, data.TargetObject);
        };
        m_valueGetFunction = () =>
        {
            return a_field.GetValue(a_fieldContainer);
        };
        Initialise(a_depth, a_parent, a_field.GetValue(a_fieldContainer), a_attributes, a_field.FieldType, a_constraints, a_spacerAttributes, a_hideIfAttribute);
    }

    public void SetField(int a_depth, FieldData a_parent, object a_target, AbstractFieldDrawerAttribute[] a_attributes, Type a_contentType, Dictionary<int, List<IConstraintDefinition>> a_constraints, AbstractSpacerAttribute[] a_spacerAttributes, AHideIfAttribute a_hideIfAttribute)
    {
        m_parent = a_parent;
        m_valueGetFunction = () =>
        {
            return m_parent.GetValueOfChild(Index);
        };
        Initialise(a_depth, a_parent, a_target, a_attributes, a_contentType, a_constraints, a_spacerAttributes, a_hideIfAttribute);
    }

    protected virtual void Initialise(int a_depth, FieldData a_parent, object a_target, AbstractFieldDrawerAttribute[] a_attributes, Type a_contentType, Dictionary<int, List<IConstraintDefinition>> a_constraints, AbstractSpacerAttribute[] a_spacerAttributes, AHideIfAttribute a_hideIfAttribute)
    {
        SetBaseData(a_depth, a_target, a_attributes[0], a_contentType, a_constraints, a_spacerAttributes, a_hideIfAttribute);
    }

    protected virtual void SetBaseData(int a_depth, object a_target, AbstractFieldDrawerAttribute a_attribute, Type a_objectType, Dictionary<int, List<IConstraintDefinition>> a_constraints, AbstractSpacerAttribute[] a_spacerAttributes, AHideIfAttribute a_hideIfAttribute)
    {
        m_drawerAttribute = a_attribute;
        m_objectType = a_objectType;
        m_depth = a_depth;
        m_name = m_drawerAttribute.Name;
        if(a_spacerAttributes != null)
            m_spacerAttributes = a_spacerAttributes.ToList();
        m_isNull = a_target == null;
        m_indexChangeCallback = new IndexChangedEvent();

        //Set constraints
        if (a_constraints != null)
        {
            m_constraints = new List<Constraint>(a_constraints[m_drawerAttribute.Priority].Count);
            m_constraintViolations = new List<bool>(m_constraints.Count);
            for (int i = 0; i < a_constraints[m_drawerAttribute.Priority].Count; i++)
            {
                m_constraints.Add(a_constraints[m_drawerAttribute.Priority][i].GetInstance(this, i));
                m_constraintViolations.Add(false);
            }
        }

        if (a_hideIfAttribute != null)
            m_hideIfInstance = a_hideIfAttribute.GetInstance(this);

        //Do value initialisation
        PreValueInitialise();
        SetValue(a_target, true, false);

        InvokeEvent(DrawerEventType.Initialised, GetDataObject(), Index);
    }

    public virtual void UpdateConstraints(object currentValue)
    {
        if (m_constraints == null)
            return;

        bool violationsChanged = false;
        for (int i = 0; i < m_constraints.Count; i++)
        {
            bool newState = m_constraints[i].ViolatesConstraint(currentValue);
            if (m_constraintViolations[i] != newState)
                violationsChanged = true;
            m_constraintViolations[i] = newState;
        }
        if (violationsChanged)
            DrawerManager.Instance.ConstraintWindow.UpdateViolationsForField(this);
    }

    public void ConstraintUpdated(bool a_newValue, int a_constraintIndex)
    {
        if (m_constraintViolations[a_constraintIndex] != a_newValue)
        {
            m_constraintViolations[a_constraintIndex] = a_newValue;
            DrawerManager.Instance.ConstraintWindow.UpdateViolationsForField(this);
        }
        else if (a_newValue)
            DrawerManager.Instance.ConstraintWindow.UpdateViolationsText(this);
    }

    public virtual void CreateDrawer(Transform a_drawerParent = null)
    {
        if (m_drawer != null)
        {
            Debug.LogError("Creating drawer for a field that already has one: " + Name);
            return;
        }

        if(a_drawerParent == null)
            a_drawerParent = DrawerManager.Instance.GetDrawerParentAtDepth(m_depth);

        if (m_spacerAttributes != null)
        {
            m_spacers = new List<AbstractSpacer>();
            foreach (AbstractSpacerAttribute spacerAttr in m_spacerAttributes)
            {
                AbstractSpacer spacer = DrawerManager.Instance.SpacerPool.GetObject(spacerAttr.SpacerType, a_drawerParent).GetComponent<AbstractSpacer>();
                spacer.Initialise(spacerAttr);
                m_spacers.Add(spacer);
            }
        }

        m_drawer = DrawerManager.Instance.DrawerPool.GetObject(m_drawerAttribute.DrawerType, a_drawerParent).GetComponent<AbstractFieldDrawer>();
        m_drawer.SetToFieldData(this);
        m_drawer.UpdateDisplayedValue(GetDataObject());
        if(m_hideIfInstance != null && m_hideIfInstance.Hidden)
            SetHidden(true);
    }

    public virtual void ReleaseDrawer()
    {
        if (m_drawer != null)
        {
            m_drawer.ReleaseFromFieldData();
            m_drawer = null;
        }

        if (m_spacers != null)
        {
            foreach (AbstractSpacer spacer in m_spacers)
                spacer.ReleaseFromFieldData();
            m_spacers = null;
        }
    }

    public bool IsNullable => m_valueGetFunction.Invoke().IsNullable();

    public void SetNull(bool a_isNull)
    {
        if (a_isNull)
        {
            SetValue(null);
        }
        else
        {
            SetValue(m_objectType.GetNewBaseObject());
        }
    }

    public virtual void SetValue(object a_value, bool a_setDisplayValue = true, bool a_invokeCallback = true)
    {
        m_isNull = a_value == null;
        
        if (a_setDisplayValue)
        {
            m_drawer?.UpdateDisplayedValue(a_value);
        }

        if (a_invokeCallback)
        {
            InvokeEvent(DrawerEventType.ValueChanged, a_value, Index);
            Parent?.ChildChanged(new DrawerEventData(DrawerEventType.ValueChanged, a_value, Index));
            DataManager.Instance.DataChanged();
        }

        UpdateConstraints(a_value);
    }

    public void SetHidden(bool a_hidden)
    {
        if (m_drawer != null)
        {
            m_drawer.gameObject.SetActive(!a_hidden);
        }
        if (m_spacers != null)
        {
            foreach (AbstractSpacer spacer in m_spacers)
                spacer.gameObject.SetActive(!a_hidden);
        }
    }

    public void InvokeEvent(DrawerEventType a_type, object a_objectValue, int a_objectIndex, int a_childIndex = -1, object a_childObject = null, bool a_passedFromChild = false)
    {
        m_valueChangedEvent?.Invoke(new DrawerEventData(a_type, a_objectValue, a_objectIndex, a_childIndex, a_childObject, a_passedFromChild));
    }

    public void InvokeEvent(DrawerEventData a_event)
    {
        m_valueChangedEvent?.Invoke(a_event);
    }

    protected virtual void PassOnEventFromChild(DrawerEventData a_event)
    {
        m_valueChangedEvent?.Invoke(a_event);
        Parent?.ChildChanged(a_event);
    }

    protected virtual void ChildChanged(DrawerEventData a_eventData)
    {
        if (m_isBeingDestroyed || m_ignoreChildChange)
            return;
        PassOnEventFromChild(new DrawerEventData(a_eventData.EventType, GetDataObject(), Index, a_eventData.TargetIndex, a_eventData.TargetObject, true));
    }

    public virtual void ChangeChildIndex(int a_oldChildIndex, int a_newChildIndex)
    { }

    public virtual void DestroyChild(int a_childIndex)
    { }

    public int Index
    {
        set
        {
            m_index = value;
            m_indexChangeCallback?.Invoke(value);
        }
        get => m_index;
    }

    public void DestroyFieldData(object a_oldValue = null)
    {
        m_isBeingDestroyed = true;

        //Update drawer first to avoid updating it during delete
        ReleaseDrawer();
        SetExpanded(false);

        //Pass event for destruction
        object currentvalue = a_oldValue ?? GetDataObject();
        DrawerEventData newEvent = new DrawerEventData(DrawerEventType.Destroyed, currentvalue, Index);
        InvokeEvent(newEvent);
        if(Parent != null && !Parent.IsBeingDestroyed)
            Parent.ChildChanged(newEvent);

        if (m_constraints != null)
        {
            foreach (Constraint constraint in m_constraints)
                constraint.RemoveAllReferences();
            DrawerManager.Instance.ConstraintWindow.RemoveViolationsForFieldData(this);
        }
        CleanUpReferences(currentvalue);
    }

    protected virtual void CleanUpReferences(object a_currentValue)
    {
        m_parent = null;
        m_hideIfInstance?.RemoveAllReferences();
    }

    public virtual FieldData GetChild(string a_fieldName)
    {
        Debug.LogError("This drawer does not implement getting children with field names.");
        return null;
    }

    public virtual FieldData GetChild(int a_childIndex)
    {
        Debug.LogError("This drawer does not implement getting children with indices.");
        return null;
    }

    public virtual IEnumerable<FieldData> GetAllChildren()
    {
        Debug.LogError("This drawer does not implement getting all children.");
        return null;
    }

    public virtual object GetDataObject()
    {
        if (m_valueGetFunction == null)
            return null;
        return m_valueGetFunction();
    }

    public void SubscribeEventHandler(ObjectChanged a_handler)
    {
        m_valueChangedEvent += a_handler;
    }

    public void UnSubscribeEventHandler(ObjectChanged a_handler)
    {
        m_valueChangedEvent -= a_handler;
    }

    public void OpenPathToFieldData()
    {
        List<FieldData> parentFieldData = new List<FieldData>();
        FieldData current = m_parent;
        while (current != null)
        {
            parentFieldData.Add(current);
            current = current.m_parent;
        }
        for (int i = parentFieldData.Count - 1; i >= 0; i--)
            parentFieldData[i].Select();
        if (m_drawer != null)
        {
            DrawerManager.Instance.HighlightRim.HighlightObject(m_drawer.transform);
            DrawerManager.Instance.ScrollColumnToViewDrawer(m_drawer);
        }
    }

    public virtual void Select()
    { }

    public virtual void SetExpanded(bool a_expanded)
    { }

    public virtual object GetValueOfChild(int a_childIndex)
    {
        return null;
    }

    public List<ADrawerSupportElement> GetDrawerSupportElements(Transform a_parent)
    {
        List<ADrawerSupportElement> result = new List<ADrawerSupportElement>();
        if (m_drawerSupportElements != null)
        {
            foreach (Type t in m_drawerSupportElements)
            {
                GameObject prefab = DrawerManager.Instance.DrawerOptions.GetDrawerSupportForType(t);
                if (prefab != null)
                {
                    ADrawerSupportElement element = DrawerManager.Instance.SupportElementPool.GetObject(t, a_parent).GetComponent<ADrawerSupportElement>();
                    element.Initialise(m_drawerAttribute);
                    result.Add(element);
                }
            }
        }

        return result;
    }

    public string GetClipboardCopy()
    {
        return m_objectType.ToString() + "#" + JsonConvert.SerializeObject(GetDataObject());
    }

    public bool CanPasteClipboard(string a_clipboard)
    {
        if (string.IsNullOrEmpty(a_clipboard))
            return false;

        int separatorIndex = a_clipboard.IndexOf('#', 0);
        if (separatorIndex < 1)
            return false;

        return Type.GetType(a_clipboard.Substring(0, separatorIndex)) == m_objectType;
    }

    //Should only ever be called after CanPasteClipboard is true
    public void PasteClipboard(string a_clipboard)
    {
        int jsonStartindex = a_clipboard.IndexOf('#', 0)+1;
        try
        {
            object pastedObj = JsonConvert.DeserializeObject(a_clipboard.Substring(jsonStartindex), m_objectType);
            SetValue(pastedObj);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to paste object despite correct type: " + e.Message);
            DrawerManager.Instance.PopupWindowManager.OpenNotificationWindow(
                "Paste failed",
                "The object could not be pasted despite having the correct type. If it is from an older config version the object might have been changed.\n\nError message: " + e.Message,
                "Continue",
                null);
        }
    }
}


[System.Serializable]
public class IndexChangedEvent : UnityEvent<int>
{
}


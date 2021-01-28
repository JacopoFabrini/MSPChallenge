using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Linq;
using System.Globalization;

public class DrawerManager : SerializedMonoBehaviour
{
    static DrawerManager m_instance;
    public static DrawerManager Instance
    {
        get
        {
            if (m_instance == null)
                Debug.LogError("MenuManager instance needed before initialisation.");
            return m_instance;
        }
    }

    [SerializeField]
    DataDrawerOptions m_drawerOptions;

    [SerializeField]
    GameObject m_drawerColumnPrefab;

    [SerializeField]
    Transform m_drawerColumnParent;

    [SerializeField]
    Transform m_unusedDrawerParent;
    public static Transform UnusedDrawerParent => m_instance.m_unusedDrawerParent;

    [SerializeField]
    HighlightRim m_highlightRim;

    [SerializeField]
    ContextMenuWindow m_contextMenu;

    [SerializeField]
    ConstraintWindow m_constraintWindow;

    [SerializeField]
    ColourPickerWindow m_colourPicker;

    [SerializeField]
    DrawerInfoWindow m_drawerInfoWindow;

    [SerializeField]
    PopupWindowManager m_popupWindowManager;

    List<DrawerColumn> m_drawerColumns;
    FieldData m_fieldDataRoot;
    static CultureInfo m_cultureInfo;

    public DataDrawerOptions DrawerOptions => m_drawerOptions; 
    public ColourPickerWindow ColourPicker => m_colourPicker; 
    public FieldData FieldDataRoot => m_fieldDataRoot; 
    public ConstraintWindow ConstraintWindow => m_constraintWindow; 
    public static CultureInfo CultureInfo => m_cultureInfo; 
    public HighlightRim HighlightRim => m_highlightRim; 
    public PopupWindowManager PopupWindowManager => m_popupWindowManager; 
    public DrawerInfoWindow DrawerInfoWindow => m_drawerInfoWindow; 

    private TypedObjectPool<AbstractFieldDrawer> m_drawerPool;
    public TypedObjectPool<AbstractFieldDrawer> DrawerPool => m_drawerPool;

    private TypedObjectPool<AbstractSpacer> m_spacerPool;
    public TypedObjectPool<AbstractSpacer> SpacerPool => m_spacerPool;

    private TypedObjectPool<ADrawerSupportElement> m_supportElementPool;
    public TypedObjectPool<ADrawerSupportElement> SupportElementPool => m_supportElementPool;
    void Awake()
    {
        m_instance = this;
        m_cultureInfo = new CultureInfo("en-US");
        m_drawerColumns = new List<DrawerColumn>();
        m_drawerPool = new TypedObjectPool<AbstractFieldDrawer>(m_unusedDrawerParent, DrawerOptions.GetDrawerForType);
        m_spacerPool = new TypedObjectPool<AbstractSpacer>(m_unusedDrawerParent, DrawerOptions.GetSpacerForType);
        m_supportElementPool = new TypedObjectPool<ADrawerSupportElement>(m_unusedDrawerParent, DrawerOptions.GetDrawerSupportForType);
    }

    public void DeselectAtDepth(int a_depth, bool a_hideColumn)
    {
        if (a_depth >= 0 && a_depth < m_drawerColumns.Count)
            m_drawerColumns[a_depth].Deselect(a_hideColumn);
    }

    public void SelectAtDepth(NewLineFieldData a_fieldData, int a_depth)
    {
        if (a_depth < 0)
            return;
        m_drawerColumns[a_depth].Select(a_fieldData);
    }

    void CreateNewMenuColumn()
    {
        DrawerColumn newColumn = GameObject.Instantiate(m_drawerColumnPrefab, m_drawerColumnParent).GetComponent<DrawerColumn>();
        m_drawerColumns.Add(newColumn);
    }

    //public void ReparentToDepth(GameObject a_object, int a_depth)
    //{
    //    if (a_depth >= m_drawerColumns.Count)
    //        CreateNewMenuColumn();

    //    a_object.transform.SetParent(m_drawerColumns[a_depth].Container, false);
    //    a_object.SetActive(true);
    //}

    public Transform GetDrawerParentAtDepth(int a_depth)
    {
        if (a_depth >= m_drawerColumns.Count)
            CreateNewMenuColumn();
        return m_drawerColumns[a_depth].Container;
    }

    public void ShowDrawerColumn(int a_depth)
    {
        if (a_depth >= m_drawerColumns.Count)
            CreateNewMenuColumn();

        m_drawerColumns[a_depth].SetExpanded(true);
        m_drawerColumns[a_depth].gameObject.SetActive(true);
    }

    public void SetDrawerColumnName(int a_depth, string a_name)
    {
        m_drawerColumns[a_depth].Name = a_name;
    }
    
    public void CreateRootDataHolder(object a_root)
    {
        m_fieldDataRoot = new NewLineFieldData();
        m_fieldDataRoot.SetField(-1, null, a_root, new AbstractFieldDrawerAttribute[1] { new NewLineFieldDrawerAttribute("Root") }, a_root.GetType(), null, null, null);
        m_fieldDataRoot.PostCreateInitialise();
        m_fieldDataRoot.SetExpanded(true);
    }

    public void ClearRoot()
    {
        m_fieldDataRoot?.DestroyFieldData();
        m_fieldDataRoot = null;
    }

    public Dictionary<string, FieldData> CreateChildFieldData(object a_target,
        FieldData a_parentFieldData, int a_depth, bool a_postCreateInitialise)
    {
        return CreateChildFieldData(a_target, m_unusedDrawerParent, a_parentFieldData, a_depth, a_postCreateInitialise);
    }

    public Dictionary<string, FieldData> CreateChildFieldData(object a_target, Transform a_parentTransform,
        FieldData a_parentFieldData, int a_depth, bool a_postCreateInitialise)
    {
        Dictionary<string, FieldData> result = new Dictionary<string, FieldData>();
        Dictionary<string, Dictionary<int, List<IConstraintDefinition>>> typeConstraints = ConstraintManager.GetConstraintsForType(a_target.GetType());
        foreach (FieldInfo field in a_target.GetType().GetFields().OrderBy(f => f.MetadataToken))
        {
            AbstractSpacerAttribute[] spacerAttributes = Array.ConvertAll(Attribute.GetCustomAttributes(field, typeof(AbstractSpacerAttribute)), attr => (AbstractSpacerAttribute)attr);
            AbstractFieldDrawerAttribute[] drawerAttributes = Array.ConvertAll(Attribute.GetCustomAttributes(field, typeof(AbstractFieldDrawerAttribute)), attr => (AbstractFieldDrawerAttribute)attr);
            AHideIfAttribute hideIfAttribute = (AHideIfAttribute)Attribute.GetCustomAttribute(field, typeof(AHideIfAttribute));
            Dictionary<int, List<IConstraintDefinition>> fieldConstraints = null;
            if(typeConstraints != null)
                typeConstraints.TryGetValue(field.Name, out fieldConstraints);
            
            if (spacerAttributes != null && spacerAttributes.Length > 0)
            {
                spacerAttributes.OrderBy(o => o.Priority);
            }

            if (drawerAttributes != null && drawerAttributes.Length > 0)
            {
                drawerAttributes.OrderBy(o => o.Priority);
                FieldData fieldData = (FieldData) Activator.CreateInstance(drawerAttributes[0].FieldDataType);
                fieldData.SetField(a_depth, a_parentFieldData, field, a_target, drawerAttributes, fieldConstraints, spacerAttributes, hideIfAttribute);
                if (a_postCreateInitialise)
                    fieldData.PostCreateInitialise();
                result.Add(field.Name, fieldData);
            }
        }
        return result;
    }

    public static FieldData CreateObjectFieldData(int a_depth, FieldData a_parent, object a_target, AbstractFieldDrawerAttribute[] a_attributes, Type a_contentType, Dictionary<int, List<IConstraintDefinition>> a_constraints, AbstractSpacerAttribute[] a_spacerAttributes, bool a_postCreateInitialise)
    {
        FieldData result = (FieldData) Activator.CreateInstance(a_attributes[0].FieldDataType);
        result.SetField(a_depth, a_parent, a_target, a_attributes, a_contentType, a_constraints, a_spacerAttributes, null);
        if (a_postCreateInitialise)
            result.PostCreateInitialise();
        return result;
    }

    public void OpenContextMenu(AbstractFieldDrawer a_drawer)
    {
        m_contextMenu.SetToDrawer(a_drawer);
    }

    public void ScrollColumnToViewDrawer(AbstractFieldDrawer a_drawer)
    {
        m_drawerColumns[a_drawer.FieldData.Depth].ScollToViewDrawer(a_drawer);
    }

}

using System;

public class DictionaryFieldDrawerAttribute : ReferenceFieldDrawerAttribute, IListElementIndexerAttribute
{
    bool m_reorderable = true;
    bool m_removable = true;
    bool m_autoIncrement = false;
    int m_autoIncrementMin = 0;
    bool m_autoIncrementFillGaps = false;

	public DictionaryFieldDrawerAttribute(string a_name) : base(a_name)
    {
        m_name = a_name;
    }

    public bool Reorderable
    {
        get { return m_reorderable; }
        set { m_reorderable = value; }
    }

    public bool Removable
    {
        get { return m_removable; }
        set { m_removable = value; }
    }

    public bool AutoIncrement
    {
	    get { return m_autoIncrement; }
	    set { m_autoIncrement = value; }
    }

    public int AutoIncrementMin
    {
	    get { return m_autoIncrementMin; }
	    set { m_autoIncrementMin = value; }
    }

    public bool AutoIncrementFillGaps
    {
	    get { return m_autoIncrementFillGaps; }
	    set { m_autoIncrementFillGaps = value; }
    }

	public override Type DrawerType
    {
        get { return typeof(DictionaryFieldDrawer); }
    }

    public override Type FieldDataType => typeof(DictionaryFieldData);
}

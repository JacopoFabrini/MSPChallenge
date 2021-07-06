using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class DictionaryEntryDrawerAttribute : AbstractFieldDrawerAttribute
{
    public DictionaryEntryDrawerAttribute() : base(null)
    {
        m_nullable = false;
    }

    public override Type DrawerType
    {
        get { return typeof(DictionaryEntryDrawer); }
    }

    public override Type FieldDataType => typeof(DictionaryEntryFieldData);
}


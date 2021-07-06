using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class RangeConstraintRef<T> : IConstraintDefinition where T : IComparable
{
    private Variable<T> m_min;
    private Variable<T> m_max;
    private bool m_inverse;
    private bool m_inclusiveMin;
    private bool m_inclusiveMax;
    private EConstraintType m_constaintType;
    private string m_minRefName;
    private string m_maxRefName;

    public RangeConstraintRef(Variable<T> a_min, Variable<T> a_max, EConstraintType a_type, string a_minRefName, string a_maxRefName, bool a_inverse = false, bool a_inclusiveMin = true, bool a_inclusiveMax = true)
    {
        m_min = a_min;
        m_max = a_max;
        m_inverse = a_inverse;
        m_inclusiveMin = a_inclusiveMin;
        m_inclusiveMax = a_inclusiveMax;
        m_constaintType = a_type;
        m_minRefName = a_minRefName;
        m_maxRefName = a_maxRefName;
    }

    public Variable<T> Min { get => m_min;  }
    public Variable<T> Max { get => m_max;  }
    public bool Inverse { get => m_inverse; }
    public bool InclusiveMin { get => m_inclusiveMin; }
    public bool InclusiveMax { get => m_inclusiveMax;  }
    public EConstraintType ConstaintType { get => m_constaintType; }
    public string MinRefName { get => m_minRefName;  }
    public string MaxRefName { get => m_maxRefName;  }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return new RangeConstraintRefInstance<T>(this, a_fieldData, a_constraintIndex);
    }
}


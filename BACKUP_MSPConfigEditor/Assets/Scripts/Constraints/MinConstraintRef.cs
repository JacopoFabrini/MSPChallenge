using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MinConstraintRef<T> : IConstraintDefinition where T : IComparable
{
    Variable<T> m_minVariable;
    private bool m_acceptEqual;
    private EConstraintType m_constaintType;
    string m_referenceName;

    public Variable<T> MinVariable { get => m_minVariable; }
    public bool AcceptEqual { get => m_acceptEqual;  }
    public EConstraintType ConstaintType { get => m_constaintType; }
    public string ReferenceName { get => m_referenceName; }

    public MinConstraintRef(Variable<T> a_minVariable, bool a_acceptEqual, EConstraintType a_type, string a_referenceName)
    {
        m_minVariable = a_minVariable;
        m_acceptEqual = a_acceptEqual;
        m_constaintType = a_type;
        m_referenceName = a_referenceName;
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return new MinConstraintRefInstance<T>(this, a_fieldData, a_constraintIndex);
    }
}

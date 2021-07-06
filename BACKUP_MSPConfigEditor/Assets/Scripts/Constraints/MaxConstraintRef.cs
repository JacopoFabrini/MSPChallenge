using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MaxConstraintRef<T> : IConstraintDefinition where T : IComparable
{
    Variable<T> m_maxVariable;
    private bool m_acceptEqual;
    private EConstraintType m_constaintType;
    string m_referenceName;

    public Variable<T> MaxVariable { get => m_maxVariable; }
    public bool AcceptEqual { get => m_acceptEqual; }
    public EConstraintType ConstaintType { get => m_constaintType; }
    public string ReferenceName { get => m_referenceName; }

    public MaxConstraintRef(Variable<T> a_maxVariable, bool a_acceptEqual, EConstraintType a_type, string a_referenceName)
    {
        m_maxVariable = a_maxVariable;
        m_acceptEqual = a_acceptEqual;
        m_constaintType = a_type;
        m_referenceName = a_referenceName;
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return new MaxConstraintRefInstance<T>(this, a_fieldData, a_constraintIndex);
    }
}

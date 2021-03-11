using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MinConstraint<T> : Constraint, IConstraintDefinition where T : IComparable
{
    protected T m_min;
    protected bool m_acceptEqual;

    public MinConstraint(T a_min, bool a_acceptEqual, EConstraintType a_type)
    {
        m_min = a_min;
        m_acceptEqual = a_acceptEqual;
        m_constaintType = a_type;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        IComparable comparable = (IComparable)a_value;
        if (comparable == null)
            return true;
        int result = comparable.CompareTo(m_min);
        return m_acceptEqual ? result < 0 : result <= 0; 
    }

    public override string GetViolationText()
    {
        if (m_acceptEqual)
            return "Value lower than: " + m_min.ToString();
        return "Value lower than or equal to: " + m_min.ToString();
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return this;
    }

    public override void RemoveAllReferences()
    { }
}

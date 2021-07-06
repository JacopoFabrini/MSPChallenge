using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MaxConstraint<T> : Constraint, IConstraintDefinition where T : IComparable
{
    protected T m_max;
    protected bool m_acceptEqual;

    public MaxConstraint(T a_max, bool a_acceptEqual, EConstraintType a_type)
    {
        m_max = a_max;
        m_acceptEqual = a_acceptEqual;
        m_constaintType = a_type;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        IComparable comparable = (IComparable)a_value;
        if (comparable == null)
            return true;
        int result = comparable.CompareTo(m_max);
        return m_acceptEqual ? result > 0 : result >= 0;
    }

    public override string GetViolationText()
    {
        if (m_acceptEqual)
            return "Value higher than: " + m_max.ToString();
        return "Value higher than or equal to: " + m_max.ToString();
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return this;
    }

    public override void RemoveAllReferences()
    { }
}
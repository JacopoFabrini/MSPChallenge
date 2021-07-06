using System;
using System.Collections.Generic;

class ContainedConstraint<T> :  IConstraintDefinition
    where T : IEquatable<T>
{
    RefVar<IList<T>> m_listVariable;
    private bool m_inverse;
    private EConstraintType m_constaintType;
    string m_referenceName;

    public RefVar<IList<T>> ListVariable { get => m_listVariable; }
    public bool Inverse { get => m_inverse; }
    public EConstraintType ConstaintType { get => m_constaintType; }
    public string ReferenceName { get => m_referenceName; }

    public ContainedConstraint(RefVar<IList<T>> a_listVariable, EConstraintType a_type, string a_referenceName, bool a_inverse = false)
    {
        m_listVariable = a_listVariable;
        m_inverse = a_inverse;
        m_constaintType = a_type;
        m_referenceName = a_referenceName;
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return new ContainedConstraintInstance<T>(this, a_fieldData, a_constraintIndex);
    }
}


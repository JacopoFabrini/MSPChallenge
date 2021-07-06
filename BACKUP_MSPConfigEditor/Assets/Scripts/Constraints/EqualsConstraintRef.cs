using System;

public class EqualsConstraintRef<T> : IConstraintDefinition where T : IEquatable<T>
{
    Variable<T> m_equalsVariable;
    private bool m_inverse;
    private EConstraintType m_constaintType;
    string m_referenceName;

    public Variable<T> EqualsVariable { get => m_equalsVariable; }
    public bool Inverse { get => m_inverse; }
    public EConstraintType ConstaintType { get => m_constaintType; }
    public string ReferenceName { get => m_referenceName; }

    public EqualsConstraintRef(Variable<T> a_equalsVariable, EConstraintType a_type, string a_referenceName, bool a_inverse = false)
    {
        m_equalsVariable = a_equalsVariable;
        m_inverse = a_inverse;
        m_constaintType = a_type;
        m_referenceName = a_referenceName;
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return new EqualsConstraintRefInstance<T>(this, a_fieldData, a_constraintIndex);
    }
}


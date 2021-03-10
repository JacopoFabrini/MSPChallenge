using System;
using System.Collections;
using System.Collections.Generic;

class ContainedDictConstraint<T> : IConstraintDefinition
    where T : IEquatable<T>
{
    string m_dictVariablePath;
    private bool m_inverse;
    private EConstraintType m_constaintType;
    string m_referenceName;
    bool m_matchKeys;
    
    public bool Inverse { get => m_inverse; }
    public EConstraintType ConstaintType { get => m_constaintType; }
    public string ReferenceName { get => m_referenceName; }
    public bool MatchKeys { get => m_matchKeys;}

    public ContainedDictConstraint(string a_dictVariablePath, EConstraintType a_type, string a_referenceName, bool a_matchKeys = true, bool a_inverse = false)
    {
        m_dictVariablePath = a_dictVariablePath;
        m_inverse = a_inverse;
        m_constaintType = a_type;
        m_referenceName = a_referenceName;
        m_matchKeys = a_matchKeys;
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return new ContainedDictConstraintInstance<T>(m_dictVariablePath, this, a_fieldData, a_constraintIndex);
    }
}


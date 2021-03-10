using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DuplicateKeyConstraint : Constraint
{
    bool m_hasDuplicateKey;
    int m_constraintIndex;

    public bool HasDuplicateKey { get => m_hasDuplicateKey; set => m_hasDuplicateKey = value; }
    public int ConstraintIndex { get => m_constraintIndex; }

    public DuplicateKeyConstraint(int a_index)
    {
        m_constaintType = EConstraintType.FatalError;
        m_constraintIndex = a_index;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        return HasDuplicateKey;
    }

    public override string GetViolationText()
    {
        return "Contains duplicate keys";
    }

    public override void RemoveAllReferences()
    {}
}

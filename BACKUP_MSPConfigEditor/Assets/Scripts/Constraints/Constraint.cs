using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EConstraintType { None = -1, Warning = 0, Error = 1, FatalError = 2 }

public abstract class Constraint
{
    protected EConstraintType m_constaintType;

    public bool IsError { get => ConstaintType == EConstraintType.Error; }
    public EConstraintType ConstaintType { get => m_constaintType; }

    public abstract bool ViolatesConstraint(object a_value);
    public abstract string GetViolationText();
    public abstract void RemoveAllReferences();
}

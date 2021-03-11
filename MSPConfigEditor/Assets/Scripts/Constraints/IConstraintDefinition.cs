using System;
using System.Collections.Generic;

public interface IConstraintDefinition
{
    Constraint GetInstance(FieldData a_fieldData, int a_index);
}


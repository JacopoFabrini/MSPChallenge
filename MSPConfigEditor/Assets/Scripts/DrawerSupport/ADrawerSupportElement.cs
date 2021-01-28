using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ADrawerSupportElement : MonoBehaviour
{
    public abstract void Initialise(AbstractFieldDrawerAttribute a_attribute);
    public abstract void SetToField(FieldData a_fieldData, AbstractFieldDrawer a_drawer);
    public abstract void ReleaseField();
}


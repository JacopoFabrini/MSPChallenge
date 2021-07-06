using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class AbstractSpacer : MonoBehaviour
{
    public abstract void Initialise(AbstractSpacerAttribute a_attribute);

    /// <summary>
    /// Remove all references to the current drawer
    /// </summary>
    public abstract void ReleaseFromFieldData();
}


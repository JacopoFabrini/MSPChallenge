using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ConstraintWindow : MonoBehaviour
{
    [SerializeField]
    GameObject m_violationPrefab;
    [SerializeField]
    Transform m_childContainer;

    Dictionary<FieldData, UIConstraintViolation> m_violations = new Dictionary<FieldData, UIConstraintViolation>();
    int m_errors;
    int m_fatalErrors;

    public void UpdateViolationsForField(FieldData a_fieldData)
    {
        bool hasViolations = false;
        foreach (bool b in a_fieldData.ConstraintViolations)
        {
            if (b)
            {
                hasViolations = true;
                break;
            }
        }

        UIConstraintViolation violation;
        if (m_violations.TryGetValue(a_fieldData, out violation))
        {
            bool hadError = violation.IsError;
            bool hadFatalError = violation.IsFatalError;
            if (hasViolations)
            {
                violation.SetToFieldData(a_fieldData);
                //Was fatal error before?
                if (hadFatalError)
                {
                    if (!violation.IsFatalError)
                        m_fatalErrors--;
                    if (violation.IsError)
                        m_errors++;
                }
                //Was error before?
                else if (hadError)
                {
                    if (!violation.IsError)
                        m_errors--;
                    if (violation.IsFatalError)
                        m_fatalErrors++;
                }
                //Was warning before
                else
                {
                    if (violation.IsFatalError)
                        m_fatalErrors++;
                    else if (violation.IsError)
                        m_errors++;
                }
            }
            else
            {
                m_violations.Remove(a_fieldData);
                Destroy(violation.gameObject);
                a_fieldData.WorstConstraintType = EConstraintType.None;
                if (hadError)
                    m_errors--;
                else if (hadFatalError)
                    m_fatalErrors--;
            }
        }
        else if (hasViolations)
        {
            UIConstraintViolation newViolation = Instantiate(m_violationPrefab, m_childContainer).GetComponent<UIConstraintViolation>();
            newViolation.SetToFieldData(a_fieldData);
            m_violations.Add(a_fieldData, newViolation);
            if (newViolation.IsError)
                m_errors++;
        }
    }

    public void UpdateViolationsText(FieldData a_fieldData)
    {
        UIConstraintViolation violation;
        if (m_violations.TryGetValue(a_fieldData, out violation))
        {
            violation.UpdateDetailText(a_fieldData);
        }
    }

    public void RemoveViolationsForFieldData(FieldData a_fieldData)
    {
        UIConstraintViolation violation;
        if (m_violations.TryGetValue(a_fieldData, out violation))
        {
            if (violation.IsError)
                m_errors--;
            m_violations.Remove(a_fieldData);
            Destroy(violation.gameObject);
        }
    }

    public int Errors => m_errors;

    public bool HasErrors
    {
        get { return m_errors > 0; }
    }

    public bool HasFatalErrors
    {
        get { return m_fatalErrors > 0; }
    }
}


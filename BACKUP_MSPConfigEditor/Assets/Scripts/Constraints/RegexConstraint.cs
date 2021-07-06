using System;
using System.Text.RegularExpressions;

class RegexConstraint : Constraint, IConstraintDefinition 
{
    protected Regex m_regex;
    string m_pattern;
    string m_regexDescription;
    bool m_allowNull;

    public RegexConstraint(string a_regexPattern, EConstraintType a_type, string a_regexDescription, bool a_allowNull = false, RegexOptions a_options = RegexOptions.None)
    {
        m_regex = new Regex(a_regexPattern, a_options);
        m_pattern = a_regexPattern;
        m_regexDescription = a_regexDescription;
        m_constaintType = a_type;
        m_allowNull = a_allowNull;
    }

    public override bool ViolatesConstraint(object a_value)
    {
        if (a_value == null)
            return !m_allowNull;
        Match match = m_regex.Match((string)a_value);
        return !match.Success;
    }

    public override string GetViolationText()
    {
        return string.Format("Match failed: {0}. Regex not matched: {1}", m_regexDescription, m_pattern);
    }

    public Constraint GetInstance(FieldData a_fieldData, int a_constraintIndex)
    {
        return this;
    }

    public override void RemoveAllReferences()
    {}
}


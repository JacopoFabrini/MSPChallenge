using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConstraintManager
{
    static Dictionary<Type, Dictionary<string, Dictionary<int, List<IConstraintDefinition>>>> m_constraints;

    public static ConstraintManager Instance { get; private set; }
    static ConstraintManager()
    {
        Instance = new ConstraintManager();
        m_constraints = new Dictionary<Type, Dictionary<string, Dictionary<int, List<IConstraintDefinition>>>>();
    }

    public static void RegisterConstraint(Type a_objectType, string a_fieldName, int a_targetPriority, IConstraintDefinition a_constaint)
    {
        if (m_constraints.ContainsKey(a_objectType))
        {
            Dictionary<string, Dictionary<int, List<IConstraintDefinition>>> typeConstraints = m_constraints[a_objectType];
            if (typeConstraints.ContainsKey(a_fieldName))
            {
                Dictionary<int, List<IConstraintDefinition>> fieldConstraints = typeConstraints[a_fieldName];
                if (fieldConstraints.ContainsKey(a_targetPriority))
                    fieldConstraints[a_targetPriority].Add(a_constaint);
                else
                    fieldConstraints.Add(a_targetPriority, new List<IConstraintDefinition> { a_constaint });
            }
            else
            {
                Dictionary<int, List<IConstraintDefinition>> fieldConstaints = new Dictionary<int, List<IConstraintDefinition>>();
                fieldConstaints.Add(a_targetPriority, new List<IConstraintDefinition> { a_constaint });
                typeConstraints.Add(a_fieldName, fieldConstaints);
            }
        }
        else
        {
            Dictionary<string, Dictionary<int, List<IConstraintDefinition>>> typeConstraints = new Dictionary<string, Dictionary<int, List<IConstraintDefinition>>>();
            Dictionary<int, List<IConstraintDefinition>> fieldConstaints = new Dictionary<int, List<IConstraintDefinition>>();
            fieldConstaints.Add(a_targetPriority, new List<IConstraintDefinition> { a_constaint });
            typeConstraints.Add(a_fieldName, fieldConstaints);
            m_constraints.Add(a_objectType, typeConstraints);
        }
        
    }

    public static Dictionary<string, Dictionary<int, List<IConstraintDefinition>>> GetConstraintsForType(Type a_objectType)
    {
        Dictionary<string, Dictionary<int, List<IConstraintDefinition>>> result = null;
        m_constraints.TryGetValue(a_objectType, out result);
        return result;
    }
}


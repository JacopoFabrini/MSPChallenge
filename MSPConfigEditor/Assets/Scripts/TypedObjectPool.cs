using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TypedObjectPool<U> where U: Component
{
    private Dictionary<Type, Stack<GameObject>> m_pool;
    private Transform m_pooledObjectsParent;

    public delegate GameObject GetPrefabForTypeFunction(Type a_type);
    private GetPrefabForTypeFunction m_getPrefabFunction;

    public TypedObjectPool(Transform a_pooledObjectsParent, GetPrefabForTypeFunction a_getPrefabFunction)
    {
        m_pooledObjectsParent = a_pooledObjectsParent;
        m_pool = new Dictionary<Type, Stack<GameObject>>();
        m_getPrefabFunction = a_getPrefabFunction;
    }

    public GameObject GetObject(Type a_type, Transform a_newParent)
    {
        if (m_pool.TryGetValue(a_type, out var stack) && stack.Count > 0)
        {
            GameObject targetObject = stack.Pop();
            if (targetObject != null)
            {
                targetObject.transform.SetParent(a_newParent, false);
                targetObject.SetActive(true);
            }
            return targetObject;
        }
        else
        {
            return GameObject.Instantiate(m_getPrefabFunction(a_type), a_newParent);
        }
    }

    public void ReleaseObject<T>(T a_object) where T : U
    {
        a_object.gameObject.SetActive(false);
        a_object.transform.SetParent(m_pooledObjectsParent);

        if (m_pool.TryGetValue(typeof(T), out var stack))
        {
            stack.Push(a_object.gameObject);
        }
        else
        {
            Stack<GameObject> newStack = new Stack<GameObject>();
            newStack.Push(a_object.gameObject);
            m_pool.Add(typeof(T), newStack);
        }
    }
}


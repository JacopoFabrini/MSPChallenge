using System;
using System.Collections.Generic;
using UnityEngine;

class GameObjectPool<T> where T : MonoBehaviour
{
    List<T> m_pool;
    GameObject m_prefab;
    Transform m_parent;
    int m_nextObjectIndex;

    public GameObjectPool(GameObject a_prefab, Transform a_parent)
    {
        m_pool = new List<T>(4);
        m_parent = a_parent;
        m_prefab = a_prefab;
    }

    public void ResetIndex()
    {
        m_nextObjectIndex = 0;
    }

    public T GetNext()
    {
        if (m_nextObjectIndex >= m_pool.Count)
        {
            T temp = GameObject.Instantiate(m_prefab, m_parent).GetComponent<T>();
            m_nextObjectIndex++;
            m_pool.Add(temp);
            return temp;
        }
        else
        {
            m_pool[m_nextObjectIndex].gameObject.SetActive(true);
            return m_pool[m_nextObjectIndex++];
        }
    }

    public void DisableUnused()
    {
        for (int i = m_nextObjectIndex; i < m_pool.Count; i++)
            m_pool[i].gameObject.SetActive(false);
    }
}
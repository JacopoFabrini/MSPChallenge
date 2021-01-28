using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DrawerEventData
{
    private DrawerEventType m_eventType;
    private object m_targetObject;
    private object m_childObject;
    private int m_targetIndex;
    private int m_childIndex;
    private bool m_passedFromChild;

    public DrawerEventData(DrawerEventType a_eventType, object a_targetObject, int a_targetIndex, int a_childIndex = -1, object a_childObject = null, bool a_passedFromChild = false)
    {
        m_eventType = a_eventType;
        m_targetObject = a_targetObject;
        m_targetIndex = a_targetIndex;
        m_childObject = a_childObject;
        m_childIndex = a_childIndex;
        m_passedFromChild = a_passedFromChild;
    }

    public DrawerEventType EventType { get => m_eventType; }
    public object TargetObject { get => m_targetObject; }
    public int TargetIndex { get => m_targetIndex; }
    public object ChildObject { get => m_childObject; }
    public int ChildIndex { get => m_childIndex; }
    public bool PassedFromChild { get => m_passedFromChild; }
}


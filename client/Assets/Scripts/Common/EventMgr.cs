using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EEventType
{
	EEventType_AttackFinish ,
	EEventType_UpdateIcon ,
	EEventType_UpdateEmployessJiuGuanUI,
	EEventType_UpdateEmployessShangZhenUI,
	EEventType_UpdateEmployessLiebiaoUI,
	Max,
}

public class EventItem
{
	public void Add(HandleEvent handle)
	{
		m_Handle += handle;
	}
	
	public void Remove(HandleEvent handle)
	{
		m_Handle -= handle;
	}
	
	public void Handle(object objParam)
	{
		if (null == m_Handle)
		{
			return ;
		}
		
		m_Handle(objParam);
	}
	
	private HandleEvent			m_Handle;
}

public delegate void HandleEvent(object objParam);

public class EventPair
{
	private EEventType 			m_eType;
	private object				m_objParam;
	private HandleEvent			m_handle;
	
	public EventPair(EEventType eType, object obj)
	{
		m_eType = eType;
		m_objParam = obj;
	}
	
	public EventPair(HandleEvent handle, object obj)
	{
		m_handle = handle;
		m_objParam = obj;
	}
	
	public HandleEvent Handle
	{
		get
		{
			return m_handle;
		}
	}
	
	public EEventType Type
	{
		get
		{
			return m_eType;
		}
	}
	
	public object Param
	{
		get
		{
			return m_objParam;
		}
	}
}

public class TimeEventPair
{
	private object 				m_objParam;
	private HandleEvent			m_handle;
	public float				TimeMS;
	
	public TimeEventPair(HandleEvent handle, object obj, int time)
	{
		m_handle = handle;
		m_objParam = obj;
		TimeMS = time;
	}
	
	public HandleEvent Handle
	{
		get
		{
			return m_handle;
		}
	}
	
	public object Param
	{
		get
		{
			return m_objParam;
		}
	}
}

public class EventMgr 
{
	static private readonly EventMgr s_Insance = new EventMgr();
	
	static public EventMgr Instance
	{
		get
		{
			return s_Insance;
		}
	}
	
	public EventMgr()
	{
		for (int i = 0; i < (int)EEventType.Max; i++)
		{
			m_EventItemArray[i] = new EventItem();
		}
	}
	
	private EventItem[]										m_EventItemArray = new EventItem[(int)EEventType.Max];
	private List<EventPair>									m_lstEvent = new List<EventPair>();
	private Dictionary<HandleEvent, TimeEventPair>			m_dictEvent = new Dictionary<HandleEvent, TimeEventPair>();
		
	public void RegisterEvent(EEventType eType, HandleEvent handle)
	{
		if (null == m_EventItemArray)
		{
			return ;
		}
		
		EventItem item = GetEventItem(eType);
		
		if (null == item)
		{
			item = new EventItem();
		}
		
		item.Add(handle);
	}
	
	public void UnRegisterEvent(EEventType eType, HandleEvent handle)
	{
		if (null == m_EventItemArray)
		{
			return ;
		}
		
		EventItem item = GetEventItem(eType);
		
		if (null == item)
		{
			return ;
		}
		
		item.Remove(handle);
	}
	
	public void FireEvent(EEventType eType, object obj)
	{
		EventItem item = GetEventItem(eType);
		
		if (null == item)
		{
			return ;
		}
		
		item.Handle(obj);
	}
	
	public void PushEvent(EEventType eType, object obj)
	{
		if (null == m_lstEvent)
		{
			return ;
		}
		
		m_lstEvent.Add(new EventPair(eType, obj));
	}
	
	public void PushEvent(HandleEvent handle, object obj)
	{
		if (null == m_lstEvent)
		{
			return ;
		}
		
		m_lstEvent.Add(new EventPair(handle, obj));
	}
	
	public void PushTimeEvent(HandleEvent handle, object obj, int timeMS)
	{
		if (null == handle || null == m_dictEvent)
		{
			return ;
		}
		
		if(m_dictEvent.ContainsKey(handle)) 
		{
			return;
		}
		
		m_dictEvent.Add(handle, new TimeEventPair(handle, obj, timeMS));
	}
	
	public void PopTimeEvent(HandleEvent handle)
	{
		if (null == handle || null == m_dictEvent)
		{
			return ;
		}
		
		if(!m_dictEvent.ContainsKey(handle)) 
		{
			return;
		}
		
		m_dictEvent.Remove(handle);
	}
	
	private EventItem GetEventItem(EEventType eType)
	{
		if (null == m_EventItemArray)
		{
			return null;
		}
		
		return m_EventItemArray[(int)eType];
	}
	
	List<HandleEvent> dictClearList = new List<HandleEvent>();
	public void Update()
	{
		if (null != m_lstEvent)
		{
			for (int i = 0; i < m_lstEvent.Count; i++)
			{
				EventPair child = m_lstEvent[i];
				if (null == child)
				{
					continue ;
				}
	
				if (null == child.Handle)
				{
					FireEvent(child.Type, child.Param);
				}
				else
				{
					child.Handle(child.Param);
				}
			}
			m_lstEvent.Clear();
		}
		
		
		
		if(null != m_dictEvent)
		{
			dictClearList.Clear();
			foreach(KeyValuePair<HandleEvent, TimeEventPair> kv in m_dictEvent)
			{
				TimeEventPair p = kv.Value;
				p.TimeMS -= (int)(Time.deltaTime * 1000f);
				if(p.TimeMS <= 0)
				{
					p.Handle(p.Param);
					dictClearList.Add(p.Handle);
				}
			}
			
			for(int i = 0; i < dictClearList.Count; ++i)
			{
				m_dictEvent.Remove(dictClearList[i]);
			}
		}
	}
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FriendSystem
{
    public event UpdateFriendHandler UpdateFriend;
    public event UpdateBlackHandler UpdateBlack;
    public event UpdateOffenHandler UpdateOffen;
	public event FriendChatHandler FriendChat;
	public event RequestEventHandler<COM_ContactInfo> FindFriendOkEvent;
	public event FindFriendFailHandler FindFriendFail;
	public event RequestEventHandler<int> recommendEvent;
	public event RequestEventHandler<bool> friendOnLineEvent;
    public IList friends_ = new ArrayList();
    public IList blacks_ = new ArrayList();
    public IList offens_ = new ArrayList();
	public bool isOpenFried;

	private COM_ContactInfo[] _recommendFriends;

	private Dictionary<string,List<COM_Chat>> _chatDic = new Dictionary<string, List<COM_Chat>> (); 
	private Dictionary<string,List<COM_Chat>> _noChatDic = new Dictionary<string, List<COM_Chat>> ();

    private static FriendSystem FS;

    FriendSystem()
    {
   
    }

    public static FriendSystem Instance()
    {
        if (null == FS)
        {
            FS = new FriendSystem();
        }
        return FS;
    }

    private void Add(COM_ContactInfo v, IList l)
    {
        if (Get(v.instId_,l) != null)
        {
            return;
        }

        l.Add(v);
    }
	public bool IsBlack(uint insid)
	{
		foreach(COM_ContactInfo v in  blacks_)
		{
			if(v.instId_ == insid)
			{
				return true;
			}
		}
		return false;
	}

    private COM_ContactInfo Get(uint uuid, IList l)
    {
        COM_ContactInfo ci = null;
        for (int i=0; i < l.Count; ++i)
        {
            ci = (COM_ContactInfo)l[i];
            if (ci.instId_ == uuid)
            {
                return ci;
            }
        }
        return null;
    }

    private COM_ContactInfo Get(string name, IList l)
    {
        COM_ContactInfo ci = null;
        for (int i = 0; i < l.Count; ++i)
        {
            ci = (COM_ContactInfo)l[i];
            if (ci.name_ == name)
            {
                return ci;
            }
        }
        return null;
    }

    private void Del(uint uuid, IList l)
    {
        COM_ContactInfo v = Get(uuid, l);
        if (v != null)
            l.Remove(v);
    }

    private void Del(string name, IList l)
    {
        COM_ContactInfo v = Get(name, l);
        if (v != null)
            l.Remove(v);
    }

    public void AddFriend(COM_ContactInfo v)
    {
        Add(v, friends_);

        if (UpdateFriend != null)
		{
			UpdateFriend (v,true);
		}
    }

    public COM_ContactInfo GetFriend(uint uuid)
    {
        return Get(uuid, friends_);
    }

    public void DelFriend(uint uuid)
    {
		COM_ContactInfo v = Get(uuid, friends_);
		if (v != null)
		{
			Del(uuid, friends_);

        	if(UpdateFriend!=null)
            	UpdateFriend(v,false);
		}

    }


    public void AddBlack(COM_ContactInfo v)
    {
        Add(v, blacks_);

        if(UpdateBlack!=null)
            UpdateBlack(v,true);
    }

    public COM_ContactInfo GetBlack(uint uuid)
    {
        return Get(uuid, blacks_);
    }

    public void DelBlack(uint uuid)
	{
		COM_ContactInfo v = Get(uuid, blacks_);
		if (v != null)
		{
			Del(uuid, blacks_);
        	if(UpdateBlack!=null)
           		 UpdateBlack(v,false);
		}

    }

	public void RequestContactInfoOk(COM_ContactInfo req)
	{
		if(req == null)
		{
			return;
		}
		if(FindFriendOkEvent!=null)
		{
			FindFriendOkEvent(req);
		}
	}


    public void AddOffen(COM_ContactInfo v)
    {
        Add(v, offens_);

        if(UpdateOffen!=null)
			UpdateOffen(v,true);
    }

    public COM_ContactInfo GetOffen(uint uuid)
    {
        return Get(uuid, offens_);
    }
    
    public void DelOffen(uint uuid)
    {
		COM_ContactInfo off = Get (uuid, offens_);

        if(UpdateOffen!=null)
			UpdateOffen(off,false);

		Del(uuid, offens_);
    }

	public void InitFrintList(COM_ContactInfo[] friends)
	{
        friends_.Clear();
        offens_.Clear();
		for(int i =0;i<friends.Length;i++)
		{
			friends_.Add(friends[i]);
			offens_.Add(friends[i]);
		}
	}

	public void InitBlackList(COM_ContactInfo[] blacks)
	{
		for(int i =0;i<blacks.Length;i++)
		{
			blacks_.Add(blacks[i]);
		}
	}


	public void FriendChatOk(COM_ContactInfo contact,COM_Chat msg)
	{

		COM_ContactInfo frined = Get (contact.name_, friends_);
		if(frined == null)
		{
			COM_ContactInfo offens = Get (contact.name_, offens_);
			if(offens == null)
			{
				AddOffen(contact);
				AddNoChat(contact.name_,msg);
			}
		}

		if(_chatDic.ContainsKey (contact.name_))
		{
			_chatDic [contact.name_].Add (msg);
		}
		else
		{
			List<COM_Chat> str = new List<COM_Chat>();
			str.Add(msg);
			_chatDic [contact.name_] =  str;
		}


		if (FriendChat != null)
		{
			FriendChat (contact,msg);
		}
	}

	public void addMyChat(string name,COM_Chat msg)
	{
		if(_chatDic.ContainsKey (name))
		{
			_chatDic [name].Add (msg);
		}
		else
		{
			List<COM_Chat> str = new List<COM_Chat>();
			str.Add(msg);
			_chatDic [name] =  str;
		}

	}


	public void AddNoChat(string name,COM_Chat msg)
	{
		if(_noChatDic.ContainsKey (name))
		{
			_noChatDic [name].Add (msg);
		}
		else
		{
			List<COM_Chat> str = new List<COM_Chat>();
			str.Add(msg);
			_noChatDic [name] =  str;
		}

	}

	public List<COM_Chat> NoChatCache(COM_ContactInfo contact)
	{
		if (_noChatDic.ContainsKey(contact.name_))
			return _noChatDic [contact.name_];
		
		return null;
	}



	public void OnFindFriendFail()
	{
		if (FindFriendFail != null)
				FindFriendFail ();
	}



	public List<COM_Chat> ChatCache(COM_ContactInfo contact)
	{
		if (_chatDic.ContainsKey(contact.name_))
			return _chatDic [contact.name_];

		return null;
	}

	public Dictionary<string,List<COM_Chat>> ChatDict
	{
		get
		{
			return _chatDic;
		}
	}
	public Dictionary<string,List<COM_Chat>> NoChatDict
	{
		get
		{
			return _noChatDic;
		}
	}

	public bool IsmyFriend(int id)
	{
        COM_ContactInfo ci = null;
		for ( int i=0; i < friends_.Count; ++i)
		{
            ci = (COM_ContactInfo)friends_[i];
            if (ci.instId_ == id)
			{
				return true;
			}
		}
		return false;
	}


	public COM_ContactInfo[] RecommendFriends
	{
		set
		{
			_recommendFriends = value;
			if(recommendEvent != null)
				recommendEvent(1);
		}
		get
		{
			return _recommendFriends;
		}

	}


	public void IsFriendOnLine(bool line)
	{
		if(friendOnLineEvent != null)
			friendOnLineEvent(line);
	}

	public void requestFriendList(COM_ContactInfo[] list)
	{
		friends_.Clear ();
		for(int i =0;i<list.Length;i++)
		{
			friends_.Add(list[i]);
		}
	}
}


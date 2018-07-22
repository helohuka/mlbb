using bin;
using UnityEngine;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class NetConnection : 
	Client2ServerStub,
	bin.IReader, 
	bin.IWriter
{

    public SocketWrapper Socket_ = new SocketWrapper();
    ushort MsgLength_ = 0;
    int MsgLengthPtr_ = 0;

    public System.AsyncCallback Socket_Callback;

	private static NetConnection inst_;
	public static NetConnection Instance
	{
		get
		{
			if(inst_ == null)
				inst_ = new NetConnection();
			return inst_;
		}
	}

	private ARPCProxy proxy_;

    private bool tipOnce_;

    public delegate void SocketHandler(int errCode);
    public event SocketHandler OnSocketError;

	public NetConnection()
	{
		proxy_ = new ARPCProxy ();
        Socket_ = new SocketWrapper();
	}

	public bool connect(string ipaddr, int port, System.AsyncCallback callback)
	{
       // if (!Socket_.IsWorking_)
        Socket_.Init(ipaddr, port);
        Socket_Callback = callback;
        disconnect();
        return Socket_.Connect(ipaddr, port);
	}

	public void disconnect()
	{
        Socket_.Deconnect();
	}

	public void Update()
	{
        if (!Socket_.IsWorking_)
            return;

        Socket_.DoOutgoing();
        Socket_.DoIncoming();

        if (Socket_.IncomingBuffer.Length != 0)
        {
             while (dispatch())
             {

             }
             Socket_.IncomingBuffer.Crunch();
        }
        

        if (Socket_.LastError != 0)
        {
            if(OnSocketError != null)
                OnSocketError(Socket_.LastError);
            Socket_.LastError = 0;
        }
	}

    public bool discard()
    {
        if (Socket_ == null)
            return true;
        return Socket_.Reset();
    }

	private bool dispatch()
	{
        //lock (Socket_.IncomingBuffer)
        //{
            if (Socket_.IncomingBuffer.Length < 2)
                return false;
            ushort msglen = System.BitConverter.ToUInt16(Socket_.IncomingBuffer.Memory, Socket_.IncomingBuffer.RdPtr());
            if (Socket_.IncomingBuffer.Length - 2 < msglen)
                return false;

            Socket_.IncomingBuffer.RdPtr(2);
            if (!Server2ClientDispatcher.dispatch(this, proxy_))
            {

                return false;
            }
        //}
		return true;
	}

	override protected bin.IWriter methodBegin()
	{
        //lock (Socket_.OutgoingBuffer)
        //{
            MsgLengthPtr_ = Socket_.OutgoingBuffer.WrPtr();
            MsgLength_ = 0;
            Socket_.OutgoingBuffer.WrPtr(2);       
        //}

		return this;
	}

	override protected void methodEnd()
	{
        byte[] len = System.BitConverter.GetBytes(MsgLength_);

        //lock (Socket_.OutgoingBuffer)
        //{
            System.Array.Copy(len,0, Socket_.OutgoingBuffer.Memory, MsgLengthPtr_, len.Length);
        //}

       
        Socket_.Flush();

    }

	public bool read(uint size, out byte[] data, out int startId)
	{
        
        //lock (Socket_.IncomingBuffer)
        //{
          
            data = Socket_.IncomingBuffer.Memory;
            startId = Socket_.IncomingBuffer.RdPtr();
            Socket_.IncomingBuffer.RdPtr((int)size);
           
        //}
        if (Socket_.IncomingBuffer.Length < 0)
            return false;
		return true;
	}

	public void write(byte[] data)
	{

        //lock (Socket_.OutgoingBuffer)
        //{
            if (Socket_.OutgoingBuffer.Space == 0)
                return;
            Array.Copy(data, 0, Socket_.OutgoingBuffer.Memory, Socket_.OutgoingBuffer.WrPtr(), data.Length);
            Socket_.OutgoingBuffer.WrPtr(data.Length);
        //}
        MsgLength_ += (ushort)data.Length;
	}

    public bool IsShutDown
    {
        get { return !Socket_.IsConneted || Socket_.LastError != 0; }
    }

    public bool SupportIPV4
    {
        get
        {
            try
            {
                string HostName = System.Net.Dns.GetHostName();
                System.Net.IPHostEntry IpEntry = System.Net.Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }

    public bool SupportIPV6
    {
        get
        {
            try
            {
                string HostName = System.Net.Dns.GetHostName();
                System.Net.IPHostEntry IpEntry = System.Net.Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
    }

    public static string GetIPv6(string mHost, string mPort)
    {
#if UNITY_IPHONE && !UNITY_EDITOR
		string mIPv6 = XyskIOSAPI.GetIPv6(mHost, mPort);
		return mIPv6;
#else
        return mHost + "&&ipv4";
#endif
    }

    public void getIPType(string serverIp, string serverPorts, out string newServerIp, out AddressFamily mIPType)
    {
        mIPType = AddressFamily.InterNetwork;
        newServerIp = serverIp;
        try
        {
            string mIPv6 = GetIPv6(serverIp, serverPorts);
            if (!string.IsNullOrEmpty(mIPv6))
            {
                string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
                if (m_StrTemp != null && m_StrTemp.Length >= 2)
                {
                    string IPType = m_StrTemp[1];
                    if (IPType == "ipv6")
                    {
                        newServerIp = m_StrTemp[0];
                        mIPType = AddressFamily.InterNetworkV6;
                    }
                }
            }
        }
        catch (Exception e)
        {
            ClientLog.Instance.Log("GetIPv6 error:" + e);
        }
    }

//    public bool transforScene(int sceneId)
//    {
//        if(GamePlayer.Instance.isInBattle == false)
//        {
////			if(CopyData.IsCopyScene(GameManager.SceneID))
////			{
////				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
////					NetConnection.Instance.exitCopy();
////					base.transforScene(sceneId);
////				});
////			}else
////			{
//                base.transforScene(sceneId);
////			}
//        }
//        return true;
//    }

    public bool move(float x, float z)
    {
        if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
            return true;
        Avatar player = Prebattle.Instance.GetSelf();
        if(player != null)
            player.PlayerStoped_ = false;
        base.move(x, z);
        return true;
    }

    public bool moveToNpc(int npcId)
    {
        if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
            return true;
        Avatar player = Prebattle.Instance.GetSelf();
        if (player != null)
            player.PlayerStoped_ = false;
        Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_AFP);
        base.moveToNpc(npcId);
        return true;
    }

    public bool moveToNpc2(NpcType type)
    {
        if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
            return true;
        Avatar player = Prebattle.Instance.GetSelf();
        if (player != null)
            player.PlayerStoped_ = false;
        Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_AFP);
        base.moveToNpc2(type);
        return true;
    }

    public bool moveToZone(int sceneId, int zoneId)
    {
        if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
            return true;
        Avatar player = Prebattle.Instance.GetSelf();
        if (player != null)
            player.PlayerStoped_ = false;
        Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_AFP);
        base.moveToZone(sceneId, zoneId);
        return true;
    }

    public bool stopMove()
    {
        Avatar player = Prebattle.Instance.GetSelf();
        if (player != null)
            player.PlayerStoped_ = true;
        base.stopMove();
        return true;
    }

    public bool stopAutoBattle()
    {
        Avatar player = Prebattle.Instance.GetSelf();
        if (player != null)
            player.PlayerStoped_ = true;
        base.stopAutoBattle();
        return true;
    }

    public bool logout()
    {
        GameManager.Instance.ClearCurrentState();
        base.logout();
        return true;
    }

	public bool requestPhoto()
	{
		GameManager.Instance.reconnectionLocker_ = true;
		base.requestPhoto();
		return true;
	}

    public bool delEquipment(uint target, uint itemInstId)
    {
        BagSystem.instance.recentlyTakeoffEquip = itemInstId;
        base.delEquipment(target, itemInstId);
        return true;
    }
}

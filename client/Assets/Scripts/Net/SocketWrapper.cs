using UnityEngine;
using System.Collections;
using System.Net.Sockets;

public class SocketWrapper {

    public bool supportipv4_, supportipv6_;

    string ipaddr_, port_;

    const int BUFFER_SIZE = 0XFFFF * 2 ;
    public SocketWrapper()
    {
        IncomingBuffer_ = new MessageBlock(BUFFER_SIZE);
        OutgoingBuffer_ = new MessageBlock(BUFFER_SIZE);
    }

    public string LocalAddress
    {
        get
        {
            System.Net.IPEndPoint ip = Socket_.LocalEndPoint as System.Net.IPEndPoint;
            return ip.Address.ToString();
        }
    }

    public void Init(string ipaddr, int port)
    {
        ipaddr_ = ipaddr;
        port_ = port.ToString();
        CreateSocket();

        //RecvBuffer_ = new MessageBlock(BUFFER_SIZE);
        //SendBuffer_ = new MessageBlock(BUFFER_SIZE * 2);

        //lock (IncomingBuffer_)
        //{
        //if (supportipv4_)
        //{
        //    Debug.Log("SupportIPV4");
            //Socket_ = new System.Net.Sockets.Socket(
            //    System.Net.Sockets.AddressFamily.InterNetwork,
            //    System.Net.Sockets.SocketType.Stream,
            //    System.Net.Sockets.ProtocolType.Tcp);
        //}
        //else if (supportipv6_)
        //{
        //    Debug.Log("SupportIPV6");
        //    Socket_ = new System.Net.Sockets.Socket(
        //        System.Net.Sockets.AddressFamily.InterNetworkV6,
        //        System.Net.Sockets.SocketType.Stream,
        //        System.Net.Sockets.ProtocolType.Tcp);
        //}
        //Socket_.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.SendTimeout, 75000);
        //Socket_.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.ReceiveTimeout, 75000);

        //EnableNoneBlock(true);
        //}
        //IncomingThread_ = new System.Threading.Thread(ImcomingThreadFunc);
        //OutgoingThread_ = new System.Threading.Thread(OutgoingThreadFunc);

       

        //IsWorking_ = true;
        //IncomingThread_.Start();
        //OutgoingThread_.Start();
    }

    void CreateSocket()
    {
        string newServerIp = "";
        AddressFamily newAddressFamily = AddressFamily.InterNetwork;
        NetConnection.Instance.getIPType(ipaddr_, port_, out newServerIp, out newAddressFamily);
        if (!string.IsNullOrEmpty(newServerIp)) { ipaddr_ = newServerIp; }
        Socket_ = new Socket(newAddressFamily, SocketType.Stream, ProtocolType.Tcp);

        Socket_.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.SendTimeout, 75000);
        Socket_.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, System.Net.Sockets.SocketOptionName.ReceiveTimeout, 75000);
        EnableNoneBlock(true);
        IsWorking_ = true;
    }

    bool timeoutflag;
    void BeginTimeout()
    {
        timeoutflag = true;
        GlobalInstanceFunction.Instance.Invoke(delegate
        {
            if (timeoutflag)
            {
                EndTimeout();
                LastError_ = 99001;
            }
        }, 5f);
    }

    void EndTimeout()
    {
        timeoutflag = false;
    }

    public bool Connect(string ipaddr, int port)
    {
        //lock (Socket_)
        //{
            try
            {
				//System.Net.IPAddress[] addr = System.Net.Dns.GetHostAddresses(ipaddr);
				//Socket_.Connect(new System.Net.IPEndPoint(addr[0], port));
				BeginTimeout();
                Socket_.BeginConnect(ipaddr_, int.Parse(port_), ConnectServerCallback, Socket_);
            }
            catch (System.Net.Sockets.SocketException se)
            {
                LastError_ = se.ErrorCode;
                return false;
            }
            return true;
        //}
    }

    private void ConnectServerCallback(System.IAsyncResult ar)
    {
        try
        {
            EndTimeout();
            Socket_.EndConnect(ar);
            GlobalInstanceFunction.Instance.Invoke(() =>
            {
                if (NetConnection.Instance.Socket_Callback != null)
                {
                    NetConnection.Instance.Socket_Callback(ar);
                    NetConnection.Instance.Socket_Callback = null;
                }
            }, 1);
        }
        catch (System.Net.Sockets.SocketException se)
        {
            ClientLog.Instance.Log(se.Message);
            GlobalInstanceFunction.Instance.Invoke(() =>
            {
                if (NetConnection.Instance.Socket_Callback != null)
                {
                    NetConnection.Instance.Socket_Callback(null);
                    NetConnection.Instance.Socket_Callback = null;
                    LastError_ = se.ErrorCode;
                }
            }, 1);
        }
    }

    public void Deconnect()
    {
        //lock (Socket_)
        //{
            try
            {
                Socket_.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                Socket_.Close();
            }
            catch (System.Net.Sockets.SocketException se)
            {
                CreateSocket();
                return;
            }
            CreateSocket();
        //}
    }

    public void EnableDebug(bool v)
    {
        //lock (Socket_)
        //{
            Socket_.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Tcp, System.Net.Sockets.SocketOptionName.Debug, v);
        //}
    }

    public void EnableNoneBlock(bool v)
    {
        //lock (Socket_)
        //{
            Socket_.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Tcp, System.Net.Sockets.SocketOptionName.NoDelay, v);
        //}
    }
    public void Fini()
    {
        IsWorking_ = false;
        Deconnect();
        //IncomingThread_.Join();
        //OutgoingThread_.Join();
        //lock (Socket_)
        //{
            Socket_ = null;
        //}
        //IncomingThread_ = null;
        //OutgoingThread_ = null;
        IncomingBuffer_ = null;
        OutgoingBuffer_ = null;
    }

    public void DoIncoming()
    {
        //lock (Socket_)
        //{
            if (!Socket_.Connected)
                return;
            if (Socket_.Poll(0, System.Net.Sockets.SelectMode.SelectRead))
            {
                try
                {
                    int recved = Socket_.Receive(IncomingBuffer_.Memory, IncomingBuffer_.WrPtr(), IncomingBuffer_.Space, System.Net.Sockets.SocketFlags.None);
                    IncomingBuffer_.WrPtr(recved);
                    TotalIncoming_ += recved;
                   // ClientLog.Instance.LogError("Incoming once");  
                }
                catch (System.Net.Sockets.SocketException se)
                {
                    ClientLog.Instance.Log("Exception at DoIncoming" + se.ErrorCode);
                    LastError_ = se.ErrorCode;
                }
            }
        //}

        //lock (IncomingBuffer_)
        //{
            //if (RecvBuffer_.Length != 0)
            //{
            //    //解压
            //    RecvBuffer_.Decompression(IncomingBuffer_);

            //}
        //}
       
    }
   

    //private void ImcomingThreadFunc()
    //{
    //    do
    //    {
    //        DoIncoming();
    //        System.Threading.Thread.Sleep(1);
    //    } while (IsWorking_);
    //}

    public void DoOutgoing()
    {

        if (!CanFlush_)
            return;
        //lock (Socket_)
        //{
            if (!Socket_.Connected)
                return;
            if (OutgoingBuffer_.Length != 0)
            {
                //发送
                if (Socket_.Poll(0, System.Net.Sockets.SelectMode.SelectWrite))
                {
                    try
                    {
                        int sended = Socket_.Send(OutgoingBuffer_.Memory, OutgoingBuffer_.RdPtr(), OutgoingBuffer_.Length, System.Net.Sockets.SocketFlags.None);
                        OutgoingBuffer_.RdPtr(sended);
                        OutgoingBuffer_.Crunch();
                        TotalOutgoing_ += sended;

                        CanFlush_ = false;
                    }
                    catch (System.Net.Sockets.SocketException se)
                    {
                        ClientLog.Instance.Log("Exception at DoOutgoing" + se.ErrorCode);
                        LastError_ = se.ErrorCode;
                    }
                }
            }
        //}
    }

    //private void OutgoingThreadFunc()
    //{
    //    do
    //    {
    //        DoOutgoing();
    //        System.Threading.Thread.Sleep(1);
    //    } while (IsWorking_);
    //}

    public void Flush()
    {
        //lock (OutgoingBuffer_)
        //{
            CanFlush_ = true;
        //}
    }

    public MessageBlock IncomingBuffer
    {
        get
        {
            return IncomingBuffer_;
        }
    }

    public MessageBlock OutgoingBuffer
    {
        get
        {
            return OutgoingBuffer_;
        }
    }

    public int TotalOutgoing
    {
        get
        {
            return TotalOutgoing_;
        }

    }

    public int TotalIncoming
    {
        get
        {
            return TotalIncoming_;
        }
     
    }

    public int LastError
    {
        set { LastError_ = value; }
        get
        {
            return LastError_;
        }
    }

    public bool IsConneted
    {
        get
        {
            return Socket_.Connected;
        }
    }

    public bool Reset()
    {
        if (!IsWorking_)
            return true;

        bool hasData = false;

        while (IsConneted && LastError == 0)
        {
            if (Socket_.Poll(0, System.Net.Sockets.SelectMode.SelectRead))
            {
                try
                {
                    if (0 >= Socket_.Receive(IncomingBuffer_.Memory, IncomingBuffer_.WrPtr(), IncomingBuffer_.Space, System.Net.Sockets.SocketFlags.None))
                        break;
                    else
                        hasData = true;
                }
                catch (System.Net.Sockets.SocketException se)
                {
                    ClientLog.Instance.Log("Exception at Reset" + se.ErrorCode);
                    LastError_ = se.ErrorCode;
                    break;
                }
            }
            else
                break;
        }
        IncomingBuffer_.Reset();
        OutgoingBuffer_.Reset();
        return hasData;
    }

    //
    public bool IsWorking_;
    bool CanFlush_;
    int LastError_;
    int TotalOutgoing_;
    int TotalIncoming_;
    System.Net.Sockets.Socket Socket_;
    //System.Threading.Thread IncomingThread_;
    //System.Threading.Thread OutgoingThread_;
    System.IO.Compression.GZipStream IncomingGzip_;
    System.IO.Compression.GZipStream OutgoingGzip_;
    MessageBlock IncomingBuffer_;
    MessageBlock OutgoingBuffer_;
    //MessageBlock RecvBuffer_;
    //MessageBlock SendBuffer_;

    
}

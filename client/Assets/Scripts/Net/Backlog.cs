

using bin;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class Backlog:
    BacklogStub,
    bin.IWriter
{
    Socket socket_ = null;
    const int SendBufSize = 16384;
    byte[] sendBuf_ = new byte[SendBufSize];
    int sendBufWPtr_;
    int sendMsgLen_;


    private static Backlog inst_;
    public static Backlog Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new Backlog();
            return inst_;
        }
    }

    override protected bin.IWriter methodBegin()
    {
        sendBufWPtr_ = 0;
        sendMsgLen_ = 2;
        string host;
        int port;
        GlobalValue.Get(Constant.C_BacklogHost, out host);
        GlobalValue.Get(Constant.C_BacklogPort, out port);
        if (socket_ == null || !socket_.Connected)
        {
            socket_ = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket_.Connect(new IPEndPoint(IPAddress.Parse(host), port));
            }
            catch (Exception e)
            {
                socket_ = null;
            }
        }
       
        return this;
    }

    override protected void methodEnd()
    {
        if (null == socket_ || !socket_.Connected) return;

        byte[] b = BitConverter.GetBytes((ushort)(sendMsgLen_ - 2));
        Array.Copy(b, 0, sendBuf_, sendBufWPtr_, 2);
        sendBufWPtr_ += sendMsgLen_;
        
        if (socket_.Poll(0, SelectMode.SelectWrite))
        {
            try
            {
                int sended = socket_.Send(sendBuf_, sendBufWPtr_, SocketFlags.None);
                sendBufWPtr_ -= sended;
            }
            catch (Exception e)
            {
                socket_.Shutdown(SocketShutdown.Both);
                socket_.Close();
                socket_ = null;
            }
        }
    }

    public void write(byte[] data)
    {
        Array.Copy(data, 0, sendBuf_, sendBufWPtr_ + sendMsgLen_, data.Length);
        sendMsgLen_ += data.Length;
    }

}
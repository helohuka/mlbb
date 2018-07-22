using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;


public static class ChatSystem
{

	public delegate void PublishItemInstRes(COM_ShowItemInstInfo ShowItem, ChatKind Kind);
	public static PublishItemInstRes PublishItemInstResOk;

	public delegate void startAudio(int aid);
	public static startAudio startAudioOk;

    public delegate void finishAudio();
    public static finishAudio finishAudioOk;

	public delegate void PublishBabyInstRes(COM_ShowbabyInstInfo ShowBaby, ChatKind Kind);
	public static PublishBabyInstRes PublishBabyInstResOk;

	public delegate void ShowBabyInstRes(COM_ShowbabyInst babyInst);
	public static ShowBabyInstRes ShowBabyInstResOk;

	public delegate void ShowItemInstRes(COM_ShowItemInst ItemInst);
	public static ShowItemInstRes ShowItemInstResOk;
	public  delegate  void CloseAudioUI();
	public static CloseAudioUI CloseAudioUIOk;
	//static public string[]faceStr = {"#01","#02","#03","#04","#05","#06","#07","#08","#09","#10","#11","#12","#13","#14","#15","#16","#17","#18"};
	static public string[]faceStrl = {"{00}","{01}","{02}","{03}","{04}","{05}","{06}","{07}","{08}","{09}","{10}","{11}","{12}","{13}","{14}","{15}","{16}","{17}","{18}","{19}","{20}","{21}","{22}","{22}","{23}","{24}","{25}","{26}","{27}","{28}","{29}"};
	//static public List<string> faceStr_1 = new List<string>("{1}","{2}","{3}","{4}","{5}");
	//static public string[]faceSpStr = {"biaoqing01","biaoqing02","biaoqing03","biaoqing04","biaoqing05","biaoqing06","biaoqing07","biaoqing08","biaoqing09","biaoqing10","biaoqing11","biaoqing12","biaoqing13","biaoqing14","biaoqing15","biaoqing16","biaoqing17","biaoqing18"};
    static AudioClip AClip;
	static UIFont font;
    //AudioSource ASource = new AudioSource();
    public static AudioSource audios;
	public static float time = 0.0F;
    const int RecordTime = 15;
    const int Frequency = 14400;
    const int _RecordSize = 50;
    static int _RecordLength = 0;
    static LinkedList<COM_ChatInfo> _Records = new LinkedList<COM_ChatInfo>();
	public  const int chatPanleDeth = 1000;
    static int _SendRecordSize = 10;
    static LinkedList<string> _SendRecords = new LinkedList<string>();

	public static Dictionary<int,byte[]> AudioDic = new Dictionary<int, byte[]> ();

    public delegate void MakeDirtyFunc();
    static MakeDirtyFunc _MakeDirtyFunc;

    static public void RegMakeDirtyFunc(MakeDirtyFunc func){
        _MakeDirtyFunc += func;
    }

	public static  LinkedList<string> SendRecords
	{
		get
		{
			return _SendRecords;
		}
	}

    static public COM_ChatInfo GetLastestChat()
    {
        if (_Records.Last == null)
            return null;

        if (_Records.Last == null)
            return null;

        return _Records.Last.Value;
    }

    static List<byte> _AudioPiece = new List<byte>();
    public static void PushRecord(COM_ChatInfo info)
    {
        //if (info.audioId_ != 0)
        //{
        //    _AudioPiece.AddRange(info.audio_);

        //    if (info.audioId_ == -1)
        //    {
        //        info.audio_ = _AudioPiece.ToArray();
        //        _AudioPiece.Clear();
        //    }
        //    else
        //    {
        //        return;
        //    }

        //}

        ++_RecordLength;
        _Records.AddLast(info);
        if (_RecordLength > _RecordSize)
        {
            _Records.RemoveFirst();
        }
        if (_MakeDirtyFunc != null)
        {
            _MakeDirtyFunc();
        }
        
    }

    //player say
    public static void PushRecord(COM_Chat info)
    {
        COM_ChatInfo c = new COM_ChatInfo();
        c.ck_ = info.ck_;
        c.content_ = info.content_;
        //c.audioId_ = info.audioId_;
		c.audio_ = info.audio_;
        c.assetId_ = (ushort)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId);
        c.playerName_ = GamePlayer.Instance.InstName; //only player 
        PushRecord(c);
    }

    //system say 
    public static void PushRecord(ChatKind ck, string content)
    {
        COM_ChatInfo c = new COM_ChatInfo();
        c.ck_ = ck;
        c.content_ = content;
        PushRecord(c);
    }

	public static bool GetChannelInfo(bool[] channels, ref List<COM_ChatInfo> infos,ref int index)
    {
        if (channels == null || channels.Length == 0 || channels.Length != (int)ChatKind.CK_Max)
            return false;

        if (infos == null)
        {
            infos = new List<COM_ChatInfo>();
        }
        else
        {
            infos.Clear();
        }
		index = 0;
        for(LinkedListNode<COM_ChatInfo> n = _Records.First; n!=null; n = n.Next)
        {
            if (channels[(int)n.Value.ck_])
            {
                infos.Add(n.Value);
			    index = infos.IndexOf(n.Value);

            }
        }

        return true;
    }

    public static void PushSystemMessage(string content)
    {
        PushRecord(ChatKind.CK_System, content);
    }

    public static void PushPlayerSay(COM_Chat c)
    {
        PushRecord(c);
    }
	public static void AudioOk(int id, byte[] content)
	{
		if(!AudioDic.ContainsKey(id))
		{
			AudioDic.Add (id,content);
		}

        SoundTools.PauseMusic();
        SoundTools.PauseSound();
        ChatSystem.PlayRecord(content, audios, delegate
        {
            GlobalInstanceFunction.Instance.Invoke(FinishPlayRecord, audios.clip.length);
        });

		if(startAudioOk != null)
		{
			startAudioOk(id);
		}
	}

    public static void FinishPlayRecord()
    {
        audios.Stop();
        SoundTools.ResumeMusic();
        SoundTools.ResumeSound();
        if (finishAudioOk != null)
            finishAudioOk();
    }

    public static bool isPlayingAudio
    {
        get
        {
            if (audios != null)
            {
                return audios.isPlaying;
            }
            return false;
        }
    }
    
    public static void SendToServer(ChatKind ck ,string target, string content, byte[] audio = null , int audioTime=  0)
    {

		if(content ==""&&target == ""&&audio == null)
		{
			return;
		}
        _SendRecords.AddLast(content);
        if (_SendRecords.Count > _SendRecordSize)
        {
            _SendRecords.RemoveFirst();
        }

        COM_Chat cc = new COM_Chat();
        cc.ck_ = ck;
        //if(content == "openlog")
        //{
        //    ApplicationEntry.Openlog = true; 
        //}
        //else if(content == "closelog")
        //{
        //    ApplicationEntry.Openlog = false;
        //}
        if (content.IndexOf("^") == 0)
        {
            cc.ck_ = ChatKind.CK_GM;
            cc.content_ = content.Substring(1);
        }
        else
        {
            cc.audio_ = audio;
			cc.audioTime_ = audioTime;
            cc.content_ = content;
            
        }
        NetConnection.Instance.sendChat(cc, target);        
    }

    //private static void SendOneAudioPiece()
    //{
    //    if (null == audio_)
    //        return;
    //    COM_Chat cc = new COM_Chat();
    //    cc.ck_ = audioKind_;
    //    if( (audioI_+1) * MaxAudioPieceSize > audio_.Length)
    //    {//结束包
    //        int l = audio_.Length - audioI_ * MaxAudioPieceSize;
    //        cc.audio_ = new byte[l];
    //        cc.audioId_ = -1;
    //        Array.Copy(audio_, audioI_ * MaxAudioPieceSize, cc.audio_, 0, l);

    //        NetConnection.Instance.sendChat(cc, audioTarget_);

    //        audio_ = null;
    //    }
    //    else
    //    {
    //        cc.audioId_ = audioL_ - audioI_ + 1;
    //        cc.audio_ = new byte[MaxAudioPieceSize];
    //        Array.Copy(audio_, audioI_ * MaxAudioPieceSize, cc.audio_, 0, MaxAudioPieceSize);

    //        NetConnection.Instance.sendChat(cc, audioTarget_);

    //        ++audioI_;
    //    }

    //}

    const int MaxAudioPieceSize = 2048;
    static byte[] audio_ = null;
    static int audioI_ = 0;
    static int audioL_ = 0;
    static string audioTarget_ = "";
    static ChatKind audioKind_;
    public static void Update()
    {
        //SendOneAudioPiece();
    }

	public static void publishItemInstRes(COM_ShowItemInstInfo ShowItem, ChatKind Kind)
	{
		if(PublishItemInstResOk != null)
		{
			PublishItemInstResOk(ShowItem,Kind);
		}
	}
	public static void publishBabyInstRes(COM_ShowbabyInstInfo InstInfo, ChatKind Kind)
	{
		if(PublishBabyInstResOk != null)
		{
			PublishBabyInstResOk(InstInfo,Kind);
		}
	}

	public static void queryItemInstRes(COM_ShowItemInst ItemInst)
	{
		if(ShowItemInstResOk != null)
		{
			ShowItemInstResOk(ItemInst);
		}
	}
	public static void queryBabyInstRes(COM_ShowbabyInst babyInst)
	{
		if(ShowBabyInstResOk != null)
		{
			ShowBabyInstResOk(babyInst);
		}
	}

    public static void StartRecord()
    {
        SoundTools.PauseMusic();
        SoundTools.PauseSound();
        AClip = Microphone.Start(null, false, RecordTime, Frequency); //14400 
        while (!(Microphone.GetPosition(null) > 0)) ;
        time = Time.realtimeSinceStartup;
        ClientLog.Instance.Log("StartRecord");
    }
    public static  void StopRecord()
    {
        SoundTools.ResumeMusic();
        SoundTools.ResumeSound();
        if (!Microphone.IsRecording(null))
        {
            return;
        }
        Microphone.End(null);

        time = Time.realtimeSinceStartup - time;
        time = time > RecordTime ? RecordTime : time;

        ClientLog.Instance.Log("Stop record clip time is " + time);
    }

    static NSpeex.SpeexEncoder SE = new NSpeex.SpeexEncoder(NSpeex.BandMode.Narrow);
    static NSpeex.SpeexJitterBuffer JB = new NSpeex.SpeexJitterBuffer(new NSpeex.SpeexDecoder(NSpeex.BandMode.Narrow));
    static float[] FS = new float[Frequency*RecordTime];
    static byte[] BS = new byte[Frequency * RecordTime];
	public static void GetClip(ref byte[] samples)
	{
        if (AClip == null)
        {
            ClientLog.Instance.Log("audioClip == null");
            return;
        }
        int length = Mathf.CeilToInt(time * AClip.frequency);
        length = length > AClip.samples ? AClip.samples : length;
        AClip.GetData(FS, 0);
        int eLeng = SE.Encode(FS, 0, length, BS, 0, length);
        samples = new byte[eLeng];
        Array.Copy(BS, 0, samples, 0, eLeng);
        ClientLog.Instance.Log("GetClip === " + eLeng + " bytes.");
	}
    public static void SetClip(byte[] samples)
    {
        JB.Put(samples);
        int oLeng = JB.Get(FS);
        AClip = AudioClip.Create("Record", oLeng, 1, Frequency, false, false);
        AClip.SetData(FS, 0);
    }
    //public static float GetClipLength(byte[] samples)
    //{
    //    SetClip(samples);
    //    return AClip.length;

    //}
   static int index;
	public delegate void PlayCallback();
	public static void PlayRecord(byte[] samples,AudioSource asource, PlayCallback callback = null)  
    {
        if (ChatSystem.AsyncSetCallBack != null)
            return;
        else
            ChatSystem.AsyncSetCallBack = delegate
            {
                if (asource != null)
                {
                    AClip.name = index++.ToString();
                    asource.clip = AClip;
                    asource.loop = false;
                    asource.Play();
                }
			    if(callback != null)
				    callback();
            };
        AsyncSet(samples);
        //SetClip(samples);
    }

    public static void AsyncSet(byte[] samples)
    {
        ChatSystem.waitSetData = samples;

        System.Threading.Thread athread = new System.Threading.Thread(new System.Threading.ThreadStart(AsyncSetClip));
        athread.IsBackground = true;
		athread.Start();
    }
	static int length;
    public static void AsyncGet()
    {
		if (AClip == null)
		{
			ClientLog.Instance.Log("audioClip == null");
			return;
		}
		length = Mathf.CeilToInt(time * AClip.frequency);
		length = length > AClip.samples ? AClip.samples : length;
		AClip.GetData(FS, 0);

        System.Threading.Thread athread = new System.Threading.Thread(new System.Threading.ThreadStart(AsyncGetClip));
        athread.IsBackground = true;
		athread.Start();
    }

    public delegate void AsyncGetClipHandler(byte[] samples);
    public static AsyncGetClipHandler AsyncGetCallBack;
    public static void AsyncGetClip()
	{
        int eLeng = SE.Encode(FS, 0, length, BS, 0, length);
        byte[] samples = new byte[eLeng];
        Array.Copy(BS, 0, samples, 0, eLeng);
        ClientLog.Instance.Log("GetClip === " + eLeng + " bytes.");
        if (ChatSystem.AsyncGetCallBack != null)
        {
            ChatSystem.AsyncGetCallBack(samples);
            ChatSystem.AsyncGetCallBack = null;
        }
    }

	static int oLeng;
    public delegate void AsyncSetClipHandler();
    public static AsyncSetClipHandler AsyncSetCallBack;
    public static byte[] waitSetData;
    public static void AsyncSetClip()
    {
        JB.Put(ChatSystem.waitSetData);
        oLeng = JB.Get(FS);
		GlobalInstanceFunction.Instance.Invoke(delegate
		{
			TempAsyncSetCallback();
		}, 1);
    }

	static void TempAsyncSetCallback()
	{
		AClip = AudioClip.Create("Record", oLeng, 1, Frequency, false, false);
		AClip.SetData(FS, 0);
		if (ChatSystem.AsyncSetCallBack != null)
		{
			ChatSystem.AsyncSetCallBack();
			ChatSystem.AsyncSetCallBack = null;
		}
	}
}
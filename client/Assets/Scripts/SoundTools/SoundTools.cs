using UnityEngine;
using System.Collections;

public class SoundTools
{
	public enum ESoundType
	{
		SOUNDTYPE_SOUNND , 
		SOUNDTYPE_MUSIC , 
		SOUNDTYPE_MAX , 
	}

	public class SoundPlayInfo
	{
		public string 		szName;
		public float		fTime;
		public AudioClip	SoundObj;
		public AudioSource	SoundPlayer;
		public ESoundType	eSoundType;

		public SoundPlayInfo( ESoundType	eType )
		{
			Clear();
			eSoundType = eType;
		}

		public void Clear()
		{
			szName = "";
			fTime = 0f;
			SoundObj = null;
			SoundPlayer = null;
		}
	}

	private	const int 	MAX_KEEP_SOUND = 4;
	private	const float SOUNDVOLME = 1f;
	static private	SoundPlayInfo[] 		m_SoundPlayers;
	static private 	SoundPlayInfo			m_MusicPlayer;
	static private 	bool					m_bSoundInited = false;
	static private 	bool					m_bMusicInited = false;
    static public bool settingDirty = true;

	static private void InitPlayMusic()
	{
		m_bMusicInited = true;
		m_MusicPlayer = new SoundPlayInfo( ESoundType.SOUNDTYPE_MUSIC );
		//
		GameObject Obj = GameObject.Find( "/CommonScript/SoundPlay/MusicPlayer" );
		if( null != Obj )
		{
			m_MusicPlayer.SoundPlayer = Obj.GetComponent<AudioSource>();
		}
	}

	static private void InitPlaySound()
	{
		m_bSoundInited = true;
		m_SoundPlayers = new SoundPlayInfo[MAX_KEEP_SOUND];

		for( int iLoop = 0; iLoop < MAX_KEEP_SOUND; ++iLoop )
		{
			m_SoundPlayers[iLoop] = new SoundPlayInfo( ESoundType.SOUNDTYPE_SOUNND );
			string szObjName = "/CommonScript/SoundPlay/SoundPlayer/SoundPlayer" + iLoop.ToString();
			GameObject	Obj = GameObject.Find( szObjName );
			if( null != Obj )
			{
				m_SoundPlayers[iLoop].SoundPlayer = Obj.GetComponent<AudioSource>();
			}
		}

        string yyp = "/CommonScript/SoundPlay/SoundPlayer/YuyinPlayer";
        GameObject yyObj = GameObject.Find(yyp);
        if (yyObj != null)
        {
            ChatSystem.audios = yyObj.GetComponent<AudioSource>();
        }
	}
	
	static public void PlaySound( SOUND_ID id )
	{
		if (PlayerPrefs.GetInt ("settingSound") == 1)
			return;
		PlaySound( id , false );
	}

	static public void PlaySound( SOUND_ID id , bool bLoop )
	{
		if (PlayerPrefs.GetInt ("settingSound") == 1)
			return;
		if( !m_bSoundInited )
		{
			InitPlaySound();
		}

		string szName = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_SOUND );
		if( !GlobalInstanceFunction.Instance.IsValidName( szName ) ) return ;

		for( int iLoop = 0; iLoop < m_SoundPlayers.Length; ++iLoop )
		{
			if( m_SoundPlayers[iLoop] != null && m_SoundPlayers[iLoop].szName == szName )
			{
				if( null != m_SoundPlayers[iLoop].SoundPlayer )
				{
					m_SoundPlayers[iLoop].SoundPlayer.volume = SOUNDVOLME;
					m_SoundPlayers[iLoop].SoundPlayer.loop = bLoop;
					m_SoundPlayers[iLoop].SoundPlayer.Play();
					return;
				}
			}
		}

		SoundAssetMgr.LoadAsset( szName , new ParamData( bLoop) );
	}

	static private SoundPlayInfo	GetMaxTimeInfo()
	{
		if( !m_bSoundInited )
		{
			InitPlaySound();
		}
		int 	iFindIndex = 0;
		float	fFindTime = m_SoundPlayers[iFindIndex].fTime;
		for( int iLoop = 0; iLoop < MAX_KEEP_SOUND; ++iLoop )
		{
			if(m_SoundPlayers[iLoop] != null && m_SoundPlayers[iLoop].fTime < fFindTime )
			{
				fFindTime = m_SoundPlayers[iLoop].fTime;
				iFindIndex = iLoop;
			}
		}
		return m_SoundPlayers[iFindIndex];
	}

	static public void StopSound( SOUND_ID	ID )
	{

	}
	static public void OpenSound()
	{
        PlayerPrefs.SetInt("settingSound", 0);
        settingDirty = true;
        if (m_SoundPlayers == null)
        {
            return;
        }

        for (int iLoop = 0; iLoop < m_SoundPlayers.Length; ++iLoop)
        {
            if (m_SoundPlayers[iLoop] != null && m_SoundPlayers[iLoop].SoundPlayer != null)
                m_SoundPlayers[iLoop].SoundPlayer.Play();
        }
	}

	static public void StopSound()
	{
        PlayerPrefs.SetInt("settingSound", 1);
        settingDirty = true;
        if (m_SoundPlayers == null)
        {
            return;
        }

		for( int iLoop = 0; iLoop < m_SoundPlayers.Length; ++iLoop )
		{
			if(m_SoundPlayers[iLoop] != null && m_SoundPlayers[iLoop].SoundPlayer != null)
				m_SoundPlayers[iLoop].SoundPlayer.Stop();
		}
	}

    static bool preSoundStatusOpen = false;
    static public void PauseSound()
    {
        if (m_SoundPlayers == null)
        {
            return;
        }

        preSoundStatusOpen = (PlayerPrefs.GetInt("settingSound") == 0);
        if (preSoundStatusOpen)
        {
            PlayerPrefs.SetInt("settingSound", 1);
            settingDirty = true;
            for (int iLoop = 0; iLoop < m_SoundPlayers.Length; ++iLoop)
            {
                if (m_SoundPlayers[iLoop] != null && m_SoundPlayers[iLoop].SoundPlayer != null)
                    m_SoundPlayers[iLoop].SoundPlayer.Stop();
            }
        }
    }

    static public void ResumeSound()
    {
        if (m_SoundPlayers == null)
        {
            return;
        }

        if (preSoundStatusOpen)
        {
            PlayerPrefs.SetInt("settingSound", 0);
            settingDirty = true;
            //for (int iLoop = 0; iLoop < m_SoundPlayers.Length; ++iLoop)
            //{
            //    if (m_SoundPlayers[iLoop] != null && m_SoundPlayers[iLoop].SoundPlayer != null)
            //        m_SoundPlayers[iLoop].SoundPlayer.Play();
            //}
        }
    }

	static public void PlayMusic( MUSIC_ID id )
	{
		if (PlayerPrefs.GetInt ("settingMusic") == 1)
			return;
		PlayMusic( id , true );
	}

	static private void PlayMusic(MUSIC_ID id , bool bLoop )
	{
		if (PlayerPrefs.GetInt ("settingMusic") == 1)
			return;
		if( !m_bMusicInited )
		{
			InitPlayMusic();
		}

		string szName = GlobalInstanceFunction.Instance.GetAssetsName( (int)id , AssetLoader.EAssetType.ASSET_MUSIC );
		if( !GlobalInstanceFunction.Instance.IsValidName( szName ) ) return ;

		if( szName == m_MusicPlayer.szName && m_MusicPlayer.SoundObj != null && m_MusicPlayer.SoundPlayer != null )
		{
			m_MusicPlayer.SoundPlayer.volume = SOUNDVOLME;
			m_MusicPlayer.SoundPlayer.loop = bLoop;
			m_MusicPlayer.SoundPlayer.Play();
			return ;
		}

		MusicAssetMgr.LoadAsset( szName , new ParamData( bLoop ) );
	}


	static public void OpenMusic()
	{
		PlayerPrefs.SetInt ("settingMusic", 0);
        settingDirty = true;
		int mucId = 0;
		if(GamePlayer.Instance.isInBattle)
		{
			if(Battle.Instance.battleType == BattleType.BT_PVE)
			{
				if(Battle.Instance.BattleID==0)
				{
					GlobalValue.Get(Constant.C_BossMuc, out mucId);
					PlayMusic((MUSIC_ID)mucId);
				}else
				{
					GlobalValue.Get(Constant.C_PutnMuc, out mucId);
					PlayMusic((MUSIC_ID)mucId);
				}
			}else
				if(Battle.Instance.battleType == BattleType.BT_PVP)
			{
				GlobalValue.Get(Constant.C_PvpMuc, out mucId);
				PlayMusic((MUSIC_ID)mucId);
			}
		}
		else
		{
			SceneData sd = SceneData.GetData (GameManager.SceneID);
			if(sd == null)
				return;
			MusicAssetsData mdata = MusicAssetsData.GetData (sd.M_ID);
			PlayMusic ((MUSIC_ID)mdata.id_);
		}

	}

	static public void StopMusic()
	{
        if (!m_bMusicInited)
        {
            return;
        }

		if( !m_bMusicInited || null == m_MusicPlayer || null == m_MusicPlayer.SoundPlayer ) 
			return ;

        if (m_MusicPlayer.SoundPlayer.isPlaying)
		    m_MusicPlayer.SoundPlayer.Stop();

		//if( Application.platform == RuntimePlatform.Android || Application.platform ==  RuntimePlatform.IPhonePlayer )
		//{
		//	if( null != m_MusicPlayer.SoundObj )
		//	{
			//	GameObject.DestroyImmediate( m_MusicPlayer.SoundObj , true );
			//}
		//}
		MusicAssetMgr.DeleteAsset( m_MusicPlayer.szName , true );
		m_MusicPlayer.SoundPlayer.clip = null;
		m_MusicPlayer.SoundObj = null;
		m_MusicPlayer.szName = "";
	}

    static bool preMusicStatusOpen = false;
    static public void PauseMusic()
    {
        if (!m_bMusicInited)
        {
            return;
        }

        preMusicStatusOpen = (PlayerPrefs.GetInt("settingMusic") == 0);

        if (!m_bMusicInited || null == m_MusicPlayer || null == m_MusicPlayer.SoundPlayer)
            return;

        if (preMusicStatusOpen)
        {
            m_MusicPlayer.SoundPlayer.Stop();
            PlayerPrefs.SetInt("settingMusic", 1);
        }
    }

    static public void ResumeMusic()
    {
        if (!m_bMusicInited)
        {
            return;
        }

        if (!m_bMusicInited || null == m_MusicPlayer || null == m_MusicPlayer.SoundPlayer)
            return;

        if (preMusicStatusOpen)
        {
            PlayerPrefs.SetInt("settingMusic", 0);
            m_MusicPlayer.SoundPlayer.Play();
        }
    }

	static public void SetStopMusic()
	{
		PlayerPrefs.SetInt ("settingMusic", 1);
        settingDirty = true;
		if( !m_bMusicInited || null == m_MusicPlayer || null == m_MusicPlayer.SoundPlayer ) 
			return ;
		m_MusicPlayer.SoundPlayer.Stop();
		
		/*if( Application.platform == RuntimePlatform.Android || Application.platform ==  RuntimePlatform.IPhonePlayer )
		{
			if( null != m_MusicPlayer.SoundObj )
			{
				GameObject.DestroyImmediate( m_MusicPlayer.SoundObj , true );
			}
		}
		*/
		MusicAssetMgr.DeleteAsset( m_MusicPlayer.szName , true );
		m_MusicPlayer.SoundPlayer.clip = null;
		m_MusicPlayer.SoundObj = null;
		m_MusicPlayer.szName = "";
	}


	static public void PlaySoundCallBack( AssetBundle AssetData , ParamData paramData )
	{
		if( null == AssetData || null == AssetData.mainAsset )
		{
			SoundAssetMgr.DeleteAsset( AssetData , true );
			return ;
		}

		AudioClip		SoundObj = AssetData.mainAsset as AudioClip;
		SoundPlayInfo	MaxTimeSound = GetMaxTimeInfo();

		if( null != SoundObj )
		{
			if( null != MaxTimeSound.SoundPlayer )
			{
				MaxTimeSound.SoundPlayer.Stop();
			}

			if( null != MaxTimeSound )
			{
				SoundAssetMgr.DeleteAsset( MaxTimeSound.szName , true );
			}

			if( null != paramData )
			{
				MaxTimeSound.szName = paramData.szAssetName;
				MaxTimeSound.fTime = Time.realtimeSinceStartup;
				MaxTimeSound.SoundObj = SoundObj;
				MaxTimeSound.SoundPlayer.clip = SoundObj;
				MaxTimeSound.SoundPlayer.volume = SOUNDVOLME;
				MaxTimeSound.SoundPlayer.loop = paramData.bParam;
				MaxTimeSound.SoundPlayer.Play();
			}
		}
	}

	static public void PlayMusicCallBack( AssetBundle AssetData , ParamData paramData )
	{
		if( null == AssetData || null == AssetData.mainAsset )
		{
			MusicAssetMgr.DeleteAsset( AssetData , true );
			return ;
		}

		//Delete Asset and Play new Music 
		if( null != paramData )
		{
			if( null != m_MusicPlayer.SoundPlayer )
			{
				StopMusic();
				AudioClip	Source = AssetData.mainAsset as AudioClip;
				m_MusicPlayer.SoundObj = Source;
				m_MusicPlayer.SoundPlayer.clip = Source;
				m_MusicPlayer.SoundPlayer.volume = SOUNDVOLME;
				m_MusicPlayer.SoundPlayer.loop = paramData.bParam;
				m_MusicPlayer.szName = paramData.szAssetName;
				m_MusicPlayer.SoundPlayer.Play();
			}
		}
	}
}




using UnityEngine;
using System.Collections;

public class PrebattleEvent {

	public delegate void AUTOBtnClick();
	public AUTOBtnClick AUTOEvent;
	public delegate void BackBtnClick();
	public BackBtnClick BackEvent;

	public delegate void ReceiveChat(COM_ChatInfo chatInfo);
	public ReceiveChat ReceiveEvent;


	public delegate void CaptureScreen (Texture2D tex);
	public CaptureScreen DoCaptureScreen;
	private static PrebattleEvent instance;
	public static PrebattleEvent getInstance {
		get {
			if (instance == null) {
				instance = new PrebattleEvent ();
			}
			return instance;
		}
	}
}

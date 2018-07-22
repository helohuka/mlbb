using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class playerGallery : MonoBehaviour {

	public UILabel playerID;
	public UILabel playerName;
	public UILabel playerzhiye;

	public UISprite [] icons;
	private COM_Item [] citem;
	private List<ItemData> itemDatas = new List<ItemData>();
	void Start () {
		citem = GamePlayer.Instance.Equips;
		for (int i = 0; i<citem.Length; i++)
		{
			if(citem[i]==null)continue;
			ItemData idata = ItemData.GetData((int)citem[i].itemId_);
			itemDatas.Add(idata);
		}
		for(int j = 0;j< itemDatas.Count&&j<icons.Length;j++)
		{
			icons[j].spriteName = itemDatas[j].icon_;
		}
		playerID.text = "ID :"+ GamePlayer.Instance.InstId.ToString();
		playerName.text = "名字 :"+GamePlayer.Instance.InstName;
		playerzhiye.text = "职业 :"+"弓箭手";
	}
	

}

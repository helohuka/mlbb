using UnityEngine;
using System.Collections;

public class Roleui : MonoBehaviour {

	public UISlider hp;
	public UISlider mp;
	public UISprite spN;
    public UISprite readyAnim;
	public UILabel nameLabel;
	public UILabel lvLbel;
    public UILabel skillName_;
	public UISprite numSp;
    public UISprite bubble;
    public UILabel content;
    private BattleActor selfActor_;
	private string[] iconNames = {"tf_hong0","tf_hong1","tf_hong2","tf_hong3","tf_hong4","tf_hong5","tf_hong6","tf_hong7","tf_hong8","tf_hong9"};
	private string[]  diaoNums = {"tf_lan0","tf_lan1","tf_lan2","tf_lan3","tf_lan4","tf_lan5","tf_lan6","tf_lan7","tf_lan8","tf_lan9"};
	private string[]  aspiriNums0 = {"tf_huang0","tf_huang1","tf_huang2","tf_huang3","tf_huang4","tf_huang5","tf_huang6","tf_huang7","tf_huang8","tf_huang9"};
	private bool isPlus = true;
	private string[] numbers = {"yidong","erdong"};
	//public Font f;
	void Start()
	{
		spN.transform.gameObject.SetActive (false);
        skillName_.gameObject.SetActive(false);
        skillName_.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        skillName_.transform.localScale = Vector3.one;
	}

    public void InitData(BattleActor entity)
    {
        selfActor_ = entity;
        nameLabel.text = string.Format("[b]{0}[-]", entity.battlePlayer.instName_);
        if (GamePlayer.Instance.InstId == entity.InstId ||
            (GamePlayer.Instance.BattleBaby != null && GamePlayer.Instance.BattleBaby.InstId == entity.InstId))
        {
            nameLabel.color = new Color32(255, 255, 47, 255);
        }
        else if (Battle.Instance.isEnemy(Battle.Instance.GetActorByInstId(entity.InstId).BattlePos))
        {
            nameLabel.color = new Color32(255, 144, 0, 255);
        }
        else
        {
            nameLabel.color = new Color32(66, 255, 253, 255);
        }
        lvLbel.text = "" + entity.battlePlayer.level_.ToString();
        hp.value = entity.battlePlayer.hpCrt_ * 1f / entity.battlePlayer.hpMax_ * 1f;
        mp.value = entity.battlePlayer.mpCrt_ * 1f / entity.battlePlayer.mpMax_ * 1f;
    }

	public void ValueChange(PropertyType type, int val, int hpMax, int mpMax, bool popText = true,bool isisCrit = false)
	{
		Color textColor = Color.black;
	
		if(type.Equals(PropertyType.PT_HpCurr))
		{
			if (val > 0) {
				isPlus = true;
			} else {
				isPlus = false;
			}
			//textColor = val > 0 ? Color.green : Color.red;
			BloodChanged(hpMax);
			if(popText)
				Poptext(Mathf.Abs(val), type,isisCrit);
		}
		else if(type.Equals(PropertyType.PT_MpCurr))
		{
			//textColor = Color.blue;
			MagicChanged(mpMax);
			if(popText)
				Poptext(Mathf.Abs(val), type,isisCrit);
		}

	}

    public void ShowSkill(int effId, Vector3 uiPos, float time = 2f)
    {
        EffectAPI.PlaySceneEffect((EFFECT_ID)effId, uiPos, null, null, true);
        //skillName_.text = name;
        //skillName_.transform.localPosition = uiPos;
        //skillName_.gameObject.SetActive(true);
        //GlobalInstanceFunction.Instance.Invoke(() =>
        //{
            
        //    if (skillName_ == null || skillName_.gameObject == null)
        //        return;
        //    skillName_.gameObject.SetActive(false);
        //}, time);
    }

    public void SetReady(bool visable)
    {
        readyAnim.gameObject.SetActive(visable);
    }

	private	void  BloodChanged(int maxvalue)
	{
        if (hp == null || selfActor_ == null || selfActor_.battlePlayer == null) return;
		hp.value = (selfActor_.battlePlayer.hpCrt_ *1f) /(maxvalue*1f);
	}
	private void MagicChanged(int maxvalue)
	{
        if (mp == null || selfActor_ == null || selfActor_.battlePlayer == null) return;
        mp.value = (selfActor_.battlePlayer.mpCrt_ * 1f) / (maxvalue * 1f);
	}
	private void Poptext(int v,PropertyType type,bool isCrit=false)
	{

		char [] nums = v.ToString ().ToCharArray (); 
		UISprite sp;
		TweenPosition tp;
		TweenScale ts;
		for (int i = 0; i<nums.Length; i++) {
			int index = int.Parse( nums[i].ToString());
			sp = GameObject.Instantiate(spN)as UISprite;
			sp.transform.gameObject.SetActive(true);
			tp = sp.GetComponent<TweenPosition>();
			ts = sp.GetComponent<TweenScale>();
			sp.transform.parent = transform;
            sp.transform.localPosition = GlobalInstanceFunction.WorldToUI(new Vector3(GetRoleObj().transform.position.x, (GetRoleObj().transform.position.y + GetRoleObj().collider.bounds.size.y) + 0.3f, GetRoleObj().transform.position.z));
			sp.transform.localPosition = new Vector2(sp.transform.localPosition.x + i * (sp.localSize.x+10),sp.transform.position.y+300);
			sp.transform.localScale = Vector3.one;
			if(type.Equals(PropertyType.PT_MpCurr))
			{
				sp.spriteName = diaoNums[index];
			}
			else
			{
				if(isPlus)
				{
					sp.spriteName = aspiriNums0[index];
					
				}else
				{
					sp.spriteName =iconNames [index];
				}
			}
			sp.MakePixelPerfect();
			tp.from = new Vector2(sp.transform.position.x + i * (sp.localSize.x),sp.transform.position.y+300);
			tp.to = new Vector2(sp.transform.position.x + i * (sp.localSize.x),sp.transform.position.y+400);
			tp.enabled = true;
			ts.enabled = isCrit;

			Destroy(sp.gameObject,1);
		}
	}
	public void DisplayNumberOfAttacks(bool isShow,int num)
	{
		if (isShow) 
		{
            int count = num - 1;
			if(count < 0 || count > numbers.Length)
				return;
            numSp.spriteName = numbers[count];
			numSp.transform.gameObject.SetActive (true);
		} 
		else 
		{
			numSp.transform.gameObject.SetActive (false);
		}
	}
	private GameObject role;
	public void SetRoleObj(GameObject obj)
	{
		role = obj;
	}
	private GameObject GetRoleObj()
	{
		return role;
	}

    void OnDestroy()
    {
        Destroy(skillName_.gameObject);
    }

    public void ChatBubble(string msg)
	{
        CancelInvoke("HideBubble");
        content.text = msg;
        //int wid = msg.Length > 10 ? 10 * 40 : msg.Length % 10 * 40;
        int hei = (msg.Length / 6 + (msg.Length % 6 > 0 ? 1 : 0)) * 40;
       // bubble.width = wid;
        bubble.height = hei+30;
        bubble.gameObject.SetActive(true);
		bubble.gameObject.transform.localPosition = new Vector3(GetRoleObj().transform.position.x-GetRoleObj().collider.bounds.size.x/2, (GetRoleObj().transform.position.y +320) , GetRoleObj().transform.position.z)/*GlobalInstanceFunction.WorldToUI(new Vector3(GetRoleObj().transform.position.x-GetRoleObj().collider.bounds.size.x/2, (GetRoleObj().transform.position.y + GetRoleObj().collider.bounds.size.y) , GetRoleObj().transform.position.z))*/;
        Invoke("HideBubble", 5f);
    }

    void HideBubble()
    {
        bubble.gameObject.SetActive(false);
    }
}

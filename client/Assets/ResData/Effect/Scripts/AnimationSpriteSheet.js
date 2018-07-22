#pragma strict

var uvX = 4;  
var uvY = 2; 
var fps = 24.0;
 
 
 var delayTime = 1.0f;
 
 var begin_ = false;
 
 var crtTime = 0f;
	
	// Use this for initialization
	function Start () {		
		gameObject.SetActiveRecursively(false);
		Invoke("DelayFunc", delayTime);
	}
	
	function DelayFunc()
	{
	begin_ = true;
		gameObject.SetActiveRecursively(true);
	}
 
function Update () {
 
if(!begin_)
return;
crtTime += Time.deltaTime;
	var index : int = crtTime * fps;

	index = index % (uvX * uvY);
 if("UI_guaguaka_yanhua" == gameObject.name)
Debug.Log("begin_" + index.ToString());
	var size = Vector2 (1.0 / uvX, 1.0 / uvY);

	var uIndex = index % uvX;
	var vIndex = index / uvX;
 	var offset = Vector2 (uIndex * size.x, 1.0 - size.y - vIndex * size.y);
 
	renderer.material.SetTextureOffset ("_MainTex", offset);
	renderer.material.SetTextureScale ("_MainTex", size);
}


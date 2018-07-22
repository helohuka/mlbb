Shader "Custom/Alpha" {
	Properties {
		_MainTex ("Texture(RGBA)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Transparent" }

		Pass 
    	{
      		Blend SrcAlpha  OneMinusSrcAlpha    // Alpha混合
	      	SetTexture [_MainTex]
    	}
	} 

}

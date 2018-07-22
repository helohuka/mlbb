Shader "Scene Manager/Pixelate Effect" {
	Properties {
		_BlockSize("Block Size", float) = 1
		_FadeOffset ("Fade Offset", range(0,1)) = 0		// 0 == image, 1 == black	
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	sampler2D _ScreenContent;
	half4 _ScreenContent_ST;
	float _BlockSize;
	float _FadeOffset;
				
	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	v2f vert(appdata_full v) {
		v2f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	
		o.uv.xy = TRANSFORM_TEX(v.texcoord, _ScreenContent);
		#if UNITY_UV_STARTS_AT_TOP
		o.uv.y = 1 - o.uv.y;
		#endif		
		return o; 
	}
	
	fixed4 frag(v2f i) : COLOR {	
		float dx = _BlockSize*(1./_ScreenParams.x);
    	float dy = _BlockSize*(1./_ScreenParams.y);
    	half2 pixelUV = half2(dx * floor(i.uv.x / dx), dy * floor(i.uv.y / dy));
		return tex2D(_ScreenContent, pixelUV) * (1 - _FadeOffset);
	}
	
	ENDCG        
	
	SubShader {
		Tags { "RenderType"="Opaque" }
		Lighting Off
		LOD 200
	
		GrabPass { "_ScreenContent" }
	
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	
	FallBack "Diffuse"
} 
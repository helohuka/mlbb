Shader "Scene Manager/Cartoon Effect" {
	Properties {
	    _BorderColor ("Border Color", Color) = (.5,0,0,1)
		_Distance ("Distance", float) = 0	
		_CenterX ("CenterX", float) = .5
		_CenterY ("CenterY", float) = .5
		_Background ("Background", 2D) = "black" {}		
	}
	
	CGINCLUDE
	#include "UnityCG.cginc"
	
	float4 _BorderColor;
	float _Distance;
	float _CenterX;
	float _CenterY;
	sampler2D _Background;
	half4 _Background_ST;	
							
	struct v2f {
		half4 pos : SV_POSITION;
		half2 uv : TEXCOORD0;
	};
	
	v2f vert(appdata_full v) {
		v2f o;	
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	
		o.uv.xy = TRANSFORM_TEX(v.texcoord, _Background);			
		#if UNITY_UV_STARTS_AT_TOP
		o.uv.y = 1 - o.uv.y;
		#endif
		return o; 
	}
	
	fixed4 frag(v2f i) : COLOR {	
		#if UNITY_UV_STARTS_AT_TOP
		float realY = _ScreenParams.y - _CenterY;
		#else
		float realY = _CenterY;
		#endif
		float distance = length (half2(i.uv.x * _ScreenParams.x, i.uv.y * _ScreenParams.y) - half2(_CenterX, realY));
		float4 screenColor = float4(0,0,0,0); // transparent black
		float delta = distance - _Distance; // < 0 for any pixel within the circle; 0 for any pixel at the circle; > 0 for any pixel outside of the circle
		fixed4 maskColor = lerp(_BorderColor, tex2D(_Background, i.uv.xy), clamp(delta / 5, 0, 1));
		return lerp(screenColor, maskColor, (clamp(delta, -15, 0) / 15) + 1);  // fade inside
	}
	
	ENDCG        
	
	SubShader {
		Tags { 
			"Queue" = "Transparent" 
			}
		Lighting Off
		LOD 200
	
		Pass {
			CGPROGRAM	
			#pragma vertex vert
			#pragma fragment frag	
			ENDCG
			// don't write the cutout pixels in the center
			Blend SrcAlpha OneMinusSrcAlpha
		}            
	}
	
	FallBack "Diffuse"
} 
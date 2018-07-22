// 
// Generic backbuffer copy effect. Used for many transitions.
// Draws the given geometry using a texture which contains
// the contents of the screen.
//

Shader "Scene Manager/Back Buffer Copy" {

		
	CGINCLUDE
	#include "UnityCG.cginc"
	
	sampler2D _ScreenContent;
	half4 _ScreenContent_ST;
				
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
		return tex2D(_ScreenContent, i.uv.xy);
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
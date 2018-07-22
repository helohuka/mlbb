// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECT' with 'tex2D'

Shader "Hidden/Desaturate Effect" {
Properties {
	_MainTex ("Base (RGB)", RECT) = "white" {}
	_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
	_Desat("Desaturate", Float) = 0.5
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform sampler2D _RampTex;
uniform float4 _RampOffset;
uniform float _Desat;

float4 frag (v2f_img i) : COLOR {
	float4 original = tex2D(_MainTex, i.uv);
	float grayscale = Luminance(original.rgb);
	float2 remap = float2 (grayscale, .5);
	float4 output = tex2D(_RampTex, remap);
	float invert = 1 - _Desat;
	output.r = original.r * invert + output.r * _Desat + _RampOffset.r;
	output.g = original.g * invert + output.g * _Desat + _RampOffset.g;
	output.b = original.b * invert + output.b * _Desat + _RampOffset.b;
	output.a = original.a;
	return output;
}
ENDCG

	}
}

Fallback off

}
Shader "Backface Rendered Bumped Diffuse" {
Properties {
_Color ("Main Color", Color) = (1,1,1,1)
_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
// We use the material in many passes by defining them in the subshader.
// Anything defined here becomes default values for all contained passes.
Material {
Diffuse [_Color]
Ambient [_Color]
}
Lighting On

// Set up alpha blending
Blend SrcAlpha OneMinusSrcAlpha

// Render the back facing parts of the object.
// If the object is convex, these will always be further away
// than the front-faces.
Pass {
Cull Front
SetTexture [_MainTex] {
Combine Primary * Texture
}
}
// Render the parts of the object facing us.
// If the object is convex, these will be closer than the
// back-faces.
Pass {
Cull Back
SetTexture [_MainTex] {
Combine Primary * Texture
}
}
}

FallBack "Diffuse"
}
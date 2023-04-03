// ----------------------------------------------------------------------------
// Glow 11
// Copyright © 2013 Sven Herrmann
// ----------------------------------------------------------------------------
Shader "Hidden/Glow 11/Compose" {
Properties {
    _MainTex ("", 2D) = "white" {}
    _Strength ("Strength", Float) = 1.0
    _ColorBuffer ("Color", 2D) = "" {}
}
       
        CGINCLUDE
       
        #include "UnityCG.cginc"
        struct v2f {
            half4 pos : POSITION;
            half2 uv : TEXCOORD0;
        };
       
        sampler2D _MainTex;
        uniform fixed _Strength;
       
        v2f vert( appdata_img v )
        {
            v2f o;
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            o.uv = v.texcoord.xy;
            return o;
        }
       
        fixed4 frag(v2f pixelData) : COLOR
        {
            return tex2D(_MainTex, pixelData.uv) * _Strength;
        }
       
       
        ENDCG
       
    Subshader {
        Pass {
            // Additive
            Name "Add"
            Blend One One
            ZTest Always Cull Off ZWrite Off Fog { Mode Off }
            ColorMask RGB
            
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
        
        Pass {
            // Screen
            Name "Screen"
            Blend One OneMinusSrcColor          
            ZTest Always Cull Off ZWrite Off Fog { Mode Off }
            ColorMask RGB
            
            CGPROGRAM
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }      
    }
    
    Fallback off
}
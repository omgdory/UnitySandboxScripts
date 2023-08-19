Shader "Unlit/TexturedShader"
{
    Properties
    {
        // specifies 2D texture (there is also 3D and cubemaps)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            #define PI 3.14159265358979323

            sampler2D _MainTex;
            float4 _MainTex_ST; // optional -- tiling and offset
            /* magical var name; if it is sampler2D name + "_ST", then it will make it
                the tiling and offset thing */
            // ----------------------------------------------------------
            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv0 : TEXCOORD0;
            };
            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            // ----------------------------------------------------------
            Interpolators vert (Attributes v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv0, _MainTex);
                // o.uv.x += _Time.y*0.1; // adding to uv coordinate means offset
                // ^^ function scales and offsetts uv coords (i.e. also optional)
                return o;
            }
            // ----------------------------------------------------------
            float4 frag (Interpolators i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                // picks color from the sampled texture
                return col;
            }
            ENDCG
        }
    }
}

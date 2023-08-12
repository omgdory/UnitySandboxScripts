Shader "Unlit/MyFirstShader"
{
    // https://www.youtube.com/watch?v=kfM-yu0iQBk&t=127s
    Properties
    {
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (0,0,0,1)

        _ColorStart ("Color Start", Range(0,1) ) = 0
        _ColorEnd ("Color End", Range(0,1) ) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

            // gets data from vertex
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // data to be transferred from vertex shader to fragment shader
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // makes clip position of the vertex be the unity object
                o.vertex = UnityObjectToClipPos(v.vertex);
                // passes uv to fragment shader
                o.uv = v.uv;

                return o;
            }

            float InverseLerp( float a, float b, float t ) {
                return (t-a)/(b-a);
            }

            // fragment shader
            float4 frag (v2f i) : SV_Target
            {
                float t = saturate(InverseLerp(_ColorStart, _ColorEnd, i.uv.x));
                float4 outColor = lerp(_ColorA, _ColorB, t);

                return outColor;
            }
            ENDCG
        }
    }
}

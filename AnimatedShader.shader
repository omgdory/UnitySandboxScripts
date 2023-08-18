Shader "Unlit/AnimatedShader"
{
    // https://www.youtube.com/watch?v=kfM-yu0iQBk&list=WL&index=4&t=9624s
    Properties
    {
        _ColorA ("Color A", Color) = (1,1,1,1)
        _ColorB ("Color B", Color) = (0,0,0,1)
        _Rotation ("Horizontal", Range(0,1)) = 0
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        LOD 100

        Pass
        {
            // tags
            Cull Off
            ZWrite Off
            ZTest LEqual
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define PI 3.14159265358979323

            float4 _ColorA;
            float4 _ColorB;
            float _Rotation;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normals : NORMAL;
                float4 uv0 : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION; // clip space position
                float3 normal : TEXCOORD0;
                float2 uv : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal( v.normals );
                o.uv = v.uv0;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                // cosine wave along the horizontals of the uv map
                float xOffset = cos(i.uv.x * 2 * PI * 8) * 0.01;

                float t = 0;

                t = cos( ((i.uv.x*_Rotation + i.uv.y*(1-_Rotation)) + xOffset - _Time.y * 0.1) * 2 * PI * 5 ) * 0.5 + 0.5;
                // fragment gets darker as it is rendered higher on the uv map
                t *= 1-i.uv.y;
                // Only render if normal is not pointing up or down
                float topBottomRemover = (abs(i.normal.y) < 0.999f);
                
                float waves = t * topBottomRemover;

                float4 gradient = lerp(_ColorA, _ColorB, i.uv.y);

                return gradient * waves;
            }
            ENDCG
        }
    }
}

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
        // "RenderType"="Opaque" is default
        Tags {
            "RenderType"="Transparent" // tag for render pipeline and post-processing
            "Queue"="Transparent" // changes the render order
        }

        Pass
        {
            // additive blending "Blend x y"
            //  where color = (src*x)+(dst*y)
            
            Cull Off // will render all sides of mesh (default is "Cull Back")
            ZWrite Off // do not write to depth buffer (anything that writes to depthbuffer will render in front)
            //ZTest LEqual // if less than or equal to what is already written to depth buffer, then show it (default)
            //ZTest GEqual // will only render if behind something (very useful!!!)

            // Blend One One // additive (lighter); (src*1)+(dst*1)
            Blend DstColor Zero // multiplicative (darker); (src*dst)+(dst*0)

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define PI 3.14159265358979323

            float4 _ColorA;
            float4 _ColorB;
            float _ColorStart;
            float _ColorEnd;

            // gets data from vertex
            struct appdata
            {
                float4 vertex : POSITION;
                // TEXCOORD channels here specifically refers to UV channels
                float2 uv : TEXCOORD0;
            };

            // data to be transferred from vertex shader to fragment shader
            struct v2f
            {
                float4 vertex : SV_POSITION;
                // TEXCOORD channels here can be anything; they are unrelated to "appdata" TEXCOORD channels
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

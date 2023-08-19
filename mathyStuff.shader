Shader "Custom/MathyStuff"
{
    Properties
    {
        // nothing yet
    }

    SubShader
    {
        Tags
        { "Queue" = "Geometry" } // first opaque to be rendered after "background"

        Pass
        {
            HLSLPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex Vert
            #pragma fragment Frag

            struct Attributes{
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators{
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators Vert(Attributes a) {
                Interpolators i;
                i.positionCS = UnityObjectToClipPos(a.positionOS);
                i.uv = a.uv;
                return i;
            }

            float4 Frag(Interpolators i) : SV_Target {
                float t = sin(i.uv.x) + sin(i.uv.y);
                return t;
                //return float4(x, y, 0, 1);
            }

            ENDHLSL
        }
    }
}
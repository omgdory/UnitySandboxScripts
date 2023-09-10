Shader "Custom/RippleShader"
{
    // https://www.youtube.com/watch?v=kfM-yu0iQBk&list=WL&index=5&t=9632s
    Properties
    {
        _MainTex ("Texture", 2D) = "grey" {}

        _wavelengthX ("Wavelength X", Range(0,30)) = 10
        _wavelengthY ("Wavelength Y", Range(0,30)) = 10
        _WaveAmplitude ("Wave Amplitude", Range(0,5)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            // Tags
            Cull Off

            HLSLPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex Vert
            #pragma fragment Frag

            #define PI 3.14159265358979323

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _wavelengthX;
            float _wavelengthY;
            float _WaveAmplitude;
            // ----------------------------------------------------------------------------
            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv0 : TEXCOORD0;
            };
            struct Interpolators {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            // ----------------------------------------------------------------------------
            float GetWave(float2 uv) {
                float2 uvsCentered = uv * 2 - 1; // -1 to 1
                // ^^ essentially starts coloring at 0.5
                float radialDistance = length( uvsCentered );
                // ^^ distance from center (magnitude of UV coordinate vector)
                float wave = cos( (radialDistance - _Time.y*0.1) * PI * 10) * 0.5 + 0.5;
                wave *= 1-radialDistance; // fade out as ripple gets farther
                return wave;
            }
            // ----------------------------------------------------------------------------
            Interpolators Vert(Attributes a) {
                Interpolators i;
                // vertex offset
                // float x_wave = cos( (a.uv0.x) * _wavelengthX - _Time.y*0.5) * _WaveAmplitude;
                // float y_wave = cos( (a.uv0.y) * _wavelengthY - _Time.y*0.5) * _WaveAmplitude;
                // a.positionOS.y = x_wave * y_wave;

                a.positionOS.y = GetWave(a.uv0) * _WaveAmplitude;
                i.positionCS = UnityObjectToClipPos(a.positionOS);
                i.uv = TRANSFORM_TEX(a.uv0, _MainTex);
                return i; 
            }
            // ----------------------------------------------------------------------------
            float4 Frag(Interpolators i) : SV_Target {
                // my attempt at rippling:
                // float ripple = frac((pow(abs(0.5-i.uv.x),2) + pow(abs(0.5-i.uv.y),2)));
                // float wave = cos(ripple * PI * _wavelengthX - _Time.y) * 0.5 + 0.5;
                // return wave;
                
                // freya's solution
                //return GetWave(i.uv);
                
                // adding texture
                float4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            // ----------------------------------------------------------------------------
            ENDHLSL
        }
    }
}
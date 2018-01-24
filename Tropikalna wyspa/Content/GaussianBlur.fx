sampler tex;
float2 Offsets[15];
float Weights[15];

void PixelShaderFunction(inout float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	color = float4(0, 0, 0, 1);
	for (int i = 0; i < 15; i++)
		color += tex2D(tex, texCoord + Offsets[i]) * Weights[i];
}
technique Tech
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}

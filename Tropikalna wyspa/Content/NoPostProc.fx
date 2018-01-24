sampler tex;

void PixelShaderFunction(inout float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	color = tex2D(tex, texCoord);
}
technique Tech
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}

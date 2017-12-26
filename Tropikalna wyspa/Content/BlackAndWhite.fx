sampler tex;

void PixelShaderFunction(inout float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	color = tex2D(tex, texCoord);
	float intensity = 0.3*color.r + 0.59*color.g + 0.11*color.b;
	color = float4(intensity, intensity, intensity, 1.0f);
}
technique Grayscale
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}

texture2D blurred; 
sampler2D blurredSampler = sampler_state {
	Texture = <blurred>;
	MagFilter = Linear;
	MinFilter = None;
	MipFilter = None;
	AddressU = Wrap;
	AddressV = Wrap;
};
texture2D clear;
sampler2D clearSampler = sampler_state {
	Texture = <clear>;
	MagFilter = Linear;
	MinFilter = None;
	MipFilter = None;
	AddressU = Wrap;
	AddressV = Wrap;
};
texture2D depth;
sampler2D depthSampler = sampler_state {
	Texture = <depth>;
	MagFilter = Linear;
	MinFilter = None;
	MipFilter = None;
	AddressU = Wrap;
	AddressV = Wrap;
};

float MaxDepth;
// Distance at which blur starts
float Focus;

void PixelShaderFunction(inout float4 color : COLOR0, float2 texCoord : TEXCOORD0)
{
	float Fading = Focus/4;
	// Determine depth
	float depth = tex2D(blurredSampler, texCoord).r * MaxDepth;
	float4 depthTex = tex2D(blurredSampler, texCoord);
	// Get blurred and unblurred render of scene
	float4 unblurred = tex2D(clearSampler, texCoord);
	float4 blurred = tex2D(depthSampler, texCoord);
	// Determine blur amount (similar to fog calculation)
	float blurAmt = clamp(abs((depth - Focus) / Fading),0, 1);
	// Blend between unblurred and blurred images
	color = lerp(unblurred, blurred, blurAmt);
}
technique Tech
{
	pass Pass1
	{
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}

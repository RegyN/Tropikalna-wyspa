float4x4 WorldMatrix;
float4x4 ViewMatrix;
float4x4 ProjectionMatrix;

float MaxDepth = 50;
struct VertexShaderInput
{
	float4 Position : POSITION0;
};
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float Depth : TEXCOORD0;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	// Output position and depth
	output.Position = mul(input.Position, mul(WorldMatrix, mul(ViewMatrix, ProjectionMatrix)));
	output.Depth = output.Position.z;
	return output;
}
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Return depth, scaled/clamped to [0, 1]
	float depth = input.Depth / MaxDepth;
	return float4(depth, depth, depth, 1);
}
technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}
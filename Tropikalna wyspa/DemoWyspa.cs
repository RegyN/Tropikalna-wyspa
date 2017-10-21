using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace Tropikalna_wyspa
{
    public class DemoWyspa : Game
    {
        GraphicsDeviceManager graphics;
        KeyboardState prevState;
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix worldMatrix;
        Matrix viewMatrix;

        BasicEffect basicEffect;

        VertexPositionColor[] triangleVertices;
        VertexBuffer vertexBuffer;

        bool orbit;

        public DemoWyspa()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            prevState = Keyboard.GetState();
            base.Initialize();

            //Setup camera
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -100f);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45f),
                GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);

            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);

            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.Forward, Vector3.Up);

            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1.0f;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            //Create our triangle
            triangleVertices = new VertexPositionColor[3];
            triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
            triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);

            vertexBuffer = new VertexBuffer(GraphicsDevice, 
                typeof(VertexPositionColor), 
                3, 
                BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(triangleVertices);
        }
        
        protected override void LoadContent()
        {

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                KeyboardState state = Keyboard.GetState();
                if (state.IsKeyDown(Keys.Right))
                {
                    camPosition.X -= 1f;
                    camTarget.X -= 1f;
                }
                if (state.IsKeyDown(Keys.Left))
                {
                    camPosition.X += 1f;
                    camTarget.X += 1f;
                }
                if (state.IsKeyDown(Keys.Down))
                {
                    camPosition.Y -= 1f;
                    camTarget.Y -= 1f;
                }
                if (state.IsKeyDown(Keys.Up))
                {
                    camPosition.Y += 1f;
                    camTarget.Y += 1f;
                }
                if (state.IsKeyDown(Keys.OemPlus))
                {
                    camPosition.Z += 1f;
                }
                if (state.IsKeyDown(Keys.OemMinus))
                {
                    camPosition.Z -= 1f;
                }
                if(state.IsKeyDown(Keys.Space))
                {
                    orbit = !orbit;
                }
                if(orbit)
                {
                    Matrix rotationMatrix = Matrix.CreateRotationY(
                        MathHelper.ToRadians(1f));
                    camPosition = Vector3.Transform(camPosition, rotationMatrix);
                }
                viewMatrix = Matrix.CreateLookAt(camPosition, camTarget, Vector3.Up);
                
                MouseState mstate = Mouse.GetState();

                base.Update(gameTime);
                prevState = state;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            // Wyłącz wycinanie tylnych ścian
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (var pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            base.Draw(gameTime);
        }
    }
}
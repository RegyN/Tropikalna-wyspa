using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Collections.Generic;

namespace Tropikalna_wyspa
{
    public class DemoWyspa : Game
    {
        GraphicsDeviceManager graphics;

        KeyboardState prevKState;
        MouseState prevMState;
        Matrix projectionMatrix;
        List<Object3D> obiekty;

        Camera3D kamera;

        Model palma;
        
        public DemoWyspa()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            prevKState = Keyboard.GetState();

            base.Initialize();

            //Setup Camera
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), graphics.
                               GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);


            obiekty = new List<Object3D>();
            obiekty.Add(new Object3D(palma, new Vector3(0f, 0f, 0f)));

            kamera = new Camera3D(new Vector3(0f, 5f, 15f), Vector3.Forward, Vector3.Up, projectionMatrix);
            //PrzygotujTrojkat();
        }


        protected override void LoadContent()
        {
            palma = Content.Load<Model>("Palma");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                SprawdzSterowanie();

                base.Update(gameTime);
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //RysujTrojkat();
            foreach (var obiekt in obiekty)
            {
                foreach (ModelMesh mesh in obiekt.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.View = kamera.ViewMatrix;
                        effect.World = obiekt.worldMatrix;
                        effect.Projection = kamera.ProjectionMatrix;
                        mesh.Draw();
                    }
                }
            }
            base.Draw(gameTime);
        }

        private void SprawdzSterowanie()
        {
            KeyboardState kState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            #region Przesuwanie kamery
            if (kState.IsKeyDown(Keys.Right))
            {
                kamera.StrafeHorz(-0.1f);
            }
            if (kState.IsKeyDown(Keys.Left))
            {
                kamera.StrafeHorz(0.1f);
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                kamera.StrafeVert(-0.1f);
            }
            if (kState.IsKeyDown(Keys.Up))
            {
                kamera.StrafeVert(0.1f);
            }
            if (kState.IsKeyDown(Keys.OemPlus) || mState.LeftButton == ButtonState.Pressed)
            {
                kamera.Thrust(0.3f);
            }
            if (kState.IsKeyDown(Keys.OemMinus) || mState.RightButton == ButtonState.Pressed)
            {
                kamera.Thrust(-0.3f);
            }
            #endregion

            #region Obracanie kamery
            if (kState.IsKeyDown(Keys.D))
            {
                kamera.Yaw(-1.5f);
            }
            if (kState.IsKeyDown(Keys.A))
            {
                kamera.Yaw(1.5f);
            }
            if (kState.IsKeyDown(Keys.S))
            {
                kamera.Pitch(1.5f);
            }
            if (kState.IsKeyDown(Keys.W))
            {
                kamera.Pitch(-1.5f);
            }
            if (kState.IsKeyDown(Keys.Q))
            {
                kamera.Roll(1.5f);
            }
            if (kState.IsKeyDown(Keys.E))
            {
                kamera.Roll(-1.5f);
            }
            #endregion

            prevKState = kState;
            prevMState = mState;
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Primitives3D;

namespace Tropikalna_wyspa
{
    public class DemoWyspa : Game
    {
        GraphicsDeviceManager graphics;
        
        KeyboardState prevKState;
        MouseState prevMState;
        List<Object3D> obiekty;
        List<GeometricPrimitive> wyspa;
        GeometricPrimitive morze;

        Vector3 swiatloKierunkowe;

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
            
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(45f), graphics.
                               GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);

            swiatloKierunkowe = new Vector3(1f,1f,1f);

            obiekty = new List<Object3D>
            {
                new Object3D(palma, new Vector3(0f, 0f, 0f)),
                new Object3D(palma, new Vector3(2,-0.5f,2.1f), new Vector3(0.5f,1.0f,0.1f), Vector3.Backward)
            };

            kamera = new Camera3D(new Vector3(0f, 5f, 15f), Vector3.Forward, Vector3.Up, proj);

            wyspa = new List<GeometricPrimitive>
            {
                GeneratorWyspy.ZrobWyspe(GraphicsDevice, 20)
            };

            morze = new SquarePrimitive(GraphicsDevice, 200);
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
            
            foreach (var obiekt in obiekty)
            {
                foreach (ModelMesh mesh in obiekt.model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        Debug.WriteLine("Kolor: R{0}, G{1}, B{2}", effect.DiffuseColor.X, effect.DiffuseColor.Y, effect.DiffuseColor.Z);
                        effect.View = kamera.ViewMatrix;
                        effect.World = obiekt.worldMatrix;
                        effect.Projection = kamera.ProjectionMatrix;
                        mesh.Draw();
                    }
                }
            }

            BasicEffect WyspaFX = new BasicEffect(GraphicsDevice);
            WyspaFX.World = Matrix.CreateWorld(new Vector3(-10, 1, -10), Vector3.Forward, Vector3.Up);
            WyspaFX.Projection = kamera.ProjectionMatrix;
            WyspaFX.View = kamera.ViewMatrix;
            WyspaFX.DiffuseColor = Color.LightYellow.ToVector3();
            WyspaFX.LightingEnabled = true;
            WyspaFX.DirectionalLight0.Enabled = true;
            WyspaFX.DirectionalLight0.Direction = swiatloKierunkowe;
            WyspaFX.DirectionalLight0.DiffuseColor = Color.GhostWhite.ToVector3()/2;
            WyspaFX.PreferPerPixelLighting = true;

            foreach (var ksztalt in wyspa)
            {
                ksztalt.Draw(WyspaFX);
            }

            morze.Draw(Matrix.CreateWorld(new Vector3(0, 0, 0), Vector3.Forward, Vector3.Up), kamera.ViewMatrix, kamera.ProjectionMatrix, Color.CornflowerBlue);
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
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var game = new DemoWyspa();
            game.Run();
            game.Exit();
            game.Dispose();
        }
    }
}
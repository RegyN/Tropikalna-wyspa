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
        GeometricPrimitive wyspa;
        GeometricPrimitive morze;

        Vector3 swiatloKierunkowe;
        Vector3 swiatloPunktowe;

        Camera3D kamera;
        
        Shader phong;
        
        public DemoWyspa()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            PrzygotujOkno();
            
            base.Initialize();

            swiatloKierunkowe = new Vector3(1f, -1f, -1f);
            swiatloPunktowe = new Vector3(-2f, .4f, 2f);

            prevKState = Keyboard.GetState();

            PrzygotujKamere();
            PrzygotujObiekty();
        }

        private void PrzygotujOkno()
        {
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.ApplyChanges();
        }

        private void PrzygotujObiekty()
        {
            obiekty = new List<Object3D>
            {
                new Skrzynka(Content, new Vector3(1f,.6f,4.5f), new Vector3(0f,1f,0.55f), new Vector3(-1f,0.1f,0.07f)),
                new Krysztal(Content, (swiatloPunktowe + new Vector3(0f,-.4f,0f)), new Vector3(0.3f,1f,0f), new Vector3(-1f,0.1f,0.07f)),
                new Palma(Content, new Vector3(0f, 0.6f, 0f), new Vector3(-0.1f,1.0f,-0.1f), Vector3.Forward),
                new Palma(Content, new Vector3(2,-0.5f,2.1f), new Vector3(0.5f,1.0f,0.1f), Vector3.Backward)
            };

            wyspa = GeneratorWyspy.ZrobWyspe(GraphicsDevice, 20, Color.SandyBrown);

            morze = new SquarePrimitive(GraphicsDevice, 200, Color.CornflowerBlue);
        }

        private void PrzygotujKamere()
        {
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                                           MathHelper.ToRadians(50f), graphics.
                                           GraphicsDevice.Viewport.AspectRatio, 1f, 5000f);

            kamera = new Camera3D(new Vector3(0f, 5f, 15f), Vector3.Forward, Vector3.Up, proj);
        }

        protected override void LoadContent()
        {
            phong = new Shader(Content.Load<Effect>("MyPhong"));
        }

        protected override void UnloadContent() => Content.Unload();

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                SprawdzSterowanie(gameTime);

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
                        effect.View = kamera.ViewMatrix;
                        effect.World = obiekt.worldMatrix;
                        effect.Projection = kamera.ProjectionMatrix;
                        effect.LightingEnabled = true;
                        effect.AmbientLightColor = Color.White.ToVector3() / 2;
                        effect.DirectionalLight0.Direction = swiatloKierunkowe;
                        effect.DirectionalLight0.DiffuseColor = Color.White.ToVector3();
                        mesh.Draw();
                    }
                }
            }

            phong.viewMatrix = kamera.ViewMatrix;
            phong.projectionMatrix = kamera.ProjectionMatrix;
            phong.worldMatrix = Matrix.CreateWorld(new Vector3(-10, 1.5f, -10), Vector3.Forward, Vector3.Up);
            phong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(-10, 1.5f, -10), Vector3.Forward, Vector3.Up)));
            phong.diffuseColor = Color.Green;
            phong.viewPosition = kamera.Position;
            phong.diffuseLightDirection = swiatloKierunkowe;
            phong.diffuseLightColor = Color.White;
            phong.materialEmissive = new Vector3(0f, 0f, 0f);
            phong.materialAmbient = new Vector3(1f, 1f, 1f)/3;
            phong.materialDiffuse = new Vector3(1f, 1f, 1f);
            phong.materialSpecular = Color.LightYellow.ToVector3();
            phong.materialPower = 50f;


            wyspa.Draw(phong.efekt);

            phong.worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), Vector3.Forward, Vector3.Up);
            phong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(0f, 0f, 0f), Vector3.Forward, Vector3.Up)));
            morze.Draw(phong.efekt);
            base.Draw(gameTime);
        }

        private void SprawdzSterowanie(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            #region Przesuwanie kamery
            if (kState.IsKeyDown(Keys.Right))
            {
                kamera.StrafeHorz(-6f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.Left))
            {
                kamera.StrafeHorz(6f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.Down))
            {
                kamera.StrafeVert(-6f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.Up))
            {
                kamera.StrafeVert(6f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.Z) || mState.LeftButton == ButtonState.Pressed)
            {
                kamera.Thrust(20f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.X) || mState.RightButton == ButtonState.Pressed)
            {
                kamera.Thrust(-20f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            #endregion

            #region Obracanie kamery
            if (kState.IsKeyDown(Keys.D))
            {
                kamera.Yaw(-60f*gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.A))
            {
                kamera.Yaw(60f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.S))
            {
                kamera.Pitch(60f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.W))
            {
                kamera.Pitch(-60f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.Q))
            {
                kamera.Roll(90f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (kState.IsKeyDown(Keys.E))
            {
                kamera.Roll(-90f * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            #endregion

            if (kState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
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
            game.Window.Position = new Point(100, 100);
            game.Window.IsBorderless = true;
            game.Run();
            game.Exit();
            game.Dispose();
        }
    }
}
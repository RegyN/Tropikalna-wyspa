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
        float czasOdZmianySwiatla;
        Color swiatloPunktoweKolor;

        Camera3D kamera;

        Shader phong;
        Shader phongStat;

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
            swiatloPunktowe = new Vector3(5f, 1f, 9f);
            swiatloPunktoweKolor = Color.Red;
            czasOdZmianySwiatla = 0.0f;

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
                new Skrzynka(Content, new Vector3(8.5f,0.6f,12f), new Vector3(0f,1f,0.55f), new Vector3(-1f,0.1f,0.07f)),
                new Krysztal(Content, new Vector3(5f, 0.0f, 9f), new Vector3(0.3f,1f,0f), new Vector3(-1f,0.1f,0.07f)),
                new Palma(Content, new Vector3(7.5f, 0.6f, 7.5f), new Vector3(-0.1f,1.0f,-0.1f), Vector3.Forward),
                new Palma(Content, new Vector3(9.5f,-0.5f,9.6f), new Vector3(0.5f,1.0f,0.1f), Vector3.Backward)
            };

            wyspa = GeneratorWyspy.ZrobWyspe(GraphicsDevice, 15, Color.SandyBrown);

            morze = new SquarePrimitive(GraphicsDevice, 200, Color.CornflowerBlue);
        }

        private void PrzygotujKamere()
        {
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                                           MathHelper.ToRadians(50f), graphics.
                                           GraphicsDevice.Viewport.AspectRatio, 1f, 5000f);

            kamera = new Camera3D(new Vector3(5f, 5f, 25f), Vector3.Forward, Vector3.Up, proj);
        }

        protected override void LoadContent()
        {
            phong = new Shader(Content.Load<Effect>("NewPhong"));
            phongStat = new Shader(Content.Load<Effect>("PhongStaticColor"));
        }

        protected override void UnloadContent() => Content.Unload();

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                SprawdzSterowanie(gameTime);
                czasOdZmianySwiatla += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                AktualizujKolorSwiatla();
                base.Update(gameTime);
            }
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            phong.diffuseColor = Color.White;
            phong.viewPosition = kamera.Position;
            phong.diffuseLightDirection = swiatloKierunkowe;
            phong.diffuseLightColor = Color.White;
            phong.materialEmissive = new Vector3(0f, 0f, 0f);
            phong.materialAmbient = new Vector3(.1f, .1f, .1f);
            phong.materialDiffuse = Color.LightYellow.ToVector3();
            phong.materialSpecular = Color.LightYellow.ToVector3();
            phong.materialPower = 50f;
            phong.specularIntensity = 1f;
            phong.pointLightFalloff = 5f;
            phong.pointLightRange = 150f;
            phong.pointLightPos = swiatloPunktowe;
            phong.pointLightColor = swiatloPunktoweKolor.ToVector4();

            phongStat.diffuseColor = Color.White;
            phongStat.viewPosition = kamera.Position;
            phongStat.diffuseLightDirection = swiatloKierunkowe;
            phongStat.diffuseLightColor = Color.White;
            phongStat.materialEmissive = new Vector3(0f, 0f, 0f);
            phongStat.materialAmbient = new Vector3(.1f, .1f, .1f);
            phongStat.materialDiffuse = Color.LightYellow.ToVector3();
            phongStat.materialSpecular = Color.LightYellow.ToVector3();
            phongStat.materialPower = 50f;
            phongStat.specularIntensity = 1f;
            phongStat.pointLightFalloff = 5f;
            phongStat.pointLightRange = 150f;
            phongStat.pointLightPos = swiatloPunktowe;
            phongStat.pointLightColor = swiatloPunktoweKolor.ToVector4();

            foreach (var obiekt in obiekty)
            {
                for (int i = 0; i < obiekt.model.Meshes.Count; i++)
                {
                    ModelMesh mesh = obiekt.model.Meshes[i];
                    foreach (var part in mesh.MeshParts)
                    {
                        phongStat.diffuseColor = Color.Red;
                        phongStat.viewMatrix = kamera.ViewMatrix;
                        phongStat.projectionMatrix = kamera.ProjectionMatrix;
                        phongStat.worldMatrix = obiekt.worldMatrix;
                        phongStat.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(obiekt.worldMatrix));

                        part.Effect = phongStat.efekt;
                        mesh.Draw();
                    }
                }
            }

            phong.viewMatrix = kamera.ViewMatrix;
            phong.projectionMatrix = kamera.ProjectionMatrix;
            phong.worldMatrix = Matrix.CreateWorld(new Vector3(0f, 1.5f, 0f), Vector3.Forward, Vector3.Up);
            phong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(0f, 1.5f, 0f), Vector3.Forward, Vector3.Up)));
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

        private void AktualizujKolorSwiatla()
        {
            if(czasOdZmianySwiatla<2.5f)
            {
                swiatloPunktoweKolor = new Color((250f - 100f*czasOdZmianySwiatla)/250, 100f*czasOdZmianySwiatla/250f, 0f);
            }
            else if (czasOdZmianySwiatla < 5f)
            {
                float czasZast = czasOdZmianySwiatla - 2.5f;
                swiatloPunktoweKolor = new Color(0f, (250f - 100f * czasZast) / 250, 100f * czasZast / 250f);
            }
            else if (czasOdZmianySwiatla < 7.5f)
            {
                float czasZast = czasOdZmianySwiatla - 5f;
                swiatloPunktoweKolor = new Color(100f * czasZast / 250f, 0f, (250f - 100f * czasZast) / 250);
            }
            else
            {
                czasOdZmianySwiatla = 0.0f;
                swiatloPunktoweKolor = Color.Red;
            }
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
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
        Object3D krysztal;

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

            krysztal = obiekty[1];

            wyspa = GeneratorWyspy.ZrobWyspe(GraphicsDevice, 15, Color.SandyBrown);

            morze = new SquarePrimitive(GraphicsDevice, 200, Color.DarkBlue);
        }

        private void PrzygotujKamere()
        {
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                                           MathHelper.ToRadians(50f), graphics.
                                           GraphicsDevice.Viewport.AspectRatio, 1f, 50f);

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
        
        private void RysujBasicEffectami()
        {
            krysztal.model.Meshes[0].Effects[0].Parameters["EmissiveColor"].SetValue(swiatloPunktoweKolor.ToVector3());
            foreach (var obiekt in obiekty)
            {
                foreach (var mesh in obiekt.model.Meshes)
                {
                    foreach (BasicEffect efekt in mesh.Effects)
                    {
                        efekt.World = obiekt.worldMatrix;
                        efekt.View = kamera.ViewMatrix;
                        efekt.Projection = kamera.ProjectionMatrix;
                        efekt.LightingEnabled = true;
                        //efekt.AmbientLightColor = new Color(30,30,30,255).ToVector3();
                        efekt.DirectionalLight0.Enabled = true;
                        efekt.DirectionalLight1.Enabled = true;
                        efekt.DirectionalLight0.Direction = swiatloKierunkowe;
                        efekt.DirectionalLight0.DiffuseColor = Color.White.ToVector3() / 10;
                        efekt.DirectionalLight1.Direction = -swiatloPunktowe + obiekt.position;
                        efekt.DirectionalLight1.DiffuseColor = swiatloPunktoweKolor.ToVector3() / 3;
                        mesh.Draw();
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            phong.diffuseColor = Color.White;
            phong.viewPosition = kamera.Position;
            phong.diffuseLightDirection = swiatloKierunkowe;
            phong.diffuseLightColor = Color.DimGray;
            phong.materialEmissive = new Vector3(0f, 0f, 0f);
            phong.materialAmbient = new Vector3(.05f, .05f, .1f);
            phong.materialDiffuse = Color.White.ToVector3();
            phong.materialSpecular = Color.White.ToVector3();
            phong.materialPower = 50f;
            phong.specularIntensity = 1f;
            phong.pointLightFalloff = 7f;
            phong.pointLightRange = 70f;
            phong.pointLightPos = swiatloPunktowe;
            phong.pointLightColor = swiatloPunktoweKolor.ToVector4() / 1.5f;

            RysujMoimShaderem();
            //RysujBasicEffectami();

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

        private void RysujMoimShaderem()
        {
            foreach (var obiekt in obiekty)
            {
                obiekt.shader.viewMatrix = kamera.ViewMatrix;
                obiekt.shader.projectionMatrix = kamera.ProjectionMatrix;
                obiekt.shader.worldMatrix = obiekt.worldMatrix;
                obiekt.shader.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(obiekt.worldMatrix));

                obiekt.shader.viewPosition = kamera.Position;
                obiekt.shader.diffuseLightDirection = swiatloKierunkowe;
                obiekt.shader.diffuseLightColor = Color.White;
                obiekt.shader.pointLightFalloff = 5f;
                obiekt.shader.pointLightRange = 150f;
                obiekt.shader.pointLightPos = swiatloPunktowe;
                obiekt.shader.pointLightColor = swiatloPunktoweKolor.ToVector4();
                obiekt.Draw();
                for (int i = 0; i < obiekt.model.Meshes.Count; i++)
                {
                    ModelMesh mesh = obiekt.model.Meshes[i];
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = obiekt.shader.efekt;
                        mesh.Draw();
                    }
                }
            }
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
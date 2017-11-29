using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

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
        GeometricPrimitive dno;
        Object3D krysztal;
        Skybox skybox;
        float czasOdZmianySwiatla;

        Vector3 swiatloKierunkowe;
        Vector3 swiatloPunktowe;
        Color swiatloPunktoweKolor;

        Camera3D kamera;

        Shader phong;
        Shader ocean;

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
            // TODO: Klasa ze światłami
            swiatloKierunkowe = new Vector3(-12f, -1.5f, 4f);
            swiatloPunktowe = new Vector3(5f, 2.5f, 9f);
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
                new Samolot(Content, new Vector3(7.5f, 5.0f, 7.5f), new Vector3(-0.1f, 1.0f, -0.1f), Vector3.Forward, 1.0f),
                new Skrzynka(Content, new Vector3(8.5f,0.6f,12f), new Vector3(0f,1f,0.55f), new Vector3(-1f,0.1f,0.07f)),
                new Krysztal(Content, new Vector3(5f, 0.0f, 9f), new Vector3(0.3f,1f,0f), Vector3.Left),
                new Palma(Content, new Vector3(7.5f, 0.6f, 7.5f), new Vector3(-0.1f,1.0f,-0.1f), Vector3.Forward),
                new Palma(Content, new Vector3(9.5f,-0.5f,9.6f), new Vector3(0.5f,1.0f,0.1f), Vector3.Backward)
            };

            krysztal = obiekty[2];

            skybox = new Skybox("Skybox", Content);

            // TODO: Jakiś rozsądniejszy sposób przechowywania prymitywów? Połączyć z obiektami przez dziedziczenie? 
            wyspa = GeneratorWyspy.ZrobWyspe(GraphicsDevice, 15);

            morze = new SquarePrimitive(GraphicsDevice, 200);
            dno = new SquarePrimitive(GraphicsDevice, 200);
        }

        private void PrzygotujKamere()
        {
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                                           MathHelper.ToRadians(50f), graphics.
                                           GraphicsDevice.Viewport.AspectRatio, 1f, 500f);

            kamera = new Camera3D(new Vector3(5f, 5f, 25f), Vector3.Forward, Vector3.Up, proj);
        }

        private void PrzygotujShadery()
        {
            phong.diffuseColor = Color.Gray;
            phong.viewPosition = kamera.Position;
            phong.diffuseLightDirection = swiatloKierunkowe;
            phong.diffuseLightColor = Color.DimGray;
            phong.materialEmissive = new Vector3(0f, 0f, 0f);
            phong.materialAmbient = new Vector3(.05f, .05f, .1f);
            phong.materialDiffuse = Color.White.ToVector3();
            phong.materialSpecular = Color.White.ToVector3();
            phong.materialPower = 50f;
            phong.specularIntensity = 1f;
            phong.pointLightFalloff = 1f;
            phong.pointLightRange = 300f;
            phong.pointLightPos = swiatloPunktowe;
            phong.pointLightColor = swiatloPunktoweKolor.ToVector4() / 1.5f;

            ocean.diffuseColor = Color.Gray;
            ocean.viewPosition = kamera.Position;
            ocean.diffuseLightDirection = swiatloKierunkowe;
            ocean.diffuseLightColor = Color.DimGray;
            ocean.materialEmissive = new Vector3(0f, 0f, 0f);
            ocean.materialAmbient = new Vector3(.05f, .05f, .1f);
            ocean.materialDiffuse = Color.White.ToVector3();
            ocean.materialSpecular = Color.White.ToVector3();
            ocean.materialPower = 50f;
            ocean.specularIntensity = 1f;
            ocean.pointLightFalloff = 1f;
            ocean.pointLightRange = 300f;
            ocean.pointLightPos = swiatloPunktowe;
            ocean.pointLightColor = swiatloPunktoweKolor.ToVector4() / 1.5f;
        }

        protected override void LoadContent()
        {
            phong = new Shader(Content.Load<Effect>("NoTexturePhong"));
            ocean = new Shader(Content.Load<Effect>("OceanShader"));
            // skybox = new Shader(Content.Load<Effect>("SkyboxShader"));
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
            GraphicsDevice.Clear(Color.DarkBlue);
            
            PrzygotujShadery();
            RasterizerState nowy = new RasterizerState();
            nowy.CullMode = CullMode.CullClockwiseFace;
            graphics.GraphicsDevice.RasterizerState = nowy;
            skybox.Draw(kamera.ViewMatrix, kamera.ProjectionMatrix, kamera.Position);
            RasterizerState nowszy = new RasterizerState();
            nowszy.CullMode = CullMode.CullCounterClockwiseFace;
            graphics.GraphicsDevice.RasterizerState = nowszy;
            RysujMoimShaderem();

            phong.diffuseColor = Color.SandyBrown;
            phong.viewMatrix = kamera.ViewMatrix;
            phong.projectionMatrix = kamera.ProjectionMatrix;
            phong.worldMatrix = Matrix.CreateWorld(new Vector3(0f, 1.5f, 0f), Vector3.Forward, Vector3.Up);
            phong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(0f, 1.5f, 0f), Vector3.Forward, Vector3.Up)));
            wyspa.Draw(phong.efekt);

            phong.diffuseColor = Color.DarkBlue;
            phong.worldMatrix = Matrix.CreateWorld(new Vector3(0f, 0f, 0f), Vector3.Forward, Vector3.Up);
            phong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(0f, 0f, 0f), Vector3.Forward, Vector3.Up)));

            //Texture[] array = new Texture[2];
            //array[0] = Content.Load<Texture>("TexV");
            //array[1] = Content.Load<Texture>("TexH");
            //ocean.efekt.Parameters["tex"].SetValue(Content.Load<Texture>("TexH"));
            morze.Draw(phong.efekt);

            phong.diffuseColor = Color.Brown;
            phong.worldMatrix = Matrix.CreateWorld(new Vector3(0f, -2.5f, 0f), Vector3.Forward, Vector3.Up);
            phong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(0f, 0f, 0f), Vector3.Forward, Vector3.Up)));
            dno.Draw(phong.efekt);

            base.Draw(gameTime);
        }
        
        private void RysujMoimShaderem()
        {
            foreach (var obiekt in obiekty)     // TODO: możliwie dużą część tych rzeczy przenieść do metod w rysowanych obiektach
            {
                obiekt.shader.viewMatrix = kamera.ViewMatrix;
                obiekt.shader.projectionMatrix = kamera.ProjectionMatrix;
                obiekt.shader.worldMatrix = obiekt.worldMatrix;
                obiekt.shader.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(obiekt.worldMatrix));

                obiekt.shader.viewPosition = kamera.Position;
                obiekt.shader.diffuseLightDirection = swiatloKierunkowe;
                obiekt.shader.diffuseLightColor = Color.Gray;
                obiekt.shader.pointLightFalloff = 5f;
                obiekt.shader.pointLightRange = 150f;
                obiekt.shader.pointLightPos = swiatloPunktowe;
                obiekt.shader.pointLightColor = swiatloPunktoweKolor.ToVector4();
                obiekt.Draw();
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
            if (kState.IsKeyDown(Keys.Space) && !prevKState.IsKeyDown(Keys.Space))
            {
                this.PrzelaczMSAA();
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

        private void PrzelaczMSAA()
        {
            this.ZmienStatusMSAA(!graphics.PreferMultiSampling);
        }

        private void ZmienStatusMSAA(bool enable)
        {
            graphics.PreferMultiSampling = enable;

            var rasterizerState = new RasterizerState
            {
                MultiSampleAntiAlias = enable,
            };

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.PresentationParameters.MultiSampleCount = enable ? 4 : 0;

            graphics.ApplyChanges();
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
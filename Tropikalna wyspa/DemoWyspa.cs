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
        Object3D drogowskaz;
        Object3D krysztal;
        Skybox skybox;
        ReflectionSphere kulka;
        Texture2D dynamicznaTekstura;
        Vector2 przesuniecieMorza;
        int licznikPrzesuniecia;
        
        float czasOdZmianySwiatla;
        Vector3 swiatloKierunkowe;
        Vector3 swiatloPunktowe;
        Color swiatloPunktoweKolor;

        RenderCapture renderCapture;
        RenderTarget2D renderTarget;
        PostProcessor postprocessor;

        SpriteBatch spriteBatch;

        Camera3D kamera;

        Shader phong;
        Shader texPhong;
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

            swiatloKierunkowe = new Vector3(-12f, -1.5f, 4f);
            swiatloPunktowe = new Vector3(5f, 2.5f, 9f);
            swiatloPunktoweKolor = Color.Red;
            czasOdZmianySwiatla = 0.0f;

            prevKState = Keyboard.GetState();

            przesuniecieMorza = new Vector2(0.0f, 0.0f);

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
                new Drogowskaz(Content, new Vector3(8.5f, 1.0f, 8.5f), new Vector3(-0.1f, 1.0f, -0.1f), Vector3.Forward, 0.001f),
                new Skrzynka(Content, new Vector3(8.5f,1.3f,12f), new Vector3(0f,1f,0.55f), new Vector3(-1f,0.1f,0.07f)),
                new Krysztal(Content, new Vector3(5f, 0.0f, 9f), new Vector3(0.3f,1f,0f), Vector3.Left),
                new Palma(Content, new Vector3(7.5f, 0.6f, 7.5f), new Vector3(-0.1f,1.0f,-0.1f), Vector3.Forward),
                new Palma(Content, new Vector3(9.5f,-0.5f,9.6f), new Vector3(0.5f,1.0f,0.1f), Vector3.Backward)
            };

            drogowskaz = obiekty[0];
            krysztal = obiekty[2];

            skybox = new Skybox("Skybox", Content);
            kulka = new ReflectionSphere("Skybox", Content, new Vector3(16.0f,4.5f,16.0f), 2.0f);

            // TODO: Jakiś rozsądniejszy sposób przechowywania prymitywów? Połączyć z obiektami przez dziedziczenie? 
            wyspa = GeneratorMap.ZrobParabolicznaWyspe(GraphicsDevice, 40);
            
            morze = GeneratorMap.ZrobMorze(GraphicsDevice, 30);
            dno = GeneratorMap.ZrobMorze(GraphicsDevice, 30);
        }

        private void PrzygotujKamere()
        {
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                                           MathHelper.ToRadians(50f), graphics.
                                           GraphicsDevice.Viewport.AspectRatio, 1f, 200f);

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

            texPhong.diffuseColor = Color.Gray;
            texPhong.viewPosition = kamera.Position;
            texPhong.diffuseLightDirection = swiatloKierunkowe;
            texPhong.diffuseLightColor = Color.DimGray;
            texPhong.materialEmissive = new Vector3(0f, 0f, 0f);
            texPhong.materialAmbient = new Vector3(.05f, .05f, .1f);
            texPhong.materialDiffuse = Color.White.ToVector3();
            texPhong.materialSpecular = Color.White.ToVector3();
            texPhong.materialPower = 50f;
            texPhong.specularIntensity = 1f;
            texPhong.pointLightFalloff = 1f;
            texPhong.pointLightRange = 300f;
            texPhong.pointLightPos = swiatloPunktowe;
            texPhong.pointLightColor = swiatloPunktoweKolor.ToVector4() / 1.5f;

            ocean.diffuseColor = Color.Gray;
            ocean.viewPosition = kamera.Position;
            ocean.diffuseLightDirection = swiatloKierunkowe;
            ocean.diffuseLightColor = Color.DimGray;
            ocean.materialEmissive = new Vector3(0f, 0f, 0f);
            ocean.materialAmbient = new Vector3(.05f, .05f, .1f);
            ocean.materialDiffuse = Color.White.ToVector3();
            phong.materialSpecular = Color.White.ToVector3();
            ocean.materialPower = 50f;
            ocean.specularIntensity = 1f;
            ocean.pointLightFalloff = 1f;
            ocean.pointLightRange = 300f;
            ocean.pointLightPos = swiatloPunktowe;
            ocean.pointLightColor = swiatloPunktoweKolor.ToVector4() / 1.5f;
            ocean.Displacement = przesuniecieMorza;

            krysztal.shader.efekt.Parameters["materialEmissive"].SetValue(swiatloPunktoweKolor.ToVector3());
        }

        protected override void LoadContent()
        {
            renderCapture = new RenderCapture(GraphicsDevice);
            postprocessor = new PostProcessor(Content.Load<Effect>("BlackAndWhite"), GraphicsDevice);
            phong = new Shader(Content.Load<Effect>("NoTexturePhong"));
            texPhong = new Shader(Content.Load<Effect>("TexturePhong"));
            ocean = new Shader(Content.Load<Effect>("OceanShader"));
            spriteBatch = new SpriteBatch(GraphicsDevice);
            int screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            renderTarget = new RenderTarget2D(GraphicsDevice, screenWidth, screenHeight, false, 
                                                SurfaceFormat.Color, DepthFormat.Depth24);
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
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.DarkBlue);
            
            PrzygotujShadery();

            AktualizujPrzesuniecieMorza(gameTime);

            RasterizerState nowy = new RasterizerState();
            nowy.CullMode = CullMode.CullClockwiseFace;
            graphics.GraphicsDevice.RasterizerState = nowy;
            skybox.Draw(kamera.ViewMatrix, kamera.ProjectionMatrix, kamera.Position);
            RasterizerState nowszy = new RasterizerState();
            nowszy.CullMode = CullMode.CullCounterClockwiseFace;
            graphics.GraphicsDevice.RasterizerState = nowszy;

            RysujMoimShaderem();
            
            GraphicsDevice.SetRenderTarget(null);
            
            int screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            var efekt = Content.Load<Effect>("BlackAndWhite");
            // Set effect parameters if necessary
            var viewportSize = new Vector2(screenWidth, screenHeight);
            var textureSize = new Vector2(renderTarget.Width, renderTarget.Height);
            // Initialize the spritebatch and effect
            efekt.CurrentTechnique = efekt.Techniques["Grayscale"];
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, effect: efekt);
            // Draw the input texture

            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            spriteBatch.Draw(renderTarget, screenRectangle, Color.White);
            // End the spritebatch and effect
            spriteBatch.End();
            // Clean up render states changed by the spritebatch
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;

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

            phong.pointLightColor = swiatloPunktoweKolor.ToVector4();
            phong.diffuseColor = Color.SandyBrown;
            phong.viewMatrix = kamera.ViewMatrix;
            phong.projectionMatrix = kamera.ProjectionMatrix;

            texPhong.pointLightColor = swiatloPunktoweKolor.ToVector4();
            texPhong.diffuseColor = Color.SandyBrown;
            texPhong.viewMatrix = kamera.ViewMatrix;
            texPhong.projectionMatrix = kamera.ProjectionMatrix;
            texPhong.worldMatrix = Matrix.CreateWorld(new Vector3(-13f, 1.5f, -13f), Vector3.Forward, Vector3.Up);
            texPhong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(-13f, 1.5f, -13f), Vector3.Forward, Vector3.Up)));
            texPhong.PrimaryTex = Content.Load<Texture2D>("sand");
            wyspa.Draw(texPhong.efekt);

            phong.diffuseColor = Color.Brown;
            phong.worldMatrix = Matrix.CreateWorld(new Vector3(-80f, -2.5f, -80f), Vector3.Forward, Vector3.Up);
            phong.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(-40f, -2.5f, -40f), Vector3.Forward, Vector3.Up)));
            dno.Draw(phong.efekt);

            ocean.diffuseColor = Color.White;
            ocean.viewMatrix = kamera.ViewMatrix;
            ocean.projectionMatrix = kamera.ProjectionMatrix;
            ocean.worldMatrix = Matrix.CreateWorld(new Vector3(-80f, 0f, -80f), Vector3.Forward, Vector3.Up);
            ocean.WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(Matrix.CreateWorld(new Vector3(-40f, 0f, -40f), Vector3.Forward, Vector3.Up)));
            ocean.PrimaryTex = Content.Load<Texture2D>("Sea1");
            ocean.SecondaryTex = Content.Load<Texture2D>("Sea2");

            morze.Draw(ocean.efekt);
        }

        private void PrzelaczMgle()
        {
            if (ocean.FogEnabled >= 0.9f)
                WylaczMgle();
            else
                WlaczMgle();
        }

        private void WylaczMgle()
        {
            foreach (var obiekt in this.obiekty)
            {
                obiekt.shader.FogEnabled = 0.0f;
            }
            ocean.FogEnabled    = 0.0f;
            phong.FogEnabled    = 0.0f;
            texPhong.FogEnabled = 0.0f;
            kulka.efekt.Parameters["fogEnabled"].SetValue(0.0f);
        }

        private void WlaczMgle()
        {
            foreach (var obiekt in this.obiekty)
            {
                obiekt.shader.FogEnabled = 1.0f;
            }
            ocean.FogEnabled    = 1.0f;
            phong.FogEnabled    = 1.0f;
            texPhong.FogEnabled = 1.0f;
            kulka.efekt.Parameters["fogEnabled"].SetValue(1.0f);
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
                this.PrzelaczMgle();
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

        private void AktualizujPrzesuniecieMorza(GameTime gt)
        {
            przesuniecieMorza = przesuniecieMorza + new Vector2(0.001f, -0.001f);
            if(przesuniecieMorza.X > 1.0f)
            {
                przesuniecieMorza.X = przesuniecieMorza.X - 1.0f;
            }
            if (przesuniecieMorza.Y > 1.0f)
            {
                przesuniecieMorza.Y = przesuniecieMorza.Y - 1.0f;
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
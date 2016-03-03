using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System.Drawing.Imaging;
using System.Threading;

namespace BattleCity
{
    class RootThingy
    {
        public static int windowX = 800;
        public static int windowY = 608;
        public static string exePath = Environment.CurrentDirectory;
        public static int sceneX = 800;
        public static int sceneY = 600;
        public static bool debugInfo = true;
        private static bool fullscreen = false;

        public static bool zoomed = false;
        public static double zoom = 2;
        public static double camX;
        public static double camY;
        private static double diffx;
        private static double diffy;

        

        public static bool frameState = true;
        private static Byte frame = 0;
        private static Byte frameDelay = 30;

        public static short npcFreezeTimer = 0;
        public static bool gameOver = false;
        private static short itemSpawnTimer;
        public static string eaglebaseShild = "";
        public static string spawner = "";

        
        public static KeyboardState keyboard;

        

        
        //
        //Texture;

        public static Random rnd = new Random();

        public struct Point
        {
            public double x;
            public double y;
        }

        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [STAThread]
        public static void Main()
        {
            OpenTK.DisplayDevice display=DisplayDevice.GetDisplay(0);

            ////////////////////////////////////////////////////////////////////////////OpenAL-test
            AudioContext context = new AudioContext();
            int ALBuffer = AL.GenBuffer();
            int ALSource = AL.GenSource();
            int ALState;
            
            int channels, bits_per_sample, sample_rate;
            Console.WriteLine("OpenAL-Error1: " + AL.GetError());

            byte[] soundData = Sound.LoadWave(File.Open(exePath + @"\sfx\item_star_clock_helmet_shovel.wav", FileMode.Open), out channels, out bits_per_sample, out sample_rate);
            Console.WriteLine("OpenAL-Error2: " + AL.GetError());

            //AL.BufferData(ALBuffer, Sound.GetSoundFormat(channels, bits_per_sample), soundData, soundData.Length, sample_rate);
            AL.BufferData(ALBuffer, ALFormat.Stereo16, soundData, soundData.Length, 44100);
            Console.WriteLine("OpenAL-Error3: " + AL.GetError() + "  Format" + Sound.GetSoundFormat(channels, bits_per_sample) +  "  ch" + channels + "  bps" + bits_per_sample + "  Hz" + sample_rate + "  Buffer" + ALBuffer);

            AL.Source(ALSource, ALSourcei.Buffer, ALBuffer);
            Console.WriteLine("OpenAL-Error4: " + AL.GetError());

            AL.SourcePlay(ALSource); 
            Console.WriteLine("OpenAL-Error5: " + AL.GetError());

            Console.Write("Playing");
            // Query the source to find out when it stops playing.
            do
            {
                Thread.Sleep(250);
                Console.Write(".");
                AL.GetSource(ALSource, ALGetSourcei.SourceState, out ALState);
            }
            while ((ALSourceState)ALState == ALSourceState.Playing);

            Console.WriteLine("");
            AL.SourceStop(ALSource);
            AL.DeleteSource(ALSource);
            AL.DeleteBuffer(ALBuffer);

            Console.WriteLine("OpenAL-Error: " + AL.GetError());
            ////////////////////////////////////////////////////////////////////////////End of OpenAL-test

            System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity = (System.IntPtr)(System.Environment.ProcessorCount); //Assign the process to the last CPU-core of the System
            Console.WriteLine("We have " + System.Environment.ProcessorCount + " CPU-Cores.");
            
            itemSpawnTimer = (short)rnd.Next(300, 1200);



            GameWindow game = new GameWindow();
            {

                

                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    
                    game.X = 32;
                    game.Y = 16;
                    game.VSync = VSyncMode.On;
                    game.Width = windowX;
                    game.Height = windowY;
                    game.WindowBorder = WindowBorder.Fixed; //Disables the resizable windowframe
                    GL.Enable(EnableCap.Blend);                                                     //These lines
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  //enable transparency using alpha-channel
                    game.TargetRenderFrequency=60;
                };

                Map karte = new Map();
                

                game.Resize += (sender, e) =>
                {
                    //sceneX = game.Height;
                    //sceneY = game.Width;
                    GL.Viewport(0, 0, windowX, windowY);  //unZoomed
                };

                var mouse = Mouse.GetState();
                game.UpdateFrame += (sender, e) =>
                {
                    // add game logic, input handling
                    mouse = Mouse.GetState();

                    keyboard = Keyboard.GetState();

                    if (keyboard[Key.Escape])
                    {
                        game.Exit();
                    }

                    if (keyboard[Key.F12])
                    {
                        debugInfo = !debugInfo;
                        Thread.Sleep(200);
                    }

                    if (keyboard[Key.T])
                        itemSpawnTimer = 0;

                    if (keyboard[Key.F])
                    {
                        fullscreen = !fullscreen;
                        Thread.Sleep(200);

                        if (fullscreen)
                        {
                            game.WindowState = WindowState.Fullscreen;

                            display.ChangeResolution(800, 600, 32, 60);
                        }
                        else
                        {
                            game.WindowState = WindowState.Normal;
                            display.RestoreResolution();
                        }
                    }

                    if (keyboard[Key.T])
                        karte.spriteAdd2(new Tank((short)rnd.Next(16, 768), (short)rnd.Next(16, 568), (Byte)rnd.Next(0, 4), (Byte)rnd.Next(0, 7), true, (Byte)rnd.Next(0, 4)));

                    game.Title = ("Battle City - FPS: " + (int)(game.RenderFrequency) + "  " + Map.spriteCount + " Objects, " + "Mouse[" + game.Mouse.X + " ; " + game.Mouse.Y + "]");
                };

                game.RenderFrame += (sender, e) =>
                {
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    //GL.Scale(Map.temp, Map.temp, 0);
                    GL.MatrixMode(MatrixMode.Projection);
                    //GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();

                    //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    //Console.WriteLine("Cam (X:" + camX + " |Y:" + camY + ")");


                    GL.Viewport((int)(0), (int)(0), windowX, windowY);
                    GL.LineWidth(1.0f);
                    game.Width = windowX;
                    game.Height = windowY;
                    Vector2d mouseVector = new Vector2d(game.Mouse.X, game.Mouse.Y);

                    
                    GL.Ortho((int)0, (int)windowX, (int)windowY, (int)0, -1000, 1000);  //Render  distant objects smaller
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.PopMatrix();
                    GL.PushMatrix();

                    MyImage.beginDraw2D();

                    if (npcFreezeTimer > 0)
                        npcFreezeTimer--;

                    if (itemSpawnTimer > 0)
                        itemSpawnTimer--;
                    if (itemSpawnTimer == 0)
                    {
                        karte.spriteAdd2(new Item((short)(rnd.Next(0, 48) * 16), (short)(rnd.Next(0, 36) * 16)));
                        itemSpawnTimer = (short)rnd.Next(300, 1200);
                    }
                                        
                    frame++;
                    if (frame == frameDelay)
                    {
                        frame = 0;
                        frameState = !frameState;
                    }
                    
                    karte.process(game.Mouse.X, game.Mouse.Y);


                    

                    



                    MyImage.endDraw2D();

                    game.SwapBuffers();
                };

                // Run the game at 60 updates per second
                game.Run(60);
            }
        }
        
        //End of Root =============================================================






        public static void process()
        {


        }

    }
}

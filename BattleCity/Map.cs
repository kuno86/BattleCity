using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;

namespace BattleCity
{
    class Map
    {
        public static double mouseX;
        public static double mouseY;
        public static int mouseWheel;

        public static MapTile[,] map = new MapTile[38, 50]; //y,X !!!

        public static short spriteArrMax;
        public static BaseObj[] spriteArray = new BaseObj[2048];
        public static short spriteCount;

        public Map()
        {
            Random rnd = new Random();

            for (short yy = 0; yy < map.GetLength(0); yy++)
            {
                for (short xx = 0; xx < map.GetLength(1); xx++)
                {
                    map[yy, xx] = new MapTile(0, xx, yy);
                }
            }

            for (short yy = 0; yy < map.GetLength(0); yy += 2)
            {
                for (short xx = 0; xx < map.GetLength(1); xx += 2)
                {
                    Byte tmpTile = (Byte)rnd.Next(0, 50);
                    if (tmpTile > 10)
                        tmpTile = 0;
                    map[yy, xx] = new MapTile(tmpTile, xx, yy);
                    map[yy + 1, xx] = new MapTile(tmpTile, xx, (short)(yy + 1));
                    map[yy, xx + 1] = new MapTile(tmpTile, (short)(xx + 1), yy);
                    map[yy + 1, xx + 1] = new MapTile(tmpTile, (short)(xx + 1), (short)(yy + 1));
                }
            }

            spriteAdd(new Eaglebase());
            spriteAdd(new Spawn(200, 576, 0));
            spriteAdd(new Spawn(600, 0, 1));
            spriteAdd(new Tank(100, 100, 1, 0, false, 0, 0));
            spriteAdd(new Tank(500, 100, 2, 5, true, 2, 1));
        }

        public void process(int mausX, int mausY)
        {
            for (short yy = 0; yy < map.GetLength(0); yy++)
            {
                for (short xx = 0; xx < map.GetLength(1); xx++)
                {
                    if (map[yy, xx].id != 7 && map[yy, xx].id != 11)  //7 = Bushes which are rendered over tanks    11 = Eaglebase tile wich is not rendered as a tile, but as a sprite
                    {
                        map[yy, xx].tick();

                    }
                    if (RootThingy.debugInfo && (map[yy, xx].colRect.w > 0 || map[yy, xx].colRect.h > 0))
                    {
                        MyImage.endDraw2D();
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Color3(Color.Red);
                        GL.Vertex2(map[yy, xx].colRect.x, map[yy, xx].colRect.y);
                        GL.Vertex2(map[yy, xx].colRect.x + map[yy, xx].colRect.w, map[yy, xx].colRect.y);
                        GL.Vertex2(map[yy, xx].colRect.x + map[yy, xx].colRect.w, map[yy, xx].colRect.y + map[yy, xx].colRect.h);
                        GL.Vertex2(map[yy, xx].colRect.x, map[yy, xx].colRect.y + map[yy, xx].colRect.h);
                        GL.End();
                        MyImage.beginDraw2D();
                    }
                }
            }

            spriteCount = 0;
            for (int i = 0; i <= spriteArrMax; i++)
            {
                if (spriteArray[i] != null)
                {
                    spriteCount++;
                    if ((spriteArray[i].x > RootThingy.windowX) || (spriteArray[i].x + spriteArray[i].w < 0) || (spriteArray[i].y > RootThingy.windowY) || (spriteArray[i].y + spriteArray[i].h < 0))
                    {
                        spriteArray[i] = null;
                        continue;
                    }
                    spriteArray[i].tick();

                    if (RootThingy.debugInfo)
                    {
                        MyImage.endDraw2D();
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Color3(Color.Aqua);
                        GL.Vertex2(spriteArray[i].colRect.x, spriteArray[i].colRect.y);
                        GL.Vertex2(spriteArray[i].colRect.x + spriteArray[i].colRect.w, spriteArray[i].colRect.y);
                        GL.Vertex2(spriteArray[i].colRect.x + spriteArray[i].colRect.w, spriteArray[i].colRect.y + spriteArray[i].colRect.h);
                        GL.Vertex2(spriteArray[i].colRect.x, spriteArray[i].colRect.y + spriteArray[i].colRect.h);
                        GL.End();
                        MyImage.beginDraw2D();
                    }

                }
            }

            for (short yy = 0; yy < map.GetLength(0); yy++)
            {
                for (short xx = 0; xx < map.GetLength(1); xx++)
                {
                    if (map[yy, xx].id == 7)  //
                    {
                        map[yy, xx].tick();
                    }
                }
            }

            var maus = Mouse.GetState();
            mouseWheel = maus.Wheel;
            while (mouseWheel < 0)
                mouseWheel += 10;
            while (mouseWheel > 10)
                mouseWheel -= 10;

            if (RootThingy.frameState)
                MyImage.drawTileFromXY(Texture.gfx_tiles, 0, mouseWheel, 16, ((short)(mausX / 16) * 16), ((short)(mausY / 16) * 16));
            else
                MyImage.drawTileFromXY(Texture.gfx_tiles, 1, mouseWheel, 16, ((short)(mausX / 16) * 16), ((short)(mausY / 16) * 16));

            if (maus[MouseButton.Left])
            {
                map[(short)(mausY / 16), (short)(mausX / 16)] = new MapTile((Byte)mouseWheel, (short)(mausX / 16), (short)(mausY / 16));
            }
            if (maus[MouseButton.Right])
            {
                map[mausY / 16, mausX / 16] = new MapTile(0, (short)(mausX / 16), (short)(mausY / 16));
            }

        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ADD_A_SPRITE
        public static int spriteAdd(BaseObj spriteObj)
        {
            bool done = false;
            short i = 0;
            while (!done && i <= spriteArray.Count())
            {
                if (spriteArray[i] == null)
                {
                    spriteArray[i] = spriteObj;
                    spriteArray[i].id = i;
                    done = true;
                    if (i > spriteArrMax)
                        spriteArrMax = i;
                }
                else
                {
                    i++;
                }
            }
            return i;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ADD_A_SPRITE
        public void spriteAdd2(BaseObj spriteObj)
        {
            bool done = false;
            short i = 0;
            while (!done && i <= spriteArray.Count())
            {
                if (spriteArray[i] == null)
                {
                    spriteArray[i] = spriteObj;
                    spriteArray[i].id = i;
                    done = true;
                    if (i > spriteArrMax)
                        spriteArrMax = i;
                }
                else
                {
                    i++;
                }
            }
        }

        public void Arsch()
        { ;}


    }
}

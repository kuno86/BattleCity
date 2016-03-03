using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace BattleCity
{
    class MapTile : BaseObj
    {
        public Byte id;
        public bool destroyed=false;
        public bool takesDmg;

        private static Byte[,] cols = new Byte[13, 4]{
            {0,0,0,0},
            {16,16,0,0},
            {16,16,0,0},
            {8,16,8,0},
            {8,16,0,0},
            {16,8,0,8},
            {16,8,0,0},
            {0,0,0,0},
            {0,0,0,0},
            {16,16,0,0},
            {16,16,0,0},
            {0,0,0,0},
            {0,0,0,0}};

        public MapTile(Byte id, short x, short y)
            : base((short)(x*16), (short)(y*16), 16, 16, (short)(x*16), (short)(y*16), cols[id, 0], cols[id, 1], cols[id, 2], cols[id, 3])
        {
            this.x = (short)(x * 16);
            this.y = (short)(y * 16);

            this.texture = Texture.gfx_tiles;
            
            switch (id)
            {
                default:
                case 0:     //Nothing
                    this.id = 0; 
                    this.takesDmg = false; 
                    break;
                case 1:     //Iron
                    this.id = 1;
                    this.takesDmg = false;
                    break;
                case 2:     //Bricks
                    this.id = 2;
                    this.takesDmg = true;
                    break;
                case 3:     //Half brick right
                    this.id = 3;
                    this.takesDmg = true;
                    break;
                case 4:     //Half brick left
                    this.id = 4;
                    this.takesDmg = true;
                    break;
                case 5:     //Half brick bottom
                    this.id = 5;
                    this.takesDmg = true;
                    break;
                case 6:     //Half brick top
                    this.id = 6;
                    this.takesDmg = true;
                    break;
                case 7:     //Bush/tall grass
                    this.id = 7;
                    this.takesDmg = false;
                    break;
                case 8:     //Ice floor
                    this.id = 8;
                    this.takesDmg = false;
                    break;
                case 9:     //Still water
                    this.id = 9;
                    this.takesDmg = false;
                    break;
                case 10:    //Animated water
                    this.id = 10;
                    this.takesDmg = false;
                    break;
                case 11:    //Eaglebase
                    this.id = 11;
                    this.takesDmg = false;
                    break;
                case 12:    //Spawn
                    this.id = 12;
                    this.takesDmg = false;
                    break;
            }
            

        }

        public override void tick()
        {
            if (RootThingy.frameState)
                MyImage.drawTileFromXY(this.texture, 0 ,this.id, 16, x, y);
            else
                MyImage.drawTileFromXY(this.texture, 1, this.id, 16, x, y);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BattleCity  //Only purpose is to hold all the textures
{
    class Texture
    {
        public static int gfx_ASCII = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\ASCII-Characters_8x12.bmp");
        public static int gfx_tiles = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tiles.bmp");
        public static int gfx_base = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\base.bmp");
        public static int gfx_incoming = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\incoming.bmp");

        public static int gfx_tank1 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank1_all_30.bmp");
        public static int gfx_tank2 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank2_all_42.bmp");
        public static int gfx_tank3 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank3_all_36.bmp");
        public static int gfx_tank4 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank4_all_32.bmp");
        public static int gfx_tank5 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank5_all_34.bmp");
        public static int gfx_tank6 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank6_all_34.bmp");
        public static int gfx_tank7 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank7_all_36.bmp");
        public static int gfx_tank8 = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank8_all_30.bmp");

        public static int gfx_spawned = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\spawned.bmp");
        public static int gfx_bullet = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\bullet.bmp");
        public static int gfx_explosion = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\explosion.bmp");
        public static int gfx_explosionb = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\explosionbig.bmp");
        public static int gfx_powerups = MyImage.LoadTexture(RootThingy.exePath + @"\gfx\powerups.bmp");



        //public int gfx_ASCII;
        //public int gfx_tiles;
        //public int gfx_base;
        //public int gfx_incoming;

        //public int gfx_tank1;
        //public int gfx_tank2;
        //public int gfx_tank3;
        //public int gfx_tank4;
        //public int gfx_tank5;
        //public int gfx_tank6;
        //public int gfx_tank7;
        //public int gfx_tank8;
            
        //public int gfx_spawned;
        //public int gfx_bullet;
        //public int gfx_explosion;
        //public int gfx_explosionb;
        //public int gfx_powerups;
                
        //public Texture()
        //{
        //    gfx_ASCII =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\ASCII-Characters_8x12.bmp");
        //    gfx_tiles =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tiles.bmp");
        //    gfx_base =        MyImage.LoadTexture(RootThingy.exePath + @"\gfx\base.bmp");
        //    gfx_incoming =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\incoming.bmp");

        //    gfx_tank1 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank1_all_30.bmp");
        //    gfx_tank2 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank2_all_42.bmp");
        //    gfx_tank3 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank3_all_36.bmp");
        //    gfx_tank4 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank4_all_32.bmp");
        //    gfx_tank5 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank5_all_34.bmp");
        //    gfx_tank6 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank6_all_34.bmp");
        //    gfx_tank7 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank7_all_36.bmp");
        //    gfx_tank8 =       MyImage.LoadTexture(RootThingy.exePath + @"\gfx\tank8_all_30.bmp");
            
        //    gfx_spawned =     MyImage.LoadTexture(RootThingy.exePath + @"\gfx\spawned.bmp");
        //    gfx_bullet =      MyImage.LoadTexture(RootThingy.exePath + @"\gfx\bullet.bmp");
        //    gfx_explosion =   MyImage.LoadTexture(RootThingy.exePath + @"\gfx\explosion.bmp");
        //    gfx_explosionb =  MyImage.LoadTexture(RootThingy.exePath + @"\gfx\explosionbig.bmp");
        //    gfx_powerups =    MyImage.LoadTexture(RootThingy.exePath + @"\gfx\powerups.bmp");

        //}
    }
}


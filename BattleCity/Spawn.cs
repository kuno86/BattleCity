using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BattleCity
{
    class Spawn : BaseObj
    {
        public Byte id = 12;
        public bool takesDmg = false;

        private Byte team;
        private short timer = 0;

        private int counter = 0;

        private SByte frame = 0;
        private SByte frames=4;
        private Byte delay;
        private bool small;
        private SByte adder = 1;

        public Spawn(short x = 25, short y = 19, Byte team = 0, bool npcSpawn = false)
            : base(x, y, 32, 32, 0, 0)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.texture = Texture.gfx_incoming;
            this.name = "spawn" + team;
            if (npcSpawn)
                this.name = "spawn#";
            short xx = (short)(x / 16);
            short yy = (short)(y / 16);
            Map.map[yy, xx] = new MapTile(12, xx, yy);
            Map.map[yy + 1, xx] = new MapTile(12, xx, (short)(yy + 1));
            Map.map[yy, xx + 1] = new MapTile(12, (short)(xx + 1), yy);
            Map.map[yy + 1, xx + 1] = new MapTile(12, (short)(xx + 1), (short)(yy + 1));

        }

        public override void tick()
        {
            
            if (RootThingy.spawner != "")
            {
                string newString = "";
                foreach (string s in (RootThingy.spawner.Split(',')))
                {
                    Byte tempTeam;
                    if (byte.TryParse(s, out tempTeam))
                    {
                        if (tempTeam != team)
                            newString += tempTeam.ToString() + ",";
                        else
                            timer = 20; //20s
                    }
                }
                RootThingy.spawner = newString;
            }

            //if (timer > 0)
            //{
                
            //    frame += adder;
            //    if (frame == frames)
            //        adder = (sbyte)(adder * -1);
            //    //Thread.Sleep(400);
            //    Console.WriteLine("Frame: " + frame + "  Adder: " + adder);

            //    if (frame == 0 && adder < 0)
            //    {
            //        timer--;
            //        adder = (sbyte)(adder * -1);
            //    }
            //}
            //if (timer == 0)
            //    frame = 0;

            if (delay > 0)
                delay--;
            if (delay == 0)
            {
                delay = 2;
                frame += adder;
                if (frame == frames)
                    adder = (sbyte)(adder * -1);

                //Thread.Sleep(400);
                Console.WriteLine("Frame: " + frame + "  Adder: " + adder);

                if (frame == 0 && adder < 0)
                {
                    x = -100;
                    y = -100;
                }
            }

            MyImage.drawTileFrame(this.texture, frame, 5, (x * 16) - 32, (y * 16) - 32);

        }



        

    }
}


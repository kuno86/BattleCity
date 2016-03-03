using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BattleCity
{
    class Explosion : BaseObj
    {
        private short frame=0;
        private short frames;
        private bool small;
        private short adder = 1;
        private short delay = 2;
  
        public Explosion(short x, short y, bool sizeSmall)
            : base(x, y)
        {
            this.x = x;
            this.y = y;
            this.name = "explosion";
            if (sizeSmall)
            {
                frames = 2;
                this.small = true;
                this.texture = Texture.gfx_explosion;
            }
            else
            {
                frames = 4;
                this.small = false;
                this.texture = Texture.gfx_explosionb;
            }

        }

        public override void tick()
        {
            if (small)
                MyImage.drawTileFrame(this.texture, frame, 3, x - 16, y - 16);
            else
                MyImage.drawTileFrame(this.texture, frame, 5, x - 32, y - 32);
            if (delay > 0)
                delay--;
            if(delay==0)
            {
                delay = 2;
                frame+=adder;
                if (frame == frames)
                    adder = (short)(adder * -1);

                //Thread.Sleep(400);
                Console.WriteLine("Frame: " + frame + "  Adder: " + adder);

                if (frame == 0 && adder < 0)
                {
                    x = -100;
                    y = -100;
                }
            }
        }


    }
}

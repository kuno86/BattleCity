using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    class Item : BaseObj
    {
        private Byte type;

        public Item(short x, short y, SByte type = -1)
            :base(x, y, 32, 32, 32, 32)
        {
            this.x = x;
            this.y = y;
            
            if (type < 0 || type > 6)
                this.type = (Byte)(RootThingy.rnd.Next(0, 7));
            else
                this.type = (Byte)type;
            this.name = "item" + this.type;
            this.texture = Texture.gfx_powerups;
        }

        public override void tick()
        {
            MyImage.drawTileFrame(this.texture,type, 7, x, y);
        }
    }
}

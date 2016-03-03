using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    class BaseObj
    {

        public struct ColRect
        {
            public Byte w;
            public Byte h;
            public Byte offsetX;
            public Byte offsetY;
            public short x;
            public short y;
        }
        public short id;
        public double x;
        public double y;
        public Byte w;
        public Byte h;

        public ColRect colRect;
        public int texture;
        public bool isNPC;

        public string name;

        public BaseObj(short x, short y, Byte w, Byte h, byte colW, byte colH, byte colOffsetX = 0, byte colOffsetY = 0)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.colRect.w = colW;
            this.colRect.h = colH;
            this.colRect.offsetX = colOffsetX;
            this.colRect.offsetY = colOffsetY;
            refreshColRect();
        }

        public BaseObj(short x, short y, Byte w, Byte h, short colx, short coly, byte colW, byte colH, byte colOffsetX = 0, byte colOffsetY = 0)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.colRect.w = colW;
            this.colRect.h = colH;
            this.colRect.offsetX = colOffsetX;
            this.colRect.offsetY = colOffsetY;
            this.colRect.x = (short)(colx + colRect.offsetX);
            this.colRect.y = (short)(coly + colRect.offsetY);
        }

        public BaseObj(short x, short y)
        {
            this.x = x;
            this.y = y;
        }

        public virtual void tick()
        { ;}

        public bool getCol2Obj(ColRect obj1, ColRect obj2)
        {
            if ((obj1.x + obj1.w >= obj2.x && obj1.x <= obj2.x + obj2.w) && (obj1.y + obj1.h >= obj2.y && obj1.y <= obj2.y + obj2.h))
                return true;
            else
                return false;
        }

        public void refreshColRect()
        {
            colRect.x = (short)(x + colRect.offsetX);
            colRect.y = (short)(y + colRect.offsetY);
        }
    }
}

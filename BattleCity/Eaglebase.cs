using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    class Eaglebase : BaseObj
    {
        public Byte id = 11;
        public bool destroyed = false;
        public bool takesDmg = true;

        private Byte team;
        private short shieldTimer = 0;

        public Eaglebase(short x = 400, short y = 560, Byte team = 0)
            :base(x, y, 32, 32, 32, 32)
        {
            this.x = x;
            this.y = y;
            this.team = team;
            this.name = "eaglebase";
            this.texture = Texture.gfx_base;
            short xx = (short)(x/16);
            short yy = (short)(y/16);
            Map.map[yy, xx] = new MapTile(11, xx, yy);
            Map.map[yy + 1, xx] = new MapTile(11, xx, (short)(yy + 1));
            Map.map[yy, xx + 1] = new MapTile(11, (short)(xx + 1), yy);
            Map.map[yy + 1, xx + 1] = new MapTile(11, (short)(xx + 1), (short)(yy + 1));

            setBorder(2);
        }

        public override void tick()
        {
            for (int i = 0; i <= Map.spriteArrMax; i++)
            {
                BaseObj obj = Map.spriteArray[i];
                if (Map.spriteArray[i] != null && Map.spriteArray[i] != this && getCol2Obj(this.colRect, obj.colRect))
                {
                    if (Map.spriteArray[i].name == "bullet")
                    {
                        destroyed = true;
                        Map.spriteAdd(new Explosion((short)(x + colRect.offsetX + (colRect.w / 2)), (short)(y + colRect.offsetY + (colRect.h / 2)), false));
                        Map.spriteArray[i].x = -100;
                        Map.spriteArray[i].y = -100;
                    }
                }
            }
            
            if (RootThingy.eaglebaseShild != "")
            {
                string newString = "";
                foreach (string s in (RootThingy.eaglebaseShild.Split(',')))
                {
                    Byte tempTeam;
                    if (byte.TryParse(s, out tempTeam))
                    {
                        if (tempTeam != team)
                            newString += tempTeam.ToString() + ",";
                        else
                            shieldTimer = 1201; //20s
                    }
                }
                RootThingy.eaglebaseShild = newString;
            }

            if (shieldTimer > 0)
            {
                shieldTimer--;
                if (shieldTimer == 1200)
                    setBorder(1);
                if ((shieldTimer <= 360) && (shieldTimer % 30 == 0))
                    setBorder(2);
                if ((shieldTimer <= 360) && (shieldTimer % 60 == 0))
                    setBorder(1);
                if (shieldTimer == 0)
                    setBorder(2);
            }

            if (destroyed)
                MyImage.drawTileFrame(this.texture, 1, 2, x, y);
            else
                MyImage.drawTileFrame(this.texture, 0, 2, x, y);
        }

        

        private void setBorder(Byte tile)
        {
            short xx = (short)(x / 16);
            short yy = (short)(y / 16);
            yy--;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
            { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            xx++;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            xx++;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            yy++;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            yy++;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            yy++;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            xx--;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            xx--;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            xx--;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            yy--;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            yy--;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            yy--;
            if (yy >= 0 && yy < Map.map.GetLength(0) && xx >= 0 && xx < Map.map.GetLength(1))
                { Map.map[yy, xx] = new MapTile(tile, (short)(xx), (short)(yy)); }
            
        }
        
    }
}

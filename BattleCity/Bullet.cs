using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    class Bullet : BaseObj
    {
        private Byte dir;
        private Byte ownerID;
        private double bulletSpeed;
        private bool destroysIron;

        public Bullet(short x, short y, Byte dir, Byte OwnerID, double bulletSpeed, bool destroysIron, bool isNPC)
            : base((short)x, (short)y, (byte)18, (byte)18, (byte)6, (byte)6, (byte)2, (byte)2)
        {
            this.x = x;
            this.y = y;
            this.dir = dir;
            this.ownerID = OwnerID;
            this.isNPC = isNPC;
            this.bulletSpeed = bulletSpeed;
            this.name = "bullet";
            this.texture = Texture.gfx_bullet;
        }

        public override void tick()
        {
            switch (dir)
            {
                case 0: y -= bulletSpeed; MyImage.drawTileFrame(texture, 0, 4, x, y); break; //Up
                case 2: x -= bulletSpeed; MyImage.drawTileFrame(texture, 1, 4, x, y); break; //Left
                case 4: y += bulletSpeed; MyImage.drawTileFrame(texture, 2, 4, x, y); break; //Down
                case 6: x += bulletSpeed; MyImage.drawTileFrame(texture, 3, 4, x, y); break; //Right
                default: ; break;
            }

            refreshColRect();

            for (int i = 0; i <= Map.spriteArrMax; i++)
            {
                BaseObj obj = Map.spriteArray[i];
                if (Map.spriteArray[i] != null && Map.spriteArray[i].id != this.id && getCol2Obj(this.colRect, obj.colRect))
                {
                    if (Map.spriteArray[i].name == "bullet")
                    {
                        Map.spriteAdd(new Explosion((short)(obj.x + 1), (short)(obj.y + 1), true));
                        obj.x = -100;
                        this.x = -100;
                        continue;
                    }
                }
            }

            for (short yy = 0; yy < Map.map.GetLength(0); yy++)
            {
                for (short xx = 0; xx < Map.map.GetLength(1); xx++)
                {
                    if (Map.map[yy, xx].colRect.w > 0 && Map.map[yy, xx].colRect.h > 0 && getCol2Obj(this.colRect, Map.map[yy, xx].colRect))  
                    {
                        if (Map.map[yy, xx].takesDmg)
                        {
                            Map.map[yy, xx] = new MapTile(0, xx, yy);
                            Map.spriteAdd(new Explosion((short)(x + 2), (short)(y + 2), true));
                            this.x = -100;
                            this.y = -100;
                        }
                        else
                        {
                            if (Map.map[yy, xx].id == 1 && this.destroysIron)
                            {
                                Map.spriteAdd(new Explosion((short)(x + 2), (short)(y + 2), true));
                                Map.map[yy, xx] = new MapTile(0, xx, yy);
                                this.x = -100;
                                this.y = -100;
                            }
                            if (Map.map[yy, xx].id != 9 && Map.map[yy, xx].id != 10)    //These are the two water-blocks that don't block bullets
                            {
                                Map.spriteAdd(new Explosion((short)(x + 2), (short)(y + 2), true));
                                this.x = -100;
                                this.y = -100;
                            }
                        }
                    }
                }
            }
                        
        }



    }
}

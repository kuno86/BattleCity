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

namespace BattleCity
{
    class Tank : BaseObj
    {
        private Byte dir;   //0=Up, 2=Left, 4=Down, 6=Right
        private Byte color; //0=Yellow, 1=Green, 2=Silver, 3=Red
        private double speed = 1;
        private Byte type = 0;
        private Byte reloadTime = 30; //ticks
        private Byte reloadTimer;
        private Byte health = 1;
        private Byte lives = 5;
        private bool isGameOver = false;

        private TankData thisTank;

        private Byte frameID;
        private Byte frame;
        private Byte frameDelay = 6;    //ticks

        private short spawnDelay = 160;
        private short shieldTimer = 0;
        private Byte shieldframe = 0;
        private Byte team;

        private bool canGoLeft, canGoRight, canGoUp, canGoDown;

        public static Byte playerID = 0;
        
        private static byte[,] cols = new Byte[8,3]{
        {30,4,22},
        {42,10,26},
        {36,6,24},
        {32,4,28}, //{w & h, colOffsetX & colOffsetY, colW & colH}
        {34,6,22},
        {34,6,22},
        {36,6,22},
        {30,4,28}
        };


        private static TankData[] stats = new TankData[8]{   //{speed, reloadtime, bullet-speed, bulletDestroysIron}
        new TankData(1.0, 40, 1.5, false),
        new TankData(1.5, 20, 1.7, false),
        new TankData(1.5, 20, 2.0, false),
        new TankData(1.5, 20, 2.0, true),
        new TankData(1.0, 40, 1.5, false),
        new TankData(1.7, 40, 1.5, false),
        new TankData(1.0, 40, 1.5, false),
        new TankData(1.0, 40, 1.5, false)
        };

        public Tank(short x, short y, Byte color = 0, Byte type = 0, bool isNPC = false, Byte dir = 0, Byte team=0)
            : base(x, y, cols[type, 0], cols[type, 0], cols[type, 2], cols[type, 2], cols[type, 1], cols[type, 1])
        {
            this.x = x;
            this.y = y;
            playerID++;
            this.type = type;
            this.name = "player";
            this.isNPC = isNPC;
            this.team = team;

            

            if (isNPC)
            {
                this.lives = 1;
                this.name = "npc";
                this.health = 1;
            }
            if (!isNPC)
                this.shieldTimer = 240; //4s

            spawn();

            selectTank(type);
            
            switch (dir)
            {
                default:
                case 0: this.dir = 0; break;
                case 1: this.dir = 2; break;
                case 2: this.dir = 4; break;
                case 3: this.dir = 6; break;
            }

            switch (color)
            {
                default:
                case 0: this.color = 0; break;
                case 1: this.color = 1; break;
                case 2: this.color = 2; break;
                case 3: this.color = 3; break;
            }
        }

        public override void tick()
        {
            canGoLeft = true;
            canGoRight = true;
            canGoUp = true;
            canGoDown = true;

            for (int i = 0; i <= Map.spriteArrMax; i++)
            {
                BaseObj obj = Map.spriteArray[i];
                if (Map.spriteArray[i] != null && getCol2Obj(this.colRect, obj.colRect))
                {
                    if (this.isNPC)
                    {
                        if (obj.name == "bullet" && !obj.isNPC)
                        {
                            Map.spriteAdd(new Explosion((short)(obj.x + 1), (short)(obj.y + 1), true));
                            obj.x = -100;
                            damage();
                            continue;
                        }
                        
                    }
                    else
                    {
                        if (obj.name == "bullet" && obj.isNPC)
                        {
                            Map.spriteAdd(new Explosion((short)(obj.x + 1), (short)(obj.y + 1), true));
                            obj.x = -100;
                            damage();                            
                            continue;
                        }
                    }
                    if (obj.name == "player" || obj.name == "npc")  // This checks against other Tanks
                    {
                        if (obj.colRect.x < colRect.x)
                            canGoUp = false;
                        if (obj.colRect.x > colRect.x)
                            canGoDown = false;
                        if (obj.colRect.y < colRect.y)
                            canGoLeft = false;
                        if (obj.colRect.y > colRect.y)
                            canGoRight = false;
                    }
                    if (!isNPC && obj.name.StartsWith("item"))  /////////////////////Item-Handling
                    {
                        short itemId = short.Parse(obj.name.Substring(obj.name.Length - 1,1));
                        Console.WriteLine("Collected item " + itemId);
                        switch (itemId)
                        {
                            case 0:     //Helmet	= gives Spawnshield for 10s 
                                shieldTimer = 600; 
                                break;
                            case 1:     //Clock	    = Freezes all NPCs  for 10s (not their bullets !!) 
                                RootThingy.npcFreezeTimer=600; 
                                break;
                            case 2:     //Shovel	= Iron-Blocks around Base for 17s

                                for (int j = 0; j < Map.spriteArray.Length; j++)
                                {
                                    BaseObj obj2 = Map.spriteArray[j];
                                    if (obj2 != null && obj2.name == "eaglebase")
                                    {
                                        RootThingy.eaglebaseShild = team.ToString()+",";
                                    }
                                }
                                break;    
                            case 3:     //Star	    = better tank 
                                upgradeTank();
                                break;    
                            case 4:     //Grenade   = Destroy all NPCs/Enemies 
                                for (int j = 0; j < Map.spriteArray.Length; j++)
                                {
                                    BaseObj obj2 = Map.spriteArray[j];
                                    if (Map.spriteArray[j] != null && Map.spriteArray[j].name == "npc")
                                    {
                                        Map.spriteAdd(new Explosion((short)(obj2.x + obj2.colRect.offsetX + (obj2.colRect.w / 2)), (short)(obj2.y + obj2.colRect.offsetY + (obj2.colRect.h / 2)), false));
                                        Map.spriteArray[j] = null;
                                    }
                                }
                                break;    
                            case 5:      //Tank	    = extra-live
                                lives++; 
                                break;    
                            case 6:      //Gun	    = gives you 3 Hp (?)
                                health = 3;
                                break;
                            default: ; break;
                        }
                        obj.x = -100;
                        obj.y = -100;
                    }
                }
            }

            foreach (MapTile mT in Map.map)
            {
                if (mT.colRect.w > 0 || mT.colRect.h > 0)    //does this tile have a collisionbox ?
                {
                    if(getCol2Obj(this.colRect, mT.colRect))
                    {
                        if (mT.colRect.x < colRect.x)
                            canGoUp = false;
                        if (mT.colRect.x > colRect.x)
                            canGoDown = false;
                        if (mT.colRect.y < colRect.y)
                            canGoLeft = false;
                        if (mT.colRect.y > colRect.y)
                            canGoRight = false;
                    }
                }
            }

            if (spawnDelay > 0)
                spawnDelay--;
            else
            {
                if (reloadTimer > 0)
                    reloadTimer--;

                if (shieldTimer > 0)
                    shieldTimer--;

                if (shieldTimer % 4 == 0)
                    shieldframe = 1;
                else
                    shieldframe = 0;

                if (isNPC)
                {
                    npcAI();
                }
                else
                {
                    var keyboard = Keyboard.GetState();
                    if (keyboard[Key.Up])
                    {
                        dir = 0;
                        frame++;
                        if (canGoUp)
                            y -= speed;
                    }

                    if (keyboard[Key.Left])
                    {
                        dir = 2;
                        frame++;
                        if (canGoLeft)
                            x -= speed;
                    }
                    if (keyboard[Key.Down])
                    {
                        dir = 4;
                        frame++;
                        if (canGoDown)
                            y += speed;
                    }
                    if (keyboard[Key.Right])
                    {
                        dir = 6;
                        frame++;
                        if (canGoRight)
                            x += speed;
                    }



                    if (keyboard[Key.Space] && reloadTimer <= 0)
                    {
                        shoot();
                    }

                    if (frame == frameDelay)
                    {
                        frameID++;
                        if (frameID > 1)
                            frameID = 0;
                        frame = 0;
                    }


                    if (keyboard[Key.L])
                    {
                        color++;
                        Thread.Sleep(150);
                        if (color > 3)
                            color = 0;
                    }
                    if (keyboard[Key.Enter])
                    {
                        type++;
                        Thread.Sleep(150);
                        if (type > 7)
                            type = 0;

                        selectTank(type);
                    }
                }

                refreshColRect();

                
                MyImage.drawTileFromXY(this.texture, (frameID + dir), color, cols[type, 0], x, y);

                if (shieldTimer > 0)
                {
                    if (type == 1)
                        MyImage.drawTileFrame(Texture.gfx_spawned, shieldframe, 2, x + 4, y + 4);
                    else
                        MyImage.drawTileFrame(Texture.gfx_spawned, shieldframe, 2, x, y);

                    if (isGameOver)
                        MyImage.drawText("GAME OVER", x - 36 + (colRect.w / 2) + colRect.offsetX, y - 12, Color.Red, Texture.gfx_ASCII);
                }
            }
        }

        private void damage()
        {
            if(shieldTimer <= 0)
                health--;

            if(health <= 0)
            {
                Map.spriteAdd(new Explosion((short)(x + colRect.offsetX + (colRect.w / 2)), (short)(y + colRect.offsetY + (colRect.h / 2)), false));
                if (isNPC)
                    gameOver();
                else
                {
                    type = 0;
                    selectTank(type);
                    loosLive();
                    spawn();
                }
            }
        }

        private void loosLive()
        {
            if (isNPC)
            {
                x = -100;
                y = -100;
            }
            else
            {
                lives--;
                if (lives <= 0)
                    gameOver();
                
            }
        }

        private void gameOver()
        {
            if (isNPC)
            {
                x = -100;
                y = -100;
            }
            else
            {
                selectTank(type);
                spawn();
                isGameOver = true;
            }
        }

        private void spawn(Byte type = 0)
        {
            RootThingy.spawner = team + ",";
            type = 0;
            health = 1;
            selectTank(type);

            foreach (BaseObj obj in Map.spriteArray)
            {
                if (obj != null && obj.name.StartsWith("spawn"))
                {
                    if (this.team == Byte.Parse(obj.name.Substring(5, 1)))
                    {
                        this.x = (short)(obj.x);
                        this.y = (short)(obj.y);
                        if(!isNPC)
                            shieldTimer = 240;
                        spawnDelay = 160;
                    }
                }
            }
        }
        

        private void shoot()
        {
            switch (dir)
            {
                case 0: Map.spriteAdd(new Bullet((short)(x + (cols[type, 0] / 2) - 5), (short)(y + cols[type, 1]), dir, playerID, thisTank.bulletSpeed, thisTank.destroysIron, isNPC)); break;   //Up
                case 2: Map.spriteAdd(new Bullet((short)(x + cols[type, 1]), (short)(y + (cols[type, 0] / 2) - 5), dir, playerID, thisTank.bulletSpeed, thisTank.destroysIron, isNPC)); break;  //Left
                case 4: Map.spriteAdd(new Bullet((short)(x + (cols[type, 0] / 2) - 5), (short)(y + cols[type, 1] + cols[type, 2] - 5), dir, playerID, thisTank.bulletSpeed, thisTank.destroysIron, isNPC)); break; //Down
                case 6: Map.spriteAdd(new Bullet((short)(x + cols[type, 1] + cols[type, 2]), (short)(y + (cols[type, 0] / 2) - 5), dir, playerID, thisTank.bulletSpeed, thisTank.destroysIron, isNPC)); break; //Right
            }
            reloadTimer = reloadTime;
        }

        private void upgradeTank()
        {
            if (!isNPC)
            {
                if (type < 3)
                    type++;
            }
            selectTank(type);
            
        }

        private void npcAI()
        {
            if (RootThingy.npcFreezeTimer == 0)
            {
                if (reloadTimer == 0)
                    shoot();
            }
        }

        private void selectTank(Byte type)
        {
            if (type >= 0 && type < 8)
            {
                switch (type)   //Update collision data
                {
                    case 0: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                    case 1: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                    case 2: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                    case 3: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                    case 4: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                    case 5: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                    case 6: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                    case 7: this.w = cols[type, 2]; this.h = cols[type, 2]; this.colRect.offsetX = cols[type, 1]; this.colRect.offsetY = cols[type, 1]; break;
                }
                switch (type)   //update the texture
                {
                    case 0: this.texture = Texture.gfx_tank1; break;
                    case 1: this.texture = Texture.gfx_tank2; break;
                    case 2: this.texture = Texture.gfx_tank3; break;
                    case 3: this.texture = Texture.gfx_tank4; break;
                    case 4: this.texture = Texture.gfx_tank5; break;
                    case 5: this.texture = Texture.gfx_tank6; break;
                    case 6: this.texture = Texture.gfx_tank7; break;
                    case 7: this.texture = Texture.gfx_tank8; break;
                }
                                
                thisTank = stats[type];
                this.speed = thisTank.speed;
                this.reloadTime = thisTank.reloadTime;
            }
        }



    }
}

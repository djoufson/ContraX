using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContraX
{
    public class Ennemy : Being
    {
        public bool shootedOnce { get; set; }
        public bool canGoLeft { get; set; }
        public bool canGoRight { get; set; }
        public int shootCounter { get; set; }
        public Random frequency { get; set; }
        public bool canShoot { get; set; }
        public bool canMove { get; set; }
        public bool isOnTheMap { get; set; }
        public bool isFalling { get; set; }
        public bool canBeDeplaced { get; set; }
        public Ennemy(int left, int top, GameMap map)
        {
            canBeDeplaced = true;
            isFalling = false;
            isThePlayer = false;
            frequency = new Random();
            int counter = 0;
            this.map = map;
            this.isLookingRight = false;
            this.canGoRight = true;
            this.canGoLeft = true;
            this.canShoot = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Image = Properties.Resources.PicLeft1;
            this.Tag = "ennemy";
            this.Location = new System.Drawing.Point(30, 143);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Location = new System.Drawing.Point(left, top);

            isOnTheMap = true;

            this.counter = counter;
            map.Controls.Add(this);
            this.BringToFront();
        }
        public void _Move(Player player, int leftBorder, int rightBorder)
        {
            canMove = true;
            //canMove = false;
            /*
            if((this.Left<player.Left + 1000  && this.Left > player.Left - 500) &&
                !(this.Left - player.Left < 250 && this.Left - player.Left > -250))
            {
                canMove = true;
            }
            */
            if (canMove)
            {
                bool isLeftKey = false;
                bool isRightKey = false;
                if (this.Left + this.Width < rightBorder)
                {
                    canGoLeft = true;
                }
                else
                {
                    canGoLeft = false;
                }

                if (this.Left > leftBorder)
                {
                    canGoRight = true;
                }
                else
                {
                    canGoRight = false;
                }
                if (canGoLeft)
                {
                    if (this.Left > player.Left)
                    {
                        isMoving = true;
                        isLeftKey = true;
                    }
                }
                if (canGoRight)
                {
                    if (this.Left <= player.Left)
                    {
                        isMoving = true;
                        isRightKey = true;
                    }
                }
                if (this.isMoving)
                {
                    if (canGoLeft)
                    {
                        if (isLeftKey)
                        {
                            this.Size = new System.Drawing.Size(55, 93);
                            if (isShooting)
                            {
                                if (isShootingUp)
                                {
                                    this.Size = new System.Drawing.Size(67, 93);
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.LeftUp1;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.LeftUp2;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.LeftUp3;
                                    }
                                }
                                else if (isShootingDown)
                                {
                                    this.Size = new System.Drawing.Size(59, 93);
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.LeftDown1;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.LeftDown2;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.LeftDown3;
                                    }
                                }
                                else
                                {
                                    this.Size = new System.Drawing.Size(70, 93);
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.LeftForward1;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.LeftForward2;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.LeftForward3;
                                    }
                                }
                            }
                            else
                            {
                                if (this.Left >= 0)
                                {
                                    this.Size = new System.Drawing.Size(55, 93);
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.PicLeft2;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.PicLeft3;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.PicLeft1;
                                    }
                                }
                            }
                            if (this.Left > 0)
                            {
                                this.Left -= 15;
                            }
                            counter++;

                            if (counter == 4)
                            {
                                counter = 0;
                            }
                        }
                    }
                    if (canGoRight)
                    {
                        if (isRightKey)
                        {
                            this.Size = new System.Drawing.Size(55, 93);
                            if (isShooting)
                            {
                                if (isShootingUp)
                                {
                                    this.Size = new System.Drawing.Size(67, 93);
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.RightUp1;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.RightUp2;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.RightUp3;
                                    }
                                }
                                else if (isShootingDown)
                                {
                                    this.Size = new System.Drawing.Size(59, 93);
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.RightDown1;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.RightDown2;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.RightDown3;
                                    }
                                }
                                else
                                {
                                    this.Size = new System.Drawing.Size(70, 93);
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.RightForward1;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.RightForward2;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.RightForward3;
                                    }
                                }
                            }
                            else
                            {
                                if (this.Left <= 1000)
                                {
                                    if (counter == 0)
                                    {
                                        this.Image = Properties.Resources.Pic2;
                                    }
                                    if (counter == 2)
                                    {
                                        this.Image = Properties.Resources.Pic3;
                                    }
                                    if (counter == 3)
                                    {
                                        this.Image = Properties.Resources.Pic1;
                                    }
                                }
                            }
                            if (this.Left < 1000)
                            {
                                if (canBeDeplaced)
                                {
                                this.Left += 15;
                                }
                            }
                            counter++;

                            if (counter == 4)
                            {
                                counter = 0;
                            }
                        }
                    }
                }
            }
        }
        public void Shoot(Player player)
        {
            canShoot = false;
            int randomNumber = frequency.Next(100);
            if(randomNumber == 3)
            {
                if (isOnTheMap)
                {
                    canShoot = true;
                    shootedOnce = true;
                }
            }
            if (this.Left < player.Left)
            {
                this.isLookingRight = true;
            }
            else
            {
                this.isLookingRight = false;
            }

            if (canShoot)
            {
                bulletList.Add(new Bullet(this, isLookingRight, map));
                if (this.Top < player.Top - player.Height)
                {
                    bulletList[bulletList.Count - 1].forward = false;
                    bulletList[bulletList.Count - 1].up = false;
                    bulletList[bulletList.Count - 1].down = true;
                }
                else if (this.Top > player.Top - player.Height)
                {
                    bulletList[bulletList.Count - 1].forward = false;
                    bulletList[bulletList.Count - 1].up = true;
                    bulletList[bulletList.Count - 1].down = false;
                }
                else
                {
                    bulletList[bulletList.Count - 1].forward = true;
                    bulletList[bulletList.Count - 1].up = false;
                    bulletList[bulletList.Count - 1].down = false;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContraX
{
    public class Being : PictureBox
    {
        public GameMap map { get; set; }
        public List<Bullet> bulletList { get; set; }
        public int counter { get; set; }
        public bool isShooting { get; set; }
        public bool isShootingUp { get; set; }
        public bool isShootingDown { get; set; }
        public bool isLookingRight { get; set; }
        public bool isMoving { get; set; }
        public bool isThePlayer { get; set; }

        public Being()
        {
            this.Size = new System.Drawing.Size(55, 93);
            this.bulletList = new List<Bullet>();
        }
        public void Shoot()
        {
            bulletList.Add(new Bullet(this, isLookingRight, map));
        }
        public void _Move(bool isLeftKey, bool isRightKey, bool isUpKey, bool isDownKey, bool isJumping)
        {
            if (this.isMoving && !isJumping)
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
                    if(this.Left > 0)
                    {
                        this.Left -= 30;
                    }
                    counter++;

                    if (counter == 4)
                    {
                        counter = 0;
                    }
                }
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
                    if (this.Left < 800)
                    {
                        this.Left += 30;
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
    public class Player : Being
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Player(int counter, GameMap map)
        {
            isThePlayer = true;
            this.map = map;
            this.isLookingRight = true;
            this.Name = "Player";
            this.BackColor = System.Drawing.Color.Transparent;
            this.Image = Properties.Resources.Pic1;
            this.Tag = "player";
            this.Location = new System.Drawing.Point(30, 143);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;


            this.counter = counter;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace ContraX
{
    public class Bullet : PictureBox
    {
        public bool up { get; set; }
        public bool down { get; set; }
        public bool forward { get; set; }
        private AxWMPLib.AxWindowsMediaPlayer BulletSound;
        public Bullet(Being character, bool isLookingRight, GameMap map)
        {
            BulletSound = new AxWMPLib.AxWindowsMediaPlayer();
            map.Controls.Add(BulletSound);
            BulletSound.Visible = false;
            BulletSound.URL = "";
            BulletSound.Ctlcontrols.play();
            up = false;
            down = false;
            forward = true;

            this.Top = character.Top + (character.Height / 2) - 11;
            if(isLookingRight)
            {
                this.Left = character.Left + character.Width;
                this.Tag = "right";
            }
            else
            {
                this.Left = character.Left;
                this.Tag = "left";
            }
            this.Size = new System.Drawing.Size(4, 4);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            map.Controls.Add(this);
            if (character.isThePlayer)
            {
                this.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.Yellow;
            }
            this.BringToFront();
        }
    }
}

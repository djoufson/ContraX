using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContraX
{
    public class Ground : Control
    {
        public List<Platform> PlatformeList = new List<Platform>();

        public Ground(GameMap map)
        {
            Platform ground1 = new Platform(0, 233, map);
            PlatformeList.Add(ground1);

            for (int i = 1; i < 16; i++)
            {
                PlatformeList.Add(new Platform(i*118, 233, map));
            }
        }
    }
    public class Platform : PictureBox
    {
        public Platform(int left, int top, GameMap map)
        {
            this.BackColor = System.Drawing.Color.Transparent;
            this.Image = global::ContraX.Properties.Resources.Platform;
            this.Location = new System.Drawing.Point(left, top);
            this.Size = new System.Drawing.Size(118, 50);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            map.Controls.Add(this);
            this.BringToFront();
        }
    }
}
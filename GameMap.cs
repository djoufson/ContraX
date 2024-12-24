using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContraX
{
    public class GameMap : PictureBox
    {
        public List<SingleMap> mapsList { get; set; }

        public GameMap()
        {
            mapsList = new List<SingleMap>();
            this.Size = new System.Drawing.Size(1350, 696);
            this.BackColor = System.Drawing.Color.Transparent;
            this.Width = 1350;
            this.Height = 696;
            this.mapsList.Add(new SingleMap());
            this.Controls.Add(mapsList[0]);

            for (int i = 1; i < 6; i++)
            {
                this.mapsList.Add(new SingleMap());
                this.mapsList[i].Left = i * 1350;
                this.Controls.Add(mapsList[i]);
            }
        }
    }

    public class SingleMap : PictureBox
    {
        public SingleMap()
        {
            this.Image = global::ContraX.Properties.Resources.Mapground;
            this.Left = 0;
            this.Top = 0;
            this.Size = new System.Drawing.Size(1350, 696);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.TabStop = false;
            this.Tag = "single map";
        }
    }
}

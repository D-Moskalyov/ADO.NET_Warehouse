using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using WarehouseDAL;

namespace Сoursework
{
    public partial class Fines : Form
    {
        ArrayList goodsList = new ArrayList();
        public Fines()
        {
            InitializeComponent();
        }

        private void Fines_Shown(object sender, EventArgs e)
        {
            foreach (Good gd in ((Form1)this.Owner).GoodsFine)
            {
                goodsList.Add(new GoodForClientInf(gd));
            }
            listBox1.DataSource = goodsList;
            listBox1.ValueMember = "GoodFromList";
            listBox1.DisplayMember = "Title";
            listBox1.SelectedItem = null;
        }

        private void Fines_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form1)this.Owner).GoodsFine.Clear();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedItem != null && e.Y <= (listBox1.Items.Count * listBox1.ItemHeight))
            {
                ((Form1)this.Owner).TmpGood = ((Good)listBox1.SelectedValue);
                GoodInfo goodInfo = new GoodInfo();
                foreach (KeyValuePair<Button, Good> btnGd in ((Form1)this.Owner).DictButGood)
                {
                    if (btnGd.Value.GoodID == ((Good)listBox1.SelectedValue).GoodID)
                    {
                        ((Form1)this.Owner).TmpButton = btnGd.Key;
                        break;
                    }
                }
                goodInfo.Show(this.Owner);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using WarehouseDAL;

namespace Сoursework
{
    public partial class GoodInfo : Form
    {
        Good good;
        public GoodInfo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Section tmpSec = new Section();
            ((Form1)this.Owner).TmpGood.Status = false;
            if (DataBase.GetSqlConnection().State == ConnectionState.Open)
            {
                string query = string.Format("update Goods set Stat = {0} where GoodID = {1}", 0, good.GoodID);
                SqlCommand command = new SqlCommand(query, DataBase.Sql);
                command.ExecuteNonQuery();

                foreach(Section section in SectionsManager.Sections)
                {
                    if(section.SectionID == good.SectionID)
                    {
                        //((Form1)this.Owner).DictKvp.Remove(section);
                        tmpSec = section;
                        break;
                    }
                }
                tmpSec.ExclusiveOrList(good.BoolenLocation);
                double loc = SectionsManager.ToDouble(tmpSec.BoolenTopography);
                query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc, tmpSec.SectionID);
                command = new SqlCommand(query, DataBase.Sql);
                command.ExecuteNonQuery();
            }
            ((Form1)this.Owner).DictButGood.Remove(((Form1)this.Owner).TmpButton);
            ((Form1)this.Owner).TmpButton.Parent.Controls.Remove(((Form1)this.Owner).TmpButton);

            this.Close();
        }

        private void GoodInfo_Shown(object sender, EventArgs e)
        {
            good = ((Form1)this.Owner).TmpGood;
            textBox1.Text = good.GoodID.ToString();
            textBox2.Text = good.ClientID.ToString();
            textBox3.Text = good.SectionID.ToString();
            textBox4.Text = good.Cost.ToString();
            textBox5.Text = good.Fine.ToString();
            textBox6.Text = good.Status.ToString();
            textBox7.Text = good.Height.ToString();
            textBox8.Text = good.Width.ToString();
            label1.Text = good.Title;
            dateTimePicker1.Value = good.DateStart;
            dateTimePicker2.Value = good.DateFinish;
            dateTimePicker1.MinDate = good.DateStart;
            dateTimePicker1.MaxDate = good.DateStart;
            dateTimePicker2.MinDate = good.DateFinish;
            if(good.Fine > 0)
                dateTimePicker2.MaxDate = good.DateFinish;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.TimeSpan diff = TimeSpan.FromDays(1);
            diff = dateTimePicker2.Value - dateTimePicker1.Value;
            good.DateFinish = dateTimePicker2.Value;
            good.Cost = good.Height / 25 * (good.Width / 25) * diff.Days * 10;

            if (DataBase.GetSqlConnection().State == ConnectionState.Open)
            {
                string query = string.Format("update Goods set DateFinish = '{0}', Cost = {1} where GoodID = {2}", good.DateFinish.ToShortDateString(), good.Cost, good.GoodID);
                SqlCommand command = new SqlCommand(query, DataBase.Sql);
                command.ExecuteNonQuery();
            }

            textBox4.Text = good.Cost.ToString();
            ((Form1)this.Owner).TmpButton.Text = String.Format("{0} {1} {2}", good.Title, good.DateStart.ToShortDateString(), good.DateFinish.ToShortDateString());
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}

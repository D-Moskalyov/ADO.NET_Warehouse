using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections;
using WarehouseDAL;

namespace Сoursework
{
    public partial class NewGood : Form
    {
        
        ArrayList clientsList = new ArrayList();

        public ArrayList ClientsList
        {
            get { return clientsList; }
            set { clientsList = value; }
        }
        Client newClient = null;

        decimal cost;

        public decimal Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        int sizeH = 0, sizeW = 0;

        public int SizeW
        {
            get { return sizeW; }
            set { sizeW = value; }
        }

        public int SizeH
        {
            get { return sizeH; }
            set { sizeH = value; }
        }
        System.TimeSpan diff = TimeSpan.FromDays(1);

        public Client NewClient
        {
            get { return newClient; }
            set { newClient = value; }
        }

        public NewGood()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            sizeH = (int)numericUpDown1.Value / 25;

            if((int)numericUpDown1.Value % 25 !=0)
            {
                sizeH++;
            }
            diff = dateTimePicker2.Value - dateTimePicker1.Value;
            cost = sizeH * sizeW * diff.Days * 10;

            label1.Text = cost.ToString();
        }

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            sizeH = (int)numericUpDown1.Value / 25;

            if ((int)numericUpDown1.Value % 25 != 0)
            {
                sizeH++;
            }
            diff = dateTimePicker2.Value - dateTimePicker1.Value;
            cost = sizeH * sizeW * diff.Days * 10;

            label1.Text = cost.ToString();
        }

        private void numericUpDown2_KeyPress(object sender, KeyPressEventArgs e)
        {
            sizeW = (int)numericUpDown2.Value / 25;

            if ((int)numericUpDown2.Value % 25 != 0)
            {
                sizeW++;
            }
            diff = dateTimePicker2.Value - dateTimePicker1.Value;
            cost = sizeH * sizeW * diff.Days * 10;

            label1.Text = cost.ToString();
        }

       
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker2.Value < dateTimePicker1.Value)
            {
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(1);
            }
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
            diff = dateTimePicker2.Value - dateTimePicker1.Value;
            cost = sizeH * sizeW * diff.Days * 10;

            label1.Text = cost.ToString();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            diff = dateTimePicker2.Value - dateTimePicker1.Value;
            cost = sizeH * sizeW * diff.Days * 10;

            label1.Text = cost.ToString();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Visible = true;
            dateTimePicker2.Visible = true;
            textBox1.Visible = true;
            numericUpDown1.Visible = true;
            numericUpDown2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewClient newClientForm = new NewClient();
            newClientForm.Show(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            sizeW = (int)numericUpDown2.Value / 25;

            if ((int)numericUpDown2.Value % 25 != 0)
            {
                sizeW++;
            }
            diff = dateTimePicker2.Value - dateTimePicker1.Value;
            cost = sizeH * sizeW * diff.Days * 10;

            label1.Text = cost.ToString();

        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            if (int.Parse(label1.Text) > 0 && textBox1.Text != "")
            {
                button2.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            //((Form1)this.Owner).HeighsFromNC = sizeH;
            //((Form1)this.Owner).WidthFromNC = sizeW;
            ((Form1)this.Owner).GoodFromNG = this;

            ((Form1)this.Owner).Activate();
            this.Hide();

            ((Form1)this.Owner).BeforeHidenNewGood();

            //if (newClient == null)
            //{

            //}
            //else
            //{

            //}
        }

        private void NewGood_Shown(object sender, EventArgs e)
        {
            DateTime dt = ((Form1)Owner).DateTimePicker1.Value;
            foreach (Client cl in ClientsManager.Clients)
            {
                clientsList.Add(new ClientForCB(cl));
            }
            comboBox1.DataSource = clientsList;
            comboBox1.ValueMember = "ClientFromList";
            comboBox1.DisplayMember = "Name";
            comboBox1.SelectedItem = null;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;
            textBox1.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            dateTimePicker1.MinDate = dt;
            dateTimePicker1.Value = dt;
            dateTimePicker2.MinDate = dt.AddDays(1);
            dateTimePicker2.Value = dt.AddDays(1);
            button2.Visible = false;
        }
    }
}

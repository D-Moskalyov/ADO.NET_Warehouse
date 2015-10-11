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
    public partial class ClientInfo : Form
    {
        int clientID = 0;
        bool changeClient = false;
        ArrayList clientsList = new ArrayList();
        ArrayList goodsList = new ArrayList();

        public ClientInfo()
        {
            InitializeComponent();
        }

        private void ClientInfo_Shown(object sender, EventArgs e)
        {
            foreach (Client cl in ClientsManager.Clients)
            {
                clientsList.Add(new ClientForCB(cl));
            }
            comboBox1.DataSource = clientsList;
            comboBox1.ValueMember = "ClientFromList";
            comboBox1.DisplayMember = "Name";
            //comboBox1.SelectedItem = null;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("comboBox1_SelectedValueChanged");
            if (comboBox1.DisplayMember == "Name" && clientID != ((Client)comboBox1.SelectedValue).ClientID)
            {
                int totalGoods = 0;
                int totalActiveGoods = 0;
                int totalFinestGoods = 0;
                int totalFine = 0;
                int totalCost = 0;
                clientID = ((Client)comboBox1.SelectedValue).ClientID;
                //changeClient = true;
                comboBox2.DataSource = null;
                //comboBox2.SelectedItem = null;
                
                foreach (Good good in Goodsmanager.Goods)
                {
                    if (good.ClientID == ((Client)comboBox1.SelectedValue).ClientID)
                    {
                        totalGoods++;
                        totalCost += good.Cost;
                        if (good.Status)
                        {
                            totalActiveGoods++;
                            if (good.Fine > 0)
                                totalFinestGoods++;
                        }
                        else
                        {
                            if (good.Fine > 0)
                                totalFine += good.Fine;
                        }
                    }
                }
                label7.Text = totalGoods.ToString();
                label8.Text = totalActiveGoods.ToString();
                label9.Text = totalFinestGoods.ToString();
                label10.Text = totalCost.ToString();
                label11.Text = totalFine.ToString();

                Client client = (Client)comboBox1.SelectedValue;
                goodsList.Clear();

                foreach (Good gd in Goodsmanager.Goods)
                {
                    if (gd.ClientID == client.ClientID)
                        goodsList.Add(new GoodForClientInf(gd));
                }
                comboBox2.DataSource = goodsList;
                comboBox2.ValueMember = "GoodFromList";
                comboBox2.DisplayMember = "Title";
            }
            //else
            //    changeClient = false;
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("comboBox2_MouseClick");
            //if (changeClient)
            //{
                //Client client = (Client)comboBox1.SelectedValue;
                //goodsList.Clear();    

                //foreach (Good gd in Goodsmanager.Goods)
                //{
                //    if (gd.ClientID == client.ClientID)
                //        goodsList.Add(new GoodForClientInf(gd));
                //}
                //comboBox2.DataSource = goodsList;
                //comboBox2.ValueMember = "GoodFromList";
                //comboBox2.DisplayMember = "Title";
                //comboBox2.SelectedItem = null;
            //}
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            
            if (comboBox2.DisplayMember == "Title" && comboBox2.SelectedValue != null)
            {
                //MessageBox.Show("comboBox2_SelectedValueChanged");
                //MessageBox.Show("xsx");
                textBox1.Text = ((Good)comboBox2.SelectedValue).GoodID.ToString();
                textBox2.Text = ((Good)comboBox2.SelectedValue).ClientID.ToString();
                textBox3.Text = ((Good)comboBox2.SelectedValue).DateStart.ToShortDateString();
                textBox4.Text = ((Good)comboBox2.SelectedValue).DateFinish.ToShortDateString();
                textBox5.Text = ((Good)comboBox2.SelectedValue).Cost.ToString();
                textBox6.Text = ((Good)comboBox2.SelectedValue).Fine.ToString();
                textBox7.Text = ((Good)comboBox2.SelectedValue).Status.ToString();
                textBox8.Text = ((Good)comboBox2.SelectedValue).Height.ToString();
                textBox9.Text = ((Good)comboBox2.SelectedValue).Width.ToString();
            }
        }
    }
}

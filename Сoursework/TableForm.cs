using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseDAL;

namespace Сoursework
{
    public partial class TableForm : Form
    {
        int rowToYellowColor, rowToGreyColor;
        TableLayoutPanel table = new TableLayoutPanel();
        public TableForm()
        {
            InitializeComponent();
        }

        private void TableForm_Shown(object sender, EventArgs e)
        {
            if (((Form1)this.Owner).GoodToForm != null && ((Form1)this.Owner).GoodToForm.Count != 0)
            {
                Label period = new Label();
                period.AutoSize = true;
                period.Text = string.Format("Period : {0} - {1}", ((Form1)this.Owner).DateTimePicker2.Value.ToShortDateString(), ((Form1)this.Owner).DateTimePicker3.Value.ToShortDateString());

                this.Controls.Add(period);

                int rowCount = 0;
                List<int> numsClients = new List<int>();
                List<Client> clients = new List<Client>();
                List<Label> labels  = new List<Label>();
                for (int i = 0; i < 5; i++)
                {
                    labels.Add(new Label());
                    labels[i].BackColor = Color.Gray;
                    labels[i].AutoSize = true;
                }
                labels[0].Text = "Name/tile";
                labels[1].Text = "Cost";
                labels[2].Text = "Fine";
                labels[3].Text = "Fined";
                labels[4].Text = "Active now";
                foreach (Good good in ((Form1)this.Owner).GoodToForm)
                {
                    if (!numsClients.Contains(good.ClientID))
                        numsClients.Add(good.ClientID);
                }
                foreach (Client client in ClientsManager.Clients)
                {
                    if (numsClients.Contains(client.ClientID))
                    {
                        if (!clients.Contains(client))
                            clients.Add(client);
                    }
                }
                
                
                //table.CellPaint += table_CellPaint;
                table.ColumnCount = 5;
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17F));
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));

                //table.RowCount = clients.Count + ((Form1)this.Owner).GoodToForm.Count + 2;
                
                //table.Controls[0].colo
                table.RowCount++;
                table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
                for (int i = 0; i < 5; i++)
                    table.Controls.Add(labels[i]);

                foreach (Client cl in clients)
                {
                    rowCount++;
                    rowToYellowColor = rowCount;

                    int cost = 0, fine = 0, fined = 0, active = 0;
                    foreach (Good gd in ((Form1)this.Owner).GoodToForm)
                    {
                        if (gd.ClientID == cl.ClientID)
                        {
                            cost += gd.Cost;
                            if (gd.Fine > 0)
                            {
                                if (!gd.Status)
                                {
                                    fined += gd.Fine;
                                    //active = "no";
                                }
                                else
                                {
                                    fine += gd.Fine;
                                }
                            }
                            if (gd.Status) active++;
                            //if (gd.Status) active++;
                        }
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        labels.Add(new Label());
                        labels[rowCount * 5 + i].BackColor = Color.Yellow;
                        labels[rowCount * 5 + i].AutoSize = true;
                    }
                    labels[rowCount * 5].Text = string.Format("{0} {1}", cl.FirstName, cl.LastName);
                    labels[rowCount * 5 + 1].Text = cost.ToString();
                    labels[rowCount * 5 + 2].Text = fine.ToString();
                    labels[rowCount * 5 + 3].Text = fined.ToString();
                    labels[rowCount * 5 + 4].Text = active.ToString();

                    table.RowCount++;
                    table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                    for (int i = 0; i < 5; i++)
                        table.Controls.Add(labels[rowCount * 5 + i]);

                    foreach (Good gd in ((Form1)this.Owner).GoodToForm)
                    {
                        if (gd.ClientID == cl.ClientID)
                        {
                            string activeG = string.Empty;
                            rowCount++;
                            
                            for (int i = 0; i < 5; i++)
                                labels.Add(new Label());

                            labels[rowCount * 5].Text = gd.Title;
                            labels[rowCount * 5 + 1].Text = gd.Cost.ToString();
                            if (gd.Status)
                            {
                                labels[rowCount * 5 + 2].Text = gd.Fine.ToString();
                                labels[rowCount * 5 + 3].Text = "0";
                            }
                            else
                            {
                                labels[rowCount * 5 + 2].Text = "0";
                                labels[rowCount * 5 + 3].Text = gd.Fine.ToString();
                            }
                            if (gd.Status)  activeG = "yes";
                            else            activeG = "no";
                            labels[rowCount * 5 + 4].Text = activeG;
                            
                            table.RowCount++;
                            table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                            for (int i = 0; i < 5; i++)
                                table.Controls.Add(labels[rowCount * 5 + i]);
                        }
                    }

                    //table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));

                }
                int costT = 0, fineT = 0, finedT = 0, activeT = 0;
                foreach (Good gd in ((Form1)this.Owner).GoodToForm)
                {
                    costT += gd.Cost;
                    if (gd.Status)
                    {
                        fineT += gd.Fine;
                        activeT++;
                    }
                    else finedT += gd.Fine;
                }

                rowCount++;
                rowToGreyColor = rowCount;

                for (int i = 0; i < 5; i++)
                {
                    labels.Add(new Label());
                    labels[rowCount * 5 + i].BackColor = Color.Gray;
                }
                
                labels[rowCount * 5].Text = "Total";
                labels[rowCount * 5 + 1].Text = costT.ToString();
                labels[rowCount * 5 + 2].Text = fineT.ToString();
                labels[rowCount * 5 + 3].Text = finedT.ToString();
                labels[rowCount * 5 + 4].Text = activeT.ToString();

                table.RowCount++;
                table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
                for (int i = 0; i < 5; i++)
                    table.Controls.Add(labels[rowCount * 5 + i]);

                //rowToGreyColor = 0;
                table.Size = new System.Drawing.Size(400, rowCount * 20 + 40);
                table.Location = new System.Drawing.Point(10, 30);
                table.Visible = true;
                table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset;
                this.Size = new Size(table.Size.Width + 40, table.Size.Height + 90);
                this.Controls.Add(table);
            }
        }

        void table_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row == 0 || e.Row == rowToGreyColor)
            {
                e.Graphics.FillRectangle(Brushes.Gray, e.CellBounds);
            }
            if (e.Row == rowToYellowColor)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.CellBounds);
            }
        }

        private void TableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((Form1)this.Owner).GoodToForm.Clear();
        }
    }
}

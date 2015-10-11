using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    //public class DoubleClickButton : Button
    //{
    //    public DoubleClickButton()
    //    {
    //        SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
    //    }
    //}

    public partial class Form1 : Form
    {
        
        int columnCount = 0;
        bool emptyTopography = true;
        List<Good> goodsFine = new List<Good>();
        List<Good> goodToForm = new List<Good>();
        bool changeStat = false;

        public System.Windows.Forms.DateTimePicker DateTimePicker2
        {
            get { return dateTimePicker2; }
            set { dateTimePicker2 = value; }
        }

        public System.Windows.Forms.DateTimePicker DateTimePicker3
        {
            get { return dateTimePicker3; }
            set { dateTimePicker3 = value; }
        }

        public List<Good> GoodToForm
        {
            get { return goodToForm; }
            set { goodToForm = value; }
        }

        public List<Good> GoodsFine
        {
            get { return goodsFine; }
            set { goodsFine = value; }
        }
        //List<KeyValuePair<Section, Panel>> ListKvp = new List<KeyValuePair<Section, Panel>>();
        Dictionary<Section, Panel> dictKvp = new Dictionary<Section, Panel>();

        public Dictionary<Section, Panel> DictKvp
        {
            get { return dictKvp; }
            set { dictKvp = value; }
        }
        Dictionary<Button, Good> dictButGood = new Dictionary<Button, Good>();

        public Dictionary<Button, Good> DictButGood
        {
            get { return dictButGood; }
            set { dictButGood = value; }
        }
        Dictionary<Panel, Graphics> dictGraph = new Dictionary<Panel, Graphics>();
        Dictionary<Section, bool[,]> dictTemp = new Dictionary<Section, bool[,]>();

        //int heighsFromNC = 0, widthFromNC = 0;
        NewGood goodFromNG;
        Good tmpGood;
        Button tmpButton;

        public System.Windows.Forms.DateTimePicker DateTimePicker1
        {
            get { return dateTimePicker1; }
            set { dateTimePicker1 = value; }
        }

        public Button TmpButton
        {
            get { return tmpButton; }
            set { tmpButton = value; }
        }

        public Good TmpGood
        {
            get { return tmpGood; }
            set { tmpGood = value; }
        }
        Button shiftBtn;
        Label controlLabel;
        Section sectionTmp;

        public NewGood GoodFromNG
        {
            get { return goodFromNG; }
            set { goodFromNG = value; }
        }

        public Form1()
        {
            if (DataBase.GetSqlConnection().State == ConnectionState.Open)
            {
                InitializeComponent();

                dateTimePicker1.MinDate = DateTime.Now;
                dateTimePicker1.MaxDate = DateTime.Now;
                //graphics = panel1.CreateGraphics();

                string query = "select * from Sections";
                SqlCommand command = new SqlCommand(query, DataBase.Sql);
                SqlDataReader reader = command.ExecuteReader();
                SectionsManager.LoadSections(reader);
                reader.Close();

                query = "select * from Clients";
                command = new SqlCommand(query, DataBase.Sql);
                reader = command.ExecuteReader();
                ClientsManager.LoadClients(reader);
                reader.Close();

                query = "select * from Goods";
                command = new SqlCommand(query, DataBase.Sql);
                reader = command.ExecuteReader();
                Goodsmanager.LoadGoods(reader);
                reader.Close();

                columnCount = this.Size.Width / 300;
                if (columnCount > 0)
                {
                    Panel panel;
                    int sectCount = 0;
                    for (int i = 0; SectionsManager.Sections.Count > sectCount; i++)
                    {
                        for (int j = 0; j < this.panel1.ClientSize.Width / 300 && SectionsManager.Sections.Count > sectCount; j++)
                        {
                            panel = new System.Windows.Forms.Panel();
                            panel.Location = new System.Drawing.Point(j * 300, i * 100);
                            panel.Size = new System.Drawing.Size(300, 100);
                            panel.BorderStyle = BorderStyle.FixedSingle;
                            panel.MouseEnter += new System.EventHandler(this.panel_MouseEnter);
                            panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);
                            //ListKvp.Add(new KeyValuePair<Section,Panel>(SectionsManager.Sections[sectCount], panel));
                            dictKvp.Add(SectionsManager.Sections[sectCount], panel);
                            //MessageBox.Show(dictKvp[SectionsManager.Sections[sectCount]].Name);

                            dictGraph.Add(panel, panel.CreateGraphics());

                            this.panel1.Controls.Add(panel);

                            for (int t = 0; t < 14; t++)
                            {
                                if (SectionsManager.Sections[sectCount].BoolenTopography[t])
                                {
                                    emptyTopography = false;
                                    break;
                                }
                            }

                            sectCount++;
                        }
                    }
                    if (emptyTopography)
                    {
                        for (int i = 0; i < SectionsManager.Sections.Count; i++)
                        {
                            foreach (Good good in Goodsmanager.Goods)
                            {
                                if (good.SectionID == SectionsManager.Sections[i].SectionID && good.Status)
                                    SectionsManager.Sections[i].Or(good.BoolenLocation);
                            }
                        }
                    }

                    foreach (Good good in Goodsmanager.Goods)
                    {
                        foreach (Section section in SectionsManager.Sections)
                        {
                            if (good.SectionID == section.SectionID && good.Status)
                            {
                                Button button = new Button();

                                button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                                int iter = -1;
                                for (int i = 0; i < 48; i++)
                                {
                                    if (good.BoolenLocation[i])
                                    {
                                        iter = i;
                                        break;
                                    }
                                }

                                button.Location = new System.Drawing.Point(iter % 12 * 25, iter / 12 * 25);
                                button.Size = new System.Drawing.Size(good.Width, good.Height);
                                button.Text = string.Format("{0} {1} {2}", good.Title, good.DateStart.ToShortDateString(), good.DateFinish.ToShortDateString());
                                //button.DoubleClick += button_DoubleClick;
                                button.MouseClick += button_MouseClick;

                                dictButGood.Add(button, good);
                                dictKvp[section].Controls.Add(button);
                            }
                        }
                    }

                    Dictionary<Button, Good>.KeyCollection kC = dictButGood.Keys;
                    for (int i = 0; i < kC.Count; i++)
                    {
                        if (dictButGood[kC.ElementAt(i)].DateFinish.Ticks < dateTimePicker1.Value.AddDays(3).Ticks)
                            kC.ElementAt(i).BackColor = Color.Yellow;
                        else
                        {
                            if (dateTimePicker1.Value.Ticks > dictButGood[kC.ElementAt(i)].DateFinish.Ticks)
                            {
                                //MessageBox.Show(i.ToString());
                                kC.ElementAt(i).BackColor = Color.Red;
                                Good tmp = dictButGood[kC.ElementAt(i)];
                                long elapsedSpan = dateTimePicker1.Value.Ticks - tmp.DateFinish.Ticks;
                                TimeSpan tS = new TimeSpan(elapsedSpan);
                                tmp.Fine = tS.Days * (tmp.Height / 25) * (tmp.Width / 25) * 10;
                                query = string.Format("update Goods set Fine = {0} where GoodID = {1}", tmp.Fine, tmp.GoodID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                Good tmp = dictButGood[kC.ElementAt(i)];
                                tmp.Fine = 0;
                                query = string.Format("update Goods set Fine = {0} where GoodID = {1}", tmp.Fine, tmp.GoodID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    //MessageBox.Show("");

                }
                else
                {
                    MessageBox.Show("Increase size and press redraw, please");
                }

                ColorButton();
            }
            else
            {
                MessageBox.Show("!Connect Failed!");
                //this.Close();
                Application.Exit();
            }

        }

        void button_MouseClick(object sender, MouseEventArgs e)
        {
            if (!changeStat)
            {
                tmpGood = dictButGood[(Button)sender];
                tmpButton = (Button)sender;
                GoodInfo goodInfo = new GoodInfo();
                goodInfo.Show(this);
            }
        }

        void button_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show("fdf");
            //tmpGood = dictButGood[(Button)sender];
            //GoodInfo goodInfo = new GoodInfo();
            //goodInfo.Show(this);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SectionsManager.UpdateSections();
            //DialogResult result = MessageBox.Show("Сохранить изменения?", "Завершение работы", MessageBoxButtons.YesNo);
            //if (result == DialogResult.Yes)
            //{
            //    DataBase.SaveChange();
            //}
        }

        private void panel_MouseEnter(object sender, EventArgs e)
        {
            //MessageBox.Show(((Panel)sender).Parent.Name);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(((Panel)sender).Parent.Name);
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            //if (sender is Button)
            //{
            //    Panel p = (Panel)((Button)sender).Parent;
            //}
            //MessageBox.Show(e.X.ToString() + "  " + e.Y.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int n, m;
            if (int.TryParse(textBox1.Text, out n) && int.TryParse(textBox2.Text, out m))
            {
                if (n > 0 && n <= 100 && m > 0 && m <= 300)
                {
                    foreach (Graphics gr in dictGraph.Values)
                    {
                        gr.Clear(panel1.BackColor);
                    }
                    //bool[,] boolMas = new bool[4, 12];
                    if (n % 25 == 0)
                        n = n / 25;
                    else
                        n = n / 25 + 1;

                    if (m % 25 == 0)
                        m = m / 25;
                    else
                        m = m / 25 + 1;

                    Dictionary<Section, bool[,]> dictTemp = new Dictionary<Section, bool[,]>();
                    foreach (Section section in SectionsManager.Sections)
                    {
                        dictTemp.Add(section, new bool[4,12]);
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 12; j++)
                            {
                                (dictTemp[section])[i, j] = section.BoolenTopography[i * 12 + j];
                            }
                        }
                    }

                    foreach (Section section in SectionsManager.Sections)
                    {
                        Button button;
                        for (int k = 0; k < 4; k++)
                        {
                            for (int l = 0; l < 12; l++)
                            {
                                if (OffSet(k, l, m - 1, n - 1, dictTemp[section]))//!!!
                                {
                                    dictGraph[dictKvp[section]].DrawRectangle(new Pen(Color.Red), new Rectangle(l * 25, k * 25, m * 25, n * 25));

                                    for (int x = 0; x < n; x++)
                                    {
                                        for (int z = 0; z < m; z++)
                                        {
                                            dictTemp[section][k + x, l + z] = true;
                                        }
                                    }
                                }
                                else
                                {
                                    //int p = n, r = m;
                                    if (OffSet(k, l, n - 1, m - 1, dictTemp[section]))
                                    {
                                        dictGraph[dictKvp[section]].DrawRectangle(new Pen(Color.Red), new Rectangle(l * 25, k * 25, n * 25, m * 25));

                                        for (int x = 0; x < m; x++)
                                        {
                                            for (int z = 0; z < n; z++)
                                            {
                                                dictTemp[section][k + x, l + z] = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("Incorrect date: h:1-300; w:1-100");

            }
            else
                MessageBox.Show("Incorrect date");
        }

        private bool OffSet(int startY, int startX, int offset, int down, bool[,] mas)
        {
            if (!mas[startY, startX])
            {
                if (offset > 0)
                {
                    
                    if (startX + 1 <= 11)
                    {
                        if (!OffSet(startY, startX + 1, offset - 1, down, mas))
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (down > 0)
                {
                    if (startY + 1 <= 3)
                    {
                        if (!OffSet(startY + 1, startX, offset, down - 1, mas))
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Graphics gr in dictGraph.Values)
            {
                gr.Clear(panel1.BackColor);
            }
            NewGood newGood = new NewGood();
            newGood.Show(this);
        }

        public void BeforeHidenNewGood()
        {
            
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].Name != "panel1")
                    this.Controls[i].Hide();
            }

            dictTemp.Clear();
            
            foreach (Section section in SectionsManager.Sections)
            {
                dictTemp.Add(section, new bool[4, 12]);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        (dictTemp[section])[i, j] = section.BoolenTopography[i * 12 + j];
                    }
                }
            }

            bool yes = false;

            foreach (Section section in SectionsManager.Sections)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        if (OffSet(k, l, GoodFromNG.SizeW - 1, GoodFromNG.SizeH - 1, dictTemp[section]))
                        {
                            yes = true;
                            dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25 + 13, k * 25);
                            dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25, k * 25 + 13);
                        }
                        else
                        {
                            if (OffSet(k, l, GoodFromNG.SizeH - 1, GoodFromNG.SizeW - 1, dictTemp[section]))
                            {
                                yes = true;
                                dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25 + 13, k * 25);
                                dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25, k * 25 + 13);
                            }
                        }
                    }
                }
            }

            if (yes)
            {
                foreach (Section section in SectionsManager.Sections)
                {
                    dictKvp[section].MouseClick += Choice_MouseClick;
                }
            }

            controlLabel = new Label();

            controlLabel.Location = new System.Drawing.Point(panel1.Location.X, panel1.Location.Y + panel1.Size.Height + 2);
            controlLabel.AutoSize = true;
            controlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            controlLabel.ForeColor = System.Drawing.Color.Red;
            controlLabel.Text = "Choice empty place. Click this if you want reject";
            controlLabel.Click += new System.EventHandler(this.labelTmp_Click);
            this.Controls.Add(controlLabel);
            this.Controls[this.Controls.Count - 1].Show();
        }

        void Choice_MouseClick(object sender, MouseEventArgs e)
        {
            dictTemp.Clear();
            dictTemp = fillDictTemp();

            foreach (Section section in SectionsManager.Sections)
            {
                if(dictKvp[section] == (Panel)sender)
                {
                    int x = (int)(e.X / 25);

                    if (x  == 12)
                    {
                        x--;
                    }
                    int y = (int)(e.Y / 25);

                    if (y == 4)
                    {
                        y--;
                    }
                    
                    if (OffSet(y, x, GoodFromNG.SizeW - 1, GoodFromNG.SizeH - 1, dictTemp[section]))
                    {
                        if (DataBase.GetSqlConnection().State == ConnectionState.Open)
                        {
                            SqlCommand command;
                            string query;
                            decimal clID;
                            if (goodFromNG.NewClient == null)
                            {
                                clID = ((Client)goodFromNG.ComboBox1.SelectedValue).ClientID;
                            }
                            else
                            {
                                query = string.Format("insert into Clients(FirstName, LastName, email) values('{0}', '{1}', '{2}')", GoodFromNG.NewClient.FirstName, GoodFromNG.NewClient.LastName, GoodFromNG.NewClient.Email);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();

                                query = "select IDENT_CURRENT ('Clients')";
                                command = new SqlCommand(query, DataBase.Sql);
                                SqlDataReader reader2 = command.ExecuteReader();
                                reader2.Read();
                                clID = (decimal)reader2[0];
                                reader2.Close();

                                query = string.Format("select * from Clients where ClientID = {0}", clID);
                                command = new SqlCommand(query, DataBase.Sql);
                                SqlDataReader reader = command.ExecuteReader();
                                ClientsManager.LoadClients(reader);
                                reader.Close();

                            }

                            int secID = section.SectionID;
                            DateTime dt1 = GoodFromNG.DateTimePicker1.Value;
                            DateTime dt2 = GoodFromNG.DateTimePicker2.Value;
                            string date1 = GoodFromNG.DateTimePicker1.Value.ToShortDateString();
                            string date2 = GoodFromNG.DateTimePicker2.Value.ToShortDateString();
                            string title = GoodFromNG.TextBox1.Text;
                            int cost = (int)GoodFromNG.Cost;
                            int h = GoodFromNG.SizeH;
                            int w = GoodFromNG.SizeW;

                            List<bool> boolTmpForGood = new List<bool>();
                            for (int i = 0; i < 48; i++)
                                boolTmpForGood.Add(false);

                            for (int i = 0; i < y; i++)
                            {
                                for (int j = 0; j < 12; j++)
                                    boolTmpForGood[i * 12 + j] = false;
                            }

                            for (int i = 0; i < h; i++)
                            {
                                for (int l = 0; l < x; l++)
                                    boolTmpForGood[(y + i) * 12 + l] = false;
                                for (int l = x; l < x + w; l++)
                                    boolTmpForGood[(y + i) * 12 + l] = true;
                                for (int l = x + w; l < 12; l++)
                                    boolTmpForGood[(y + i) * 12 + l] = false;
                            }

                            for (int i = y + h; i < 4; i++)
                            {
                                for (int j = 0; j < 12; j++)
                                    boolTmpForGood[i * 12 + j] = false;
                            }

                            double loc = Goodsmanager.ToDouble(boolTmpForGood);

                            query = string.Format("insert into Goods(ClientID, SectionID, DateStart, DateFinish, Cost, Fine, Stat, Location, Title, Height, Width) values ({0}, {1}, '{2}', '{3}', {4}, 0, 1, {5}, '{6}', {7}, {8})", clID, secID, date1, date2, cost, loc, title, h * 25, w * 25);
                            command = new SqlCommand(query, DataBase.Sql);
                            command.ExecuteNonQuery();

                            query = "select IDENT_CURRENT ('Goods')";
                            command = new SqlCommand(query, DataBase.Sql);
                            SqlDataReader reader3 = command.ExecuteReader();
                            reader3.Read();
                            decimal numOfGoods = (decimal)reader3[0];
                            reader3.Close();

                            query = string.Format("select * from Goods where GoodID = {0}", numOfGoods);
                            command = new SqlCommand(query, DataBase.Sql);
                            reader3 = command.ExecuteReader();
                            Goodsmanager.LoadGoods(reader3);
                            reader3.Close();

                            Button button = new Button();

                            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                            int iter = -1;
                            for (int i = 0; i < 48; i++)
                            {
                                if (boolTmpForGood[i])
                                {
                                    iter = i;
                                    break;
                                }
                            }

                            button.Location = new System.Drawing.Point(x * 25, y * 25);
                            button.Size = new System.Drawing.Size(w * 25, h * 25);
                            //button.Text = "button1";
                            button.Text = string.Format("{0} {1} {2}", GoodFromNG.TextBox1.Text, GoodFromNG.DateTimePicker1.Value.ToShortDateString(), GoodFromNG.DateTimePicker2.Value.ToShortDateString());
                            button.MouseClick += button_MouseClick;

                            dictButGood.Add(button, Goodsmanager.Goods[Goodsmanager.Goods.Count - 1]);
                            dictKvp[section].Controls.Add(button);

                            List<bool> listBools = SectionsManager.OrBool(boolTmpForGood, section.BoolenTopography);

                            loc = SectionsManager.ToDouble(listBools);

                            query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc, section.SectionID);
                            command = new SqlCommand(query, DataBase.Sql);
                            command.ExecuteNonQuery();

                            section.Or(boolTmpForGood);

                            labelTmp_Click(this, EventArgs.Empty);
                            break;
                        }
                    }
                    else
                    {
                        if (OffSet(y, x, GoodFromNG.SizeH - 1, GoodFromNG.SizeW - 1, dictTemp[section]))
                        {
                            if (DataBase.GetSqlConnection().State == ConnectionState.Open)
                            {
                                SqlCommand command;
                                string query;
                                decimal clID;
                                if (goodFromNG.NewClient == null)
                                {
                                    clID = ((Client)goodFromNG.ComboBox1.SelectedValue).ClientID;
                                }
                                else
                                {
                                    query = string.Format("insert into Clients(FirstName, LastName, email) values('{0}', '{1}', '{2}')", GoodFromNG.NewClient.FirstName, GoodFromNG.NewClient.LastName, GoodFromNG.NewClient.Email);
                                    command = new SqlCommand(query, DataBase.Sql);
                                    command.ExecuteNonQuery();

                                    query = "select IDENT_CURRENT ('Clients')";
                                    command = new SqlCommand(query, DataBase.Sql);
                                    SqlDataReader reader2 = command.ExecuteReader();
                                    reader2.Read();
                                    clID = (decimal)reader2[0];
                                    reader2.Close();

                                    query = string.Format("select * from Clients where ClientID = {0}", clID);
                                    command = new SqlCommand(query, DataBase.Sql);
                                    SqlDataReader reader = command.ExecuteReader();
                                    ClientsManager.LoadClients(reader);
                                    reader.Close();

                                }

                                int secID = section.SectionID;
                                DateTime dt1 = GoodFromNG.DateTimePicker1.Value;
                                DateTime dt2 = GoodFromNG.DateTimePicker2.Value;
                                string date1 = GoodFromNG.DateTimePicker1.Value.ToShortDateString();
                                string date2 = GoodFromNG.DateTimePicker2.Value.ToShortDateString();
                                string title = GoodFromNG.TextBox1.Text;
                                int cost = (int)GoodFromNG.Cost;
                                int h = GoodFromNG.SizeW;
                                int w = GoodFromNG.SizeH;

                                List<bool> boolTmpForGood = new List<bool>();
                                for (int i = 0; i < 48; i++)
                                    boolTmpForGood.Add(false);

                                for (int i = 0; i < y; i++)
                                {
                                    for (int j = 0; j < 12; j++)
                                        boolTmpForGood[i * 12 + j] = false;
                                }

                                for (int i = 0; i < h; i++)
                                {
                                    for (int l = 0; l < x; l++)
                                        boolTmpForGood[(y + i) * 12 + l] = false;
                                    for (int l = x; l < x + w; l++)
                                        boolTmpForGood[(y + i) * 12 + l] = true;
                                    for (int l = x + w; l < 12; l++)
                                        boolTmpForGood[(y + i) * 12 + l] = false;
                                }

                                for (int i = y + h; i < 4; i++)
                                {
                                    for (int j = 0; j < 12; j++)
                                        boolTmpForGood[i * 12 + j] = false;
                                }

                                double loc = Goodsmanager.ToDouble(boolTmpForGood);

                                query = string.Format("insert into Goods(ClientID, SectionID, DateStart, DateFinish, Cost, Fine, Stat, Location, Title, Height, Width) values ({0}, {1}, '{2}', '{3}', {4}, 0, 1, {5}, '{6}', {7}, {8})", clID, secID, date1, date2, cost, loc, title, h * 25, w * 25);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();

                                query = "select IDENT_CURRENT ('Goods')";
                                command = new SqlCommand(query, DataBase.Sql);
                                SqlDataReader reader3 = command.ExecuteReader();
                                reader3.Read();
                                decimal numOfGoods = (decimal)reader3[0];
                                reader3.Close();

                                query = string.Format("select * from Goods where GoodID = {0}", numOfGoods);
                                command = new SqlCommand(query, DataBase.Sql);
                                reader3 = command.ExecuteReader();
                                Goodsmanager.LoadGoods(reader3);
                                reader3.Close();

                                Button button = new Button();

                                button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                                int iter = -1;
                                for (int i = 0; i < 48; i++)
                                {
                                    if (boolTmpForGood[i])
                                    {
                                        iter = i;
                                        break;
                                    }
                                }

                                button.Location = new System.Drawing.Point(x * 25, y * 25);
                                button.Size = new System.Drawing.Size(w * 25, h * 25);
                                //button.Text = "button1";
                                button.Text = string.Format("{0} {1} {2}", GoodFromNG.TextBox1.Text, GoodFromNG.DateTimePicker1.Value.ToShortDateString(), GoodFromNG.DateTimePicker2.Value.ToShortDateString());
                                button.MouseClick += button_MouseClick;

                                dictButGood.Add(button, Goodsmanager.Goods[Goodsmanager.Goods.Count - 1]);
                                dictKvp[section].Controls.Add(button);

                                List<bool> listBools = SectionsManager.OrBool(boolTmpForGood, section.BoolenTopography);

                                loc = SectionsManager.ToDouble(listBools);

                                query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc, section.SectionID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();

                                section.Or(boolTmpForGood);

                                labelTmp_Click(this, EventArgs.Empty);
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect place");
                        }
                    }
                    break;
                }
            }

        }

        private void labelTmp_Click(object sender, EventArgs e)
        {
            changeStat = false;
            controlLabel.Parent.Controls.Remove(controlLabel);
            foreach (Section section in SectionsManager.Sections)
            {
                dictKvp[section].MouseClick -= Choice_MouseClick;
                dictKvp[section].MouseClick -= Choice_MouseClick1;
            }
            Dictionary<Button, Good>.KeyCollection kC = dictButGood.Keys;
            for (int i = 0; i < kC.Count; i++)
            {
                kC.ElementAt(i).MouseClick -= Choice_Good;
            }
            if (shiftBtn != null)
                shiftBtn.Show();

            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].Show();
            }
            dictTemp.Clear();
            if (GoodFromNG != null)
                GoodFromNG.Close();

            if (sectionTmp != null)
                sectionTmp = null;

            for (int i = 0; i < panel1.Controls.Count; i++)
            {
                if (panel1.Controls[i] is Panel)
                {
                    dictGraph[(Panel)panel1.Controls[i]].Clear(this.BackColor);
                }
            }
            ColorButton();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            changeStat = true;
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].Name != "panel1")
                    this.Controls[i].Hide();
            }
            controlLabel = new Label();

            controlLabel.Location = new System.Drawing.Point(panel1.Location.X, panel1.Location.Y + panel1.Size.Height + 2);
            controlLabel.AutoSize = true;
            controlLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            controlLabel.ForeColor = System.Drawing.Color.Red;
            controlLabel.Text = "Choice empty place. Click this if you want reject";
            controlLabel.Click += new System.EventHandler(this.labelTmp_Click);
            this.Controls.Add(controlLabel);
            this.Controls[this.Controls.Count - 1].Show();

            Dictionary<Button, Good>.KeyCollection kC = dictButGood.Keys;
            for (int i = 0; i < kC.Count; i++)
            {
                kC.ElementAt(i).MouseClick += Choice_Good;
            }
        }

        void Choice_Good(object sender, MouseEventArgs e)
        {
            if (shiftBtn != null)
            {
                foreach (Section section in SectionsManager.Sections)
                    dictGraph[dictKvp[section]].Clear(((Button)sender).Parent.BackColor);
                shiftBtn.Show();
            }
            shiftBtn = (Button)sender;
            tmpGood = dictButGood[(Button)sender];
            ((Button)sender).Hide();
            dictTemp.Clear();
            dictTemp = fillDictTemp();
            int n, m;
            n = tmpGood.Height / 25;
            m = tmpGood.Width / 25;

            foreach (Section section in SectionsManager.Sections)
            {
                if (dictKvp[section] == (Panel)(((Button)sender).Parent))
                {
                    sectionTmp = section;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            dictTemp[section][i, j] = dictTemp[section][i, j] ^ tmpGood.BoolenLocation[i * 12 + j];//копать тут
                        }
                    }
                    break;
                }
            }

            bool yes = false;
            dictGraph.Clear();
            foreach (Section section in SectionsManager.Sections)
            {
                dictGraph.Add(dictKvp[section], dictKvp[section].CreateGraphics());
            }
            foreach (Section section in SectionsManager.Sections)
            {
                for (int k = 0; k < 4; k++)
                {
                    for (int l = 0; l < 12; l++)
                    {
                        if (OffSet(k, l, m - 1, n - 1, dictTemp[section]))
                        {
                            yes = true;
                            dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25 + 13, k * 25);
                            dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25, k * 25 + 13);
                        }
                        else
                        {
                            if (OffSet(k, l, n - 1, m - 1, dictTemp[section]))
                            {
                                yes = true;
                                dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25 + 13, k * 25);
                                dictGraph[dictKvp[section]].DrawLine(new Pen(Color.Red), l * 25, k * 25, l * 25, k * 25 + 13);
                            }
                        }
                    }
                }
            }

            if (yes)
            {
                foreach (Section section in SectionsManager.Sections)
                {
                    dictKvp[section].MouseClick += Choice_MouseClick1;
                }
            }
        }

        void Choice_MouseClick1(object sender, MouseEventArgs e)
        {
            foreach (Section section in SectionsManager.Sections)
            {
                if (dictKvp[section] == (Panel)sender)
                {
                    int x = (int)(e.X / 25);

                    if (x == 12)   x--;

                    int y = (int)(e.Y / 25);

                    if (y == 4)    y--;

                    if (OffSet(y, x, tmpGood.Width / 25 - 1, tmpGood.Height / 25 - 1, dictTemp[section]))
                    {
                        if (DataBase.GetSqlConnection().State == ConnectionState.Open)
                        {
                            //MessageBox.Show(x.ToString() + " " + y.ToString());
                            SqlCommand command;
                            string query;
                            decimal clID;
                            clID = tmpGood.ClientID;
                            int secID = section.SectionID;
                            DateTime dt1 = tmpGood.DateStart;
                            DateTime dt2 = tmpGood.DateFinish;
                            string date1 = tmpGood.DateStart.ToShortDateString();
                            string date2 = tmpGood.DateFinish.ToShortDateString();
                            string title = tmpGood.Title;
                            int cost = (int)tmpGood.Cost;
                            int fine = (int)tmpGood.Fine;
                            int stat;
                            if (tmpGood.Status) stat = 1;
                            else                stat = 0;
                            int h = tmpGood.Height / 25;
                            int w = tmpGood.Width / 25;

                            List<bool> boolTmpForGood = new List<bool>();
                            for (int i = 0; i < 48; i++)
                                boolTmpForGood.Add(false);

                            for (int i = 0; i < y; i++)
                            {
                                for (int j = 0; j < 12; j++)
                                    boolTmpForGood[i * 12 + j] = false;
                            }

                            for (int i = 0; i < h; i++)
                            {
                                for (int l = 0; l < x; l++)
                                    boolTmpForGood[(y + i) * 12 + l] = false;
                                for (int l = x; l < x + w; l++)
                                    boolTmpForGood[(y + i) * 12 + l] = true;
                                for (int l = x + w; l < 12; l++)
                                    boolTmpForGood[(y + i) * 12 + l] = false;
                            }

                            for (int i = y + h; i < 4; i++)
                            {
                                for (int j = 0; j < 12; j++)
                                    boolTmpForGood[i * 12 + j] = false;
                            }

                            double loc = Goodsmanager.ToDouble(boolTmpForGood);

                            query = string.Format("update Goods set ClientID = {11}, SectionID = {0}, DateStart = '{1}', DateFinish = '{2}', Cost = {3}, Fine = {4}, Stat = {5}, Location = {6}, Title = '{7}', Height = {8}, Width = {9} where GoodID = {10}", secID, date1, date2, cost, fine, stat, loc, title, h * 25, w * 25, tmpGood.GoodID ,clID);
                            command = new SqlCommand(query, DataBase.Sql);
                            command.ExecuteNonQuery();

                            tmpGood.BoolenLocation = boolTmpForGood;

                            List<bool> listBools = new List<bool>() ;
                            if (section != sectionTmp)
                            {
                                listBools = SectionsManager.OrBool(boolTmpForGood, section.BoolenTopography);

                                loc = Goodsmanager.ToDouble(listBools);
                                //section.Or(listBools);

                                query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc, section.SectionID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();
                            }

                            Good tmpTmpGood;
                            foreach (Good good in Goodsmanager.Goods)
                            {
                                if (good.GoodID == tmpGood.GoodID)
                                {
                                    tmpTmpGood = good;
                                    break;
                                }
                            }
                            //tmpTmpGood = tmpGood;

                            Button button = new Button();

                            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                            shiftBtn.Location = new System.Drawing.Point(x * 25, y * 25);
                            shiftBtn.Size = new System.Drawing.Size(w * 25, h * 25);
                            //shiftBtn.Text = "button1";

                            //dictButGood.Add(button, Goodsmanager.Goods[Goodsmanager.Goods.Count - 1]);//не то 
                            shiftBtn.Parent.Controls.Remove(shiftBtn);
                            dictKvp[section].Controls.Add(shiftBtn);
                            shiftBtn.Visible = true;
                            if (section != sectionTmp)
                            {
                                sectionTmp.FromMasToList(dictTemp[sectionTmp]);//!!!копать тут!!!

                                double loc2 = Goodsmanager.ToDouble(sectionTmp.BoolenTopography);
                                query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc2, sectionTmp.SectionID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();

                                section.BoolenTopography = listBools;
                            }

                            else
                            {
                                sectionTmp.FromMasToList(dictTemp[sectionTmp]);
                                sectionTmp.BoolenTopography = SectionsManager.OrBool(boolTmpForGood, sectionTmp.BoolenTopography);

                                double loc2 = Goodsmanager.ToDouble(sectionTmp.BoolenTopography);
                                query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc2, sectionTmp.SectionID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();
                            }

                            labelTmp_Click(this, EventArgs.Empty);
                            //shiftBtn = null;
                            break;
                        }
                    }
                    else
                    {
                        if (OffSet(y, x, tmpGood.Height / 25 - 1, tmpGood.Width / 25 - 1, dictTemp[section]))
                        {
                            if (DataBase.GetSqlConnection().State == ConnectionState.Open)
                            {
                                //MessageBox.Show(x.ToString() + " " + y.ToString());
                                SqlCommand command;
                                string query;
                                decimal clID;
                                clID = tmpGood.ClientID;
                                int secID = section.SectionID;
                                DateTime dt1 = tmpGood.DateStart;
                                DateTime dt2 = tmpGood.DateFinish;
                                string date1 = tmpGood.DateStart.ToShortDateString();
                                string date2 = tmpGood.DateFinish.ToShortDateString();
                                string title = tmpGood.Title;
                                int cost = (int)tmpGood.Cost;
                                int fine = (int)tmpGood.Fine;
                                int stat;
                                if (tmpGood.Status) stat = 1;
                                else stat = 0;
                                int w = tmpGood.Height / 25;
                                int h = tmpGood.Width / 25;

                                List<bool> boolTmpForGood = new List<bool>();
                                for (int i = 0; i < 48; i++)
                                    boolTmpForGood.Add(false);

                                for (int i = 0; i < y; i++)
                                {
                                    for (int j = 0; j < 12; j++)
                                        boolTmpForGood[i * 12 + j] = false;
                                }

                                for (int i = 0; i < h; i++)
                                {
                                    for (int l = 0; l < x; l++)
                                        boolTmpForGood[(y + i) * 12 + l] = false;
                                    for (int l = x; l < x + w; l++)
                                        boolTmpForGood[(y + i) * 12 + l] = true;
                                    for (int l = x + w; l < 12; l++)
                                        boolTmpForGood[(y + i) * 12 + l] = false;
                                }

                                for (int i = y + h; i < 4; i++)
                                {
                                    for (int j = 0; j < 12; j++)
                                        boolTmpForGood[i * 12 + j] = false;
                                }

                                double loc = Goodsmanager.ToDouble(boolTmpForGood);

                                query = string.Format("update Goods set ClientID = {11}, SectionID = {0}, DateStart = '{1}', DateFinish = '{2}', Cost = {3}, Fine = {4}, Stat = {5}, Location = {6}, Title = '{7}', Height = {8}, Width = {9} where GoodID = {10}", secID, date1, date2, cost, fine, stat, loc, title, h * 25, w * 25, tmpGood.GoodID, clID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();

                                tmpGood.BoolenLocation = boolTmpForGood;

                                List<bool> listBools = SectionsManager.OrBool(boolTmpForGood, section.BoolenTopography);
                                loc = Goodsmanager.ToDouble(listBools);

                                query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc, section.SectionID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();

                                Good tmpTmpGood;
                                foreach (Good good in Goodsmanager.Goods)
                                {
                                    if (good.GoodID == tmpGood.GoodID)
                                    {
                                        tmpTmpGood = good;
                                        break;
                                    }
                                }
                                tmpTmpGood = tmpGood;

                                Button button = new Button();

                                button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

                                shiftBtn.Location = new System.Drawing.Point(x * 25, y * 25);
                                shiftBtn.Size = new System.Drawing.Size(w * 25, h * 25);
                                //shiftBtn.Text = "button1";

                                //dictButGood.Add(button, Goodsmanager.Goods[Goodsmanager.Goods.Count - 1]);//не то 
                                shiftBtn.Parent.Controls.Remove(shiftBtn);
                                dictKvp[section].Controls.Add(shiftBtn);
                                shiftBtn.Visible = true;

                                sectionTmp.FromMasToList(dictTemp[sectionTmp]);

                                double loc2 = Goodsmanager.ToDouble(sectionTmp.BoolenTopography);
                                query = string.Format("update Sections set Topography = {0} where SectionID = {1}", loc2, sectionTmp.SectionID);
                                command = new SqlCommand(query, DataBase.Sql);
                                command.ExecuteNonQuery();

                                section.BoolenTopography = listBools;

                                labelTmp_Click(this, EventArgs.Empty);
                                //shiftBtn = null;
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Incorrect place");
                        }
                    }
                    break;
                }
            }
        }

        private Dictionary<Section, bool[,]> fillDictTemp()
        {
            Dictionary<Section, bool[,]> tmp = new Dictionary<Section,bool[,]>();

            foreach (Section section in SectionsManager.Sections)
            {
                tmp.Add(section, new bool[4, 12]);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        (tmp[section])[i, j] = section.BoolenTopography[i * 12 + j];
                    }
                }
            }

            return tmp;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dictionary<Button, Good>.KeyCollection kC = dictButGood.Keys;
            //foreach (Button btn in kC)
            //    btn.BackColor = btn.Parent.BackColor;
            ColorButton();
            
            string lastName = textBox3.Text;
            int id;
            string query = string.Format("select * from Clients where LastName = '{0}'", lastName);
            SqlCommand command = new SqlCommand(query, DataBase.Sql);
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            if (reader.HasRows)     id = (int)reader["ClientID"];
            else                    id = 0;

            reader.Close();

            kC = dictButGood.Keys;

            foreach (Button btn in kC)
            {
                if (dictButGood[btn].ClientID == id)
                    btn.BackColor = Color.Green;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            foreach (Graphics gr in dictGraph.Values)
            {
                gr.Clear(panel1.BackColor);
            }
            ColorButton();
            //Dictionary<Button, Good>.KeyCollection kC = dictButGood.Keys;
            //foreach (Button btn in kC)
            //    btn.BackColor = btn.Parent.BackColor;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dateTimePicker1.MaxDate = dateTimePicker1.Value.AddDays(1);
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
            dateTimePicker1.MinDate = dateTimePicker1.Value;

            ColorButton();

        }

        private void ColorButton()
        {
            Dictionary<Button, Good>.KeyCollection kC = dictButGood.Keys;
            for (int i = 0; i < kC.Count; i++)
            {
                if (dictButGood[kC.ElementAt(i)].DateFinish.Ticks < dateTimePicker1.Value.AddDays(3).Ticks)
                    kC.ElementAt(i).BackColor = Color.Yellow;
                if (dateTimePicker1.Value.Ticks > dictButGood[kC.ElementAt(i)].DateFinish.Ticks)
                {
                    //MessageBox.Show(i.ToString());
                    kC.ElementAt(i).BackColor = Color.Red;
                    Good tmp = dictButGood[kC.ElementAt(i)];
                    long elapsedSpan = dateTimePicker1.Value.Ticks - tmp.DateFinish.Ticks;
                    TimeSpan tS = new TimeSpan(elapsedSpan);
                    tmp.Fine = tS.Days * (tmp.Height / 25) * (tmp.Width / 25) * 10;

                    if (DataBase.GetSqlConnection().State == ConnectionState.Open)
                    {
                        string query = string.Format("update Goods set Fine = {0} where GoodID = {1}", tmp.Fine, tmp.GoodID);
                        SqlCommand command = new SqlCommand(query, DataBase.Sql);
                        command.ExecuteNonQuery();
                    }
                }
                else
                    kC.ElementAt(i).BackColor = (kC.ElementAt(i).Parent).BackColor;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.Show(this);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (Good good in Goodsmanager.Goods)
            {
                if (good.Status && good.Fine > 0)
                    goodsFine.Add(good);
            }
            if (goodsFine != null && goodsFine.Count != 0)
            {
                Fines fines = new Fines();
                fines.Show(this);
            }
            else
                MessageBox.Show("Not fine");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            goodToForm.Clear();

            if (dateTimePicker2.Value.Ticks <= dateTimePicker3.Value.Ticks)
            {
                foreach (Good good in Goodsmanager.Goods)
                {
                    if ((good.DateStart.Ticks >= dateTimePicker2.Value.Ticks && good.DateStart.Ticks <= dateTimePicker3.Value.Ticks) || (good.DateFinish.Ticks >= dateTimePicker2.Value.Ticks && good.DateFinish.Ticks <= dateTimePicker3.Value.Ticks) || (good.DateFinish.Ticks >= dateTimePicker3.Value.Ticks && good.DateStart.Ticks <= dateTimePicker2.Value.Ticks))
                    {
                        goodToForm.Add(good);
                    }
                    else
                    {
                        if (good.DateFinish.Ticks < dateTimePicker2.Value.Ticks)
                        {
                            long elapsedSpan = dateTimePicker2.Value.Ticks - good.DateFinish.Ticks;
                            TimeSpan tS = new TimeSpan(elapsedSpan);
                            if (good.Fine >= (good.Width / 25) * (good.Height / 25) * 10 * tS.Days)
                                goodToForm.Add(good);
                        }
                    }
                }
                TableForm tableForm = new TableForm();
                tableForm.Show(this);
            }
        }
    }
}

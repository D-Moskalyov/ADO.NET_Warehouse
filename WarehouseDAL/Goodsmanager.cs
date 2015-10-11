using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WarehouseDAL
{
    public class Goodsmanager
    {
        static List<Good> goods = new List<Good>();

        public static List<Good> Goods
        {
            get { return Goodsmanager.goods; }
            set { Goodsmanager.goods = value; }
        }

        public static void LoadGoods(SqlDataReader reader)
        {
            while (reader.Read())
            {
                Good good = new Good();

                good.GoodID = (int)reader["GoodID"];
                good.Title = reader["Title"].ToString();
                good.SectionID = (int)reader["SectionID"];
                good.Height = (int)reader["Height"];
                good.Width = (int)reader["Width"];
                good.ClientID = (int)reader["ClientID"];
                good.DateStart = (DateTime)reader["DateStart"];
                good.DateFinish = (DateTime)reader["DateFinish"];
                good.Cost = (int)reader["Cost"];
                good.Fine = (int)reader["Fine"];
                good.Status = (bool)reader["Stat"];

                decimal locationD = (decimal)reader["Location"];
                double location = (double)locationD;

                while (location >= 1)
                {
                    if (location % 2 == 1)
                    {
                        good.BoolenLocation.Add(true);
                        location = (location - 1) / 2;
                    }
                    else
                    {
                        good.BoolenLocation.Add(false);
                        location = location/ 2;
                    }
                    
                }
                int c = good.BoolenLocation.Count;
                for (int i = 48 - c; i > 0; i--)
                {
                    good.BoolenLocation.Insert(48 - i, false);
                }
                good.IsChanged = false;

                goods.Add(good);
            }
        }

        public static void UpdateGoods()
        {
            string query1 = string.Empty;
            string query2 = string.Empty;
            Good tmpGood = new Good();
            SqlCommand command;
            SqlDataReader reader;
            List<int> changedData;

            query1 = "select * from Goods";
            command = new SqlCommand(query1, DataBase.Sql);
            reader = command.ExecuteReader();

            changedData = new List<int>();

            foreach (Good good in Goodsmanager.Goods)
            {
                if (good.IsChanged) changedData.Add(good.GoodID);
            }
            while (reader.Read())
            {
                if (changedData.Contains((int)reader["GoodID"]))
                {
                    foreach (Good good in Goodsmanager.Goods)
                    {
                        if (good.GoodID == ((int)reader["GoodID"]))
                        {
                            tmpGood = good;
                            break;
                        }
                    }
                    query2 = string.Format("update Goods set Title = '{0}' where GoodID = {1}", tmpGood.Title, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Goods set Height = {0} where GoodID = {1}", tmpGood.Height, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Goods set Width = {0} where GoodID = {1}", tmpGood.Width, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set SectionID = {0} where GoodID = {1}", tmpGood.SectionID, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set Location = '{0}' where GoodID = {1}", ToDouble(tmpGood.BoolenLocation), tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set ClientID = {0} where GoodID = {1}", tmpGood.ClientID, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set DateStart = '{0}' where GoodID = {1}", tmpGood.DateStart.ToString(), tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set DateFinish = '{0}' where GoodID = {1}", tmpGood.DateFinish.ToString(), tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set Cost = {0} where GoodID = {1}", tmpGood.Cost, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set Fine = {0} where GoodID = {1}", tmpGood.Fine, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Offers set Status = {0} where GoodID = {1}", tmpGood.Status, tmpGood.GoodID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();


                    tmpGood.IsChanged = false;
                }
            }

            reader.Close();
        }

        public static double ToDouble(List<bool> list)
        {
            double number = 0;
            for (int i = 0; i < 48; i++)
            {
                if (list[i])
                    number += Math.Pow(2, i);
            }
            return number;
        }
    }
}

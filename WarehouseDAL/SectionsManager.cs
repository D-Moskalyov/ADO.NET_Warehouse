using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDAL;
using System.Data;
using System.Data.SqlClient;

namespace WarehouseDAL
{
    public class SectionsManager
    {
        static List<Section> sections = new List<Section>();

        public static List<Section> Sections
        {
            get { return SectionsManager.sections; }
            set { SectionsManager.sections = value; }
        }

        public static void LoadSections(SqlDataReader reader)
        {
            while (reader.Read())
            {
                Section section = new Section();

                section.SectionID = (int)reader["SectionID"];
                //section.Topography = (double)reader["Topography"];
                decimal topography = (decimal)reader["Topography"];
                while (topography != 0)
                {
                    if (topography % 2 == 1)
                    {
                        section.BoolenTopography.Add(true);
                        topography = (topography - 1) / 2;
                    }
                    else
                    {
                        section.BoolenTopography.Add(false);
                        topography = topography / 2;
                    }
                    
                }

                int c = section.BoolenTopography.Count;
                for (int i = 48 - c; i > 0; i--)
                {
                    section.BoolenTopography.Add(false);
                }

                section.IsChanged = false;
                sections.Add(section);
            }
        }

        public static void UpdateSections()
        {
            string query1 = string.Empty;
            string query2 = string.Empty;
            Section tmpSection = new Section();
            SqlCommand command;
            SqlDataReader reader;
            List<int> changedData;

            query1 = "select * from Sections";
            command = new SqlCommand(query1, DataBase.Sql);
            reader = command.ExecuteReader();

            changedData = new List<int>();

            foreach (Section section in SectionsManager.Sections)
            {
                if (section.IsChanged) changedData.Add(section.SectionID);
            }

            List<string> querys = new List<string>();
            while (reader.Read())
            {
                //if (changedData.Contains((int)reader["SectionID"]))
                //{
                    foreach (Section section in SectionsManager.Sections)
                    {
                        if (section.SectionID == ((int)reader["SectionID"]))
                        {
                            tmpSection = section;
                            break;
                        }
                    }
                    querys.Add(string.Format("update Sections set Topography = '{0}' where SectionID = {1}", ToDouble(tmpSection.BoolenTopography), tmpSection.SectionID));
                    //query2 = string.Format("update Sections set Topography = '{0}' where SectionID = {1}", ToDouble(tmpSection.BoolenTopography), tmpSection.SectionID);
                    //(new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();

                    tmpSection.IsChanged = false;
                //}
            }

            reader.Close();

            foreach(string str in querys)
            {
                (new SqlCommand(str, DataBase.Sql)).ExecuteNonQuery();
            }

            DataBase.Sql.Close();
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

        public static List<bool> OrBool(List<bool> bools1, List<bool> bools2)
        {
            List<bool> list = new List<bool>();
            for (int i = 0; i < 48; i++)
            {
                list.Add(bools1[i] | bools2[i]);
            }
            return list;
        }

    }
}

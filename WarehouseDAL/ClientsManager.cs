using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WarehouseDAL
{
    public class ClientsManager
    {
        static List<Client> clients = new List<Client>();

        public static List<Client> Clients
        {
            get { return ClientsManager.clients; }
            set { ClientsManager.clients = value; }
        }

        public static void LoadClients(SqlDataReader reader)
        {
            while (reader.Read())
            {
                Client client = new Client();

                client.ClientID = (int)reader["ClientID"];
                client.FirstName = reader["FirstName"].ToString();
                client.LastName = reader["LastName"].ToString();
                client.Email = reader["email"].ToString();
                client.IsChanged = false;

                clients.Add(client);
            }
        }

        public static void UpdateClients()
        {
            string query1 = string.Empty;
            string query2 = string.Empty;
            Client tmpClient = new Client();
            SqlCommand command;
            SqlDataReader reader;
            List<int> changedData;

            query1 = "select * from Clients";
            command = new SqlCommand(query1, DataBase.Sql);
            reader = command.ExecuteReader();

            changedData = new List<int>();

            foreach (Client client in ClientsManager.Clients)
            {  
                if (client.IsChanged) changedData.Add(client.ClientID);
            }
            while (reader.Read())
            {
                if (changedData.Contains((int)reader["ClientID"]))
                {
                    foreach (Client client in ClientsManager.Clients)
                    {
                        if (client.ClientID == ((int)reader["ClientID"]))
                        {
                            tmpClient = client;
                            break;
                        }
                    }
                    query2 = string.Format("update Clients set FirstName = '{0}' where ClientID = {1}", tmpClient.FirstName, tmpClient.ClientID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Clients set LastName = '{0}' where ClientID = {1}", tmpClient.LastName, tmpClient.ClientID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();
                    query2 = string.Format("update Clients set email = '{0}' where ClientID = {1}", tmpClient.Email, tmpClient.ClientID);
                    (new SqlCommand(query2, DataBase.Sql)).ExecuteNonQuery();

                    tmpClient.IsChanged = false;
                }
            }

            reader.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDAL;

namespace Сoursework
{
    class ClientForCB
    {
        private Client clientFromList;

        public Client ClientFromList
        {
            get { return clientFromList; }
        }
        private string name;

        public string Name
        {
            get { return name; }
        }

        public ClientForCB(Client cl)
        {
            clientFromList = cl;
            name = string.Format("{0} {1} {2}", cl.ClientID, cl.FirstName, cl.LastName);
        }

    }
}

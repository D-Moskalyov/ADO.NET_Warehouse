using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDAL
{
    public class Client
    {
        int clientID;

        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }
        string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        bool isChanged;

        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }
    }
}

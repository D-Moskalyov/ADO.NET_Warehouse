using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDAL
{
    public class Good
    {
        int goodID;

        public int GoodID
        {
            get { return goodID; }
            set { goodID = value; }
        }

        int sectionID;

        public int SectionID
        {
            get { return sectionID; }
            set { sectionID = value; }
        }
        string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        bool isChanged;

        List<bool> boolenLocation = new List<bool>(15);

        public List<bool> BoolenLocation
        {
            get { return boolenLocation; }
            set { boolenLocation = value; }
        }

        int clientID;

        public int ClientID
        {
            get { return clientID; }
            set { clientID = value; }
        }

        int cost;

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        int fine;

        public int Fine
        {
            get { return fine; }
            set { fine = value; }
        }
        DateTime dateStart;

        public DateTime DateStart
        {
            get { return dateStart; }
            set { dateStart = value; }
        }
        DateTime dateFinish;

        public DateTime DateFinish
        {
            get { return dateFinish; }
            set { dateFinish = value; }
        }
        bool status;

        public bool Status
        {
            get { return status; }
            set { status = value; }
        }

        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }
    }
}

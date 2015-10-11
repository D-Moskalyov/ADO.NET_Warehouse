using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseDAL
{
    public class Section
    {
        int sectionID;

        public int SectionID
        {
            get { return sectionID; }
            set { sectionID = value; }
        }
        //double topography;

        //public double Topography
        //{
        //    get { return topography; }
        //    set { topography = value; }
        //}
        List<bool> boolenTopography = new List<bool>(15);

        public List<bool> BoolenTopography
        {
            get { return boolenTopography; }
            set { boolenTopography = value; }
        }
        bool isChanged;

        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }

        public void Or(List<bool> b)
        {
            for (int i = 0; i < 48; i++)
            {
                this.BoolenTopography[i] = this.BoolenTopography[i] | b[i];
            }
        }

        public void ExclusiveOr(bool[,] b)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 12; j++)
                    b[i, j] = this.BoolenTopography[i * 12 + j] ^ b[i, j];
            }
        }

        public void ExclusiveOrList(List<bool> bools)
        {
            for (int i = 0; i < 48; i++)
                this.BoolenTopography[i] = this.BoolenTopography[i] ^ bools[i];
        }

        public void FromMasToList(bool[,] b)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 12; j++)
                    this.BoolenTopography[i * 12 + j] = b[i, j];
            }
        }
    }
}

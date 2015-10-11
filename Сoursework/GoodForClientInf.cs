using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseDAL;

namespace Сoursework
{
    class GoodForClientInf
    {
        private Good goodFromList;

        public Good GoodFromList
        {
          get { return goodFromList; }
          set { goodFromList = value; }
        }
        
        private string title;

        public string Title
        {
          get { return title; }
          set { title = value; }
        }

        public GoodForClientInf(Good gd)
        {
            goodFromList = gd;
            title = gd.Title;
        }
    }
}

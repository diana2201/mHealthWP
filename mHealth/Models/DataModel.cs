using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTwitter.Models;

namespace mHealth.Models
{
    public class DataModel
    {
        public ObservableCollection<Tweet> data;
        public ObservableCollection<Tweet> Data
        {
            get
            {
                if (data == null)
                    data = new ObservableCollection<Tweet>();

                return data;
            }
            set
            {
                data = value;
            }
        }
    }
}

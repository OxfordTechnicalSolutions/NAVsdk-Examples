using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavLibWPFExample
{
    public class ObservableObject : INotifyPropertyChanged
    {
        //Event that will is fired whenever a change is made to a property
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property_name)
        {
            //If any handlers are subscribed PropertyChanged, notify them with the name of the property
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
        }
    }
}

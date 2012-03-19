using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Utils
{   
    public class PropertyChangedEventArgs : EventArgs
    {
        private readonly string _propertyName;

        public string PropertyName { get; set; }

        public PropertyChangedEventArgs(string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            PropertyName = propertyName;
        }
    }

    public interface IPropertyChangedNotifiable
    {
        event EventHandler<PropertyChangedEventArgs> PropertyChanged;
    }
}

using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Utils
{
    public class PropertyChangingEventArgs : EventArgs
    {
        public string PropertyName { get; set; }
        public bool CanChange { get; set; }

        public PropertyChangingEventArgs(string propertyName, bool canChange = true)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            PropertyName = propertyName;
            CanChange = canChange;
        }
    }

    public interface IPropertyChangingNotifiable : IPropertyChangedNotifiable
    {
        event EventHandler<PropertyChangingEventArgs> PropertyChanging;
    }
}

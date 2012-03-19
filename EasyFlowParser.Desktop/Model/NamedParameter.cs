using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    ///<summary>
    /// Named parameter class.
    ///</summary>
    public class NamedParameter : ParsedObject
    {
        private string _name;
        private Expression _value = null;
        private bool _isSweepParam;
        private bool _isOntoParam = false;
        private bool _isSubscriptionParam = false;

        public bool IsSubscriptionParam
        {
            get { return _isSubscriptionParam; }
            set { _isSubscriptionParam = value; }
        }

        /// <summary>
        /// Gets or sets whether the parameter is ontological.
        /// </summary>
        public bool IsOntoParam
        {
            get { return _isOntoParam; }
            set { _isOntoParam = value; }
        }

        /// <summary>
        /// Gets or sets whether the parameter is sweeped.
        /// </summary>
        public bool IsSweepParam
        {
            get { return _isSweepParam; }
            set { _isSweepParam = value; }
        }

        /// <summary>
        /// Gets or sets parameter name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets parameter value.
        /// </summary>
        public Expression Value
        {
            get { return _value; }
            set { _value = value; }
        }

        ///<summary>
        ///</summary>
        ///<param name="name"></param>
        ///<param name="value"></param>
        ///<exception cref="ArgumentNullException"></exception>
        public NamedParameter(string name, Expression value = null)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}{2}", _name, _isSweepParam ? "sweep " : "" ,_value);
        }

        protected override object CreateClone()
        {
            return new NamedParameter(Name, (Expression) Value.Clone());
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = (NamedParameter)base.Clone();

            clone.IsOntoParam = IsOntoParam;
            clone.IsSweepParam = IsSweepParam;

            return clone;
        }
    }
}

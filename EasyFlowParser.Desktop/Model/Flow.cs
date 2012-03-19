using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Easis.Common;
using Easis.Common.IdGenerators;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, содержащий представление WF
    /// </summary>
    public class Flow : ParsedObject
    {
        private BlockCollection _blocks;        
        private NamedParameterCollection _attributes = new NamedParameterCollection();
        private static IOidGenerator<Guid> _flowIdGenerator = new GuidOidGenerator(); // TODO: IoC injection
        private Guid _id;

        /// <summary>
        /// Gets Wf guid.
        /// </summary>
        public Guid Id
        {
            get { return _id; }
        }

        ///<summary>
        /// Gets flow attributes.
        ///</summary>
        public NamedParameterCollection Attributes
        {
            get
            {
                return _attributes;
            }
        }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public Flow()
        {
            _id = _flowIdGenerator.GetId(this);
            _blocks = new BlockCollection(this);
        }

        public Flow(Guid guid)
        {
            _id = guid;
            _blocks = new BlockCollection(this);
        }

        /// <summary>
        /// Возвращает редактируемую коллекцию блоков WF.
        /// </summary>
        public BlockCollection Blocks
        {
            get { return _blocks; }
        }


        protected override object CreateClone()
        {
            return new Flow();
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
            var clone = (Flow)base.Clone();

            clone._blocks = (BlockCollection) _blocks.Clone(clone);            

            return clone;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Easis.Common;
using Easis.Common.Collections;
using Easis.Common.IdGenerators;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс для хранения коллекции блоков.
    /// </summary>
    public class BlockCollection : OwnedCollection<Flow, BlockBase>, ICloneable
    {                     
        private IOidGenerator<long> _idGenerator = new LongOidGenerator(); // TODO: IoC

        public BlockCollection(Flow parent): base(parent)
        {         
        }

        public BlockCollection(Flow parent, IEnumerable<BlockBase> itemCollection): 
            base(parent, itemCollection)
        {
        }

        /// <summary>
        /// Возвращает блок по переданному идентификатору.
        /// </summary>
        /// <param name="id">Искомый идентификатор.</param>
        /// <returns>Блок или null, если блока с таким Id нет в коллекции.</returns>
        public BlockBase GetById(long id)
        {
            return this.FirstOrDefault(block => block.Id == id);
        }        

        /// <summary>
        /// Присваивает полям блока значения для обозначения принадлежности к 
        /// Workflow, который хранит данную коллекцию.
        /// </summary>
        /// <param name="item">Блок.</param>
        protected override void AdoptItem(BlockBase item)
        {
            if (item == null) throw new ArgumentNullException("item");            

            item.Flow = Owner;
            item.Id = _idGenerator.GetId(item);
        }

        /// <summary>
        /// Метод, обратный методу <see cref="Adopt"/>.
        /// </summary>
        /// <param name="item">Блок.</param>
        protected override void DeadoptItem(BlockBase item)
        {
            item.Flow = null;
            item.Id = -1;
        }

        public object Clone()
        {
            return new BlockCollection(Owner, this.Select(block => (BlockBase)block.Clone()));
        }

        public object Clone(Flow parent)
        {
            var clone = (BlockCollection)Clone();

            foreach (var block in this)
                AdoptItem(block);

            clone.Owner = parent; 

            return clone;
        }
    }
}

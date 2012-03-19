using System;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    public class FileValue : ValueBase
    {
        public FileDescriptor AsFileDescriptor
        {
            get
            {
                return base.Value as FileDescriptor;
            }
        }

        /// <summary>
        /// Возвращает, может ли данное значение быть приведено к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип.</param>
        /// <returns>Может / не может.</returns>
        public override bool CanCastTo(DataType dataType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Приводит данное значение к переданному типу.
        /// </summary>
        /// <param name="dataType">Тип, к которому осуществляется приведение.</param>
        /// <returns>Приведённое значение.</returns>
        public override ValueBase CastTo(DataType dataType)
        {
            throw new NotImplementedException();
        }

        public FileValue(FileDescriptor val): base(val)
        {
            DataType = DataType.File;
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        protected FileValue()
        {
        }

        protected override object CreateClone()
        {
            return new FileValue();
        }
    }
}

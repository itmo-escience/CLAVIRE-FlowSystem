using System;
using Easis.Wfs.EasyFlow.Model.Expressions;

namespace Easis.Wfs.EasyFlow.Model
{
    public static class ExpressionExtensions
    {
        public static bool IsInt(this ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.DataType == DataType.Int;
        }

        public static bool IsDouble(this ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.DataType == DataType.Double;
        }

        public static bool IsString(this ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.DataType == DataType.String;
        }

        public static bool IsStructure(this ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.DataType == DataType.Structure;
        }

        public static bool IsFile(this ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.DataType == DataType.File;
        }

        public static bool IsList(this ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.DataType == DataType.List;
        }

        public static bool IsHash(this ValueBase value)
        {
            if (value == null) throw new ArgumentNullException("value");

            return value.DataType == DataType.Hash;
        }
    }
}

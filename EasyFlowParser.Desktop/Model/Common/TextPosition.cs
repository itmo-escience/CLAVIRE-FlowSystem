using System;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Позиция в тексте.
    /// </summary>
    public struct TextPosition : ICloneable
    {
        /// <summary>
        /// Строка.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Колонка (символ в строке, считая с единицы).
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="row">Строка.</param>
        /// <param name="column">Колонка.</param>
        public TextPosition(int row, int column): this()
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", Row, Column);
        }

        public object Clone()
        {
            return new TextPosition(Row, Column);
        }
    }
}

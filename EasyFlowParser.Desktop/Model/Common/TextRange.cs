using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Структура для хранения данных об участке текста.
    /// </summary>
    public struct TextRange: ICloneable
    {
        /// <summary>
        /// Возвращает или устанавливает начало участка.
        /// </summary>
        public TextPosition Start { get; set; }

        /// <summary>
        /// Возвращает или устанавливает конец участка.
        /// </summary>
        public TextPosition End { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="start">Начало участка.</param>
        /// <param name="end">Конец участка.</param>
        public TextRange(TextPosition start, TextPosition end): this()
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return string.Format("({0} - {1})", Start, End);
        }

        public object Clone()
        {
            return new TextRange((TextPosition) Start.Clone(), (TextPosition) End.Clone());
        }
    }
}

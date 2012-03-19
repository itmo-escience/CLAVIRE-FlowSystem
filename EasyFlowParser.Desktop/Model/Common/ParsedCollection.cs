using System;
using System.Collections.Generic;
using Easis.Common.Collections;

namespace Easis.Wfs.EasyFlow.Model
{
    public class ParsedCollection<TItem> : ListCollection<TItem>, IParsedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ParsedCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ParsedCollection(IEnumerable<TItem> items): base(items)
        {
        }

        /// <summary>
        /// Кусок текста скрипта, соответствующий данному объекту.
        /// </summary>
        public string TextBehind { get; set; }

        /// <summary>
        /// Область текста скрипта, которой соответствует данный объект.
        /// </summary>
        public TextRange TextRange { get; set; }

        /// <summary>
        /// Обнаружена ли ошибка в данном объекте во время разбора.
        /// </summary>
        public bool IsInvalid { get; set; }

        /// <summary>
        /// Является ли объект полностью построенным.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Область, которой принадлежит объект.
        /// </summary>
        public Scope Scope { get; set; }

        /// <summary>
        /// Сообщение об ошибке в случае её наличия.
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }
    }
}

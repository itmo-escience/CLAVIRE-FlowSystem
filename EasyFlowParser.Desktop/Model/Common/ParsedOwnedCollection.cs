using System;
using System.Collections.Generic;
using Easis.Common.Collections;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Класс, реализующий коллекцию, которая связана с каким-либо объектом. При добавлении в коллекцию
    /// нового элемента происходит его адаптация.
    /// </summary>
    /// <typeparam name="TOwner">Тип объекта, ассоциированного с коллекцией.</typeparam>
    /// <typeparam name="TItem">Тип элементов коллекции.</typeparam>
    public abstract class ParsedOwnedCollection<TOwner, TItem>: OwnedCollection<TOwner, TItem>, IParsedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected ParsedOwnedCollection(TOwner owner): base(owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected ParsedOwnedCollection(TOwner owner, IEnumerable<TItem> items): base(owner, items)
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
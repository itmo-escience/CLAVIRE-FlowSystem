using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Абстрактный класс, реализующий интерфейс IParsedObject.
    /// </summary>
    public abstract class ParsedObject : IParsedObject, ICloneable
    {
        /// <summary>
        /// Кусок текста скрипта, соответствующий данному объекту.
        /// </summary>
        public string TextBehind { get; set; }

        /// <summary>
        /// Область текста скрипта, которой соответствует данный объект.
        /// </summary>
        public TextRange TextRange { get; set; }

        /// <summary>
        /// Обнаружена ли ошибка в данном объекте во время разбора (по умолчанию = true).
        /// </summary>
        public bool IsInvalid { get; set; }

        /// <summary>
        /// Является ли объект полностью построенным (по умолчанию = false).
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Возвращает или устанавливает область, которой принадлежит объект.
        /// </summary>
        public Scope Scope { get; set; }

        public string ErrorMessage { get; set; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        protected ParsedObject()
        {
            TextRange = new TextRange();
            TextBehind = null;
            Scope = null;
            IsInvalid = true;
            IsComplete = false;
        }

        protected abstract object CreateClone();        

        public virtual object Clone()
        {
            var clone = (ParsedObject) CreateClone();

            clone.TextBehind = TextBehind;
            clone.TextRange = (TextRange) TextRange.Clone();
            if(Scope != null)
                clone.Scope = (Scope) Scope.Clone();
            clone.ErrorMessage = ErrorMessage;
            clone.IsComplete = IsComplete;
            clone.IsInvalid = IsInvalid;

            return clone;
        }
    }
}
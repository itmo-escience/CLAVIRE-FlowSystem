using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Wfs.EasyFlow.Model;

namespace Easis.Wfs.EasyFlow.Parsing
{
    /// <summary>
    /// Перечисление типов сообщений парсера.
    /// </summary>
    public enum ParserMessageType
    {
        /// <summary>
        /// Информация.
        /// </summary>
        Info,
        /// <summary>
        /// Подсказка.
        /// </summary>
        Hint,
        /// <summary>
        /// Опасность.
        /// </summary>
        Warning,
        /// <summary>
        /// Ошибка.
        /// </summary>
        Error
    }

    /// <summary>
    /// Сообщение парсера.
    /// </summary>
    public class ParserMessage
    {
        private string _message;
        private ParserMessageType _messageType;
        private IParsedObject _messageObject;
        private ScopeType _scopeType;

        /// <summary>
        /// Область кода, где возникло сообщение.
        /// </summary>
        public ScopeType ScopeType
        {
            get { return _scopeType; }
            set { _scopeType = value; }
        }

        /// <summary>
        /// Объект, в котором произошло сообщение.
        /// </summary>
        public IParsedObject MessageObject
        {
            get { return _messageObject; }
            set { _messageObject = value; }
        }

        /// <summary>
        /// Текстовое сообщение.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// Тип сообщения.
        /// </summary>
        public ParserMessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="position">Положение в тексте срипта.</param>
        public ParserMessage(string message, ScopeType scopeType, ParserMessageType messageType = ParserMessageType.Error, IParsedObject messageObject = null)
        {
            _scopeType = scopeType;
            _message = message;
            _messageType = messageType;
            _messageObject = messageObject;
        }
    }
}

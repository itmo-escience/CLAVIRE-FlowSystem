using System;
using Easis.Wfs.EasyFlow.Utils;

namespace Easis.Wfs.EasyFlow.Parsing
{
    /// <summary>
    /// Настройки работы парсера.
    /// </summary>
    public class ParserSettings: IPropertyChangingNotifiable
    {
        private bool _collectTextInformation;
        private bool _collectScopeInformation;
        private bool _logRuleTraces;
        private bool _isContinousParsingEnabled;

        public bool CollectScopeInformation
        {
            get { return _collectScopeInformation; }
            set
            {
                const string propName = "CollectScopeInformation";

                if (!CanChange(propName))
                    return;

                if (value == _collectScopeInformation)
                    return;

                _collectScopeInformation = value;
                InvokePropertyChanged(propName);
            }
        }

        /// <summary>
        ///   Возвращает или устанавливает, нужно ли собирать информацию о тексте скрипта,
        ///   которую можно в дальнейшем использовать для текстового редактора.
        /// </summary>
        public bool CollectTextInformation
        {
            get { return _collectTextInformation; }
            set
            {
                const string propName = "CollectTextInformation";

                if (!CanChange(propName))
                    return;

                if (value == _collectTextInformation)
                    return;                

                _collectTextInformation = value;
                InvokePropertyChanged(propName);
            }
        }


        /// <summary>
        ///   Выводить ли детальную трассировку по правилам грамматики.
        /// </summary>
        public bool LogRuleTraces
        {
            get { return _logRuleTraces; }
            set
            {
                const string propName = "LogRuleTraces";

                if (!CanChange(propName))
                    return;

                if (value == _logRuleTraces)
                    return;

                _logRuleTraces = value;
                InvokePropertyChanged(propName);
            }
        }

        /// <summary>
        ///   Использовать ли режим непрерывного парсинга.
        /// </summary>
        public bool IsContinousParsingEnabled
        {
            get { return _isContinousParsingEnabled; }
            set
            {
                const string propName = "IsContinousParsingEnabled";

                if (!CanChange(propName))
                    return;

                if (value == _isContinousParsingEnabled)
                    return;

                _isContinousParsingEnabled = value;
                InvokePropertyChanged(propName);
            }
        }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ParserSettings()
        {
            _collectTextInformation = false;
            _collectScopeInformation = false;
            _logRuleTraces = false;
            _isContinousParsingEnabled = false;
        }

        /// <summary>
        /// Событие при изменении одной из настроек.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        public void InvokePropertyChanged(string propertyName)
        {
            EventHandler<PropertyChangedEventArgs> handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<PropertyChangingEventArgs> PropertyChanging;

        public bool CanChange(string propertyName)
        {
            EventHandler<PropertyChangingEventArgs> handler = PropertyChanging;
            PropertyChangingEventArgs args = new PropertyChangingEventArgs(propertyName);
            if (handler != null) handler(this, args);

            return args.CanChange;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Easis.Wfs.EasyFlow.Utils;

namespace Easis.Wfs.EasyFlow.Parsing
{
    /// <summary>
    /// Парсер языка EasyFlow;
    /// </summary>
    public class ScriptParser
    {
        private EasyFlowParser _parser;
        private ParserSettings _settings;

        /// <summary>
        /// Настройки парсера.
        /// </summary>
        public ParserSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Конструктор с настройками по умолчанию.
        /// </summary>
        public ScriptParser() : this(new ParserSettings())
        {
        }

        /// <summary>
        /// Конструктор с настройками.
        /// </summary>
        /// <param name="settings">Настройки парсинга.</param>
        public ScriptParser(ParserSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            AdoptSettings(_settings);
        }

        
        private ParseResult Parse(ICharStream inputStream)
        {
            EasyFlowLexer lexer = new EasyFlowLexer(inputStream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            EasyFlowParser parser = new EasyFlowParser(tokenStream);
            parser.Settings = Settings;
            EasyFlowParser.program_return programReturn = parser.program();

            ParseResult parseResult = programReturn.ParseResult;            

            return parseResult;
        }

        /// <summary>
        /// Парсит входной поток.
        /// </summary>
        /// <param name="inStream">Входной поток.</param>
        /// <returns>Результат разбора.</returns>
        public ParseResult Parse(Stream inStream)
        {
            return Parse(new ANTLRInputStream(inStream));
        }

        /// <summary>
        /// Парсит входную строку.
        /// </summary>
        /// <param name="scriptText">Входная строка.</param>
        /// <returns>Результат разбора.</returns>
        public ParseResult Parse(string scriptText)
        {
            return Parse(new ANTLRStringStream(scriptText));
        }

        private void AdoptSettings(ParserSettings settings)
        {
            settings.PropertyChanging += SettingsOnPropertyChanging; 
        }

        private void SettingsOnPropertyChanging(object sender, PropertyChangingEventArgs propertyChangingEventArgs)
        {
            propertyChangingEventArgs.CanChange = false;

            throw new InvalidOperationException("Невозможно изменение настроек парсера после того, как он был инициализирован.");
        }        
    }
}

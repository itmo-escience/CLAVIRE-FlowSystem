using System.Collections.Generic;
using Easis.Wfs.EasyFlow.Model;


namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Интерпретатор императивного кода
    /// 
    /// 1. Конструктор
    /// 2. InitializeScopes
    /// 3. InitializeDataContext
    /// </summary>
    public interface ICodeInterpreter : IValueGetter, IValueSetter
    {
        GlobalDataScope GetGlobalDataScope();
        BlockDataScope GetBlockDataScope();

        void InterpreteCodeSection(CodeSection codeSection);

        ValueBase EvaluateExpression(Expression expr);

        /// <summary>
        /// Интерпретация параметров Run. Необходима из-за того, что параметры могут быть выражениями.
        /// Хелпер для формирования параметров запуска степа.
        /// </summary>
        /// <param name="runParameters"></param>
        /// <returns></returns>
        IDictionary<string, ValueBase> InterpreteRunParameters(RunParameters runParameters);

        void DefineVariable(Variable variable);

        void ShareVariables(IEnumerable<Variable> variables);

        void ShareVariable(Variable variable);
        
        void ShareVariables(IEnumerable<string> identifier);
        void ShareVariable(string identifier);
    }
}
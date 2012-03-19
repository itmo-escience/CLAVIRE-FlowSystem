

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Перечисление областей кода.
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        ///   Неопределённая область.
        /// </summary>
        Undefined,
        /// <summary>
        ///  Весь скрипт.
        /// </summary>
        Script,
        /// <summary>
        ///   Главное тело потока (flow {}).
        /// </summary>
        Flow,
        /// <summary>
        ///   Заголовок трансформации (trans(...) -> (..)).
        /// </summary>
        TransHeader,
        /// <summary>
        ///   Тело трансформации.
        /// </summary>
        TransBody,
        /// <summary>
        ///   Описание шага (всё целиком).
        /// </summary>
        Step,
        /// <summary>
        ///   Заголовок описания шага.
        /// </summary>
        StepHeader,
        /// <summary>
        ///   Секция перед запуском пакета.
        /// </summary>
        StepBeforeSection,
        /// <summary>
        ///   Секция после запуска пакета.
        /// </summary>
        StepAfterSection,
        /// <summary>
        ///   Секция императивного кода (не декларативного).
        /// </summary>
        ImperativeSection,
        /// <summary>
        ///  Директива запуска (run);
        /// </summary>
        StepRunClause,
        /// <summary>
        ///   Список событий в заголовке описания шага (step S1 on ...)
        /// </summary>
        StepEventList,
        /// <summary>
        ///   Перечисление шагов после директивы AFTER.
        /// </summary>
        StepAfterList,
        /// <summary>
        ///   Имя объекта запуска (Semp.SomeMethod).
        /// </summary>
        RunObjectName,
        /// <summary>
        ///   Перечисление параметров запуска.
        /// </summary>
        RunParameters,
        /// <summary>
        ///   Директива require.
        /// </summary>
        Require,
        /// <summary>
        ///   Локатор ресурса (любого).
        /// </summary>
        ResourceLocator,                
        /// <summary>
        ///   Выражение.
        /// </summary>
        Expression,
        /// <summary>
        ///   Указатель на файл.
        /// </summary>
        FilePointer,
        /// <summary>
        ///   Доступ к переменной.
        /// </summary>
        VarIdentifier
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easis.Common.Exceptions;
using Easis.Wfs.EasyFlow.Model;
using NLog;

namespace Easis.Wfs.Interpreting
{
    /// <summary>
    /// Глобальный скоп, включающий в себя глобальные переменные, скопы блоков и текущий курсор.
    /// </summary>
    public class GlobalDataScope : DataScope
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();
        // Variables - Global vars
        // Children - steps

        // один большой лок на паблик методы
        // внимательно: если переносишь syncroot в базовый класс - убедись, что проверил GetValue
        private object _syncRoot = new object();

        public object SyncRoot { get { return _syncRoot; } }

        public GlobalDataScope()
            : base(null)
        {
        }

        public void ShareVariables(string blockName, IEnumerable<Variable> variables)
        {
            if (variables == null) throw new ArgumentNullException("variables");
            if (variables.Count() == 0) throw new ArgumentException("variables is empty");
            if (String.IsNullOrEmpty(blockName)) throw new ArgumentException("blockName");

            lock (_syncRoot)
            {
                var v = FindBlockStructureVar(blockName);

                foreach (var variable in variables)
                {
                    // NOTE: здесь создается контекст из себя, потому что переменные уже отевалены
                    v.SetValue(CompoundVarIdentifier.Concat(blockName, variable.Name), variable.Value, new SimpleEvaluationContext(this));
                    _log.Trace("Shared variable '{0}' in global data scope", blockName + "." + variable.Name);
                }
            }
        }

        /// <summary>
        /// Ищет скоп в коллекции
        /// throws ObjectNotFoundException
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        protected DataScope FindBlockScope(string blockName)
        {
            // ищем степ
            var namedBlock = from child in ChildrenScopes where ((BlockDataScope)child).Name == blockName select child;
            if (namedBlock.Count() > 0)
            {
                return namedBlock.First();
            }
            else
            {
                throw new ObjectNotFoundException(String.Format("Block {0} was not found", blockName));
            }
        }

        /// <summary>
        /// Ищет переменную по имени в коллекции
        /// throws ObjectNotFoundException
        /// </summary>
        /// <param name="blockName"></param>
        /// <returns></returns>
        protected Variable FindBlockStructureVar(string blockName)
        {
            var blockVar = from v in Variables where v.Name == blockName select v;
            if (blockVar.Count() > 0)
            {
                return blockVar.First();
            }
            else
            {
                throw new ObjectNotFoundException(String.Format("Block {0} was not found", blockName));
            }
        }

        /// <summary>
        /// Создание переменной, соответствующей степу в глобальном контексте
        /// ts
        /// </summary>
        /// <param name="blockName"></param>
        /// <param name="blockStructure"></param>
        public void CreateNamedBlockStructure(string blockName)
        {
            if (String.IsNullOrEmpty(blockName)) throw new ArgumentException("blockName");

            lock (_syncRoot)
            {
                bool wasFound = false;
                try
                {
                    var v = FindBlockStructureVar(blockName);
                    wasFound = true;
                }
                catch (ObjectNotFoundException ex)
                {
                    wasFound = false;
                }

                if (wasFound)
                    throw new InterpretionException("Duplicated block name");

                StructureValue blockStructure = new StructureValue();

                Variables.Add(new Variable(blockName, blockStructure));
            }
        }

        /// <summary>
        /// ts
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public override ValueBase GetValue(CompoundVarIdentifier identifier)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (identifier.Count() == 0) throw new ArgumentException("identifier is empty");

            lock (_syncRoot)
            {
                // ВНИМАТЕЛЬНО
                // Если сделать DataScope ts - изменить эту логику
                return base.GetValue(identifier);
            }
        }

        
        public override VariableDescriptor FindVariable(CompoundVarIdentifier identifier)
        {
            string first = identifier.First().Name;

            VariableDescriptor ret = null;

            // Ищем в переменных
            if (ret == null)
            {
                var namedVar = from variable in Variables where variable.Name == first select variable;
                if (namedVar.Count() > 0)
                {
                    // есть переменная с таким именем
                    System.Diagnostics.Debug.Assert(namedVar.Count() == 1);

                    // это блок?
                    if (namedVar.First().Value is BlockStructureValue)
                    {
                        // TODO: clone variable
                        ret = new VariableDescriptor(namedVar.First(), VarScopeLocation.AnotherBlockScope);
                    }
                    // просто глобальная переменная
                    else
                    {
                        ret = new VariableDescriptor(namedVar.First(), VarScopeLocation.GlobalScope);
                    }
                }
            }

            if (ret != null) return ret;
            else throw new ObjectNotFoundException(); ;
        }
    }
}

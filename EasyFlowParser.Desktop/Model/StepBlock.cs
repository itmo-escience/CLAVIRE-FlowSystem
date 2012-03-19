using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Блок с запуском пакета.
    /// </summary>
    public class StepBlock : BlockBase
    {
        /// <summary>
        /// Gets package run parameters.        
        /// </summary>
        public RunParameters RunParameters { get; internal set; }

        /// <summary>
        /// Gets execution parameters.
        /// </summary>
        public RunParameters ExecParameters { get; internal set; }

        public bool IsLongRunning { get; internal set; }

        private string _postCodeSection;

        /// <summary>
        /// 
        /// </summary>
        public string PostCodeSection
        {
            get { return _postCodeSection; }
            internal set
            {
                if (value.StartsWith("code"))
                    value = value.Remove(0, 4);
                value = value.TrimStart(new [] {' ', '\t'});
                if (value.StartsWith("ruby"))
                    value = value.Remove(0, 4);
                if (value.EndsWith("end"))
                    value = value.Remove(value.Length - 3, 3);
                value = value.TrimEnd(new char[] { ' ', '\t' });

                if (value.EndsWith("code"))
                    value = value.Remove(value.Length - 4, 4);

                value = value.Trim();
                _postCodeSection = value;
            }
        }

        public string PreCodeSection { get; internal set; }

        /// <summary>
        /// Имя шага.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя объекта запуска.
        /// </summary>
        public RunObjectName RunObjectName { get; set; }

        /// <summary>
        /// Секция кода перед запуском (null -- отсутствует).
        /// </summary>
        public CodeSection PreSection { get; set; }

        /// <summary>
        /// Секция кода после запуска (null -- отсутствует).
        /// </summary>
        public CodeSection PostSection { get; set; }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public StepBlock()
        {
            RunParameters = new RunParameters();
            ExecParameters = new RunParameters();
        }

        public override string ToString()
        {
            return string.Format("Step {0}({1})", Name, Id);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = (StepBlock)base.Clone();

            clone.Name = Name;
            if (PostSection != null)
                clone.PostSection = (CodeSection)PostSection.Clone();
            if (PreSection != null)
                clone.PreSection = (CodeSection)PreSection.Clone();
            if (RunObjectName != null)
                clone.RunObjectName = (RunObjectName)RunObjectName.Clone();
            if (RunParameters != null)
                clone.RunParameters = (RunParameters)RunParameters.Clone();
            if (ExecParameters != null)
                clone.ExecParameters = (RunParameters) ExecParameters.Clone();

            return clone;
        }

        protected override object CreateClone()
        {
            return new StepBlock();
        }
    }
}

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Easis.Common.DataContracts
{
    [DataContract]
    public class JobRequest
    {
        /// <summary>
        /// Текст скрипта на языке EasyFlow
        /// </summary>
        [DataMember]
        public string Script { get; set; }

        /// <summary>
        /// Контекст выполнения скрипта, включающий описание входных данных и параметров.
        /// Напр. соответствия идентификаторам входных данных ID в хранилище "semp1input = 891-289-1244-9001-023"
        /// </summary>
        [DataMember]
        public string ScriptDataContext { get; set; }

        /// <summary>
        /// Системные параметры выполнения скрипта.
        /// Напр. "только на Гриде"
        /// </summary>
        [DataMember]
        public string FlowExecutionProperties { get; set; }

        [DataMember]
        public Dictionary<string, string> ExecutionProperties { get; set; }

    }
}
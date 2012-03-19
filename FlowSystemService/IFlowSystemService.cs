using System;
using System.ServiceModel;
using Easis.Common.DataContracts;
using Eventing;

namespace Easis.Wfs.FlowSystemService
{
    [ServiceContract(Namespace = "http://escience.ifmo.ru/easis/workflows/")]
    public interface IFlowSystemService : INotifiable
    {
        /// <summary>
        /// Отправка команды на исполнение WF
        /// </summary>
        /// <param name="jobRequest">Дескриптор задания</param>
        /// <returns>Идентификатор запущенного WF</returns>
        [OperationContract(Action = "http://escience.ifmo.ru/easis/workflows/PushJob")]
        Guid PushJob(JobRequest jobRequest);
        /// <summary>
        /// Управление WF или конкретным узлом WF.
        /// </summary>
        /// <param name="action">Команда (например, pause, stop, resume)</param>
        /// <param name="WfId">Идентификатор WF</param>
        /// <param name="blockId">Идентификатор узла</param>
        [OperationContract(Action = "http://escience.ifmo.ru/easis/workflows/Control")]
        void Control(string action, Guid WfId, long blockId);
        /// <summary>
        /// Получение информации о WF
        /// </summary>
        /// <param name="wfId">Идентификатор WF</param>
        /// <returns></returns>
        [OperationContract(Action = "http://escience.ifmo.ru/easis/workflows/GetJobDescription")]
        JobDescription GetJobDescription(Guid wfId);
        /// <summary>
        /// Получение информации о исполняющихся WF.
        /// </summary>
        /// <returns>Список JobDescription</returns>
        [OperationContract(Action = "http://escience.ifmo.ru/easis/workflows/GetJobDescriptions")]
        JobDescription[] GetJobDescriptions();
        /// <summary>
        /// Получение информации о WF длительного исполнения с фильтрацией по имени пакета.
        /// </summary>
        /// <param name="packageName">Имя пакета для фильтрации</param>
        /// <returns>Массив LongRunningTaskInfo</returns>
        [OperationContract(Action = "http://escience.ifmo.ru/easis/workflows/GetLongRunningTasks")]
        LongRunningTaskInfo[] GetLongRunningTasks(string packageName);

    }
}

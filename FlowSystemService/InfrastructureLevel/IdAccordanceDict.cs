using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Easis.Wfs.FlowSystemService.Utils;
using Easis.Wfs.Interpreting;
using NLog;

namespace Easis.Wfs.FlowSystemService
{
    public class TaskAccordance
    {
        public ulong TaskId { get; set; }
        public Pair<Guid, long> WfStepId { get; set; }
        public StepRunDescriptor Descriptor { get; set; }
        public JobRunMode RunMode { get; set; }
    }

    public class IdAccordanceDict
    {
        #region Singleton
        private static volatile IdAccordanceDict _instance;
        private static readonly object _syncRoot = new Object();

        private IdAccordanceDict()
        {
            _Archieve = new Dictionary<ulong, TaskAccordance>();
        }

        public static IdAccordanceDict Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new IdAccordanceDict();
                        }
                    }
                }

                return _instance;
            }
        }
        #endregion

        // thread safe dict

        private IDictionary<ulong, TaskAccordance> _Archieve;

        public TaskAccordance GetAccordanceObject(ulong PesId)
        {
            lock (_syncRoot)
                return _Archieve[PesId];
        }

        public Pair<Guid, long> GetWfAndStepId(ulong PesId)
        {
            lock (_syncRoot)
                return _Archieve[PesId].WfStepId;
        }

        public ulong GetRealTaskId(Guid wfid, long stepid)
        {
            lock (_syncRoot)
            {
                ulong ret = _Archieve.Where(pair => pair.Value.WfStepId.First == wfid && pair.Value.WfStepId.Second == stepid).Single().Key;
                return ret;
            }
        }

        public void SaveDescriptor(ulong PesId, StepRunDescriptor descriptor)
        {
            lock (_syncRoot)
            {
                if (!_Archieve.ContainsKey(PesId))
                {
                    _Archieve[PesId] = new TaskAccordance();
                    _Archieve[PesId].TaskId = PesId;
                }
                _Archieve[PesId].Descriptor = descriptor;
            }
        }

        public StepRunDescriptor PopDescriptor(ulong PesId)
        {
            lock (_syncRoot)
            {
                StepRunDescriptor ret = _Archieve[PesId].Descriptor;
                //_Archieve.Remove(PesId);
                return ret;
            }
        }

        public StepRunDescriptor GetDescriptor(ulong PesId)
        {
            lock (_syncRoot)
            {
                return _Archieve[PesId].Descriptor;
            }
        }

        public void AddIdAccordance(Guid WfId, long StepId, ulong PesStepId, JobRunMode mode = JobRunMode.Normal)
        {
            lock (_syncRoot)
            {
                _Archieve[PesStepId] = new TaskAccordance();
                _Archieve[PesStepId].TaskId = PesStepId;
                _Archieve[PesStepId].WfStepId = new Pair<Guid, long>(WfId, StepId);
                _Archieve[PesStepId].RunMode = mode;
            }
        }

        public void RemoveAllEntriesForWf(Guid wfid)
        {
            lock (_syncRoot)
            {
                var toRemove = _Archieve.Where(pair => pair.Value.WfStepId.First == wfid).ToArray();
                foreach (var pair in toRemove)
                {
                    _Archieve.Remove(pair.Key);
                }
                WfLog log = new WfLog(LogManager.GetCurrentClassLogger(), wfid);
                log.Trace("Information about the following tasks of workflow {0} were removed from dict: {1}", wfid, String.Join(",", toRemove.Select(valuePair => valuePair.Key)));
            }
        }
    }
}
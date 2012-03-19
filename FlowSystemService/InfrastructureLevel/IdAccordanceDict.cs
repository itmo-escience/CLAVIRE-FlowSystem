using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Easis.Wfs.FlowSystemService.Utils;
using Easis.Wfs.Interpreting;

namespace Easis.Wfs.FlowSystemService
{
    public class IdAccordanceDict
    {
        #region Singleton
        private static volatile IdAccordanceDict _instance;
        private static readonly object _syncRoot = new Object();
        
        private IdAccordanceDict()
        {
            _idArchieve = new Dictionary<ulong, Pair<Guid,long>>();
            _descriptors = new Dictionary<ulong, StepRunDescriptor>();

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
        private IDictionary<ulong, Pair<Guid,long>> _idArchieve;
        private IDictionary<ulong, StepRunDescriptor> _descriptors; 

        public Pair<Guid,long> GetWfAndStepId(ulong PesId)
        {
            lock (_syncRoot)
                return _idArchieve[PesId];
        }

        public ulong GetRealId(Guid wfid, long stepid)
        {
            lock (_syncRoot)
            {
                ulong ret = _idArchieve.Where(pair => pair.Value.First == wfid && pair.Value.Second == stepid).Single().Key;
                return ret;
            }
        }

        public void SaveDescriptor(ulong PesId, StepRunDescriptor descriptor)
        {
            lock (_syncRoot)
            {
                _descriptors.Add(PesId, descriptor);
            }
        }

        public StepRunDescriptor PopDescriptor(ulong PesId)
        {
            lock (_syncRoot)
            {
                StepRunDescriptor ret = _descriptors[PesId];
                _descriptors.Remove(PesId);
                return ret;
            }
        }

        public StepRunDescriptor GetDescriptor(ulong PesId)
        {
            lock (_syncRoot)
            {
                return _descriptors[PesId];
            }
        }

        public void AddIdTriplet(Guid WfId, long StepId, ulong PesStepId)
        {
            lock (_syncRoot)
                _idArchieve.Add(PesStepId, new Pair<Guid, long>(WfId,StepId));
        }
    }
}
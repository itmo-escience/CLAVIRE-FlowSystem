using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easis.Wfs.FlowSystemService
{
    public interface IIdGenerator<T>
    {
        T NextId();
    }
}
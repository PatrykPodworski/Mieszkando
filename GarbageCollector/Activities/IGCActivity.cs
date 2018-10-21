using System;
using System.Collections.Generic;
using System.Text;

namespace GarbageCollector.Activities
{
    public interface IGCActivity
    {
        GCActivityStatus Perform();
    }
}

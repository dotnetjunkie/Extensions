using System;
using System.Collections.Generic;
using System.Text;

namespace MockHostTypes
{
    public class Host : IHost
    {
        public IServiceProvider Services { get; }
    }
}

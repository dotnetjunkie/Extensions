using System;
using System.Collections.Generic;
using System.Text;

namespace MockHostTypes
{
    public class WebHost : IWebHost
    {
        public IServiceProvider Services { get; }
    }
}

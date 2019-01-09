using System;
using System.Collections.Generic;
using System.Text;

namespace MockHostTypes
{
    public class HostBuilder : IHostBuilder
    {
        public IHost Build() => new Host();
    }
}

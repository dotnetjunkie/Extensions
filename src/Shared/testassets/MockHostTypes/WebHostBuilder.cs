using System;
using System.Collections.Generic;
using System.Text;

namespace MockHostTypes
{
    public class WebHostBuilder : IWebHostBuilder
    {
        public IWebHost Build() => new WebHost();
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class Container
    {
        public static ServiceProvider DIContainer {  get; private set; }

        public static void SetContainer(ServiceProvider container)
        {
            DIContainer = container;
        }
    }
}

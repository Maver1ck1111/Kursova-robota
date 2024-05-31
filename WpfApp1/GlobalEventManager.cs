using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public static class GlobalEventManager
    {
        public static Action OnAlgorithmStarted;
        public static Action OnAlgorithmEnded;
        public static void SendAlgorithmIsRunning() => OnAlgorithmStarted?.Invoke();
        public static void SendOnAlgoritmEnded() => OnAlgorithmEnded?.Invoke();
    }
}

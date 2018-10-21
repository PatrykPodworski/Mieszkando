using GarbageCollector.Modules;
using Ninject;

namespace GarbageCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel(new GarbageCollectorModule());
            var gc = kernel.Get<GarbageCollectorService>();
            gc.Run();
        }
    }
}

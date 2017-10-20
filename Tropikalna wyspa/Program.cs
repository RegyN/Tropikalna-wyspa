using System;

namespace Tropikalna_wyspa
{
#if WINDOWS || LINUX

    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var game = new DemoWyspa();
            game.Run();
            game.Exit();
            game.Dispose();
        }
    }
#endif
}

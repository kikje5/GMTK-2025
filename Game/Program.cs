using System;

namespace GMTK2025;

public static class Program
{
    [STAThread]
    static void Main()
    {
        using (var game = new App())
            game.Run();
    }
}

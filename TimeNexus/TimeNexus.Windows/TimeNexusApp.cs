using SiliconStudio.Xenko.Engine;

namespace TimeNexus
{
    class TimeNexusApp
    {
        static void Main(string[] args)
        {
            using (var game = new Game())
            {
                game.Run();
            }
        }
    }
}
using Stride.Engine;

namespace MvuTest
{
    class MvuTestApp
    {
        static void Main(string[] args)
        {
            using (var game = new MVU.Game.GameCompositionRoot())
            {
                game.Run();
            }
        }
    }
}

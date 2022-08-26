using Stride.Engine;

namespace MyGame
{
    class MyGameApp
    {
        static void Main(string[] args)
        {
            using (var game = new MyGame.Core.MvuGame())
            {
                game.Run();
            }
        }
    }
}

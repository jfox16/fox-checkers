using System;

namespace FoxCheckers
{
    // Program starts here!
    class Program
    {
        static void Main(string[] args)
        {
            GameController gameController = new GameController();

            // This is the game loop, it keeps running until user quits the game.
            while(true) 
            {
                if (!gameController.ShowPrompt())
                {
                    Console.WriteLine("Thank you for playing!");
                    System.Environment.Exit(1);
                }
            }
        }
    }
}

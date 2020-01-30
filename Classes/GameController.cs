using System;
using System.Collections.Generic;

namespace FoxCheckers
{
    // GameController is used to communicate between the user and the game.
    // It shows game state to the user and prompts user for input.
    class GameController
    {
        // VARIABLES ==============================================================================
        
        Game currentGame;
        Random random;



        // LIFECYCLE ==============================================================================

        public GameController()
        {
            currentGame = new Game(); // Start up a new game
            random = new Random(); // Make a new randomizer
        }

        // Displays the prompt for the current game state to the user.
        // Returns true to continue the game, returns false to quit.
        public bool ShowPrompt()
        {
            switch(currentGame.state) 
            {
                case Game.State.Start:
                    ShowStartPrompt();
                    break;

                case Game.State.Running:
                    ShowRunningPrompt();
                    break;

                case Game.State.Over:
                    ShowOverPrompt();
                    break;
            }

            return GetContinueInput();
        }



        // PROMPT METHODS =========================================================================

        void ShowStartPrompt()
        {
            Console.WriteLine("\n === Welcome to Fox Checkers! === \n");
            currentGame.Start();
            currentGame.Update();
            currentGame.PrintGrid();
        }

        void ShowRunningPrompt()
        {
            if (currentGame.playerTurn == 0) 
            {
                ShowPlayerTurnPrompt();
            }
            else {
                DoAiTurn();
            }
            currentGame.Update();
            currentGame.PrintGrid();
        }

        void ShowOverPrompt()
        {
            Console.WriteLine(currentGame.gameOverMessage);
            Console.WriteLine(" - GAME OVER - ");
            currentGame.state = Game.State.Start;
        }

        void ShowPlayerTurnPrompt()
        {
            Console.WriteLine("Your turn!");

            // The player chooses from a list of actions. 
            // By default, this list contains the player's moves. But if 
            // the player has jumps available it uses jumps instead.
            List<Action> actions = currentGame.players[0].moves;
            List<Action> jumps = currentGame.players[0].jumps;
            if (jumps.Count > 0) actions = jumps;
            
            // Display actions to user
            for (int i = 0; i < actions.Count; i++)
            {
                Console.WriteLine((i+1) + ": " + actions[i]);
            }

            // Get player's choice as an index
            int choiceIndex;
            while (true)
            {
                if (TryGetIntInput(1, actions.Count+1, out choiceIndex)) break;
            }

            Console.WriteLine("Your Move: " + actions[choiceIndex-1]);
            currentGame.DoAction(actions[choiceIndex-1]);
        }

        void DoAiTurn()
        {
            Console.WriteLine("Opponent's Turn.");

            List<Action> actions = currentGame.players[1].moves;
            List<Action> jumps = currentGame.players[1].jumps;
            // If jumps are available, use jumps instead of moves.
            if (jumps.Count > 0) actions = jumps;

            int choiceIndex = random.Next(0, actions.Count);

            Console.WriteLine("Opponent's Move: " + actions[choiceIndex]);
            currentGame.DoAction(actions[choiceIndex]);
        }



        // PLAYER INPUT METHODS ===================================================================

        static string GetStringInput()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }

        static bool GetContinueInput()
        {
            Console.WriteLine("Press Enter to continue. (or Q to quit)");
            return (GetStringInput().ToLower() != "q");
        }

        static bool TryGetIntInput(int min, int max, out int result)
        {
            if (Int32.TryParse(GetStringInput(), out result)) 
            {
                return (result >= min && result < max);
            }
            else
            {
                Console.WriteLine("Invalid input, please try again.");
                return false;
            }
        }
    }
}
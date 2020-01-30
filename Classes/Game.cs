using System;
using System.Linq;
using System.Collections.Generic;
using Utility;

namespace FoxCheckers
{
    struct Player
    {
        public char symbol; // Symbol representing player on the printed grid
        public List<GamePiece> gamePieces; // List of game pieces belonging to the player
        public List<Action> moves; // List of valid moves available to the player
        public List<Action> jumps; // List of valid jumps available to the player
    }

    // This holds all the data for the current game being played.
    class Game 
    {
        // VARIABLES ==============================================================================

        public enum State {Start, Running, Over}
        public State state = State.Start;

        // grid is a 2D array that represents the game board.
        GamePiece[,] grid;

        // players contains data for each player in the game. Player 0 is the user, player 1 is the AI.
        public Player[] players;
        const int numPlayers = 2;

        public int playerTurn = 1;

        const string layoutFilePath = "./Maps/layout.txt";

        public string gameOverMessage;



        // LIFECYCLE METHODS ======================================================================

        public void Start() 
        {
            Console.WriteLine("Setting up new game...");

            // Initialize players.
            players = new Player[numPlayers];
            for (int i = 0; i < numPlayers; i++) 
            {
                Player player = new Player();
                player.gamePieces = new List<GamePiece>();
                players[i] = player;
            }

            players[0].symbol = 'O';
            players[1].symbol = 'X';

            BuildGrid();

            state = State.Running;
        }

        public void Update()
        {
            // Check moves and jumps for this turn's player.
            players[playerTurn].moves = CheckMoves(players[playerTurn].gamePieces);
            players[playerTurn].jumps = CheckJumps(players[playerTurn].gamePieces);

            CheckGameOver();
        }

        public void CheckGameOver()
        {
            // If the current turn player cannot make a move, they are the loser.
            if (players[playerTurn].moves.Count == 0 && players[playerTurn].jumps.Count == 0)
            {
                if (playerTurn == 0)
                    gameOverMessage = "YOU LOSE! THE ROBOTS ARE VICTORIOUS. HUMANITY IS LOST";
                else
                    gameOverMessage = "Congratulations! You won!";

                state = State.Over;
            }
        }



        // ACTION CHECKS ==========================================================================

        public static List<Action> CheckMoves(List<GamePiece> pieces)
        {
            List<Action> moves = new List<Action>();

            // iterate through pieces and add their moves to the list of moves.
            foreach (GamePiece piece in pieces)
                moves = moves.Concat(piece.GetMoves()).ToList();

            return moves;
        }

        public static List<Action> CheckJumps(List<GamePiece> pieces)
        {
            List<Action> jumps = new List<Action>();

            // iterate through pieces and add their valid jumps to the list of jumps.
            foreach (GamePiece piece in pieces)
                jumps = jumps.Concat(piece.GetJumps()).ToList();

            return jumps;
        }



        // GRID METHODS ===========================================================================

        // Prints the text representation of the current grid.
        public void PrintGrid()
        {
            Console.WriteLine();
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                // Print the row number to the left
                Console.Write((y+1) + "│");
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    Console.Write(' ');
                    if (grid[y,x] != null) {
                        int player = grid[y,x].player;
                        Console.Write(players[player].symbol);
                    }
                    else {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }

            // Print column numbers on the bottom
            Console.Write(" └");
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Console.Write("──");
            }
            Console.WriteLine();
            Console.Write("   ");
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Console.Write(Functions.IntToCheckerLetter(x) + " ");
            }
            Console.WriteLine("\n");
        }

        public GamePiece GetPieceAtPosition(Vector2 position)
        {
            return grid[position.y, position.x];
        }

        public bool CheckIfPositionIsEmpty(Vector2 position)
        {
            return (CheckPositionInBounds(position) && GetPieceAtPosition(position) == null);
        }

        public bool CheckIfPositionIsEnemyPiece(Vector2 position, int myPlayer)
        {
            if (!CheckPositionInBounds(position)) return false;
            GamePiece enemyPiece = GetPieceAtPosition(position);
            if (enemyPiece == null) return false;
            return (enemyPiece.player != myPlayer);
        }

        public void DoAction(Action action)
        {
            GamePiece piece = GetPieceAtPosition(action.startPosition);
            piece.position = action.endPosition;
            SetPieceAtPosition(action.startPosition, null);
            SetPieceAtPosition(action.endPosition, piece);

            if (action is Jump)
            {
                Jump jump = (Jump) action;
                foreach (GamePiece jumpedPiece in jump.piecesJumped)
                {
                    SetPieceAtPosition(jumpedPiece.position, null);

                    // Remove the piece from its player's list of pieces
                    players[jumpedPiece.player].gamePieces.Remove(jumpedPiece);  
                }
            }

            playerTurn = (playerTurn + 1) % numPlayers;
            Update();
        } 

        // Build grid and place pieces using data from layout file.
        void BuildGrid()
        {
            if (!System.IO.File.Exists(layoutFilePath)) 
            {
                Console.Error.WriteLine("File could not be found at " + layoutFilePath);
                return;
            }

            string[] rows = System.IO.File.ReadAllLines(layoutFilePath);

            grid = new GamePiece[rows[0].Length, rows.Length];

            // Look for pieces and add them to their players' lists according to their number
            for (int y = 0; y < rows.Length; y++)
            {
                string row = rows[y];

                for (int x = 0; x < row.Length; x++)
                {
                    int player = (int)Char.GetNumericValue(row[x]);

                    if (player >= 0 && player < numPlayers)
                    {
                        GamePiece newPiece = new GamePiece(this, player, x, y);
                        grid[y,x] = newPiece;
                        players[player].gamePieces.Add(newPiece);
                    }
                }
            }
        }

        bool CheckPositionInBounds(Vector2 position)
        {
            return (
                position.x >= 0 && 
                position.y >= 0 &&
                position.x < grid.GetLength(1) &&
                position.y < grid.GetLength(0)
            );
        }

        void SetPieceAtPosition(Vector2 position, GamePiece piece)
        {
            grid[position.y, position.x] = piece;
        }
    }
}
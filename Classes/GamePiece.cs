using System;
using System.Collections.Generic;
using Utility;

namespace FoxCheckers
{
    // Represents a checker piece on the game board.
    class GamePiece 
    {
        Game currentGame;
        public int player; // This is the number corresponding to the player that owns this piece
        public Vector2 position;
        public Vector2[] actionDirections; // These are the directions that this piece can move/jump

        public GamePiece(Game currentGame, int player, int x, int y) 
        {
            this.currentGame = currentGame;
            this.player = player;
            this.position = new Vector2(x, y);

            // Player 0 can only go up, player 1 can only go down
            int yDirection = (player == 0) ? -1 : 1;
            actionDirections = new Vector2[] {
                new Vector2(-1, yDirection),
                new Vector2(1, yDirection)
            };
        }

        // GetMoves returns a List of all valid moves that this piece can do.
        public List<Action> GetMoves()
        {
            List<Action> moves = new List<Action>();

            // Check each possible direction and see if it is a valid move.
            foreach (Vector2 actionDirection in actionDirections)
            {
                Vector2 endPosition = Vector2.Add(position, actionDirection);

                if (currentGame.CheckIfPositionIsEmpty(endPosition)) {
                    moves.Add(new Move(position, endPosition));
                }
            }

            return moves;   
        }

        // GetJumps returns a List of all valid jumps that this piece can do.
        public List<Action> GetJumps()
        {
            List<Action> jumps = new List<Action>();
            List<Vector2> jumpPositions = new List<Vector2>();
            jumpPositions.Add(position);

            CheckJumpsFromPosition(position, jumpPositions, new List<GamePiece>(), jumps);

            return jumps;
        }

        // CheckJumpsFromPosition is a recursive function to find valid jumps and multi-jumps
        // that this piece can perform. Uses backtracking with a running list of 
        // jumpPositions and piecesJumped and adds valid jumps to jumps.
        void CheckJumpsFromPosition(
            Vector2 currentPosition, 
            List<Vector2> jumpPositions, 
            List<GamePiece> piecesJumped, 
            List<Action> jumps) 
        {
            bool noMoreJumps = true;

            foreach (Vector2 actionDirection in actionDirections)
            {
                Vector2 enemyPosition = Vector2.Add(currentPosition, actionDirection);
                Vector2 endPosition = Vector2.Add(enemyPosition, actionDirection);

                if (
                    currentGame.CheckIfPositionIsEnemyPiece(enemyPosition, player) && 
                    currentGame.CheckIfPositionIsEmpty(endPosition)
                ) {
                    // Found next jump position!
                    noMoreJumps = false;
                    jumpPositions.Add(endPosition);
                    piecesJumped.Add(currentGame.GetPieceAtPosition(enemyPosition));
                    // Check at this new position
                    CheckJumpsFromPosition(endPosition, jumpPositions, piecesJumped, jumps);
                    // Backtrack by removing the stuff we just added
                    jumpPositions.RemoveAt(jumpPositions.Count-1);
                    piecesJumped.RemoveAt(piecesJumped.Count-1);
                }
            }

            if (jumpPositions.Count >= 2 && noMoreJumps)
            {
                // This is a valid jump! Add it to jumps.
                jumps.Add(new Jump(new List<Vector2>(jumpPositions), new List<GamePiece>(piecesJumped)));
            }
        }
    }
}
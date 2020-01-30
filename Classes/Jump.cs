using System.Collections.Generic;
using Utility;

namespace FoxCheckers
{
    class Jump : Action
    {
        public List<Vector2> jumpPositions;
        public List<GamePiece> piecesJumped;

        public Jump(List<Vector2> jumpPositions, List<GamePiece> piecesJumped)
        {
            this.jumpPositions = jumpPositions;
            this.startPosition = jumpPositions[0];
            this.endPosition = jumpPositions[jumpPositions.Count-1];
            this.piecesJumped = piecesJumped;
        }

        public override string ToString()
        {
            string result = Functions.Vector2ToCheckerCoordinate(jumpPositions[0]);

            for(int i = 1; i < jumpPositions.Count; i++)
            {
                result += " -> " + Functions.Vector2ToCheckerCoordinate(jumpPositions[i]);
            }

            return result;
        }
    }
}
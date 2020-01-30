using Utility;

namespace FoxCheckers
{
    class Move : Action
    {

        public Move(Vector2 startPosition, Vector2 endPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;
        }

        public override string ToString()
        {
            return Functions.Vector2ToCheckerCoordinate(startPosition) + " -> " + 
                Functions.Vector2ToCheckerCoordinate(endPosition);
        }
    }
}
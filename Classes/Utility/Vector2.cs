namespace Utility
{
    class Vector2 
    {
        public int x = 0;
        public int y = 0;

        public Vector2(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 Add(Vector2 v1, Vector2 v2)
        {
            return new Vector2(
                v1.x + v2.x,
                v1.y + v2.y
            );
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }
    }
}
namespace GameOfLife
{
    public static class Game
    {
        public static bool[,] GetNextState(bool[,] currentState)
        {
            var width = currentState.GetLength(0);
            var height = currentState.GetLength(1);
            var result = new bool[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                result[x, y] = GetNewCellValue(currentState, width, height, x, y);
            }

            return result;
        }

        public static bool[,] GetInitialState()
        {
            var result = new bool[100, 100];
            result[50, 50] = result[51, 50] = result[52, 50] = true;
            return result;
        }

        private static bool GetNewCellValue(bool[,] currentState, int width, int height, int x, int y)
        {
            var neighbourCount = 0;
            for (var x0 = x - 1; x0 <= x + 1; x0++)
            for (var y0 = y - 1; y0 <= y + 1; y0++)
            {
                if (x == x0 && y == y0) continue;
                if (InBounds(x0, y0, width, height) && currentState[x0, y0]) neighbourCount++;
            }

            if (neighbourCount < 2 || neighbourCount > 3) return false;
            if (neighbourCount == 2) return currentState[x, y];
            return true;
        }

        private static bool InBounds(int x, int y, int width, int height)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
    }
}
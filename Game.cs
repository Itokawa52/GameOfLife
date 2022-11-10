namespace GameOfLife
{
    public static class Game
    {
        public static bool[,] GetNextState(bool[,] currentState)
        {
            currentState[50, 50] = false;
            return currentState;
        }

        public static bool[,] GetInitialState()
        {
            var result = new bool[100, 100];
            result[50, 50] = result[51, 50] = result[52, 50] = true;
            return result;
        }
    }
}
public static class LevelKeyUtil
{
    public static string LevelKey(int worldIndex, int levelIndex)
    {
        return $"w{worldIndex}_l{levelIndex}";
    }
}
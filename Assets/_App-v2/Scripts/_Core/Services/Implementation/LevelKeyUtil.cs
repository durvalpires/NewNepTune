public static class LevelKeyUtil
{
    public static string LevelKey(int worldIndex, int levelIndex)
    {
        return $"w{worldIndex}_l{levelIndex}";
    }
    
    public static string LevelKey(int worldIndex, string levelIndex)
    {
        return $"w{worldIndex}_l" + levelIndex;
    }
    
    public static string LevelKey(string worldIndex, int levelIndex)
    {
        return worldIndex + $"_l{levelIndex}";
    }
}
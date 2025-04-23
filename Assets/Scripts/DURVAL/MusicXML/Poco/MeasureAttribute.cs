public struct MeasureAttribute
{
    public int? Divisions { get; set; }
    public MusicalTime? Time { get; set; }
    public Clef? Clef { get; set; }
}

public struct Clef
{
    public string Sign { get; set; }
    public int Line { get; set; }
}

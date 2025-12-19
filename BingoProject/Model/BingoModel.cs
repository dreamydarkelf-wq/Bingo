public class BingoModel
{
    public const int Size = 5;

    public string[][] Items { get; set; } = new string[5][];
    public bool[][] Selected { get; set; } = new bool[5][];

    public BingoModel()
    {
        for (int i = 0; i < 5; i++)
        {
            Items[i] = new string[5];
            Selected[i] = new bool[5];
        }
    }
}
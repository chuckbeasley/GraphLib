namespace GraphLib;

public class DistantOriginal
{
    public int Distance { get; set; }
    public int ParentVertex { get; set; }

    public DistantOriginal(int pv, int d)
    {
        Distance = d;
        ParentVertex = pv;
    }
}

public struct Info_Zomb
{
    public string gusto;
    public string apodo;
    public int años;
}
public struct Civitasinfo
{
    public string apodo;
    public int años;

    static public implicit operator Info_Zomb(Civitasinfo c)
    {
        Info_Zomb zomb = new Info_Zomb();
        zomb.gusto = "cabeza";
        zomb.años = c.años;
        zomb.apodo = "Zombie " + c.apodo;
        return zomb;
    }
}

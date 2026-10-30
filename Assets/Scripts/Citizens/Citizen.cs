public class Citizen
{
    public string Name { get; }
    public int Money { get; private set; }
    public IHousing Home { get; private set; }
    public bool HasHome => Home != null;

    public Citizen(string name, int startingMoney = 50)
    {
        Name = name;
        Money = startingMoney;
    }

    public bool MoveInto(IHousing housing)
    {
        if (HasHome || housing == null) return false;
        if (!housing.AddResident(this)) return false;
        Home = housing;
        return true;
    }
}
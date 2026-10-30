public class Citizen
{
    public string Name { get; }
    public int Money { get; private set; }
    public IHousing Home { get; private set; }
    public bool HasHome => Home != null;
    public IEmployer Job { get; private set; }
    public bool IsEmployed => Job != null;

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

    public bool TryGetJob(IEmployer employer)
    {
        if (IsEmployed || employer == null) return false;
        if (!employer.Hire(this)) return false;
        Job = employer;
        return true;
    }
}
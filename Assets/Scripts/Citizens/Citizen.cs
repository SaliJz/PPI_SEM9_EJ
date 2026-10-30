public class Citizen
{
    public string Name { get; }
    public int Money { get; private set; }

    public Citizen(string name, int startingMoney = 50)
    {
        Name = name;
        Money = startingMoney;
    }
}
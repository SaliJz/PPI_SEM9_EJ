public class Shop : Workplace
{
    private void Reset()
    {
        buildingName = "Tienda";
        cost = 300;
        maxJobs = 3;
        salary = 25;
    }

    protected override int GenerateIncome() => filledJobs * 40;
}
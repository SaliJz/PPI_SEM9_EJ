public class Farm : Workplace
{
    private void Reset()
    {
        buildingName = "Granja";
        cost = 250;
        maxJobs = 4;
        salary = 15;
    }

    protected override int GenerateIncome() => filledJobs * 25;
}
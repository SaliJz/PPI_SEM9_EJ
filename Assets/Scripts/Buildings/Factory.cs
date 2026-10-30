public class Factory : Workplace
{
    private void Reset()
    {
        buildingName = "Fábrica";
        cost = 600;
        maxJobs = 8;
        salary = 35;
    }

    protected override int GenerateIncome() => filledJobs * 60;
}
using UnityEngine;

public abstract class Workplace : Building, IEmployer
{
    [SerializeField] protected int maxJobs = 5;
    [SerializeField] protected int salary = 20;
    protected int filledJobs;

    public int MaxJobs => maxJobs;
    public int FilledJobs => filledJobs;
    public int Salary => salary;
    public bool HasVacancy => filledJobs < maxJobs;
    public int LastIncome { get; private set; }

    // Cada tipo de trabajo genera ingresos distintos
    protected abstract int GenerateIncome();

    public override void OnTick()
    {
        LastIncome = GenerateIncome();
    }
}
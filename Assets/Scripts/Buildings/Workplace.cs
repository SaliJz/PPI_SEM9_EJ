using System.Collections.Generic;
using UnityEngine;

public abstract class Workplace : Building, IEmployer
{
    [SerializeField] protected int maxJobs = 5;
    [SerializeField] protected int salary = 20;
    protected readonly List<Citizen> employees = new List<Citizen>();

    protected int filledJobs => employees.Count;

    public int MaxJobs => maxJobs;
    public int FilledJobs => employees.Count;
    public int Salary => salary;
    public bool HasVacancy => employees.Count < maxJobs;
    public int LastIncome { get; private set; }

    // Cada tipo de trabajo genera ingresos distintos
    protected abstract int GenerateIncome();

    public bool Hire(Citizen citizen)
    {
        if (citizen == null || !HasVacancy || employees.Contains(citizen)) return false;
        employees.Add(citizen);
        return true;
    }

    public override void OnTick()
    {
        LastIncome = GenerateIncome();
        if (LastIncome > 0) EconomyManager.Instance.Earn(LastIncome);
    }
}
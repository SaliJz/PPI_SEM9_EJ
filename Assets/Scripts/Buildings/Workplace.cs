using System;
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
    public Func<Citizen, float> SalaryFormula { get; set; }

    protected virtual void Awake()
    {
        // Salario base + 5% por cada punto de experiencia
        SalaryFormula = c => salary * (1f + c.Experience * 0.05f);
    }

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

        PaySalaries();
    }

    private void PaySalaries()
    {
        foreach (var worker in employees)
        {
            int pay = Mathf.RoundToInt(SalaryFormula(worker));
            if (EconomyManager.Instance.TrySpend(pay))
            {
                worker.ReceiveSalary(pay);
            }
        }
    }
}
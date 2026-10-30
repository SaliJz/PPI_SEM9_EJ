using System;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 1000;

    public int Money { get; private set; }
    public TaxCalculator TaxRule { get; set; }
    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Money = startingMoney;
        TaxRule = FlatTax;
        OnMoneyChanged?.Invoke(Money);  
    }

    public void Earn(int amount)
    {
        if (amount <= 0) return;
        Money += amount;
        OnMoneyChanged?.Invoke(Money);
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0 || !CanAfford(amount)) return false;
        Money -= amount;
        OnMoneyChanged?.Invoke(Money);
        return true;
    }

    public bool CanAfford(int amount) => Money >= amount;

    // Define la "forma" de una regla de impuestos
    public delegate int TaxCalculator(Citizen citizen);

    // Reglas intercambiables
    private int FlatTax(Citizen c) => Mathf.RoundToInt(c.Money * 0.1f);

    public int ProgressiveTax(Citizen c)
    {
        if (c.Money > 500) return Mathf.RoundToInt(c.Money * 0.2f);
        if (c.Money > 100) return Mathf.RoundToInt(c.Money * 0.1f);
        return 0;
    }

    public int CollectTaxes(IEnumerable<Citizen> citizens)
    {
        if (TaxRule == null) return 0;

        int total = 0;
        foreach (var c in citizens)
        {
            int tax = TaxRule(c);
            if (tax > 0 && c.TrySpend(tax)) total += tax;
        }

        if (total > 0) Earn(total);
        return total;
    }
}
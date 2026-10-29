using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private int startingMoney = 1000;

    public int Money { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Money = startingMoney;
    }

    public void Earn(int amount)
    {
        if (amount <= 0) return;
        Money += amount;
    }

    public bool CanAfford(int amount) => Money >= amount;

    // Devuelve false si no hay fondos suficientes
    public bool TrySpend(int amount)
    {
        if (amount <= 0 || !CanAfford(amount)) return false;
        Money -= amount;
        return true;
    }
}
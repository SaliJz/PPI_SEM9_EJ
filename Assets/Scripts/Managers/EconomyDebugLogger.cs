using UnityEngine;

public class EconomyDebugLogger : MonoBehaviour
{
    private void OnEnable()
    {
        EconomyManager.Instance.OnMoneyChanged += HandleMoneyChanged;
    }

    private void OnDisable()
    {
        if (EconomyManager.Instance != null) EconomyManager.Instance.OnMoneyChanged -= HandleMoneyChanged;
    }

    private void HandleMoneyChanged(int newMoney)
    {
        Debug.Log($"Dinero actual: ${newMoney}");
    }

    [ContextMenu("Test Earn 100")]
    private void TestEarn() => EconomyManager.Instance.Earn(100);

    [ContextMenu("Test Spend 300")]
    private void TestSpend() => EconomyManager.Instance.TrySpend(300);
}
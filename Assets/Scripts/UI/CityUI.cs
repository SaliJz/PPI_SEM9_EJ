using TMPro;
using UnityEngine;

public class CityUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private CitizenManager citizenManager;

    private void Start()
    {
        var economy = EconomyManager.Instance;
        economy.OnMoneyChanged += UpdateMoney;
        UpdateMoney(economy.Money);

        citizenManager.OnCitizenAdded += HandleCitizenAdded;
        UpdatePopulation();

        TimeManager.Instance.OnDayPassed += UpdateDay;
        UpdateDay(TimeManager.Instance.Day);
    }

    private void OnDestroy()
    {
        if (EconomyManager.Instance != null)
        {
            EconomyManager.Instance.OnMoneyChanged -= UpdateMoney;
        }

        if (citizenManager != null)
        {
            citizenManager.OnCitizenAdded -= HandleCitizenAdded;
        }

        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayPassed -= UpdateDay;
        }
    }

    private void HandleCitizenAdded(Citizen c) => UpdatePopulation();
    private void UpdatePopulation() => populationText.text = $"Población: {citizenManager.Population}";
    private void UpdateDay(int day) => dayText.text = $"Día {day}";
    private void UpdateMoney(int money) => moneyText.text = $"Dinero: ${money}";
}
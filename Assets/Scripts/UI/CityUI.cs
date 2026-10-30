using TMPro;
using UnityEngine;
using System.Collections;

public class CityUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    [SerializeField] private TMP_Text populationText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private CitizenManager citizenManager;

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_Text selectedText;
    [SerializeField] private BuildingPlacer placer;

    private Coroutine messageRoutine;

    private void Start()
    {
        var economy = EconomyManager.Instance;
        economy.OnMoneyChanged += UpdateMoney;
        UpdateMoney(economy.Money);

        citizenManager.OnCitizenAdded += HandleCitizenAdded;
        UpdatePopulation();

        TimeManager.Instance.OnDayPassed += UpdateDay;
        UpdateDay(TimeManager.Instance.Day);

        placer.OnPlacementFailed += ShowMessage;
        placer.OnBuildingPlaced += HandleBuildingPlaced;
        placer.OnSelectionChanged += UpdateSelected;
        UpdateSelected(null);
        messageText.text = "";
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

        if (placer != null)
        {
            placer.OnPlacementFailed -= ShowMessage;
            placer.OnBuildingPlaced -= HandleBuildingPlaced;
            placer.OnSelectionChanged -= UpdateSelected;
        }
    }

    private void HandleBuildingPlaced(Building b) => ShowMessage($"Construiste: {b.BuildingName}");

    private void UpdateSelected(Building b) =>
        selectedText.text = b == null ? "Sin selección" : $"Construyendo: {b.GetInfo()}";

    private void ShowMessage(string msg)
    {
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageRoutine = StartCoroutine(MessageRoutine(msg));
    }

    private IEnumerator MessageRoutine(string msg)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(2f);
        messageText.text = "";
    }

    // Métodos para los botones (OnClick)
    public void TogglePause() =>
        TimeManager.Instance.SetPaused(!TimeManager.Instance.IsPaused);

    public void SetSpeed(float speed) => TimeManager.Instance.SetSpeed(speed);
    private void HandleCitizenAdded(Citizen c) => UpdatePopulation();
    private void UpdatePopulation() => populationText.text = $"Población: {citizenManager.Population}";
    private void UpdateDay(int day) => dayText.text = $"Día {day}";
    private void UpdateMoney(int money) => moneyText.text = $"Dinero: ${money}";
}
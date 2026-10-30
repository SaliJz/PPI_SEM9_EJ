using System;
using System.Collections.Generic;
using UnityEngine;

public class CitizenManager : MonoBehaviour
{
    private static readonly string[] Names =
        { "Ana", "Luis", "María", "Carlos", "Sofía", "Diego", "Lucía", "Jorge", "Valeria", "Pedro" };

    [SerializeField] private int arrivalsPerDay = 2;
    [SerializeField] private BuildingPlacer placer;

    private readonly List<Citizen> citizens = new List<Citizen>();
    public IReadOnlyList<Citizen> Citizens => citizens;
    public int Population => citizens.Count;
    public event Action<Citizen> OnCitizenAdded;

    private void Start()
    {
        TimeManager.Instance.OnDayPassed += HandleDayPassed;

        if (placer != null) placer.OnBuildingPlaced += HandleBuildingPlaced;
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayPassed -= HandleDayPassed;
        }

        if (placer != null) placer.OnBuildingPlaced -= HandleBuildingPlaced;
    }

    private void HandleBuildingPlaced(Building building)
    {
        // Si se construyó un lugar de trabajo, contrata a los desempleados
        if (building is Workplace) AssignJobsToUnemployed();
    }

    private void AssignJobsToUnemployed()
    {
        foreach (var citizen in citizens)
        {
            if (citizen.IsEmployed) continue;
            var workplace = FindWorkplaceWithVacancy();
            if (workplace == null) break;
            citizen.TryGetJob(workplace);
        }
    }

    private void HandleDayPassed(int day)
    {
        // Llega gente nueva solo si hay casas con espacio
        for (int i = 0; i < arrivalsPerDay; i++)
        {
            if (SpawnCitizen() == null) break;
        }

        int taxes = EconomyManager.Instance.CollectTaxes(citizens);
        Debug.Log($"Día {day}: impuestos ${taxes} | Población: {Population}");
    }

    public Citizen SpawnCitizen()
    {
        // Solo llega gente si hay casa libre
        var house = FindHouseWithRoom();
        if (house == null) return null;

        var citizen = new Citizen(Names[UnityEngine.Random.Range(0, Names.Length)]);
        citizen.MoveInto(house);

        var workplace = FindWorkplaceWithVacancy();
        if (workplace != null) citizen.TryGetJob(workplace);

        TimeManager.Instance.Register(citizen);
        citizens.Add(citizen);
        OnCitizenAdded?.Invoke(citizen);
        return citizen;
    }

    private IHousing FindHouseWithRoom()
    {
        foreach (var h in FindObjectsByType<House>(FindObjectsSortMode.None))
        {
            if (h.HasRoom) return h;
        }
        return null;
    }

    private IEmployer FindWorkplaceWithVacancy()
    {
        foreach (var w in FindObjectsByType<Workplace>(FindObjectsSortMode.None))
        {
            if (w.HasVacancy) return w;
        }
        return null;
    }

    [ContextMenu("Debug: Spawn Citizen")]
    private void DebugSpawn() => SpawnCitizen();
}
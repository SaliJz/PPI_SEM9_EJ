using System.Collections.Generic;
using UnityEngine;

public class House : Building, IHousing
{
    [SerializeField] private int capacity = 4;
    private readonly List<Citizen> residents = new List<Citizen>();

    public int Capacity => capacity;
    public int Occupants => residents.Count;
    public bool HasRoom => residents.Count < capacity;
    public IReadOnlyList<Citizen> Residents => residents;

    private void Reset()
    {
        buildingName = "Casa";
        cost = 200;
    }

    public bool AddResident(Citizen citizen)
    {
        if (citizen == null || !HasRoom) return false;
        residents.Add(citizen);
        return true;
    }

    public override void OnTick() { }
}
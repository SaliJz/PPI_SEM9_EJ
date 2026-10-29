using UnityEngine;

public class House : Building, IHousing
{
    [SerializeField] private int capacity = 4;
    private int occupants;

    public int Capacity => capacity;
    public int Occupants => occupants;
    public bool HasRoom => occupants < capacity;

    private void Reset()
    {
        buildingName = "Casa";
        cost = 200;
    }

    public override void OnTick()
    {

    }
}
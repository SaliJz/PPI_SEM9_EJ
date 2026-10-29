using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField] protected string buildingName = "Building";
    [SerializeField] protected int cost = 100;

    public string BuildingName => buildingName;
    public int Cost => cost;

    // Cada edificio decide qué hace en cada tick
    public abstract void OnTick();

    public virtual string GetInfo() => $"{buildingName} (${cost})";
}
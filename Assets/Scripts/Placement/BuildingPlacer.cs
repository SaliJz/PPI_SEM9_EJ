using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Building[] buildingPrefabs;
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private float yOffset = 0.5f; // para cubos de 1 de alto

    private Building selected;
    private readonly Plane ground = new Plane(Vector3.up, Vector3.zero);

    public Building Selected => selected;
    public event Action<string> OnPlacementFailed;
    public event Action<Building> OnBuildingPlaced;
    public event Action<Building> OnSelectionChanged;
    private readonly HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    public void SelectByIndex(int index)
    {
        if (index < 0 || index >= buildingPrefabs.Length) return;
        selected = buildingPrefabs[index];
        OnSelectionChanged?.Invoke(selected);
    }

    public void Deselect()
    {
        selected = null;
        OnSelectionChanged?.Invoke(null);
    }

    private void Update()
    {
        if (selected == null) return;

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            Deselect();
            return;
        }

        // No construir si el clic fue sobre un botón de la UI
        if (Input.GetMouseButtonDown(0) &&
            !(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()))
        {
            TryPlace();
        }
    }

    private void TryPlace()
    {
        if (!TryGetCell(out Vector2Int cell)) return;

        if (occupied.Contains(cell))
        {
            OnPlacementFailed?.Invoke("Esa celda ya está ocupada");
            return;
        }

        if (!EconomyManager.Instance.TrySpend(selected.Cost))
        {
            OnPlacementFailed?.Invoke($"Dinero insuficiente: necesitas ${selected.Cost}");
            return;
        }

        occupied.Add(cell);
        Building placed = Instantiate(selected, CellToWorld(cell), Quaternion.identity);
        OnBuildingPlaced?.Invoke(placed);
    }

    private bool TryGetCell(out Vector2Int cell)
    {
        cell = default;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!ground.Raycast(ray, out float distance)) return false;

        Vector3 point = ray.GetPoint(distance);
        cell = new Vector2Int(
            Mathf.RoundToInt(point.x / cellSize),
            Mathf.RoundToInt(point.z / cellSize));
        return true;
    }

    private Vector3 CellToWorld(Vector2Int cell) =>
        new Vector3(cell.x * cellSize, yOffset, cell.y * cellSize);
}
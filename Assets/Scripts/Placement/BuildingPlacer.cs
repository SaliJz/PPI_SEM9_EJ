using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Building[] buildingPrefabs;
    [SerializeField] private float cellSize = 2f;
    [SerializeField] private float yOffset = 0.5f; // para cubos de 1 de alto

    private Building selected;
    private readonly Plane ground = new Plane(Vector3.up, Vector3.zero);

    public Building Selected => selected;

    public void SelectByIndex(int index)
    {
        if (index < 0 || index >= buildingPrefabs.Length) return;
        selected = buildingPrefabs[index];
    }

    private void Update()
    {
        if (selected == null) return;
        if (Input.GetMouseButtonDown(0)) TryPlace();
    }

    private void TryPlace()
    {
        if (!TryGetCell(out Vector2Int cell)) return;
        Instantiate(selected, CellToWorld(cell), Quaternion.identity);
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
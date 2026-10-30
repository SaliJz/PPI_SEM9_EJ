public interface IHousing
{
    int Capacity { get; }
    int Occupants { get; }
    bool HasRoom { get; }
    bool AddResident(Citizen citizen);
}
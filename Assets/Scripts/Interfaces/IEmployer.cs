public interface IEmployer
{
    int MaxJobs { get; }
    int FilledJobs { get; }
    int Salary { get; }
    bool HasVacancy { get; }
}
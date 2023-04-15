namespace DevTeam.Tools.Swapper;

public class Project
{
    public string PackageId { get; set; } = null!;
    public string ProjectPath { get; set; } = null!;
}

public class SwapperConfig
{
    public string SolutionPath { get; set; } = null!;
    public string? BaseProjectPath { get; set; }
    public List<Project> Projects { get; set; } = new();
}

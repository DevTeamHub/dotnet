using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Xml;

namespace DevTeam.Tools.Swapper;

public class ProjectService
{
    private const string ProjectNodeKey = "ProjectReference";
    private const string PackageNodeKey = "PackageReference";
    private const string IncludeAttribute = "Include";
    private const string VersionAttribute = "Version";

    private readonly SwapperConfig _options;

    public ProjectService(
        IOptions<SwapperConfig> options)
    {
        _options = options.Value;

        _options.Projects.ForEach(project =>
        {
            project.ProjectPath = @$"{_options.BaseProjectPath}\{project.ProjectPath}";
        });
    }

    private async Task ExecuteCLI(string arguments)
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = "dotnet",
            Arguments = arguments,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        var process = Process.Start(startInfo);

        ArgumentNullException.ThrowIfNull(process);

        await process.WaitForExitAsync();
    }

    private Task AddProjectToSolution(string project)
    {
        return ExecuteCLI($"sln {_options.SolutionPath} add {project} -s Swapper"); 
    }

    private Task RemoveProjectFromSolution(string project)
    {
        return ExecuteCLI($"sln {_options.SolutionPath} remove {project}");
    }

    private void SwapNodes(string swapFrom, string swapTo, Dictionary<string, string> lookup)
    {
        var solutionPath = Path.GetDirectoryName(_options.SolutionPath) ?? AppContext.BaseDirectory;
        var projectFiles = Directory.GetFiles(solutionPath, "*.csproj", SearchOption.AllDirectories);

        foreach (var projectFile in projectFiles)
        {
            var document = new XmlDocument();
            document.Load(projectFile);

            var nodes = document.SelectNodes($"//{swapFrom}")!
                .Cast<XmlNode>()
                .Select(x => new 
                { 
                    Include = x.Attributes![IncludeAttribute], 
                    Version = x.Attributes![VersionAttribute], 
                    Node = x 
                })
                .Where(x => x.Include != null && x.Version != null)
                .Select(x => new XmlProjectNode(x.Include!.Value, x.Version!.Value, x.Node))
                .Where(x => lookup.ContainsKey(x.Id))
                .ToList();

            if (!nodes.Any()) continue;

            nodes.ForEach(item => 
            {
                var parentNode = item.Node.ParentNode!;
                parentNode.RemoveChild(item.Node);

                var newNode = document.CreateElement(swapTo);
                newNode.SetAttribute(IncludeAttribute, lookup[item.Id]);
                newNode.SetAttribute(VersionAttribute, item.Version);
                parentNode.AppendChild(newNode);
            });

            document.Save(projectFile);
        }
    }

    public async Task SwapToProjects()
    {
        var lookup = _options.Projects.ToDictionary(x => x.PackageId, x => x.ProjectPath);

        foreach (var project in _options.Projects)
        {
            await AddProjectToSolution(project.ProjectPath);
        }

        SwapNodes(PackageNodeKey, ProjectNodeKey, lookup);
    }

    public async Task SwapToPackages()
    {
        var lookup = _options.Projects.ToDictionary(x => x.ProjectPath, x => x.PackageId);

        foreach (var project in _options.Projects)
        {
            await RemoveProjectFromSolution(project.ProjectPath);
        }

        SwapNodes(ProjectNodeKey, PackageNodeKey, lookup);
    }
}

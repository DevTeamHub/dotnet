using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.CommandLine;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace DevTeam.Tools.Swapper;

public class SwapperService : IHostedService
{
    private readonly SwapperConfig _options;

    public SwapperService(IOptions<SwapperConfig> options)
    {
        _options = options.Value;
    }

    public void SwapToProjects()
    {

    }

    public void SwapToPackages()
    {

    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var projectFiles = Directory.GetFiles(_options.SolutionPath, "*.csproj", SearchOption.AllDirectories);

        var all = _options.Projects.ToDictionary(x => x.PackageId, x => x.ProjectPath);

        foreach (var projectFile in projectFiles)
        {
            var doc = new XmlDocument();
            doc.Load(projectFile);

            var packages = doc.SelectNodes("//PackageReference")
                .Cast<XmlNode>()
                .Select(x => new { Name = x.Attributes["Include"].Value, Node = x })
                .Where(x => _options.Projects.Any(p => p.PackageId == x.Name))
                .ToList();

            foreach (var package in packages)
            {
                var commentContents = package.Node.OuterXml;
                var commentNode = doc.CreateComment(commentContents);
                var parentNode = package.Node.ParentNode;
                parentNode.ReplaceChild(commentNode, package.Node);

                var newNode = doc.CreateElement("ProjectReference");
                newNode.SetAttribute("Include", @$"{all[package.Name]}");

                parentNode.AppendChild(newNode);
            }

            doc.Save(projectFile);
        }

        Console.WriteLine("Done");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

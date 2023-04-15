using Microsoft.Extensions.Options;

namespace DevTeam.Tools.Swapper;

public class SwapperService
{
    private readonly CommandLineArgs _args;
    private readonly ProjectService _projectService;

    public SwapperService(
        ProjectService projectService,
        IOptions<CommandLineArgs> args)
    {
        _args = args.Value;
        _projectService = projectService;
    }

    public async Task Execute()
    {
        switch (_args.To)
        {
            case "projects":
                await _projectService.SwapToProjects();
                break;
            case "packages":
                await _projectService.SwapToPackages();
                break;
        }
    }
}

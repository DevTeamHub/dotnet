using DevTeam.Permissions.Core;
using DevTeam.Extensions.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTeam.Permissions;

public class PermissionsFilter<TRelatedData> : IAsyncActionFilter
    where TRelatedData : RelatedData
{
    private readonly ILogger<PermissionsFilter<TRelatedData>> _logger;
    private readonly IPermissionsService _permissionsService;
    private readonly IRelatedDataService<TRelatedData> _relatedDataService;
    private readonly IServiceProvider _serviceProvider;

    private readonly List<int> _permissions;

    public PermissionsFilter(
        ILogger<PermissionsFilter<TRelatedData>> logger, 
        IServiceProvider serviceProvider,
        IPermissionsService permissionsService,
        IRelatedDataService<TRelatedData> relatedDataService,
        List<int> permissions)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _permissionsService = permissionsService;
        _relatedDataService = relatedDataService;
        _permissions = permissions;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requestIds = RequestHelper.GetIdsFromRequest(context);

        var relatedData = requestIds
            .GroupBy(x => x.Type, x => x.Id)
            .Select(async x => await GetRelatedDataByType(x.Key, x.ToList()))
            .SelectMany(x => x.Result)
            .ToList();

        var permissions = _permissionsService.GetCurrentAccountPermissions();

        var permissionsToCheck = _permissions
            .Select(permissionId => permissions.FirstOrDefault(x => x.Id == permissionId) ?? new PermissionModel() { Id = permissionId }) // need fix, returns null and skips the next check 
            .ToList();

        var isAllowed = HasAllRelatedData(requestIds, relatedData)
                     && HasAllPermissions(permissionsToCheck)
                     && HasAccessToEveryEntity(permissionsToCheck, relatedData)
                     && AreInTheSameContainer(permissionsToCheck, relatedData);

        if (!isAllowed)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            return;
        }

        await next();
    }

    private bool HasAllRelatedData(List<RequestId> requestIds, List<TRelatedData> relatedData)
    {
        var hasAllData = requestIds.Count == relatedData.Count;

        if (!hasAllData)
        {
            var userId = _permissionsService.GetCurrentAccountId();
            var message = $@"Access to the action denied. Couldn't find some of the related data for Ids in the request. User Id: {userId}";
            _logger.LogWarning(message);
        }

        return hasAllData;
    }

    private bool HasAllPermissions(List<PermissionModel> permissions)
    {
        var missedPermision = permissions.FirstOrDefault(x => x.Name == null); // need fix

        if (missedPermision != null)
        {
            var userId = _permissionsService.GetCurrentAccountId();
            var message = $@"Access to the action denied. User doesn't have a permission. User Id: {userId}, {missedPermision.Id}";
            _logger.LogWarning(message);
        }

        return missedPermision == null;
    }

    public bool HasAccessToEveryEntity(List<PermissionModel> permissions, List<TRelatedData> relatedData)
    {
        if (!relatedData.Any()) return true;

        var validators = PermissionsValidators.Validators
            .Select(_serviceProvider.GetService)
            .Cast<BasePermissionsValidator<TRelatedData>>()
            .ToList();

        var isAllowed = permissions.All(permission =>
        {
            var relatedDataForPermission = relatedData.Where(x => x.EntityType == permission.EntityType).ToList();

            if (!relatedDataForPermission.Any()) return true; 

            var isAllowed = validators.All(validator =>
            {
                if ((permission.Scopes & validator.Scope) != 0)
                {
                    var isScopeAllowed = validator.IsAllowed(relatedDataForPermission);

                    if (!isScopeAllowed)
                    {
                        var userId = _permissionsService.GetCurrentAccountId();
                        var message = $@"Access to the action denied. User doesn't have access to the entity. UserId: {userId}, EntityType: {permission.EntityType}, EntityId: {permission.Id}, Scope: {validator.Scope}";
                        _logger.LogWarning(message);
                        return false;
                    }
                }

                return true;
            });

            return isAllowed;
        });

        return isAllowed;
    }

    private bool AreInTheSameContainer(List<PermissionModel> permissions, List<TRelatedData> relatedData)
    {
        if (!relatedData.Any()) return true;

        var containersToCheck = permissions
            .Where(x => x != null && x.ContainerType.HasValue)
            .Select(x => x.ContainerType!.Value)
            .Distinct()
            .ToList();

        var isAllowed = containersToCheck.All(containerType =>
        {
            var containerIds = relatedData.Select(x => x.GetIdsByType(containerType)).ToList();

            var sharedEntities = containerIds.Skip(1).Aggregate(containerIds.First(), (acc, current) => acc.Intersect(current).ToList());
            var isAllowed = sharedEntities.Any();

            if (!isAllowed)
            {
                var entities = string.Join("; ", permissions.Select(x => $@"EntityId: {x.Id}, EntityType: {x.EntityType}"));
                var message = $@"Entities don't belong to the same container: ContainerType: {containerType}; Entities: {entities}";
                _logger.LogWarning(message);
            }

            return isAllowed;
        });

        return isAllowed;
    }

    public Task<List<TRelatedData>> GetRelatedDataByType(Type type, List<int> requestIds)
    {
        return (Task<List<TRelatedData>>)typeof(PermissionsFilter<TRelatedData>)
            .GetMethod(nameof(GetRelatedData))
            .MakeGenericMethod(type)
            .Invoke(this, new[] { requestIds });
    }

    public Task<List<TRelatedData>> GetRelatedData<TEntity>(List<int> requestIds)
        where TEntity : class, IEntity, new()
    {
        return _relatedDataService.GetRelatedData<TEntity>(requestIds);
    }
}

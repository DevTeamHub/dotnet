using DevTeam.Extensions.Abstractions;
using DevTeam.GenericService.Pagination;
using DevTeam.Permissions.Core;
using DevTeam.Permissions.Core.Attributes;
using DevTeam.QueryMappings.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevTeam.Permissions
{
    public interface IAccessService<TRelatedData>
        where TRelatedData : RelatedData
    {
        bool CanPerform(TRelatedData relatedData, int permission);

        TModel SetupAccess<TModel>(TModel model)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity;

        TViewModel SetupAccess<TModel, TViewModel>(TModel model)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity;

        TViewModel SetupAccess<TModel, TViewModel, TArgs>(TModel model, TArgs args)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity;

        List<TViewModel> SetupAccess<TModel, TViewModel>(List<TModel> models)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity;

        List<TViewModel> SetupAccess<TModel, TViewModel, TArgs>(List<TModel> models, TArgs args)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity;

        PaginationModel<TModel> SetupAccess<TModel>(PaginationModel<TModel> models)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity;

        PaginationModel<TViewModel> SetupAccess<TModel, TViewModel>(PaginationModel<TModel> models)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity;

        PaginationModel<TViewModel> SetupAccess<TModel, TViewModel, TArgs>(PaginationModel<TModel> models, TArgs args)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity;
    }

    public class AccessService<TRelatedData>: IAccessService<TRelatedData>
        where TRelatedData : RelatedData
    {
        private readonly IPermissionsService _permissionsService;
        private readonly IMappingService _mappingService;
        private readonly IServiceProvider _serviceProvider;

        public AccessService(
            IServiceProvider serviceProvider,
            IPermissionsService permissionsService,
            IMappingService mappingService)
        {
            _permissionsService = permissionsService;
            _mappingService = mappingService;
            _serviceProvider = serviceProvider;
        }

        public bool CanPerform(TRelatedData relatedData, int permission)
        {
            var permissionsInfo = _permissionsService.GetCurrentAccountPermissions();
            var permissionToCheck = permissionsInfo.FirstOrDefault(x => x.Id == permission);

            if (permissionToCheck == null) return false;

            if (!permissionToCheck.Scopes.HasValue) return true;

            var scopes = permissionToCheck.Scopes.Value;

            var validators = PermissionsValidators.Validators
                .Select(_serviceProvider.GetService)
                .Cast<BasePermissionsValidator<TRelatedData>>()
                .ToList();

            var isAllowed = validators.All(validator =>
            {
                if ((scopes & validator.Scope) != 0)
                {
                    return validator.IsAllowed(new() { relatedData });
                }

                return true;
            });

            return isAllowed;
        }

        public TModel SetupAccess<TModel>(TModel model)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
        {
            return SetupAccess(model, model);
        }

        public TViewModel SetupAccess<TModel, TViewModel>(TModel model)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity
        {
            var viewModel = _mappingService.Map<TModel, TViewModel>(model);

            return SetupAccess(model, viewModel);
        }

        public TViewModel SetupAccess<TModel, TViewModel, TArgs>(TModel model, TArgs args)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel: IEntity
        {
            var viewModel = _mappingService.Map<TModel, TViewModel, TArgs>(model, args);

            return SetupAccess(model, viewModel);
        }

        public List<TViewModel> SetupAccess<TModel, TViewModel>(List<TModel> models)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity
        {
            var viewModels = _mappingService.Map<TModel, TViewModel>(models);

            return SetupAccess(models, viewModels);
        }

        public List<TViewModel> SetupAccess<TModel, TViewModel, TArgs>(List<TModel> models, TArgs args)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity
        {
            var viewModels = _mappingService.Map<TModel, TViewModel, TArgs>(models, args);

            return SetupAccess(models, viewModels);
        }

        public PaginationModel<TModel> SetupAccess<TModel>(PaginationModel<TModel> models)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
        {
            var list = SetupAccess(models.List, models.List);
            return new PaginationModel<TModel>(list, models.Count);
        }

        public PaginationModel<TViewModel> SetupAccess<TModel, TViewModel>(PaginationModel<TModel> models)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity
        {
            var viewModels = SetupAccess<TModel, TViewModel>(models.List);
            return new PaginationModel<TViewModel>(viewModels, models.Count);
        }

        public PaginationModel<TViewModel> SetupAccess<TModel, TViewModel, TArgs>(PaginationModel<TModel> models, TArgs args)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity
        {
            var viewModels = SetupAccess<TModel, TViewModel, TArgs>(models.List, args);
            return new PaginationModel<TViewModel>(viewModels, models.Count);
        }

        private List<TViewModel> SetupAccess<TModel, TViewModel>(List<TModel> models, List<TViewModel> viewModels)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity
        {
            var relatedData = models.ToDictionary(x => x.Id, x => x.RelatedData);

            var properties = typeof(TViewModel).GetProperties()
                .Select(p => new { Info = p, Attribute = p.GetCustomAttribute<CanPerformAttribute>() })
                .Where(p => p.Attribute != null)
                .ToList();

            viewModels.ForEach(model =>
            {
                var data = relatedData[model.Id];

                properties.ForEach(property =>
                {
                    var canPerform = CanPerform(data, property.Attribute.Permission);
                    property.Info.SetValue(model, canPerform);
                });
            });

            return viewModels;
        }

        private TViewModel SetupAccess<TModel, TViewModel>(TModel model, TViewModel viewModel)
            where TModel : IRelatedDataContainer<TRelatedData>, IEntity
            where TViewModel : IEntity
        {
            var properties = typeof(TModel).GetProperties()
                .Select(p => new { Info = p, Attribute = p.GetCustomAttribute<CanPerformAttribute>() })
                .Where(p => p.Attribute != null)
                .ToList();

            var data = model.RelatedData;

            properties.ForEach(property =>
            {
                var canPerform = CanPerform(data, property.Attribute.Permission);
                property.Info.SetValue(viewModel, canPerform);
            });

            return viewModel;
        }
    }
}

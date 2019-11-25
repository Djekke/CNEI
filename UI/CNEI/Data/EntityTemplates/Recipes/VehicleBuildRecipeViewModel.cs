namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Misc;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.CoreMod.Vehicles;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class VehicleBuildRecipeViewModel : RecipeViewModel
    {
        public override string ResourceDictionaryName => "VehicleBuildRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Vehicle Build";

        /// <summary>
        /// Constructor for vehicle creation recipe.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="vehicleViewModel">View Model of IProtoVehicle.</param>
        /// <param name="vehicle"></param>
        public VehicleBuildRecipeViewModel([NotNull] ProtoVehicleViewModel vehicleViewModel, IProtoVehicle vehicle)
            : base(vehicleViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Build constructor used before all entity VMs sets.");
            }

            InputItemsList = vehicle.BuildRequiredItems
                .Select(item => EntityViewModelsManager.GetEntityViewModel(item.ProtoItem))
                .ToList();

            InputItemsVMList = vehicle.BuildRequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count))
                .ToList().AsReadOnly();

            BuildRequiredElectricityAmount = vehicle.BuildRequiredElectricityAmount;

            OutputItemsList.Add(vehicleViewModel);

            VehicleVM = vehicleViewModel;

            StationsList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoVehicleAssemblyBay>()
                .AsReadOnly();
            ListedInTechNodes = vehicleViewModel.ListedInTechNodes;
        }

        public ProtoEntityViewModel VehicleVM { get; }

        public IReadOnlyList<BaseViewModel> InputItemsVMList { get; protected set; }

        public uint BuildRequiredElectricityAmount { get; }
    }
}
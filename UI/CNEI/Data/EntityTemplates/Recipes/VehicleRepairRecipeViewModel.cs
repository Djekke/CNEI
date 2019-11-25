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

    public class VehicleRepairRecipeViewModel : RecipeViewModel
    {
        public override string ResourceDictionaryName => "VehicleRepairRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Vehicle Repair";

        /// <summary>
        /// Constructor for vehicle repair recipe.
        /// Must be used only in InitEntityRelationships phase.
        /// </summary>
        /// <param name="vehicleViewModel">View Model of IProtoVehicle.</param>
        /// <param name="vehicle"></param>
        public VehicleRepairRecipeViewModel([NotNull] ProtoVehicleViewModel vehicleViewModel, IProtoVehicle vehicle)
            : base(vehicleViewModel.ProtoEntity)
        {
            if (!EntityViewModelsManager.EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Build constructor used before all entity VMs sets.");
            }

            InputItemsList = vehicle.RepairStageRequiredItems
                .Select(item => EntityViewModelsManager.GetEntityViewModel(item.ProtoItem))
                .ToList();

            InputItemsVMList = vehicle.RepairStageRequiredItems
                .Select(item => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(item.ProtoItem),
                    item.Count))
                .ToList().AsReadOnly();

            RepairRequiredElectricityAmount = vehicle.RepairRequiredElectricityAmount;

            RepairAmount = 100 / vehicle.RepairStagesCount;

            VehicleVM = vehicleViewModel;

            StationsList = EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoVehicleAssemblyBay>()
                .AsReadOnly();
            ListedInTechNodes = vehicleViewModel.ListedInTechNodes;
        }

        public ProtoEntityViewModel VehicleVM { get; }

        public IReadOnlyList<BaseViewModel> InputItemsVMList { get; protected set; }

        public int RepairAmount { get; }

        public uint RepairRequiredElectricityAmount { get; }
    }
}
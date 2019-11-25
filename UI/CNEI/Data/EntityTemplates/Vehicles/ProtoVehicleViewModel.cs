namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Vehicles;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoVehicleViewModel : ProtoDynamicWorldObjectViewModel
    {
        public override string ResourceDictionaryName => "ProtoVehicleDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "Vehicles/";

        public ProtoVehicleViewModel([NotNull] IProtoVehicle vehicle) : base(vehicle)
        {
            Description = vehicle.Description;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoVehicle vehicle)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Cargo items slots count",
                    vehicle.CargoItemsSlotsCount));
                EntityInformation.Add(new ViewModelEntityInformation("Energy max",
                    vehicle.EnergyMax));
                EntityInformation.Add(new ViewModelEntityInformation("Energy use (idle)",
                    vehicle.EnergyUsePerSecondIdle));
                EntityInformation.Add(new ViewModelEntityInformation("Energy use (moving)",
                    vehicle.EnergyUsePerSecondMoving));
                EntityInformation.Add(new ViewModelEntityInformation("Is heavy vehicle",
                    vehicle.IsHeavyVehicle ? "Yes" : "No"));
                EntityInformation.Add(new ViewModelEntityInformation("Player can use items while on it",
                    vehicle.IsPlayersHotbarAndEquipmentItemsAllowed ? "Yes" : "No"));
            }
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoVehicle vehicle)
            {
                ListedInTechNodes = vehicle.ListedInTechNodes
                    .Select(EntityViewModelsManager.GetEntityViewModel)
                    .ToList().AsReadOnly();

                EntityViewModelsManager.AddRecipe(new VehicleBuildRecipeViewModel(this, vehicle));

                RepairRecipeVM = new VehicleRepairRecipeViewModel(this, vehicle);
                EntityViewModelsManager.AddRecipe(RepairRecipeVM);
            }
        }

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; private set; } =
            new List<ProtoEntityViewModel>();

        public RecipeViewModel RepairRecipeVM { get; private set; }
    }
}
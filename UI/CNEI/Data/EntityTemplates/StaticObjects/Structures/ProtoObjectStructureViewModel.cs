namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.Systems.PowerGridSystem;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectStructureViewModel : ProtoStaticWorldObjectViewModel
    {
        public override string ResourceDictionaryName => "ProtoObjectStructureDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "StaticObjects/Structures/";

        public ProtoObjectStructureViewModel([NotNull] IProtoObjectStructure structure) : base(structure)
        {
            Description = structure.Description;
            DescriptionUpgrade = structure.DescriptionUpgrade;

            IsAutoUnlocked = structure.IsAutoUnlocked;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoObjectStructure structure)
            {
                ListedInTechNodes = structure.ListedInTechNodes
                    .Select(EntityViewModelsManager.GetEntityViewModel)
                    .ToList().AsReadOnly();

                if (structure.ConfigBuild.IsAllowed &&
                    structure.ConfigBuild != null &&
                    structure.ConfigBuild.StageRequiredItems.Count > 0)
                {
                    EntityViewModelsManager.AddRecipe(new StructureBuildRecipeViewModel(this, structure.ConfigBuild));
                }

                if (structure.ConfigUpgrade != null)
                {
                    foreach (var upgradeEntry in structure.ConfigUpgrade.Entries)
                    {
                        EntityViewModelsManager.AddRecipe(new StructureUpgradeRecipeViewModel(this, upgradeEntry));
                    }
                }
            }
        }

        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoObjectElectricityConsumer protoConsumer)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Electricity consumtion rate",
                    protoConsumer.ElectricityConsumptionPerSecondWhenActive));
            }

            if (ProtoEntity is IProtoObjectElectricityProducer protoProducer)
            {
                //protoProducer.SharedGetElectricityProduction(null, out var _, out var maxProduction);
                EntityInformation.Add(new ViewModelEntityInformation("Max electricity production rate",
                    "??"));
            }

            if (ProtoEntity is IProtoObjectElectricityStorage protoStorage)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Electricity capacity",
                    protoStorage.ElectricityCapacity));
            }
        }

        public string DescriptionUpgrade { get; }

        public bool IsAutoUnlocked { get; }

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; private set; } =
            new List<ProtoEntityViewModel>();
    }
}
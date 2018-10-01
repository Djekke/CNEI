namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using System.Collections.Generic;
    using System.Linq;

    public class ProtoObjectStructureViewModel : ProtoStaticWorldObjectViewModel
    {
        private IProtoObjectStructure structure = null;

        public ProtoObjectStructureViewModel(IProtoObjectStructure structure) : base(structure)
        {
            this.structure = structure;
            this.Description = structure.Description;
            this.DescriptionUpgrade = structure.DescriptionUpgrade;

            this.IsAutoUnlocked = structure.IsAutoUnlocked;
            //structure.IsInteractableObject
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel{IProtoEntity}" /> and
        /// <see cref="EntityViewModelsManager.GetAllEntityViewModels{}" />.
        /// </summary>
        public override void InitEntityRelationships()
        {
            if (this.structure == null)
            {
                return;
            }

            this.ListedInTechNodes = structure.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();

            if (structure.ConfigBuild.IsAllowed)
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this, structure.ConfigBuild));
            }

            foreach (var upgradeEntry in structure.ConfigUpgrade.Entries)
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this, upgradeEntry));
            }

            //structure.ConfigRepair
        }

        public string Description { get; }

        public string DescriptionUpgrade { get; }

        public bool IsAutoUnlocked { get; }

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; private set; }

        public RecipeViewModel BuildRecipe { get; }

        public RecipeViewModel UpgradeRecipe { get; }
    }
}
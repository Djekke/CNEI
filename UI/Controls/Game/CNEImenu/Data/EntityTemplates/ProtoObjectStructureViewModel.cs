namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Linq;

    public class ProtoObjectStructureViewModel : ProtoStaticWorldObjectViewModel
    {
        private readonly IProtoObjectStructure structure;

        public ProtoObjectStructureViewModel([NotNull] IProtoObjectStructure structure) : base(structure)
        {
            this.structure = structure;
            Description = structure.Description;
            DescriptionUpgrade = structure.DescriptionUpgrade;

            IsAutoUnlocked = structure.IsAutoUnlocked;
            ListedInTechNodes = new List<ProtoEntityViewModel>();
            //structure.IsInteractableObject
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (structure == null)
            {
                return;
            }

            ListedInTechNodes = structure.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();

            if (structure.ConfigBuild.IsAllowed &&
                structure.ConfigBuild != null &&
                structure.ConfigBuild.StageRequiredItems.Count > 0)
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this, structure.ConfigBuild));
            }

            if (structure.ConfigUpgrade != null)
            {
                foreach (var upgradeEntry in structure.ConfigUpgrade.Entries)
                {
                    EntityViewModelsManager.AddRecipe(new RecipeViewModel(this, upgradeEntry));
                }
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
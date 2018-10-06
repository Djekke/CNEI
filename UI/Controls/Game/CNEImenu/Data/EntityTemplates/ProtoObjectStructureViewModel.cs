namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public class ProtoObjectStructureViewModel : ProtoStaticWorldObjectViewModel
    {
        private readonly IProtoObjectStructure structure;

        public override string ResourceDictonaryName => "ProtoObjectStructureDataTemplate.xaml";

        public ProtoObjectStructureViewModel([NotNull] IProtoObjectStructure structure) : base(structure)
        {
            this.structure = structure;
            Description = structure.Description;
            DescriptionUpgrade = structure.DescriptionUpgrade;

            IsAutoUnlocked = structure.IsAutoUnlocked;
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

        /// <summary>
        /// Finalize Recipe Link creation and prepare recipe VM list to observation.
        /// </summary>
        public override void FinalizeRecipeLinking()
        {
            base.FinalizeRecipeLinking();
            if (RecipeVMList.EntityCount == 0)
            {
                RecipesVisibility = Visibility.Collapsed;
                IsRecipesExpanded = false;
            }
            if (UsageVMList.EntityCount == 0)
            {
                UsageVisibility = Visibility.Collapsed;
                IsUsageExpanded = false;
            }
            RecipePrevPage = new ActionCommand(() => RecipeVMList.PrevPage());
            RecipeNextPage = new ActionCommand(() => RecipeVMList.NextPage());
            UsagePrevPage = new ActionCommand(() => UsageVMList.PrevPage());
            UsageNextPage = new ActionCommand(() => UsageVMList.NextPage());
        }

        public BaseCommand RecipePrevPage { get; private set; }

        public BaseCommand RecipeNextPage { get; private set; }

        public BaseCommand UsagePrevPage { get; private set; }

        public BaseCommand UsageNextPage { get; private set; }

        public string Description { get; }

        public string DescriptionUpgrade { get; }

        public bool IsAutoUnlocked { get; }

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; private set; } =
            new List<ProtoEntityViewModel>();

        public Visibility RecipesVisibility { get; private set; } = Visibility.Visible;

        public Visibility UsageVisibility { get; private set; } = Visibility.Visible;

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;

        public bool IsUsageExpanded { get; set; } = true;
    }
}
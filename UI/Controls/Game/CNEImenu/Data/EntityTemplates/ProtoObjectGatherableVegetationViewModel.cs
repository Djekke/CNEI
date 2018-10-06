namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;
    using System.Windows;

    public class ProtoObjectGatherableVegetationViewModel : ProtoObjectVegetationViewModel
    {
        private readonly IProtoObjectGatherableVegetation gatherableVegetation;

        public override string ResourceDictonaryName => "ProtoObjectGatherableVegetationDataTemplate.xaml";

        public ProtoObjectGatherableVegetationViewModel([NotNull] IProtoObjectGatherableVegetation gatherableVegetation)
            : base(gatherableVegetation)
        {
            this.gatherableVegetation = gatherableVegetation;
            //vegetation.GrowthStagesCount
            //vegetation.GetGrowthStageDurationSeconds() ??

            //vegetation.IsAutoDestroyOnGather
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (gatherableVegetation == null)
            {
                return;
            }

            if (gatherableVegetation.DroplistOnDestroy != null &&
                gatherableVegetation.DroplistOnDestroy.EnumerateAllItems().Any())
            {
                DroplistOnDestroy = new RecipeViewModel(this,
                    gatherableVegetation.DroplistOnDestroy.EnumerateAllItems());
                DroplistOnDestroyVisibility = Visibility.Visible;
                EntityViewModelsManager.AddRecipe(DroplistOnDestroy);
            }

            if (gatherableVegetation.GatherDroplist != null &&
                gatherableVegetation.GatherDroplist.EnumerateAllItems().Any())
            {
                GatherDroplist = new RecipeViewModel(this,
                    gatherableVegetation.GatherDroplist.EnumerateAllItems());
                GatherDroplistVisibility = Visibility.Visible;
                EntityViewModelsManager.AddRecipe(GatherDroplist);
            }
        }

        public RecipeViewModel DroplistOnDestroy { get; private set; }

        public Visibility DroplistOnDestroyVisibility { get; private set; } = Visibility.Collapsed;

        public RecipeViewModel GatherDroplist { get; private set; }

        public Visibility GatherDroplistVisibility { get; private set; } = Visibility.Collapsed;

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsDroplistOnDestroyExpanded { get; set; } = true;

        public bool IsGatherDroplistExpanded { get; set; } = true;
    }
}
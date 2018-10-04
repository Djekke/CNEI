namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoObjectGatherableVegetationViewModel : ProtoObjectVegetationViewModel
    {
        private readonly IProtoObjectGatherableVegetation gatherableVegetation;

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
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    gatherableVegetation.DroplistOnDestroy.EnumerateAllItems()));
            }

            if (gatherableVegetation.GatherDroplist != null &&
                gatherableVegetation.GatherDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    gatherableVegetation.GatherDroplist.EnumerateAllItems()));
            }
        }
    }
}
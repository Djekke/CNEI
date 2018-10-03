namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoObjectGatherableVegetationViewModel : ProtoObjectVegetationViewModel
    {
        private IProtoObjectGatherableVegetation gatherableVegetation;

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
            if (this.gatherableVegetation == null)
            {
                return;
            }

            if (this.gatherableVegetation.DroplistOnDestroy != null &&
                this.gatherableVegetation.DroplistOnDestroy.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    this.gatherableVegetation.DroplistOnDestroy.EnumerateAllItems()));
            }

            if (this.gatherableVegetation.GatherDroplist != null &&
                this.gatherableVegetation.GatherDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    this.gatherableVegetation.GatherDroplist.EnumerateAllItems()));
            }
        }
    }
}
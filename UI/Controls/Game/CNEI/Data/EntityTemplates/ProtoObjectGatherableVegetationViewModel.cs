namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
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
            if (this.gatherableVegetation.GatherDroplist != null &&
                this.gatherableVegetation.GatherDroplist.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    this.gatherableVegetation.GatherDroplist.EnumerateAllItems()));
            }
        }
    }
}
namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoObjectVegetationViewModel : ProtoStaticWorldObjectViewModel
    {
        private IProtoObjectVegetation vegetation;

        public ProtoObjectVegetationViewModel([NotNull] IProtoObjectVegetation vegetation) : base(vegetation)
        {
            this.vegetation = vegetation;
            //vegetation.GrowthStagesCount
            //vegetation.GetGrowthStageDurationSeconds() ??

            // DroplistOnDestroy
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel{IProtoEntity}" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels{}" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (this.vegetation == null)
            {
                return;
            }

            if (this.vegetation.DroplistOnDestroy != null &&
                this.vegetation.DroplistOnDestroy.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    this.vegetation.DroplistOnDestroy.EnumerateAllItems()));
            }
        }
    }
}
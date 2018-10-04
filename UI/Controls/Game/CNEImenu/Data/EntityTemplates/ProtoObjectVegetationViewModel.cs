namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoObjectVegetationViewModel : ProtoStaticWorldObjectViewModel
    {
        private readonly IProtoObjectVegetation vegetation;

        public ProtoObjectVegetationViewModel([NotNull] IProtoObjectVegetation vegetation) : base(vegetation)
        {
            this.vegetation = vegetation;
            //vegetation.GrowthStagesCount
            //vegetation.GetGrowthStageDurationSeconds() ??

            // DroplistOnDestroy
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (vegetation == null)
            {
                return;
            }

            if (vegetation.DroplistOnDestroy != null &&
                vegetation.DroplistOnDestroy.EnumerateAllItems().Any())
            {
                EntityViewModelsManager.AddRecipe(new RecipeViewModel(this,
                    vegetation.DroplistOnDestroy.EnumerateAllItems()));
            }
        }
    }
}
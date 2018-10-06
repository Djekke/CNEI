namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;
    using System.Windows;

    public class ProtoObjectVegetationViewModel : ProtoStaticWorldObjectViewModel
    {
        private readonly IProtoObjectVegetation vegetation;

        public override string ResourceDictonaryName => "ProtoObjectVegetationDataTemplate.xaml";

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
                DroplistOnDestroy = new RecipeViewModel(this, vegetation.DroplistOnDestroy.EnumerateAllItems());
                DroplistOnDestroyVisibility = Visibility.Visible;
                EntityViewModelsManager.AddRecipe(DroplistOnDestroy);
            }
        }

        public RecipeViewModel DroplistOnDestroy { get; private set; }

        public Visibility DroplistOnDestroyVisibility { get; private set; } = Visibility.Collapsed;

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsDroplistOnDestroyExpanded { get; set; } = true;
    }
}
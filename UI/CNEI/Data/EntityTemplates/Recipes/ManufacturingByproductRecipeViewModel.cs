namespace CryoFall.CNEI.UI.Data
{
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ManufacturingByproductRecipeViewModel : StationCraftingRecipeViewModel
    {
        public override string ResourceDictionaryName => "ManufacturingByproductRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Byproduct";

        public ManufacturingByproductRecipeViewModel([NotNull] Recipe recipe) : base(recipe)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            base.InitAdditionalRecipes();

            if (!(ProtoEntity is Recipe.RecipeForManufacturingByproduct byproductRecipe))
            {
                return;
            }

            var fuelEntity = EntityViewModelsManager.GetEntityViewModel(byproductRecipe.ProtoItemFuel);
            icon = fuelEntity.Icon;

            //InputItemsList.Add(fuelEntity);

            //InputItemsVMList = new List<BaseViewModel>() {new ViewModelEntityWithCount(fuelEntity)}.AsReadOnly();

            TechVisibility = ListedInTechNodes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            OriginVisibility = StationsList.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ManufacturingByproductRecipeViewModel : BasicRecipeViewModel
    {
        public override string ResourceDictionaryName => "ManufacturingByproductRecipeDataTemplate.xaml";

        public override string RecipeTypeName => "Byproduct";

        public ManufacturingByproductRecipeViewModel([NotNull] Recipe recipe) : base(recipe)
        {
            RecipeType = recipe.RecipeType;
            OriginalDuration = recipe.OriginalDuration;
            IsDisabled = !recipe.IsEnabled;
            IsAutoUnlocked = recipe.IsAutoUnlocked;
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (!(ProtoEntity is Recipe.RecipeForManufacturingByproduct byproductRecipe))
            {
                return;
            }

            FuelEntity = EntityViewModelsManager.GetEntityViewModel(byproductRecipe.ProtoItemFuel);
            icon = FuelEntity.Icon;

            InputItemsVMList = new List<BaseViewModel>() {new ViewModelEntityWithCount(FuelEntity)}.AsReadOnly();

            OutputItemsVMList = byproductRecipe.OutputItems.Items
                .Select(i => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem),
                    i.Count, i.CountRandom, i.Probability))
                .ToList().AsReadOnly();

            // Do we need this?
            ListedInTechNodes = byproductRecipe.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();
            TechVisibility = ListedInTechNodes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            // or this?
            StationsList = byproductRecipe.StationTypes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();
            OriginVisibility = StationsList.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public ProtoEntityViewModel FuelEntity { get; private set; }
    }
}
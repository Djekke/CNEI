namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class StationCraftingRecipeViewModel : BasicRecipeViewModel
    {
        public override string RecipeTypeName => "Station craft";

        public StationCraftingRecipeViewModel([NotNull] Recipe recipe) : base(recipe)
        {
            RecipeType = recipe.RecipeType;

            IsByproduct = (RecipeType == RecipeType.ManufacturingByproduct)
                ? Visibility.Visible
                : Visibility.Collapsed;
            IsStationCraft = (RecipeType == RecipeType.Hand)
                ? Visibility.Collapsed
                : Visibility.Visible;
            IsHandCraft = (RecipeType == RecipeType.Hand)
                ? Visibility.Visible
                : Visibility.Collapsed;
            TimeVisibility = Visibility.Visible;
            OriginalDuration = recipe.OriginalDuration;
            IsDisabled = !recipe.IsEnabled;
            IsAutoUnlocked = recipe.IsAutoUnlocked;
            OriginText = (RecipeType == RecipeType.Hand) ? "Made by:" : "Made in:";
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (!(ProtoEntity is Recipe recipe))
            {
                return;
            }

            if (recipe is Recipe.RecipeForManufacturingByproduct byproductRecipe)
            {
                InputItemsVMList = new List<BaseViewModel>()
                {
                    new ViewModelEntityWithCount(
                        EntityViewModelsManager.GetEntityViewModel(byproductRecipe.ProtoItemFuel))
                }.AsReadOnly();
            }
            else
            {
                InputItemsVMList = recipe.InputItems
                    .Select(i =>
                        new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem), i.Count))
                    .ToList().AsReadOnly();
            }

            OutputItemsVMList = recipe.OutputItems.Items
                .Select(i => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem),
                    i.Count, i.CountRandom, i.Probability))
                .ToList().AsReadOnly();

            ListedInTechNodes = recipe.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();

            if (recipe is Recipe.BaseRecipeForStation stationsRecipe)
            {
                StationsList = stationsRecipe.StationTypes
                    .Select(EntityViewModelsManager.GetEntityViewModel)
                    .ToList().AsReadOnly();
            }
        }
    }
}
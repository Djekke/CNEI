namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class HandCraftingRecipeViewModel : BasicRecipeViewModel
    {
        public override string RecipeTypeName => "Hand craft";

        public HandCraftingRecipeViewModel([NotNull] Recipe recipe) : base(recipe)
        {
            RecipeType = recipe.RecipeType;

            // Delete this
            IsByproduct = Visibility.Collapsed;
            IsStationCraft = Visibility.Collapsed;
            IsHandCraft = Visibility.Visible;
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

            InputItemsVMList = recipe.InputItems
                .Select(i =>
                    new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem), i.Count))
                .ToList().AsReadOnly();

            OutputItemsVMList = recipe.OutputItems.Items
                .Select(i => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem),
                    i.Count, i.CountRandom, i.Probability))
                .ToList().AsReadOnly();

            ListedInTechNodes = recipe.ListedInTechNodes
                .Select(EntityViewModelsManager.GetEntityViewModel)
                .ToList().AsReadOnly();
        }
    }
}
namespace CryoFall.CNEI.UI.Data
{
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using JetBrains.Annotations;

    public abstract class BasicRecipeViewModel: RecipeViewModel
    {
        /// <summary>
        /// Basic recipe from Recipe class.
        /// </summary>
        protected BasicRecipeViewModel([NotNull] Recipe recipe) : base(recipe)
        {
            RecipeType = recipe.RecipeType;
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
    }
}
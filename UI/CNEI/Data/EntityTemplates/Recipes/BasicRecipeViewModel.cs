namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using JetBrains.Annotations;

    public abstract class BasicRecipeViewModel: RecipeViewModel
    {
        public override string ResourceDictionaryName => "RecipeDataTemplate.xaml";

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
            OriginalDuration = recipe.OriginalDuration;
            IsDisabled = !recipe.IsEnabled;
            IsAutoUnlocked = recipe.IsAutoUnlocked;
            OriginText = (RecipeType == RecipeType.Hand) ? "Made by:" : "Made in:";
        }

        public IReadOnlyList<BaseViewModel> InputItemsVMList { get; protected set; }

        public IReadOnlyList<BaseViewModel> OutputItemsVMList { get; protected set; }

        public double OriginalDuration { get; protected set; }

        public bool IsDisabled { get; protected set; }

        public bool IsAutoUnlocked { get; protected set; }

        public RecipeType RecipeType { get; protected set; }

        public string OriginText { get; protected set; }

        public Visibility IsStationCraft { get; protected set; }

        public Visibility IsHandCraft { get; protected set; }

        public Visibility TechVisibility { get; protected set; } = Visibility.Visible;

        public Visibility OriginVisibility { get; protected set; } = Visibility.Visible;

        public Visibility TimeVisibility { get; protected set; } = Visibility.Visible;
    }
}
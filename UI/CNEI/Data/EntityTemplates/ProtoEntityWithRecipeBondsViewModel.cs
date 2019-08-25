namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using JetBrains.Annotations;

    public abstract class ProtoEntityWithRecipeBondsViewModel: ProtoEntityViewModel
    {
        protected ProtoEntityWithRecipeBondsViewModel([NotNull] IProtoEntity entity) : base(entity)
        {
        }

        /// <summary>
        /// Finalize Recipe Link creation and prepare recipe VM list to observation.
        /// </summary>
        public override void FinalizeRecipeLinking()
        {
            base.FinalizeRecipeLinking();

            SelectedRecipeVMWrapper = RecipeVMWrappersList.FirstOrDefault(rw => rw.IsItemEnabled);
            SelectedUsageVMWrapper = UsageVMWrappersList.FirstOrDefault(rw => rw.IsItemEnabled);

            if (RecipeVMWrappersList.Count(rw => rw.IsItemEnabled) <= 1) MoreThanOneRecipe = Visibility.Collapsed;
            if (UsageVMWrappersList.Count(rw => rw.IsItemEnabled) <= 1) MoreThanOneUsage = Visibility.Collapsed;

            RecipePrevPage = new ActionCommand(() =>
                SelectedRecipeVMWrapper = PrevRecipe(RecipeVMWrappersList, SelectedRecipeVMWrapper));
            RecipeNextPage = new ActionCommand(() =>
                SelectedRecipeVMWrapper = NextRecipe(RecipeVMWrappersList, SelectedRecipeVMWrapper));

            UsagePrevPage = new ActionCommand(() =>
                SelectedUsageVMWrapper = PrevRecipe(UsageVMWrappersList, SelectedUsageVMWrapper));
            UsageNextPage = new ActionCommand(() =>
                SelectedUsageVMWrapper = NextRecipe(UsageVMWrappersList, SelectedUsageVMWrapper));

            if (SelectedRecipeVMWrapper == null) RecipesVisibility = Visibility.Hidden;
            if (SelectedUsageVMWrapper == null) UsageVisibility = Visibility.Hidden;
        }

        private RecipeViewModelComboBoxWraper selectedRecipeVMWraper;

        private RecipeViewModelComboBoxWraper selectedUsageVMWrapper;

        public RecipeViewModelComboBoxWraper SelectedRecipeVMWrapper
        {
            get => selectedRecipeVMWraper;
            set
            {
                if (value == selectedRecipeVMWraper)
                {
                    return;
                }
                selectedRecipeVMWraper = value;
                NotifyThisPropertyChanged();
            }
        }

        public RecipeViewModelComboBoxWraper SelectedUsageVMWrapper
        {
            get => selectedUsageVMWrapper;
            set
            {
                if (value == selectedUsageVMWrapper)
                {
                    return;
                }
                selectedUsageVMWrapper = value;
                NotifyThisPropertyChanged();
            }
        }

        public BaseCommand RecipePrevPage { get; private set; }

        public BaseCommand RecipeNextPage { get; private set; }

        public BaseCommand UsagePrevPage { get; private set; }

        public BaseCommand UsageNextPage { get; private set; }

        public Visibility MoreThanOneRecipe { get; private set; } = Visibility.Visible;

        public Visibility MoreThanOneUsage { get; private set; } = Visibility.Visible;

        public int SelectedTabIndex { get; set; }

        public Visibility RecipesVisibility { get; private set; } = Visibility.Visible;

        public Visibility UsageVisibility { get; private set; } = Visibility.Visible;

        public static RecipeViewModelComboBoxWraper PrevRecipe(
            List<RecipeViewModelComboBoxWraper> list,
            RecipeViewModelComboBoxWraper current)
        {
            var i = current.Index;
            do
            {
                i--;
                if (i < 0)
                {
                    i = list.Count - 1;
                }
            } while (!list[i].IsItemEnabled);
            return list[i];
        }

        public static RecipeViewModelComboBoxWraper NextRecipe(
            List<RecipeViewModelComboBoxWraper> list,
            RecipeViewModelComboBoxWraper current)
        {
            var i = current.Index;
            do
            {
                i++;
                if (i >= list.Count)
                {
                    i = 0;
                }
            } while (!list[i].IsItemEnabled);
            return list[i];
        }
    }
}
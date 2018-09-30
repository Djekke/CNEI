namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class RecipeViewModel : ProtoEntityViewModel
    {
        public RecipeViewModel(Recipe recipe) : base(recipe, recipe.Icon)
        {
            if (recipe is Recipe.RecipeForStationByproduct byproductRecipe)
            {
                this.IsByproduct = Visibility.Visible;
                this.InputItemsVMList = new List<ProtoEntityViewModel>() {
                    new ProtoItemViewModel(byproductRecipe.ProtoItemFuel)
                };
            }
            else
            {
                this.IsByproduct = Visibility.Collapsed;
                this.InputItemsVMList = recipe.InputItems
                    .Select(i => new ViewModelItemWithCount(i.ProtoItem, count: i.Count))
                    .ToList();
            }
            this.OutputItemsVMList = recipe.OutputItems.Items
                .Select(i => new ViewModelOutputItem(i.ProtoItem, i.Count, i.CountRandom, i.Probability))
                .ToList();
            this.RecipeType = recipe.RecipeType;
            this.IsStationCraft = (this.RecipeType == RecipeType.Hand) ? Visibility.Collapsed : Visibility.Visible;
            this.IsHandCraft = (this.RecipeType == RecipeType.Hand) ? Visibility.Visible : Visibility.Collapsed;
            this.OriginalDuration = recipe.OriginalDuration;
            this.IsDisabled = !recipe.IsEnabled;
            this.IsAutoUnlocked = recipe.IsAutoUnlocked;
            this.ListedInTechNodes = recipe.ListedInTechNodes
                .Select(t => new ProtoEntityViewModel(t, t.Icon))
                .ToList();
            if (recipe is Recipe.BaseRecipeForStation stationsRecipe)
            {
                this.StationsList = stationsRecipe.StationTypes
                    .Select(s => new ProtoEntityViewModel(s, s.Icon))
                    .ToList();
            }
        }

        public IReadOnlyList<ProtoEntityViewModel> InputItemsVMList { get; private set; }

        public IReadOnlyList<ProtoEntityViewModel> OutputItemsVMList { get; private set; }

        public double OriginalDuration  { get; private set; }

        public bool IsDisabled  { get; private set; }

        public bool IsAutoUnlocked { get; private set; }

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; private set; }

        public RecipeType RecipeType { get; private set; }

        public Visibility IsStationCraft { get; private set; }

        public Visibility IsHandCraft { get; private set; }

        public Visibility IsByproduct { get; private set; }

        public IReadOnlyList<ProtoEntityViewModel> StationsList { get; private set; }

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();
            foreach (var viewModelEntity in this.InputItemsVMList)
            {
                viewModelEntity.Dispose();
            }
            foreach (var viewModelEntity in this.OutputItemsVMList)
            {
                viewModelEntity.Dispose();
            }
        }
    }
}
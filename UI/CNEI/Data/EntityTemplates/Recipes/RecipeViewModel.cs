namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using JetBrains.Annotations;

    public class RecipeViewModel : ProtoEntityViewModel
    {
        public override string ResourceDictionaryName => "RecipeDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "Recipes/";

        public virtual string RecipeTypeName => "Recipe";

        public RecipeViewModel([NotNull] IProtoEntity entity) : base(entity)
        {
        }

        public static RecipeViewModel SelectBasicRecipe([NotNull] IProtoEntity entity)
        {
            if (!(entity is Recipe recipe))
                return new RecipeViewModel(entity);
            switch (recipe.RecipeType)
            {
                case RecipeType.Hand:
                    return new HandCraftingRecipeViewModel(recipe);
                case RecipeType.StationCrafting:
                    return new StationCraftingRecipeViewModel(recipe);
                case RecipeType.Manufacturing:
                    return new ManufacturingRecipeViewModel(recipe);
                case RecipeType.ManufacturingByproduct:
                    return new ManufacturingByproductRecipeViewModel(recipe);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        ///// <summary>
        ///// Initilize entity reletionships with each other - invoked after all entity view Models created,
        ///// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        ///// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        ///// </summary>
        //public override void InitAdditionalRecipes()
        //{
        //    if (!(ProtoEntity is Recipe recipe))
        //    {
        //        return;
        //    }
        //
        //    if (recipe is Recipe.RecipeForManufacturingByproduct byproductRecipe)
        //    {
        //        InputItemsVMList = new List<BaseViewModel>()
        //        {
        //            new ViewModelEntityWithCount(
        //                EntityViewModelsManager.GetEntityViewModel(byproductRecipe.ProtoItemFuel))
        //        }.AsReadOnly();
        //    }
        //    else
        //    {
        //        InputItemsVMList = recipe.InputItems
        //            .Select(i =>
        //                new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem), i.Count))
        //            .ToList().AsReadOnly();
        //    }
        //
        //    OutputItemsVMList = recipe.OutputItems.Items
        //        .Select(i => new ViewModelEntityWithCount(EntityViewModelsManager.GetEntityViewModel(i.ProtoItem),
        //            i.Count, i.CountRandom, i.Probability))
        //        .ToList().AsReadOnly();
        //
        //    ListedInTechNodes = recipe.ListedInTechNodes
        //        .Select(EntityViewModelsManager.GetEntityViewModel)
        //        .ToList().AsReadOnly();
        //
        //    if (recipe is Recipe.BaseRecipeForStation stationsRecipe)
        //    {
        //        StationsList = stationsRecipe.StationTypes
        //            .Select(EntityViewModelsManager.GetEntityViewModel)
        //            .ToList().AsReadOnly();
        //    }
        //}

        public IReadOnlyList<BaseViewModel> InputItemsVMList { get; protected set; }
            = new List<BaseViewModel>();

        public IReadOnlyList<BaseViewModel> OutputItemsVMList { get; protected set; }
            = new List<BaseViewModel>();

        public IReadOnlyList<ProtoEntityViewModel> StationsList { get; protected set; }
            = new List<ProtoEntityViewModel>();

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; protected set; }
            = new List<ProtoEntityViewModel>();

        public double OriginalDuration { get; protected set; } = 0d;

        public bool IsDisabled { get; protected set; } = false;

        public bool IsAutoUnlocked { get; protected set; }

        public RecipeType RecipeType { get; protected set; }

        public string OriginText { get; protected set; }

        public Visibility IsStationCraft { get; protected set; } = Visibility.Collapsed;

        public Visibility IsHandCraft { get; protected set; } = Visibility.Collapsed;

        public Visibility IsByproduct { get; protected set; } = Visibility.Collapsed;

        public Visibility TechVisibility { get; protected set; } = Visibility.Visible;

        public Visibility OriginVisibility { get; protected set; } = Visibility.Visible;

        public Visibility TimeVisibility { get; protected set; } = Visibility.Collapsed;
    }

    public class RecipeViewModelComboBoxWraper
    {
        /// <summary>
        /// Name that appear in ComboBox dropdown list.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// ComboBoxItem.IsEnabled link.
        /// </summary>
        public bool IsItemEnabled { get; }

        public RecipeViewModel RecipeVM { get; }

        /// <summary>
        /// Icon to show near Name in ComboBox list.
        /// </summary>
        public Brush Icon { get; }

        /// <summary>
        /// Bond to index in collection(list) to assist Prev\Next commands.
        /// </summary>
        public int Index { get; set; }

        public RecipeViewModelComboBoxWraper(RecipeViewModel recipeVM, string name, bool isEnabled, int index = -1)
        {
            RecipeVM = recipeVM;
            Icon = recipeVM?.Icon;
            Name = name;
            IsItemEnabled = isEnabled;
            Index = index;
        }
    }
}
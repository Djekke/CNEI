namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Scripting;
    using JetBrains.Annotations;

    public class RecipeViewModel : ProtoEntityViewModel
    {
        public override string ResourceDictionaryFolderName => "Recipes/";

        public virtual string RecipeTypeName => "Recipe";

        protected RecipeViewModel([NotNull] IProtoEntity entity) : base(entity)
        {
        }

        public static RecipeViewModel SelectBasicRecipe([NotNull] IProtoEntity entity)
        {
            if (entity is Recipe recipe)
            {
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
                        Api.Logger.Error("CNEI: Can not recognize recipe type for " + recipe);
                        return new BasicRecipeViewModel(recipe);
                }
            }
            Api.Logger.Error("CNEI: Entity " + entity + " is not a recipe, can not proceed.");
            throw new ArgumentException();
        }

        public List<ProtoEntityViewModel> InputItemsList { get; protected set; }
            = new List<ProtoEntityViewModel>();

        public List<ProtoEntityViewModel> OutputItemsList { get; protected set; }
            = new List<ProtoEntityViewModel>();

        public IReadOnlyList<ProtoEntityViewModel> StationsList { get; protected set; }
            = new List<ProtoEntityViewModel>();

        public IReadOnlyList<ProtoEntityViewModel> ListedInTechNodes { get; protected set; }
            = new List<ProtoEntityViewModel>();
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
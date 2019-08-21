namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using JetBrains.Annotations;

    public class ManufacturingRecipeViewModel : StationCraftingRecipeViewModel
    {
        public override string RecipeTypeName => "Manufacturing";

        public ManufacturingRecipeViewModel([NotNull] Recipe recipe) : base(recipe)
        {
        }
    }
}
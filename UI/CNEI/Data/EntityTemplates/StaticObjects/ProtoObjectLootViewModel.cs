namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectLootViewModel : ProtoStaticWorldObjectViewModel
    {
        public override string ResourceDictionaryName => "ProtoObjectLootDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "StaticObjects/";

        public ProtoObjectLootViewModel([NotNull] IProtoObjectLoot loot) : base(loot)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoObjectLoot loot &&
                loot.LootDroplist != null &&
                loot.LootDroplist.EnumerateAllItems().Any())
            {
                Droplist = new DroplistRecipeViewModel(this, loot.LootDroplist.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(Droplist);
            }
        }

        public RecipeViewModel Droplist { get; private set; }
    }
}
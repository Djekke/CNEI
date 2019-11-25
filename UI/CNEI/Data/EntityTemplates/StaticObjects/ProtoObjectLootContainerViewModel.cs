namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectLootContainerViewModel : ProtoStaticWorldObjectViewModel
    {
        public override string ResourceDictionaryName => "ProtoObjectLootContainerDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "StaticObjects/";

        public ProtoObjectLootContainerViewModel([NotNull] IProtoObjectLoot lootContainer) : base(lootContainer)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoObjectLoot lootContainer &&
                lootContainer.LootDroplist != null &&
                lootContainer.LootDroplist.EnumerateAllItems().Any())
            {
                Droplist = new DroplistRecipeViewModel(this, lootContainer.LootDroplist.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(Droplist);
            }
        }

        public RecipeViewModel Droplist { get; private set; }
    }
}
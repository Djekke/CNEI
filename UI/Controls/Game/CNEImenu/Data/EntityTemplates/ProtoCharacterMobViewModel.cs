namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.GameApi.Resources;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoCharacterMobViewModel : ProtoEntityViewModel
    {
        private static readonly ITextureResource DefaultIcon =
            new TextureResource("Content/Textures/StaticObjects/ObjectUnknown.png");

        public override string ResourceDictonaryName => "ProtoCharacterMobDataTemplate.xaml";

        public ProtoCharacterMobViewModel([NotNull] IProtoCharacterMob creature) : base(creature, DefaultIcon)
        {
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is IProtoCharacterMob creature &&
                creature.LootDroplist != null &&
                creature.LootDroplist.EnumerateAllItems().Any())
            {
                Droplist = new RecipeViewModel(this, creature.LootDroplist.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(Droplist);
            }
        }

        public RecipeViewModel Droplist { get; private set; }

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;

        // TODO: Prewiew from skeleton
        //public override TextureBrush Icon
    }
}
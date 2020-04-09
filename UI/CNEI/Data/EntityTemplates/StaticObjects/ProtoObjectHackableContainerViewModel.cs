namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Misc.Events;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectHackableContainerViewModel : ProtoStaticWorldObjectViewModel
    {
        public override string ResourceDictionaryName => "ProtoObjectHackableContainerDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "StaticObjects/";

        public ProtoObjectHackableContainerViewModel([NotNull] IProtoObjectHackableContainer hackableContainer)
            : base(hackableContainer)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            if (ProtoEntity is ProtoObjectHackableContainer hackableContainer &&
                hackableContainer.LootDroplist != null &&
                hackableContainer.LootDroplist.EnumerateAllItems().Any())
            {
                Droplist = new DroplistRecipeViewModel(this, hackableContainer.LootDroplist.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(Droplist);
            }
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoObjectHackableContainer hackableContainer)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Hacking stages number",
                    hackableContainer.HackingStagesNumber));
                EntityInformation.Add(new ViewModelEntityInformation("Hacking stage duration",
                    hackableContainer.HackingStageDuration));
            }
        }

        public RecipeViewModel Droplist { get; private set; }
    }
}
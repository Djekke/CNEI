namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Seeds;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemSeedViewModel : ProtoItemViewModel
    {
        public ProtoItemSeedViewModel([NotNull] IProtoItemSeed seed) : base(seed)
        {
        }

        /// <summary>
        /// Initilize entity reletionships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            base.InitAdditionalRecipes();

            if (ProtoEntity is IProtoItemSeed seed)
            {
                PlantRecipe = new PlantRecipeViewModel(this,
                    EntityViewModelsManager.GetEntityViewModel(seed.ObjectPlantProto));
                EntityViewModelsManager.AddRecipe(PlantRecipe);
            }
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemSeed seed)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Will grow into",
                    EntityViewModelsManager.GetEntityViewModel(seed.ObjectPlantProto)));
            }
        }

        public RecipeViewModel PlantRecipe { get; set; }
    }
}
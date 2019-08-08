namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectGatherableVegetationViewModel : ProtoObjectVegetationViewModel
    {
        public override string ResourceDictonaryName => "ProtoObjectGatherableVegetationDataTemplate.xaml";

        public ProtoObjectGatherableVegetationViewModel([NotNull] IProtoObjectGatherableVegetation gatherableVegetation)
            : base(gatherableVegetation)
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

            if (ProtoEntity is IProtoObjectGatherableVegetation gatherableVegetation &&
                gatherableVegetation.GatherDroplist != null &&
                gatherableVegetation.GatherDroplist.EnumerateAllItems().Any())
            {
                GatherDroplist = new RecipeViewModel(this,
                    gatherableVegetation.GatherDroplist.EnumerateAllItems());
                GatherDroplistVisibility = Visibility.Visible;
                EntityViewModelsManager.AddRecipe(GatherDroplist);
            }
        }

        public RecipeViewModel GatherDroplist { get; private set; }

        public Visibility GatherDroplistVisibility { get; private set; } = Visibility.Collapsed;

        public bool IsGatherDroplistExpanded { get; set; } = true;
    }
}
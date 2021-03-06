﻿namespace CryoFall.CNEI.UI.Data
{
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectGatherableVegetationViewModel : ProtoObjectVegetationViewModel
    {
        public override string ResourceDictionaryName => "ProtoObjectGatherableVegetationDataTemplate.xaml";

        public override string ResourceDictionaryFolderName => "StaticObjects/";

        public ProtoObjectGatherableVegetationViewModel([NotNull] IProtoObjectGatherableVegetation gatherableVegetation)
            : base(gatherableVegetation)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
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
                GatherDroplist = new DroplistRecipeViewModel(this,
                    gatherableVegetation.GatherDroplist.EnumerateAllItems());
                EntityViewModelsManager.AddRecipe(GatherDroplist);
            }
        }

        public RecipeViewModel GatherDroplist { get; private set; }
    }
}
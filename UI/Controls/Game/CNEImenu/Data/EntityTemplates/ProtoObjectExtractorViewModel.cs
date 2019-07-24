namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Manufacturers;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectExtractorViewModel : ProtoObjectManufacturerViewModel
    {
        public ProtoObjectExtractorViewModel([NotNull] IProtoObjectExtractor extractor) : base(extractor)
        {
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoObjectExtractor extractor)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Liquid capacity",
                    extractor.LiquidCapacity));
                EntityInformation.Add(new ViewModelEntityInformation("Liquid production per sec",
                    extractor.LiquidProductionAmountPerSecond));
            }
        }
    }
}
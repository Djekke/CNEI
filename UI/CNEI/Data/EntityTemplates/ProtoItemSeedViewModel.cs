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
    }
}
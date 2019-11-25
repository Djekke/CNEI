namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Crates;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectCrateViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectCrateViewModel([NotNull] IProtoObjectCrate crate) : base(crate)
        {
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoObjectCrate crate)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Items slots count",
                    crate.ItemsSlotsCount));
            }
        }
    }
}
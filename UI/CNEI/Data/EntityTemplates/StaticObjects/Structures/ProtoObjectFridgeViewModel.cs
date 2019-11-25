namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Crates;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Fridges;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectFridgeViewModel : ProtoObjectCrateViewModel
    {
        public ProtoObjectFridgeViewModel([NotNull] IProtoObjectCrate crate) : base(crate)
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

            if (ProtoEntity is IProtoObjectFridge fridge)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Freshness duration coef",
                    fridge.FreshnessDurationMultiplier));
            }
        }
    }
}
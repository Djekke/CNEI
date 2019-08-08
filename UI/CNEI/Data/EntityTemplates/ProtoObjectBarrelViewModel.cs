namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Barrels;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectBarrelViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectBarrelViewModel([NotNull] IProtoObjectBarrel barrel) : base(barrel)
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

            if (ProtoEntity is IProtoObjectBarrel barrel)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Liquid capacity",
                    barrel.LiquidCapacity));
            }
        }
    }
}
namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Explosives;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemExplosiveViewModel : ProtoItemViewModel
    {
        public ProtoItemExplosiveViewModel([NotNull] IProtoItemExplosive explosive) : base(explosive)
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

            if (ProtoEntity is IProtoItemExplosive explosive)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Deploy distance", explosive.DeployDistanceMax));
                EntityInformation.Add(new ViewModelEntityInformation("Deploy duration", explosive.DeployDuration));
                if (explosive?.ObjectExplosiveProto != null)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Structure damage",
                        explosive.ObjectExplosiveProto.StructureDamage));
                    EntityInformation.Add(new ViewModelEntityInformation("Structure defence penetartion",
                        explosive.ObjectExplosiveProto.StructureDefensePenetrationCoef));
                }
            }
        }
    }
}
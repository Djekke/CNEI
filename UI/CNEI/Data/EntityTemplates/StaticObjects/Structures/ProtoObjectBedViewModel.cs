namespace CryoFall.CNEI.UI.Data
{
    using System;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Beds;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectBedViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectBedViewModel([NotNull] IProtoObjectBed bed) : base(bed)
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

            if (ProtoEntity is IProtoObjectBed bed)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Respawn cooldown",
                    TimeSpan.FromSeconds(bed.RespawnCooldownDurationSeconds)));
            }
        }
    }
}
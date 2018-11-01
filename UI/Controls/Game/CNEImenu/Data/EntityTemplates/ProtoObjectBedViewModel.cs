namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Beds;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System;

    public class ProtoObjectBedViewModel : ProtoObjectStructureViewModel
    {
        // TODO: fix to IProtoObjectBed.
        public ProtoObjectBedViewModel([NotNull] ProtoObjectBed bed) : base(bed)
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

            if (ProtoEntity is IProtoObjectBed bed)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Respawn cooldown",
                    TimeSpan.FromSeconds(bed.RespawnCooldownDurationSeconds)));
            }
        }
    }
}
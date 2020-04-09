namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemGrenadeViewModel : ProtoItemAmmoViewModel
    {
        public ProtoItemGrenadeViewModel([NotNull] IAmmoGrenade grenade) : base(grenade)
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

            if (ProtoEntity is IAmmoGrenade grenade)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Structure damage",
                    grenade.StructureDamage));
                EntityInformation.Add(new ViewModelEntityInformation("Structure defence penetration coef",
                    grenade.StructureDefensePenetrationCoef));
                EntityInformation.Add(new ViewModelEntityInformation("Damage radius",
                    grenade.DamageRadius));
            }
        }
    }
}
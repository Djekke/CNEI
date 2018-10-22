namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Weapons;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System.Linq;

    public class ProtoItemWeaponViewModel : ProtoItemViewModel
    {
        private readonly IProtoItemWeapon itemWeapon;

        public ProtoItemWeaponViewModel([NotNull] IProtoItemWeapon itemWeapon) : base(itemWeapon)
        {
            this.itemWeapon = itemWeapon;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();
            EntityInformation.Add(new ViewModelEntityInformation("Durability max", itemWeapon.DurabilityMax));
            EntityInformation.Add(new ViewModelEntityInformation("Fire interval", itemWeapon.FireInterval));
            EntityInformation.Add(new ViewModelEntityInformation("Reload time", itemWeapon.ReloadDuration));

            // WeaponSkillProto not existed for ProtoItemToolAxe and ProtoItemToolPickaxe
            if (itemWeapon?.WeaponSkillProto != null)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Corresponding skill",
                    EntityViewModelsManager.GetEntityViewModel(itemWeapon.WeaponSkillProto)));
            }

            if (itemWeapon.OverrideDamageDescription != null)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Weapon range",
                    itemWeapon.OverrideDamageDescription.RangeMax));
                EntityInformation.Add(new ViewModelEntityInformation("Raw damage",
                    itemWeapon.OverrideDamageDescription.DamageValue));
            }

            if (itemWeapon.IsUseAmmo)
            {
                EntityInformation.Add(
                    new ViewModelEntityInformation("Ammo per shot", itemWeapon.AmmoConsumptionPerShot));
                EntityInformation.Add(new ViewModelEntityInformation("Magazine capacity", itemWeapon.MagazineCapacity));
                EntityInformation.Add(new ViewModelEntityInformation("Compatible ammo",
                    itemWeapon.CompatibleAmmoProtos.Select(EntityViewModelsManager.GetEntityViewModel)));
            }
        }
    }
}
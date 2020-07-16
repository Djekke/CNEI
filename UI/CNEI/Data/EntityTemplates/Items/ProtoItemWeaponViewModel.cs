namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.Items.Weapons;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemWeaponViewModel : ProtoItemViewModel
    {
        public ProtoItemWeaponViewModel([NotNull] IProtoItemWeapon itemWeapon) : base(itemWeapon)
        {
        }

        /// <summary>
        /// Initialize entity relationships with each other - invoked after all entity view Models created,
        /// so you can access them by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitAdditionalRecipes()
        {
            base.InitAdditionalRecipes();

            if (!(ProtoEntity is IProtoItemWeapon itemWeapon))
            {
                return;
            }

            if (itemWeapon.AmmoCapacity > 0)
            {
                foreach (var compatibleAmmoProto in itemWeapon.CompatibleAmmoProtos)
                {
                    var ammoVM = EntityViewModelsManager.GetEntityViewModel(compatibleAmmoProto);
                    CompatibleAmmo.Add(ammoVM);
                    if (ammoVM is ProtoItemAmmoViewModel ammoViewModel)
                    {
                        ammoViewModel.AddCompatibleGun(this);
                    }
                    else
                    {
                        Api.Logger.Error("CNEI: Gun using something else as ammo (not IProtoItemAmmo) " + ammoVM);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoItemWeapon itemWeapon)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Fire interval", itemWeapon.FireInterval));

                // WeaponSkillProto not existed for ProtoItemToolAxe and ProtoItemToolPickaxe
                if (itemWeapon.WeaponSkillProto != null)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Corresponding skill",
                        EntityViewModelsManager.GetEntityViewModel(itemWeapon.WeaponSkillProto)));
                }

                if (itemWeapon.AmmoCapacity > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Reload time",
                        itemWeapon.AmmoReloadDuration));
                    EntityInformation.Add(new ViewModelEntityInformation("Ammo per shot",
                        itemWeapon.AmmoConsumptionPerShot));
                    EntityInformation.Add(new ViewModelEntityInformation("Ammo capacity",
                        itemWeapon.AmmoCapacity));
                    EntityInformation.Add(new ViewModelEntityInformation("Compatible ammo",
                        CompatibleAmmo));
                }

                if (itemWeapon.OverrideDamageDescription != null)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Weapon range",
                        itemWeapon.OverrideDamageDescription.RangeMax));
                    EntityInformation.Add(new ViewModelEntityInformation("Raw damage",
                        itemWeapon.OverrideDamageDescription.DamageValue));
                }

                EntityInformation.Add(new ViewModelEntityInformation("Damage multiplier",
                    itemWeapon.DamageMultiplier));
                EntityInformation.Add(new ViewModelEntityInformation("Range multiplier",
                    itemWeapon.RangeMultiplier));
            }
        }

        public List<ProtoEntityViewModel> CompatibleAmmo { get; private set; }
            = new List<ProtoEntityViewModel>();
    }
}
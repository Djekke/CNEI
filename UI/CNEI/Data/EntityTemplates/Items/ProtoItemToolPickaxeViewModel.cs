﻿namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Items.Tools;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoItemToolPickaxeViewModel : ProtoItemWeaponViewModel
    {
        public ProtoItemToolPickaxeViewModel([NotNull] IProtoItemToolMining pickaxe) : base(pickaxe)
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

            if (ProtoEntity is IProtoItemToolMining pickaxe)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Damage to minerals",
                    pickaxe.DamageToMinerals));
            }
        }
    }
}
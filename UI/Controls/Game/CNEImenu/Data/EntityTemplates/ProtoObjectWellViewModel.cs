﻿namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Manufacturers;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectWellViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectWellViewModel([NotNull] IProtoObjectWell well) : base(well)
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

            if (ProtoEntity is IProtoObjectWell well)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Water capacity",
                    well.WaterCapacity));
                EntityInformation.Add(new ViewModelEntityInformation("Water production per sec",
                    well.WaterProductionAmountPerSecond));
            }
        }
    }
}
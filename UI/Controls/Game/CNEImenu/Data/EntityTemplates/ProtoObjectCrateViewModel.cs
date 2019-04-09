namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Crates;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Fridges;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectCrateViewModel : ProtoObjectStructureViewModel
    {
        // TODO: Fix it to IProtoObjectCrate when it implemented
        public ProtoObjectCrateViewModel([NotNull] IProtoObjectStructure crate) : base(crate)
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

            if (ProtoEntity is ProtoObjectCrate crate)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Items slots count",
                    crate.ItemsSlotsCount));
            }

            if (ProtoEntity is ProtoObjectDisplayCase displayCase)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Items slots count",
                    displayCase.ItemsSlotsCount));
            }

            if (ProtoEntity is ProtoObjectFridge fridge)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Items slots count",
                    fridge.ItemsSlotsCount));
            }
        }
    }
}
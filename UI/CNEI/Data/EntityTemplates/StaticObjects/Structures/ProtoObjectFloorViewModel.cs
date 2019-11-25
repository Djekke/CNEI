namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Floors;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectFloorViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectFloorViewModel([NotNull] IProtoObjectFloor floor) : base(floor)
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

            if (ProtoEntity is IProtoObjectFloor floor)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Move speed multiplier",
                    floor.CharacterMoveSpeedMultiplier));
            }
        }
    }
}
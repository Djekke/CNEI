namespace CryoFall.CNEI.UI.Data
{
    using System.Threading.Tasks;
    using AtomicTorch.CBND.CoreMod.Drones;
    using AtomicTorch.CBND.GameApi.Resources;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoDroneViewModel : ProtoDynamicWorldObjectViewModel
    {
        public ProtoDroneViewModel([NotNull] IProtoDrone drone) : base(drone)
        {
        }

        public override async Task<ITextureResource> GenerateIcon(
            ProceduralTextureRequest request,
            ushort textureWidth = 512,
            ushort textureHeight = 512,
            sbyte spriteQualityOffset = 0)
        {
            if (ProtoEntity is IProtoDrone drone)
            {
                return drone.ProtoItemDrone.Icon;
            }
            else
            {
                return await base.GenerateIcon(request, textureWidth, textureHeight, spriteQualityOffset);
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

            if (ProtoEntity is IProtoDrone drone)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Move speed",
                    drone.StatMoveSpeed));
                EntityInformation.Add(new ViewModelEntityInformation("Drone as item",
                    EntityViewModelsManager.GetEntityViewModel(drone.ProtoItemDrone)));
                EntityInformation.Add(new ViewModelEntityInformation("Mining tool",
                    EntityViewModelsManager.GetEntityViewModel(drone.ProtoItemMiningTool)));
            }
        }
    }
}
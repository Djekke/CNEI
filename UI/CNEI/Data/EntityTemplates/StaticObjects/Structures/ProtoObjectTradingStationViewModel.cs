namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.TradingStations;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectTradingStationViewModel : ProtoObjectStructureViewModel
    {
        public ProtoObjectTradingStationViewModel([NotNull] IProtoObjectTradingStation tradingStation)
            : base(tradingStation)
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

            if (ProtoEntity is IProtoObjectTradingStation tradingStation)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Lots count",
                    tradingStation.LotsCount));
            }
        }
    }
}
namespace CryoFall.CNEI.UI.Data
{
    using AtomicTorch.CBND.CoreMod.Zones;
    using AtomicTorch.CBND.GameApi.Data.Zones;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoZoneViewModel : ProtoEntityViewModel
    {
        public ProtoZoneViewModel([NotNull] IProtoZone zone) : base(zone)
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

            EntityInformation.Add(new ViewModelEntityInformation("test", "test"));

            if (ProtoEntity is ProtoZoneDefault zoneDefault)
            {
                if (zoneDefault.AttachedScripts != null)
                {
                    Api.Logger.Dev("yes1");
                    foreach (var attachedScript in zoneDefault.AttachedScripts)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation("Attached script",
                            attachedScript.ShortId));
                    }
                }
            }

            if (ProtoEntity is IProtoZone zone)
            {
                EntityInformation.Add(new ViewModelEntityInformation("name",
                    zone.Name));
                if (zone.AttachedScripts != null)
                {
                    Api.Logger.Dev("yes2");
                    foreach (var attachedScript in zone.AttachedScripts)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation("Attached script",
                            attachedScript.ShortId));
                    }
                }
            }
        }
    }
}
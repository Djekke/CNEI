namespace CryoFall.CNEI.UI.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using AtomicTorch.CBND.CoreMod.Technologies;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class TechNodeViewModel : ProtoEntityViewModel
    {
        public TechNodeViewModel([NotNull] TechNode techNode) : base(techNode)
        {
            Description = techNode.Description;
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is TechNode techNode)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Tier", techNode.Group.Tier.ToString()));
                EntityInformation.Add(new ViewModelEntityInformation("LP to unlock", techNode.LearningPointsPrice));
                EntityInformation.Add(new ViewModelEntityInformation("Tech group",
                    EntityViewModelsManager.GetEntityViewModel(techNode.Group)));
                if (techNode.RequiredNode != null)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Required node",
                        EntityViewModelsManager.GetEntityViewModel(techNode.RequiredNode)));
                }
                if (techNode.DependentNodes?.Count > 0)
                {
                    EntityInformation.Add(new ViewModelEntityInformation("Dependent nodes",
                        techNode.DependentNodes.Select(EntityViewModelsManager.GetEntityViewModel)));
                }
                if (techNode.NodeEffects?.Count > 0)
                {
                    List<ProtoEntityViewModel> tempList = new List<ProtoEntityViewModel>();
                    foreach (BaseTechNodeEffect nodeEffect in techNode.NodeEffects)
                    {
                        switch (nodeEffect)
                        {
                            case TechNodeEffectPerkUnlock techNodeEffectPerkUnlock:
                                tempList.Add(
                                    EntityViewModelsManager.GetEntityViewModel(techNodeEffectPerkUnlock.Perk));
                                break;
                            case TechNodeEffectRecipeUnlock techNodeEffectRecipeUnlock:
                                tempList.Add(
                                    EntityViewModelsManager.GetEntityViewModel(techNodeEffectRecipeUnlock.Recipe));
                                break;
                            case TechNodeEffectStructureUnlock techNodeEffectStructureUnlock:
                                tempList.Add(
                                    EntityViewModelsManager.GetEntityViewModel(techNodeEffectStructureUnlock
                                        .Structure));
                                break;
                            case TechNodeEffectVehicleUnlock techNodeEffectVehicleUnlock:
                                tempList.Add(
                                    EntityViewModelsManager.GetEntityViewModel(techNodeEffectVehicleUnlock
                                        .Vehicle));
                                break;
                            default:
                                Api.Logger.Error("CNEI: Unknown TechNodeEffect " + nodeEffect);
                                break;
                        }
                    }
                    EntityInformation.Add(new ViewModelEntityInformation("Unlocks", tempList));
                }
            }
        }
    }
}
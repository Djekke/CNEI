namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD.Data;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System;
    using System.Windows.Media;

    public class ProtoStatusEffectViewModel : ProtoEntityViewModel
    {
        public ProtoStatusEffectViewModel([NotNull] IProtoStatusEffect statusEffect)
            : base(statusEffect)
        {
            Description = statusEffect.Description;
            Kind = statusEffect.Kind;
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoStatusEffect statusEffect)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Status effect kind",
                    statusEffect.Kind.ToString()));
                EntityInformation.Add(new ViewModelEntityInformation("Is removed on respawn",
                    statusEffect.IsRemovedOnRespawn ? "Yes" : "No"));
                EntityInformation.Add(new ViewModelEntityInformation("Visibility threshold",
                    (statusEffect.VisibilityIntensityThreshold * 100) + "%"));
            }

            if (ProtoEntity is ProtoStatusEffect effect)
            {
                if (effect.IntensityAutoDecreasePerSecondValue > 0 ||
                    effect.IntensityAutoDecreasePerSecondFraction > 0)
                {
                    double seconds = 1.0 / effect.IntensityAutoDecreasePerSecondValue;
                    if (effect.IntensityAutoDecreasePerSecondFraction > 0)
                    {
                        seconds = 1.0 / effect.IntensityAutoDecreasePerSecondFraction;
                    }
                    EntityInformation.Add(new ViewModelEntityInformation("Auto-disappear in",
                        TimeSpan.FromSeconds(seconds)));
                }
            }
        }

        public StatusEffectKind Kind { get; }

        public Brush GetBackgroundBrush(double intensity)
        {
            return ViewModelStatusEffect.GetBrush(Kind, intensity);
        }
    }
}
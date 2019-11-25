namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.Stats;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD.Data;
    using CryoFall.CNEI.Managers;
    using JetBrains.Annotations;

    public class ProtoStatusEffectViewModel : ProtoEntityViewModel
    {
        public ProtoStatusEffectViewModel([NotNull] IProtoStatusEffect statusEffect)
            : base(statusEffect)
        {
            Description = statusEffect.Description;
            Kind = statusEffect.Kind;
        }

        /// <summary>
        /// Initialize information about entity - invoked after all entity view Models created,
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

                if (statusEffect.ProtoEffects?.Values.Count > 0)
                {
                    foreach (KeyValuePair<StatName, double> pair in statusEffect.ProtoEffects.Values)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation(pair.Key.ToString(), pair.Value));
                    }
                }

                if (statusEffect.ProtoEffects?.Multipliers.Count > 0)
                {
                    foreach (KeyValuePair<StatName, double> pair in statusEffect.ProtoEffects.Multipliers)
                    {
                        EntityInformation.Add(new ViewModelEntityInformation(pair.Key.ToString(), pair.Value));
                    }
                }
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
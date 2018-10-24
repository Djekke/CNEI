namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;
    using System;
    using System.Windows.Media;

    public class ProtoStatusEffectViewModel : ProtoEntityViewModel
    {
        private const byte BackgroundOpacity = 0xAA;

        private static readonly Brush BrushBuffTier0
            = new SolidColorBrush(Color.FromArgb(BackgroundOpacity, 0x33, 0x88, 0x33));

        private static readonly Brush BrushBuffTier1
            = new SolidColorBrush(Color.FromArgb(BackgroundOpacity, 0x22, 0xAA, 0x22));

        private static readonly Brush BrushBuffTier2
            = new SolidColorBrush(Color.FromArgb(BackgroundOpacity, 0x00, 0xCC, 0x00));

        private static readonly Brush BrushDebuffTier0
            = new SolidColorBrush(Color.FromArgb(BackgroundOpacity, 0xFF, 0xAA, 0x22));

        private static readonly Brush BrushDebuffTier1
            = new SolidColorBrush(Color.FromArgb(BackgroundOpacity, 0xFF, 0x77, 0x22));

        private static readonly Brush BrushDebuffTier2
            = new SolidColorBrush(Color.FromArgb(BackgroundOpacity, 0xFF, 0x22, 0x22));

        private static readonly Brush BrushNeutral
            = new SolidColorBrush(Color.FromArgb(BackgroundOpacity, 0xCC, 0xCC, 0xCC));

        public override string ResourceDictonaryName =>  "ProtoStatusEffectDataTemplate.xaml";

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

        public string Description { get; }

        public StatusEffectKind Kind { get; }

        public bool IsInfoExpanded { get; set; } = true;

        public Brush GetBackgroundBrush(double intensity)
        {
            // From Core.cpk\UI\Controls\Game\HUD\Data\ViewModelStatusEffect.cs
            byte tier;
            if (intensity < 0.333)
            {
                tier = 0;
            }
            else if (intensity < 0.667)
            {
                tier = 1;
            }
            else
            {
                tier = 2;
            }

            switch (Kind)
            {
                case StatusEffectKind.Buff:
                    switch (tier)
                    {
                        case 0:
                            return BrushBuffTier0;
                        case 1:
                            return BrushBuffTier1;
                        case 2:
                            return BrushBuffTier2;
                    }

                    break;

                case StatusEffectKind.Debuff:
                    switch (tier)
                    {
                        case 0:
                            return BrushDebuffTier0;
                        case 1:
                            return BrushDebuffTier1;
                        case 2:
                            return BrushDebuffTier2;
                    }

                    break;

                case StatusEffectKind.Neutral:
                    return BrushNeutral;
            }

            throw new Exception("Impossible");
        }
    }
}
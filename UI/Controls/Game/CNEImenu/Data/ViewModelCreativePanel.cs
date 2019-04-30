namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;

    public class ViewModelCreativePanel : BaseViewModel
    {
        public BaseCommand Heal { get; }

        public BaseCommand SetTimeOfDay { get; }

        public bool IsGodModeOn
        {
            get => CreativePanelManager.IsGodModeOn;
            set
            {
                if (value == CreativePanelManager.IsGodModeOn)
                {
                    return;
                }

                CreativePanelManager.IsGodModeOn = value;
                CreativePanelManager.ExecuteCommand("/player.setInvincibility " + value);
                NotifyThisPropertyChanged();
            }
        }

        public bool IsCreativeModeOn
        {
            get => CreativePanelManager.GetCreativeModeStatus();
            set
            {
                if (value == CreativePanelManager.GetCreativeModeStatus())
                {
                    return;
                }
                CreativePanelManager.ExecuteCommand("/player.setCreativeMode " + value);
            }
        }

        public ViewModelCreativePanel()
        {
            Heal = new ActionCommand(() =>
                CreativePanelManager.ExecuteCommand("/player.heal"));
            SetTimeOfDay = new ActionCommandWithParameter(time =>
                CreativePanelManager.ExecuteCommand("/world.setTimeOfDay " + time));

            CreativePanelManager.CreativeModeChanged += CreativeModeStatusChanged;
        }

        void CreativeModeStatusChanged() => NotifyPropertyChanged(nameof(IsCreativeModeOn));

        protected override void DisposeViewModel()
        {
            base.DisposeViewModel();

            CreativePanelManager.CreativeModeChanged -= CreativeModeStatusChanged;
        }

        public void UpdateStatus()
        {
            NotifyPropertyChanged(nameof(IsCreativeModeOn));
            NotifyPropertyChanged(nameof(IsGodModeOn));
        }
    }
}
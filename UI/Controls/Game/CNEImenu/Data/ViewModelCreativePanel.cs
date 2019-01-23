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
            get => CreativePanelManager.IsCreativeModeOn;
            set
            {
                if (value == CreativePanelManager.IsCreativeModeOn)
                {
                    return;
                }

                CreativePanelManager.IsCreativeModeOn = value;
                CreativePanelManager.ExecuteCommand("/admin.setCreativeMode " + value);
                NotifyThisPropertyChanged();
            }
        }

        public ViewModelCreativePanel()
        {
            Heal = new ActionCommand(() =>
                CreativePanelManager.ExecuteCommand("/player.heal"));
            SetTimeOfDay = new ActionCommandWithParameter(time =>
                CreativePanelManager.ExecuteCommand("/admin.setTimeOfDay " + time));
        }

        public void UpdateStatus()
        {
            CreativePanelManager.RefreshCreativeModeStatus();
            NotifyPropertyChanged(nameof(IsCreativeModeOn));
            NotifyPropertyChanged(nameof(IsGodModeOn));
        }
    }
}
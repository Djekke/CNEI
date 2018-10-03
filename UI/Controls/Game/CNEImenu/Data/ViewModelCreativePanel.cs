namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Console;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public class ViewModelCreativePanel : BaseViewModel
    {
        public BaseCommand Heal { get; }

        public BaseCommand SetTimeOfDay { get; }

        public BaseCommand GodModeToggle { get; }

        private bool IsGodModeOn = false;

        public ViewModelCreativePanel()
        {
            this.Heal = new ActionCommand(() => ExecuteCommand("/player.heal"));
            this.SetTimeOfDay = new ActionCommandWithParameter(time =>
                    ExecuteCommand("/admin.setTimeOfDay " + time));
            this.GodModeToggle = new ActionCommand(() =>
                {
                    this.IsGodModeOn = !this.IsGodModeOn;
                    ExecuteCommand("/player.setInvincibility " + this.IsGodModeOn);
                });
        }

        private void ExecuteCommand(string command)
        {
            ConsoleCommandsSystem.SharedExecuteConsoleCommand(command);
        }
    }
}
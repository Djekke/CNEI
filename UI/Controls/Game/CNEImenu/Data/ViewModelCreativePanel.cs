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

        private bool isGodModeOn = false;

        public ViewModelCreativePanel()
        {
            Heal = new ActionCommand(() => ExecuteCommand("/player.heal"));
            SetTimeOfDay = new ActionCommandWithParameter(time =>
                    ExecuteCommand("/admin.setTimeOfDay " + time));
            GodModeToggle = new ActionCommand(() =>
                {
                    isGodModeOn = !isGodModeOn;
                    ExecuteCommand("/player.setInvincibility " + isGodModeOn);
                });
        }

        private static void ExecuteCommand(string command)
        {
            ConsoleCommandsSystem.SharedExecuteConsoleCommand(command);
        }
    }
}
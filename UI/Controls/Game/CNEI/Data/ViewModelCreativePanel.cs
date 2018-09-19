namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Console;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public class ViewModelCreativePanel : BaseViewModel
    {
        public BaseCommand Heal { get; }

        public ViewModelCreativePanel()
        {
            this.Heal = new ActionCommand(() => 
                                ConsoleCommandsSystem.SharedExecuteConsoleCommand("/player.heal"));
        }
    }
}
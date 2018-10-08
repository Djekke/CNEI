namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers
{
    using AtomicTorch.CBND.CoreMod.Systems.Console;
    using AtomicTorch.CBND.CoreMod.Systems.Creative;
    using AtomicTorch.CBND.GameApi.Scripting;

    public static class CreativePanelManager
    {
        public static bool IsGodModeOn { get; set; }

        public static bool IsCreativeModeOn { get; set; }

        public static void RefreshCreativeModeStatus()
        {
            var character = Api.Client.Characters.CurrentPlayerCharacter;
            IsCreativeModeOn = CreativeModeSystem.SharedIsInCreativeMode(character);
        }

        public static void ExecuteCommand(string command)
        {
            ConsoleCommandsSystem.SharedExecuteConsoleCommand(command);
        }
    }
}
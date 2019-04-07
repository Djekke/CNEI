namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers
{
    using AtomicTorch.CBND.CoreMod.Systems.Console;
    using AtomicTorch.CBND.CoreMod.Systems.Creative;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System;

    public static class CreativePanelManager
    {
        public static event Action CreativeModeChanged;

        public static bool IsGodModeOn { get; set; }

        public static bool GetCreativeModeStatus()
        {
            var character = Api.Client.Characters.CurrentPlayerCharacter;
            return CreativeModeSystem.SharedIsInCreativeMode(character);
        }

        public static void ExecuteCommand(string command)
        {
            ConsoleCommandsSystem.SharedExecuteConsoleCommand(command);
        }

        public static void Init()
        {
            CreativeModeSystem.Instance.ClientCreativeModeChanged += CreativeModeChanged;
        }

        public static void Reset()
        {
            CreativeModeSystem.Instance.ClientCreativeModeChanged -= CreativeModeChanged;
        }
    }
}
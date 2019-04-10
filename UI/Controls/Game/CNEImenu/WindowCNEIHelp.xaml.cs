namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public partial class WindowCNEIHelp : BaseUserControlWithWindow
    {
        public static WindowCNEIHelp Instance;

        public static readonly BaseCommand CommandOpenMenu
            = new ActionCommand(
                () => Api.Client.UI.LayoutRootChildren.Add(
                    new WindowCNEIHelp()));

        public static void Close()
        {
            if (Instance != null)
            {
                Instance.CloseWindow(DialogResult.OK);
            }
        }

        public WindowCNEIHelp()
        {
            Close();

            if (Instance == null)
            {
                Instance = this;
            }
        }

        protected override void WindowClosed()
        {
            base.WindowClosed();

            Instance = null;
        }

        protected override void InitControlWithWindow()
        {
            base.InitControlWithWindow();
            this.Window.IsCached = false;
        }
    }
}
namespace CryoFall.CNEI.UI
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    public partial class HelpWindow : BaseUserControlWithWindow
    {
        public static HelpWindow Instance;

        public static readonly BaseCommand CommandOpenMenu
            = new ActionCommand(
                () => Api.Client.UI.LayoutRootChildren.Add(
                    new HelpWindow()));

        public static void Close()
        {
            if (Instance != null)
            {
                Instance.CloseWindow(DialogResult.OK);
            }
        }

        public HelpWindow()
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
namespace CryoFall.CNEI.UI
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

    // ReSharper disable once RedundantExtendsListEntry
    public partial class HelpWindow : BaseUserControlWithWindow
    {
        public static HelpWindow Instance;

        public static readonly BaseCommand CommandOpenMenu
            = new ActionCommand(
                () => Api.Client.UI.LayoutRootChildren.Add(
                    new HelpWindow()));

        public static void Close()
        {
            Instance?.CloseWindow(DialogResult.OK);
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
            Window.IsCached = false;
        }
    }
}
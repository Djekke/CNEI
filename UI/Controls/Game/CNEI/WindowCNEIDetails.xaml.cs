namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;

    public partial class WindowCNEIDetails : BaseUserControlWithWindow
    {
        public static WindowCNEIDetails Instance { get; private set; }

        public ViewModelWindowCNEIDetails ViewModel { get; private set; }

        public WindowCNEIDetails()
        {

        }

        public static WindowCNEIDetails Open()
        {
            if (Instance != null)
            {
                return Instance;
            }

            var instance = new WindowCNEIDetails();
            Instance = instance;
            Api.Client.UI.LayoutRootChildren.Add(instance);
            return Instance;
        }

        protected override void InitControlWithWindow()
        {
            base.InitControlWithWindow();
            this.Window.IsCached = false;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.DataContext = this.ViewModel = new ViewModelWindowCNEIDetails();
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            this.DataContext = null;
            this.ViewModel.Dispose();
            this.ViewModel = null;
            Instance = null;
        }
    }
}

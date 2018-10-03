namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;

    public partial class WindowCNEIDetails : BaseUserControlWithWindow
    {
        private ProtoEntityViewModel entityViewModel;

        public static WindowCNEIDetails Instance { get; private set; }

        public ViewModelWindowCNEIDetails ViewModel { get; private set; }

        public static WindowCNEIDetails Open(ProtoEntityViewModel entityViewModel)
        {
            if (Instance == null)
            {
                var instance = new WindowCNEIDetails();
                instance.entityViewModel = entityViewModel;
                Instance = instance;
                Api.Client.UI.LayoutRootChildren.Add(instance);
            }
            else
            {
                Instance.entityViewModel = entityViewModel;
            }

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
            this.DataContext = this.ViewModel = new ViewModelWindowCNEIDetails(this.entityViewModel);
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            this.DataContext = null;
            this.ViewModel.Dispose();
            this.ViewModel = null;
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
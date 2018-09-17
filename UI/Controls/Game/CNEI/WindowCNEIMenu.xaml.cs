namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;

    public partial class WindowCNEIMenu : BaseWindowMenu
    {
        private ViewModelWindowCNEIMenu viewModel;

        protected override void DisposeMenu()
        {
            base.DisposeMenu();
            this.DataContext = null;
            this.viewModel.Dispose();
            this.viewModel = null;
        }

        protected override void InitMenu()
        {
            this.DataContext = this.viewModel = new ViewModelWindowCNEIMenu();
        }
    }
}
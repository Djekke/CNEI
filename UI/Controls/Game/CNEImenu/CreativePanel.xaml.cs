namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu
{
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;

    public partial class CreativePanel : BaseUserControl
    {
        private ViewModelCreativePanel viewModel;

        public CreativePanel()
        {

        }

        protected override void InitControl()
        {
            this.DataContext = this.viewModel = new ViewModelCreativePanel();
        }

        protected override void DisposeControl()
        {
            this.DataContext = null;
            this.viewModel.Dispose();
            this.viewModel = null;
        }
    }
}
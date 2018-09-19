namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;

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
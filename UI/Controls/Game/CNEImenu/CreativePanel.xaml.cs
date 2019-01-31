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
            DataContext = viewModel = new ViewModelCreativePanel();
        }

        protected override void DisposeControl()
        {
            DataContext = null;
            viewModel.Dispose();
            viewModel = null;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            viewModel?.UpdateStatus();
        }
    }
}
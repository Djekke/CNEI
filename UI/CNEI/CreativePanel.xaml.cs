namespace CryoFall.CNEI.UI
{
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Data;

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
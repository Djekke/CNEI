namespace CryoFall.CNEI.UI
{
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Data;

    public partial class ToolsPanel : BaseUserControl
    {
        private ViewModelToolsPanel viewModel;

        public ToolsPanel()
        {
        }

        protected override void InitControl()
        {
            DataContext = viewModel = new ViewModelToolsPanel();
        }

        protected override void DisposeControl()
        {
            DataContext = null;
            viewModel.Dispose();
            viewModel = null;
        }
    }
}
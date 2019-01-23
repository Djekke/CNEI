namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu
{
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;

    public partial class CreativePanel : BaseUserControl
    {
        private ViewModelCreativePanel viewModel;

        public static CreativePanel Instance { get; private set; }

        public CreativePanel()
        {
        }

        public static void UpdateStatus()
        {
            if (Instance != null)
            {
                Instance.viewModel.UpdateStatus();
            }
        }

        protected override void InitControl()
        {
            if (Instance != null)
            {
                Api.Logger.Error("CNEI: Creative panel already created.");
            }
            DataContext = viewModel = new ViewModelCreativePanel();
            Instance = this;
        }

        protected override void DisposeControl()
        {
            DataContext = null;
            viewModel.Dispose();
            viewModel = null;
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
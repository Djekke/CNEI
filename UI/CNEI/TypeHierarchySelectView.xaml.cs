namespace CryoFall.CNEI.UI
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using CryoFall.CNEI.UI.Data;

    public partial class TypeHierarchySelectView : BaseUserControlWithWindow
    {
        private ViewModelTypeHierarchySelectView viewModel;

        public static TypeHierarchySelectView Instance { get; private set; }

        public static void Open()
        {
            EntityViewModelsManager.EntityTypeHierarchy.ResetState();

            if (Instance != null)
            {
                return;
            }

            var instance = new TypeHierarchySelectView();
            Instance = instance;
            Api.Client.UI.LayoutRootChildren.Add(instance);
        }

        public static void Close()
        {
            if (Instance?.IsOpened == true)
            {
                Instance.CloseWindow();
            }
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            DataContext = viewModel = new ViewModelTypeHierarchySelectView();
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
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
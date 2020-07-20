namespace CryoFall.CNEI.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.Systems.ServerOperator;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Data;

    public partial class MainWindow : BaseWindowMenu
    {
        private ViewModelMainWindow viewModel;

        private Canvas creativePanelCanvas;

        private Canvas toolsPanelCanvas;

        private CreativePanel creativePanel;

        private ToolsPanel toolsPanel;

        protected override void WindowOpening()
        {
            base.WindowOpening();
            if (creativePanelCanvas != null)
            {
                creativePanelCanvas.Visibility = ServerOperatorSystem.ClientIsOperator()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            var firstChild = (FrameworkElement)VisualTreeHelper.GetChild(this.Window, 0);
            var grid = firstChild.FindName<Grid>("LayoutRoot");
            if (grid != null)
            {
                if (creativePanelCanvas == null)
                {
                    creativePanelCanvas = new Canvas
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                    };

                    if (creativePanel == null)
                    {
                        creativePanel = new CreativePanel();
                        creativePanel.Width = 180;
                    }
                    creativePanelCanvas.Children.Add(creativePanel);
                    Canvas.SetLeft(creativePanel, -185);

                    grid.Children.Add(creativePanelCanvas);
                }

                if (toolsPanelCanvas == null)
                {
                    toolsPanelCanvas = new Canvas
                    {
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Visibility = Visibility.Collapsed,
                    };

                    if (toolsPanel == null)
                    {
                        toolsPanel = new ToolsPanel();
                        toolsPanel.Width = 180;
                    }
                    var toolsButton = grid.FindName<ToggleButton>("ToolsButton");
                    toolsButton.Command = new ActionCommand(
                        () => toolsPanelCanvas.Visibility = (bool)toolsButton.IsChecked
                            ? Visibility.Visible
                            : Visibility.Collapsed);
                    toolsPanelCanvas.Children.Add(toolsPanel);
                    Canvas.SetLeft(toolsPanel, 5);

                    grid.Children.Add(toolsPanelCanvas);
                }
            }
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();

            if (creativePanelCanvas != null)
            {
                ((Grid)creativePanelCanvas.Parent).Children.Remove(creativePanelCanvas);
                creativePanelCanvas = null;
            }
            if (toolsPanelCanvas != null)
            {
                ((Grid)toolsPanelCanvas.Parent).Children.Remove(toolsPanelCanvas);
                toolsPanelCanvas = null;
            }
        }

        protected override void WindowClosing()
        {
            base.WindowClosing();
            TypeHierarchySelectView.Close();
            HelpWindow.Close();
        }

        protected override void DisposeMenu()
        {
            base.DisposeMenu();
            DataContext = null;
            viewModel.Dispose();
            viewModel = null;
        }

        protected override void InitMenu()
        {
            DataContext = viewModel = new ViewModelMainWindow();
        }
    }
}
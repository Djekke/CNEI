// ReSharper disable InconsistentNaming
namespace CryoFall.CNEI.UI.Helpers
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using Menu = AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu.Menu;

    public class HUDButtonHelper
    {
        public static void AddHUDButton(Menu menu,
                                        string img_off,
                                        string img_on,
                                        string label,
                                        IButtonReference buttonRef)
        {
            var hudButtons = FindVisualChildren<HUDButton>(Api.Client.UI.LayoutRoot);
            IEnumerable<HUDButton> hudButtonsList = hudButtons.ToList();
            if (hudButtonsList.Any())
            {
                if (hudButtonsList.Last().Parent is StackPanel stackPanel)
                {
                    var newHUDButton = new HUDButton();
                    ImageBrush normalBrush = new ImageBrush();
                    BindingOperations.SetBinding(normalBrush, ImageBrush.ImageSourceProperty,
                        new Binding() {Source = img_off});
                    newHUDButton.BrushNormal = normalBrush;
                    ImageBrush highlightedBrush = new ImageBrush();
                    BindingOperations.SetBinding(highlightedBrush, ImageBrush.ImageSourceProperty,
                        new Binding() {Source = img_on});
                    newHUDButton.BrushHighlighted = highlightedBrush;
                    var labelWithButton = new LabelWithButton {Content = label, Button = buttonRef};
                    newHUDButton.SetBinding(ToolTipServiceExtend.ToolTipProperty,
                        new Binding() {Source = labelWithButton});
                    newHUDButton.SetBinding(HUDButton.CommandProperty,
                        new Binding("CommandToggle"));
                    newHUDButton.SetBinding(HUDButton.IsSelectedProperty,
                        new Binding("IsSelected"));
                    newHUDButton.DataContext = menu;
                    stackPanel.Children.Add(newHUDButton);
                }
                else
                {
                    Api.Logger.Error("CNEI: HUD buttons parent is not StackPanel.");
                }
            }
            else
            {
                Api.Logger.Error("CNEI: HUD buttons not found.");
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T dependencyObject)
                    {
                        yield return dependencyObject;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
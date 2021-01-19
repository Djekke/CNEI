namespace CryoFall.CNEI.UI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;
    using AtomicTorch.CBND.CoreMod.Items.Fishing.Base;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.Managers;
    using CryoFall.CNEI.UI.Controls;
    using CryoFall.CNEI.UI.Data;

    public partial class FishingCalculatorWindow : BaseUserControlWithWindow
    {
        private ViewModelFishingCalculator viewModel;

        private GridView fishingGridView;

        public static FishingCalculatorWindow Instance { get; private set; }

        public static void Open()
        {
            if (Instance == null)
            {
                var instance = new FishingCalculatorWindow();
                Instance = instance;
                Api.Client.UI.LayoutRootChildren.Add(instance);
            }
            Instance.Window.Open();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            DataContext = viewModel = new ViewModelFishingCalculator();
            fishingGridView = FindName("FishingGridView") as GridView;

            foreach(var baitViewModel in EntityViewModelsManager.GetAllEntityViewModelsByType<IProtoItemFishingBait>())
            {
                var entityControl = new EntitySlotControl() { DataContext = baitViewModel };
                var gridViewColumnHeader = new GridViewColumnHeader()
                {
                    Content = entityControl
                };
                var commandBinding = new Binding()
                {
                    Path = new PropertyPath("BaitSortDictionary[" + baitViewModel.Type + "]")
                };
                gridViewColumnHeader.SetBinding(GridViewColumnHeader.CommandProperty, commandBinding);
                var gridColumn = new GridViewColumn()
                {
                    Header = gridViewColumnHeader,
                    Width = double.NaN
                };
                Binding ChanceBinding = new Binding()
                {
                    Path = new PropertyPath("BaitWeightDictionary[" + baitViewModel.Type + "].Chance"),
                    StringFormat = "P0"
                };
                // Yep, it's error in VS2019, but it works for noesis.
                // https://www.noesisengine.com/bugs/view.php?id=1337
                gridColumn.DisplayMemberBinding =
                    (BindingExpression)ChanceBinding.ProvideValue(
                        new ProvideValueTarget
                        {
                            TargetObject = null,
                            TargetProperty = null
                        });
                fishingGridView.Columns.Add(gridColumn);
            }
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            DataContext = null;
            viewModel = null;

            if (Instance == this)
            {
                Instance = null;
            }
        }
    }

    public class ProvideValueTarget : IServiceProvider, IProvideValueTarget
    {
        public object TargetObject { get; set; }
        public object TargetProperty { get; set; }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IProvideValueTarget)) return this;
            return null;
        }
    }
}
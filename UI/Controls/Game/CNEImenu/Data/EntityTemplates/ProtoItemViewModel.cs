namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using JetBrains.Annotations;
    using System.Windows;

    public class ProtoItemViewModel : ProtoEntityViewModel
    {
        public override string ResourceDictonaryName => "ProtoItemDataTemplate.xaml";

        public ProtoItemViewModel([NotNull] IProtoItem item) : base(item, item.Icon)
        {
            Description = item.Description;
            IsStackable = item.IsStackable;
            MaxItemsPerStack = item.MaxItemsPerStack;
        }

        /// <summary>
        /// Finalize Recipe Link creation and prepare recipe VM list to observation.
        /// </summary>
        public override void FinalizeRecipeLinking()
        {
            base.FinalizeRecipeLinking();
            if (RecipeVMList.EntityCount == 0)
            {
                RecipesVisibility = Visibility.Collapsed;
                IsRecipesExpanded = false;
            }
            if (UsageVMList.EntityCount == 0)
            {
                UsageVisibility = Visibility.Collapsed;
                IsUsageExpanded = false;
            }
            RecipePrevPage = new ActionCommand(() => RecipeVMList.PrevPage());
            RecipeNextPage = new ActionCommand(() => RecipeVMList.NextPage());
            UsagePrevPage = new ActionCommand(() => UsageVMList.PrevPage());
            UsageNextPage = new ActionCommand(() => UsageVMList.NextPage());
        }

        public BaseCommand RecipePrevPage { get; private set; }

        public BaseCommand RecipeNextPage { get; private set; }

        public BaseCommand UsagePrevPage { get; private set; }

        public BaseCommand UsageNextPage { get; private set; }

        public string Description { get; }

        public bool IsStackable { get; }

        public ushort MaxItemsPerStack { get; }

        public Visibility RecipesVisibility { get; private set; } = Visibility.Visible;

        public Visibility UsageVisibility { get; private set; } = Visibility.Visible;

        public bool IsInfoExpanded { get; set; } = true;

        public bool IsRecipesExpanded { get; set; } = true;

        public bool IsUsageExpanded { get; set; } = true;
    }
}
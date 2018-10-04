namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using JetBrains.Annotations;

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
    }
}
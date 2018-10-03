namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using JetBrains.Annotations;

    public class ProtoItemViewModel : ProtoEntityViewModel
    {
        public ProtoItemViewModel([NotNull] IProtoItem item) : base(item, item.Icon)
        {
            this.Description = item.Description;
            this.IsStackable = item.IsStackable;
            this.MaxItemsPerStack = item.MaxItemsPerStack;
        }

        /// <summary>
        /// Finalize Recipe Link creation and prepare recipe VM list to observation.
        /// </summary>
        public override void FinalizeRecipeLinking()
        {
            base.FinalizeRecipeLinking();
            this.RecipePrevPage = new ActionCommand(() => this.RecipeVMList.PrevPage());
            this.RecipeNextPage = new ActionCommand(() => this.RecipeVMList.NextPage());
            this.UsagePrevPage = new ActionCommand(() => this.UsageVMList.PrevPage());
            this.UsageNextPage = new ActionCommand(() => this.UsageVMList.NextPage());
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
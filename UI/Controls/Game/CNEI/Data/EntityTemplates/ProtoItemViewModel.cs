namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System.Collections.Generic;

    public class ProtoItemViewModel : ProtoEntityViewModel
    {
        private IReadOnlyList<Recipe> recipeList;

        private IReadOnlyList<Recipe> usesList;

        public ProtoItemViewModel(IProtoItem item) : base(item, item.Icon)
        {
            this.Description = item.Description;
            this.IsStackable = item.IsStackable;
            this.MaxItemsPerStack = item.MaxItemsPerStack;
            if (EntityList.RecipeDictonary.ContainsKey(item))
            {
                this.recipeList = EntityList.RecipeDictonary[item];
            }
            else
            {
                Api.Logger.Error("There is no key for " + item + " in RecipeDictonary.");
            }
            if (EntityList.UsesDictonary.ContainsKey(item))
            {
                this.usesList = EntityList.UsesDictonary[item];
            }
            else
            {
                Api.Logger.Error("There is no key for " + item + " in UsesDictonary.");
            }
        }

        public string Description { get; }

        public bool IsStackable { get; }

        public ushort MaxItemsPerStack { get; }
    }
}
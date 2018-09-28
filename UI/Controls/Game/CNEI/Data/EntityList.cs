namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.Systems.Crafting;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class EntityList
    {
        private static IReadOnlyList<IProtoEntity> allEntity;

        private static IReadOnlyList<IProtoItem> allItems;

        private static IReadOnlyList<Recipe> allRecipes;

        private static IReadOnlyDictionary<IProtoItem, IReadOnlyList<Recipe>> recipeDictonary;

        private static IReadOnlyDictionary<IProtoItem, IReadOnlyList<Recipe>> usesDictonary;

        public static IReadOnlyList<IProtoEntity> AllEntity
        {
            get
            {
                if (allEntity == null)
                {
                    allEntity = Api.FindProtoEntities<IProtoEntity>();
                }
                return allEntity;
            }
        }

        public static IReadOnlyList<IProtoItem> AllItems
        {
            get
            {
                if (allItems == null)
                {
                    allItems = Api.FindProtoEntities<IProtoItem>();
                }
                return allItems;
            }
        }

        public static IReadOnlyList<Recipe> AllRecipes
        {
            get
            {
                if (allRecipes == null)
                {
                    allRecipes = Api.FindProtoEntities<Recipe>();
                }
                return allRecipes;
            }
        }

        public static IReadOnlyDictionary<IProtoItem, IReadOnlyList<Recipe>> RecipeDictonary
        {
            get
            {
                if (recipeDictonary == null)
                {
                    var tempDict = new Dictionary<IProtoItem, IReadOnlyList<Recipe>>();
                    foreach (IProtoItem item in AllItems)
                    {
                        var itemRecipeList = AllRecipes.Where(r => r.InputItems.Any(i => i.ProtoItem == item))
                                                       .ToList()
                                                       .AsReadOnly();
                        tempDict.Add(item, itemRecipeList);
                    }
                    recipeDictonary = tempDict;
                }
                return recipeDictonary;
            }
        }

        public static IReadOnlyDictionary<IProtoItem, IReadOnlyList<Recipe>> UsesDictonary
        {
            get
            {
                if (usesDictonary == null)
                {
                    var tempDict = new Dictionary<IProtoItem, IReadOnlyList<Recipe>>();
                    foreach (IProtoItem item in AllItems)
                    {
                        var itemRecipeList = AllRecipes.Where(r => r.OutputItems.Items.Any(i => i.ProtoItem == item))
                                                       .ToList()
                                                       .AsReadOnly();
                        tempDict.Add(item, itemRecipeList);
                    }
                    usesDictonary = tempDict;
                }
                return usesDictonary;
            }
        }

        public EntityList()
        {

        }
    }
}
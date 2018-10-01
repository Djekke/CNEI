namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Managers
{
    using AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EntityViewModelsManager
    {
        private static Dictionary<IProtoEntity, ProtoEntityViewModel> allEntityDictonary =
            new Dictionary<IProtoEntity, ProtoEntityViewModel>();

        private static List<RecipeViewModel> recipeList = new List<RecipeViewModel>();

        /// <summary>
        /// Return substring before ` symbol.
        /// </summary>
        /// <param name="s">Input string.</param>
        private static string GetNameWithoutGenericArity(string s)
        {
            int index = s.IndexOf('`');
            return index == -1 ? s : s.Substring(0, index);
        }

        /// <summary>
        /// Populate allEntityDictonary for all instance of IProtoEntity with corresponding ViewModels.
        /// </summary>
        private static void SetAllEntitiesViewModels()
        {
            List<IProtoEntity> allEntitiesList = Api.FindProtoEntities<IProtoEntity>();
            foreach (IProtoEntity entity in allEntitiesList)
            {
                Type entityType = entity.GetType();
                //Api.Logger.Warning("CNEI: Looking types for " + entity);
                bool templateFound = false;
                ProtoEntityViewModel newEntityViewModel = null;
                do
                {
                    //Api.Logger.Info("CNEI: " + entityType.Name + "  " + GetNameWithoutGenericArity(entityType.Name));
                    Type currentType = Type.GetType("AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data." +
                                                    GetNameWithoutGenericArity(entityType.Name) + "ViewModel");
                    if (currentType != null && !currentType.IsAbstract)
                    {
                        try
                        {
                            newEntityViewModel = (ProtoEntityViewModel)Activator.CreateInstance(currentType, entity);
                        }
                        catch (MissingMethodException)
                        {
                            Api.Logger.Warning("CNEI: Can not apply constructor of " + currentType + " type for " + entity);
                        }
                        if (newEntityViewModel != null)
                        {
                            if (newEntityViewModel is RecipeViewModel newRecipeViewModel)
                            {
                                AddRecipe(newRecipeViewModel);
                            }
                            allEntityDictonary.Add(entity ,newEntityViewModel);
                            templateFound = true;
                        }
                    }
                    entityType = entityType.BaseType;
                } while (entityType != null && (entityType.BaseType != null) && (!templateFound));
                if (entityType == null)
                {
                    Api.Logger.Warning("CNEI: Template for " + entity + "not found");
                    newEntityViewModel = new ProtoEntityViewModel(entity);
                    allEntityDictonary.Add(entity ,newEntityViewModel);
                }
            }
        }

        /// <summary>
        /// Return reference to existed View Model.
        /// </summary>
        public static ProtoEntityViewModel GetEntityViewModel(IProtoEntity entity)
        {
            if (entity != null && allEntityDictonary.ContainsKey(entity))
            {
                return allEntityDictonary[entity];
            }
            return null;
        }

        /// <summary>
        /// Return enumerator to View Models for all entities in game.
        /// </summary>
        public static IEnumerable<ProtoEntityViewModel> GetAllEntityViewModels()
        {
            return allEntityDictonary.Values;
        }

        /// <summary>
        /// Add View Model for recipe to list of all recipesViewModels.
        /// </summary>
        /// <param name="recipeViewModel"></param>
        public static void AddRecipe(RecipeViewModel recipeViewModel)
        {
            if (recipeViewModel != null && !recipeList.Contains(recipeViewModel))
            {
                recipeList.Add(recipeViewModel);
            }
        }

        public static void Init()
        {
            SetAllEntitiesViewModels();

            foreach (ProtoEntityViewModel entityViewModel in allEntityDictonary.Values)
            {
                entityViewModel.InitEntityRelationships();
            }

        }
    }
}
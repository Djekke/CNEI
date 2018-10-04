namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.GameEngine.Common.Extensions;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    public static class EntityViewModelsManager
    {
        private static Dictionary<IProtoEntity, ProtoEntityViewModel> allEntityDictonary =
            new Dictionary<IProtoEntity, ProtoEntityViewModel>();

        private static List<RecipeViewModel> recipeList = new List<RecipeViewModel>();

        private static HashSet<string> resourceDictionaryNames = new HashSet<string>();

        public static ResourceDictionary AllEntityTemplatesResourceDictionary = new ResourceDictionary();

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
                    Type currentType = Type.GetType("CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data." +
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
                            allEntityDictonary.Add(entity, newEntityViewModel);
                            resourceDictionaryNames.Add(newEntityViewModel.ResourceDictonaryName);
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
                    resourceDictionaryNames.Add(newEntityViewModel.ResourceDictonaryName);
                }
            }
        }

        /// <summary>
        /// Init all links for every recipe in recipeList.
        /// </summary>
        private static void InitAllRecipesLinks()
        {
            foreach (RecipeViewModel recipeViewModel in recipeList)
            {
                foreach (BaseViewModel viewModel in recipeViewModel.InputItemsVMList)
                {
                    switch (viewModel)
                    {
                        case ViewModelEntityWithCount viewModelEntityWithCount:
                            viewModelEntityWithCount.EntityViewModel.AddRecipeLink(recipeViewModel, 1);
                            break;
                        case ProtoEntityViewModel protoEntityViewModel:
                            protoEntityViewModel.AddRecipeLink(recipeViewModel, 1);
                            break;
                        default:
                            Api.Logger.Error("CNEI: Unknown view model type: " + viewModel);
                            break;
                    }
                }
                foreach (BaseViewModel viewModel in recipeViewModel.OutputItemsVMList)
                {
                    switch (viewModel)
                    {
                        case ViewModelEntityWithCount viewModelEntityWithCount:
                            viewModelEntityWithCount.EntityViewModel.AddRecipeLink(recipeViewModel, 2);
                            break;
                        case ProtoEntityViewModel protoEntityViewModel:
                            protoEntityViewModel.AddRecipeLink(recipeViewModel, 2);
                            break;
                        default:
                            Api.Logger.Error("CNEI: Unknown view model type: " + viewModel);
                            break;
                    }
                }
                foreach (ProtoEntityViewModel protoEntityViewModel in recipeViewModel.StationsList)
                {
                    protoEntityViewModel.AddRecipeLink(recipeViewModel, 3);
                }
                foreach (ProtoEntityViewModel protoEntityViewModel in recipeViewModel.ListedInTechNodes)
                {
                    protoEntityViewModel.AddRecipeLink(recipeViewModel, 4);
                }
            }
        }

        /// <summary>
        /// Assemble all DataTemplates for different entities in one ResourceDictionary.
        /// </summary>
        private static void AssembleAllTemplates()
        {
            foreach (string resourceDictionaryName in resourceDictionaryNames)
            {
                ResourceDictionary newDict = null;
#if GAME
                newDict = (ResourceDictionary)Noesis.GUI.LoadXaml(
                    "UI/Controls/Game/CNEImenu/Data/EntityTemplates/" + resourceDictionaryName);
#endif
                if (newDict != null)
                {
                    AllEntityTemplatesResourceDictionary.MergedDictionaries.Add(newDict);
                }
                else
                {
                    Api.Logger.Error("CNEI: Cannot load template " + resourceDictionaryName);
                }
            }
        }

        public static bool EntityDictonaryCreated = false;

        /// <summary>
        /// Return reference to existed View Model.
        /// </summary>
        public static ProtoEntityViewModel GetEntityViewModel([NotNull] IProtoEntity entity)
        {
            if (!EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Call GetEntityViewModel before all entity VMs sets.");
            }
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
            if (!EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Call GetAllEntityViewModels before all entity VMs sets.");
            }
            return allEntityDictonary.Values;
        }

        /// <summary>
        /// Add View Model for recipe to list of all recipesViewModels.
        /// </summary>
        /// <param name="recipeViewModel"></param>
        public static void AddRecipe([NotNull] RecipeViewModel recipeViewModel)
        {
            recipeList.AddIfNotContains(recipeViewModel);
        }

        public static void Init()
        {
            SetAllEntitiesViewModels();
            EntityDictonaryCreated = true;
            AssembleAllTemplates();

            foreach (ProtoEntityViewModel entityViewModel in allEntityDictonary.Values)
            {
                entityViewModel.InitAdditionalRecipes();
            }

            InitAllRecipesLinks();

            foreach (ProtoEntityViewModel entityViewModel in allEntityDictonary.Values)
            {
                entityViewModel.FinalizeRecipeLinking();
            }
        }
    }
}
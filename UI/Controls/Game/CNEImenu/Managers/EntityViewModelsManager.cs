namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.CBND.GameApi.ServicesClient;
    using AtomicTorch.GameEngine.Common.Extensions;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    public static class EntityViewModelsManager
    {
        private static Dictionary<IProtoEntity, ProtoEntityViewModel> allEntityDictonary =
            new Dictionary<IProtoEntity, ProtoEntityViewModel>();

        private static List<RecipeViewModel> recipeList = new List<RecipeViewModel>();

        private static HashSet<string> resourceDictionaryNames = new HashSet<string>();

        private static IClientStorage settingsStorage;

        private static Settings settingsInstance;

        public static TypeHierarchy EntityTypeHierarchy = new TypeHierarchy();

        public static Dictionary<string, TypeHierarchy> TypeHierarchyDictionary;

        public static ObservableCollection<TypeHierarchy> TypeHierarchyPlaneCollection;

        public static List<TypeHierarchy> DefaultViewPreset = new List<TypeHierarchy>();

        public static ObservableCollection<ProtoEntityViewModel> DefaultView =
            new ObservableCollection<ProtoEntityViewModel>();

        public static ObservableCollection<ProtoEntityViewModel> AllEntity;

        public static ObservableCollection<ProtoEntityViewModel> AllEntityWithTemplates;

        public static ObservableCollection<ProtoEntityViewModel> CurrentView;

        public static ResourceDictionary AllEntityTemplatesResourceDictionary = new ResourceDictionary();

        public static bool EntityDictonaryCreated = false;

        public static Visibility TypeVisibility = Visibility.Collapsed;

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
                            EntityTypeHierarchy.Add(entity.GetType(), newEntityViewModel);
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
                    EntityTypeHierarchy.Add(entity.GetType(), newEntityViewModel);
                    allEntityDictonary.Add(entity, newEntityViewModel);
                    resourceDictionaryNames.Add(newEntityViewModel.ResourceDictonaryName);
                }
            }
        }

        /// <summary>
        /// Get plane represantation from tree TypeHierarchy to list.
        /// </summary>
        private static void GetPlaneListOfAllTypesHierarchy()
        {
            List<TypeHierarchy> tempList = new List<TypeHierarchy>();
            RecursiveTypeListBuilder(EntityTypeHierarchy.Derivatives.FirstOrDefault());
            void RecursiveTypeListBuilder(TypeHierarchy t)
            {
                if (t.IsChild)
                {
                    return;
                }
                tempList.Add(t);
                foreach (TypeHierarchy derivative in t.Derivatives)
                {
                    RecursiveTypeListBuilder(derivative);
                }
            }
            TypeHierarchyPlaneCollection = new ObservableCollection<TypeHierarchy>(tempList);
            TypeHierarchyDictionary = tempList.ToDictionary(t => t.ShortName, t => t);
        }

        /// <summary>
        /// Convert TypeHierarchy tree view IsChecked states to list of global selected nodes.
        /// </summary>
        public static void SaveViewPreset()
        {
            List<TypeHierarchy> tempList = new List<TypeHierarchy>();
            RecursiveStringListBuilder(EntityTypeHierarchy.Derivatives.FirstOrDefault());
            void RecursiveStringListBuilder(TypeHierarchy t)
            {
                switch (t.IsCheckedSavedState)
                {
                    case true:
                        tempList.Add(t);
                        break;
                    case false:
                        return;
                    case null:
                        foreach (TypeHierarchy derivative in t.Derivatives)
                        {
                            if (t.IsCheckedSavedState != false)
                            {
                                RecursiveStringListBuilder(derivative);
                            }
                        }
                        break;
                    default:
                        throw new Exception("CNEI: And how it happened?");
                }
            }

            // TODO: Check for changes.
            tempList.Sort();
            DefaultViewPreset = tempList;

            SaveDefaultViewToSettings();
            AssembleDefaultView();
        }

        /// <summary>
        /// Collect all ProtoEntityViewModel from selected nodes in DefaultViewPreset to DefaultView.
        /// </summary>
        public static void AssembleDefaultView()
        {
            List<ProtoEntityViewModel> tempList = new List<ProtoEntityViewModel>();
            foreach (TypeHierarchy node in DefaultViewPreset)
            {
                tempList.AddRange(node.EntityViewModelsFullList);
            }
            DefaultView.Clear();
            DefaultView = new ObservableCollection<ProtoEntityViewModel>(tempList);
        }

        /// <summary>
        /// Try to load settings from client storage or init deafult one.
        /// </summary>
        public static void InitSettings()
        {
            settingsStorage = Api.Client.Storage.GetStorage("Mods/CNEI.Settings");
            settingsStorage.RegisterType(typeof(Settings));
            if (settingsStorage.TryLoad(out settingsInstance))
            {
                // LoadSettings.
            }
            else
            {
                // Default settings.
                settingsInstance.IsDefaultViewOn = true;
                settingsInstance.IsShowingEntityWithTemplates = false;
                settingsInstance.IsShowingAll = false;

                settingsInstance.DefaultViewPreset = new List<string>();

                settingsInstance.IsTypeVisibile = false;
            }

            LoadDefaultViewFromSettings();
        }

        /// <summary>
        /// Save settings in ClientStorage.
        /// </summary>
        public static void SaveSettings()
        {
            // TODO: Check if settings changed.
            settingsStorage.Save(settingsInstance);
        }

        /// <summary>
        /// Load default view preset from settings (convert list from string to TypeHierarhy).
        /// And set corresponding nodes IsChecked state.
        /// </summary>
        private static void LoadDefaultViewFromSettings()
        {
            foreach (string s in settingsInstance.DefaultViewPreset)
            {
                if (TypeHierarchyDictionary.ContainsKey(s))
                {
                    TypeHierarchyDictionary[s].IsChecked = true;
                }
                else
                {
                    Api.Logger.Error("CNEI: Error during loading default view, can not find corresponding type " + s);
                }
            }
            ViewModelTypeHierarchySelectView.SaveChanges();
        }

        /// <summary>
        /// Save default view preset to settings (convert list from TypeHierarhy to string).
        /// </summary>
        private static void SaveDefaultViewToSettings()
        {
            var tempList = DefaultViewPreset.Select(t => t.ShortName).ToList();

            //TODO: Check for changes.
            settingsInstance.DefaultViewPreset = tempList;
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
                ResourceDictionary newDict = Api.Client.UI.LoadResourceDictionary(
                    "UI/Controls/Game/CNEImenu/Data/EntityTemplates/" + resourceDictionaryName);
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

        /// <summary>
        /// Return reference to existed View Model.
        /// </summary>
        public static ProtoEntityViewModel GetEntityViewModel([NotNull] IProtoEntity entity)
        {
            if (!EntityDictonaryCreated)
            {
                throw new Exception("CNEI: Call GetEntityViewModel before all entity VMs sets.");
            }
            if (allEntityDictonary.ContainsKey(entity))
            {
                return allEntityDictonary[entity];
            }
            else
            {
                throw new Exception("CNEI: Unknown entity type " + entity);
            }
        }

        /// <summary>
        /// Gets view models of proto-classes of the specified type. For example, use IItemType as type parameter
        /// to get all view models of IItemType.
        /// </summary>
        /// <typeparam name="TProtoEntity">Type of proto entity.</typeparam>
        /// <returns>Collection of view models of instances which implements specified prototype.</returns>
        public static List<ProtoEntityViewModel> GetAllEntityViewModelsByType<TProtoEntity>()
            where TProtoEntity : class, IProtoEntity
        {
            return Api.FindProtoEntities<TProtoEntity>().Select(GetEntityViewModel).ToList();
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
            GetPlaneListOfAllTypesHierarchy();
            InitSettings();
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


        // Settings that save\load from\to ClientStorage.
        public struct Settings
        {
            public bool IsDefaultViewOn;
            public bool IsShowingEntityWithTemplates;
            public bool IsShowingAll;

            public List<string> DefaultViewPreset;

            public bool IsTypeVisibile;
        }
    }
}
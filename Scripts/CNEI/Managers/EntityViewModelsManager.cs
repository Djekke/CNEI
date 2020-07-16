namespace CryoFall.CNEI.Managers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.Vehicles;
    using AtomicTorch.CBND.GameApi;
    using AtomicTorch.CBND.GameApi.Data;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.CBND.GameApi.ServicesClient;
    using AtomicTorch.GameEngine.Common.Extensions;
    using CryoFall.CNEI.UI.Data;
    using JetBrains.Annotations;

    public static class EntityViewModelsManager
    {
        private static Dictionary<IProtoEntity, ProtoEntityViewModel> allEntityDictionary =
            new Dictionary<IProtoEntity, ProtoEntityViewModel>();

        private static List<RecipeViewModel> recipeList = new List<RecipeViewModel>();

        private static readonly HashSet<string> resourceDictionaryPaths = new HashSet<string>();

        private static IClientStorage versionStorage;

        private static IClientStorage defaultViewStorage;

        private static List<string> defaultViewPresetFromSettings;

        private static IClientStorage settingsStorage;

        private static Settings settingsInstance;

        private static bool isSettingsChanged = false;

        private static ViewType tempView;

        private static ObservableCollection<ProtoEntityViewModel> defaultViewCollection =
            new ObservableCollection<ProtoEntityViewModel>();

        private static ObservableCollection<ProtoEntityViewModel> defaultViewFilteredCollection =
            new ObservableCollection<ProtoEntityViewModel>();

        private static ObservableCollection<ProtoEntityViewModel> allEntityCollection;

        private static ObservableCollection<ProtoEntityViewModel> allEntityWithTemplatesCollection;

        /// <summary>
        /// Main node of TypeHierarchy tree.
        /// </summary>
        public static TypeHierarchy EntityTypeHierarchy = new TypeHierarchy();

        /// <summary>
        /// Dictionary for all nodes of TypeHierarchy tree with node ShortNames as keys.
        /// </summary>
        public static Dictionary<string, TypeHierarchy> TypeHierarchyDictionary;

        /// <summary>
        /// Observable collection with all nodes of TypeHierarchy tree.
        /// </summary>
        public static ObservableCollection<TypeHierarchy> TypeHierarchyPlaneCollection;

        /// <summary>
        /// Default preset, selected in settings, of main nodes from TypeHierarchy tree.
        /// </summary>
        public static List<TypeHierarchy> DefaultViewPreset = new List<TypeHierarchy>();

        /// <summary>
        /// Collection of types presenting in current view.
        /// </summary>
        public static ObservableCollection<TypeHierarchy> CurrentViewTypesCollection;

        /// <summary>
        /// Current View collection that shows in main menu.
        /// </summary>
        public static FilteredObservableWithPaging<ProtoEntityViewModel> CurrentView =
            new FilteredObservableWithPaging<ProtoEntityViewModel>();

        /// <summary>
        /// Resource dictionary containing all entity templates as merged dictionary.
        /// </summary>
        public static ResourceDictionary AllEntityTemplatesResourceDictionary = new ResourceDictionary();

        /// <summary>
        /// Is it safe to work with entity view models.
        /// </summary>
        public static bool EntityDictionaryCreated = false;

        public static Version CurrentVersion => new Version("0.4.5");

        public static Version VersionFromClientStorage = null;

        /// <summary>
        /// Main function, called on game start.
        /// </summary>
        public static void Init()
        {
            SetAllEntitiesViewModels();
            GetPlaneListOfAllTypesHierarchy();
            InitSettings();
            EntityDictionaryCreated = true;

            foreach (ProtoEntityViewModel entityViewModel in allEntityDictionary.Values)
            {
                entityViewModel.InitAdditionalRecipes();
                entityViewModel.InitInformation();
            }

            AssembleAllTemplates();

            InitAllRecipesLinks();

            foreach (ProtoEntityViewModel entityViewModel in allEntityDictionary.Values)
            {
                entityViewModel.FinalizeRecipeLinking();
            }
        }

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
        /// Populate allEntityDictionary for all instance of IProtoEntity with corresponding ViewModels.
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
                    Type currentType = Type.GetType("CryoFall.CNEI.UI.Data." +
                                                    GetNameWithoutGenericArity(entityType.Name) + "ViewModel");
                    //Api.Logger.Info("CNEI: " + entityType.Name + "  " + GetNameWithoutGenericArity(entityType.Name)
                    //                + " " + currentType);
                    if (currentType != null && !currentType.IsAbstract)
                    {
                        try
                        {
                            if (currentType == typeof(RecipeViewModel))
                            {//This is shit code and I don't like it.
                                newEntityViewModel = RecipeViewModel.SelectBasicRecipe(entity);
                            }
                            else
                            {
                                newEntityViewModel = (ProtoEntityViewModel)Activator.CreateInstance(currentType, entity);
                            }
                        }
                        catch (MissingMethodException)
                        {
                            Api.Logger.Error("CNEI: Can not apply constructor of " + currentType + " type for " + entity);
                        }
                        if (newEntityViewModel != null)
                        {
                            if (newEntityViewModel is RecipeViewModel newRecipeViewModel)
                            {
                                AddRecipe(newRecipeViewModel);
                            }
                            EntityTypeHierarchy.Add(entity.GetType(), newEntityViewModel);
                            allEntityDictionary.Add(entity, newEntityViewModel);
                            resourceDictionaryPaths.Add(newEntityViewModel.ResourceDictionaryFolderName +
                                                        newEntityViewModel.ResourceDictionaryName);
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
                    allEntityDictionary.Add(entity, newEntityViewModel);
                    resourceDictionaryPaths.Add(newEntityViewModel.ResourceDictionaryFolderName +
                                                newEntityViewModel.ResourceDictionaryName);
                }
            }

            allEntityCollection = new ObservableCollection<ProtoEntityViewModel>(allEntityDictionary.Values);
            allEntityWithTemplatesCollection = new ObservableCollection<ProtoEntityViewModel>(
                allEntityCollection.Where(vm => vm.GetType().IsSubclassOf(typeof(ProtoEntityViewModel))));
        }

        /// <summary>
        /// Get plane representation from tree TypeHierarchy to list.
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
                    // ReSharper disable once HeuristicUnreachableCode
                    default:
                        throw new Exception("CNEI: And how it happened?");
                }
            }

            // Change main nodes order.
            //tempList.Sort((t1,t2) => string.Compare(t1.ShortName, t2.ShortName, StringComparison.OrdinalIgnoreCase));
            if (!tempList.SequenceEqual(DefaultViewPreset))
            {
                DefaultViewPreset = tempList;

                SaveDefaultViewToSettings();
                AssembleDefaultView();
            }
        }

        /// <summary>
        /// Collect all ProtoEntityViewModel from selected nodes in DefaultViewPreset to DefaultView.
        /// </summary>
        public static void AssembleDefaultView()
        {
            List<ProtoEntityViewModel> tempList = new List<ProtoEntityViewModel>();
            List<ProtoEntityViewModel> tempListFiltered = new List<ProtoEntityViewModel>();
            foreach (TypeHierarchy node in DefaultViewPreset)
            {
                tempList.AddRange(node.EntityViewModelsFullList);
            }
            // Sort EntityList by Type
            tempList.Sort((t1,t2) => string.Compare(t1.Type, t2.Type, StringComparison.OrdinalIgnoreCase));

            // Filtering default view with all structures and vehicles that not available in technology.
            foreach (var entityViewModel in tempList)
            {
                switch (entityViewModel.ProtoEntity)
                {
                    case IProtoObjectStructure protoObjectStructure:
                        if (protoObjectStructure.IsAutoUnlocked || protoObjectStructure.IsListedInTechNodes)
                        {
                            tempListFiltered.Add(entityViewModel);
                        }
                        else
                        {
                            Api.Logger.Info("CNEI: Found unreachable structure: " + protoObjectStructure);
                        }
                        break;
                    case IProtoVehicle protoVehicle:
                        if (protoVehicle.ListedInTechNodes.Count > 0)
                        {
                            tempListFiltered.Add(entityViewModel);
                        }
                        else
                        {
                            Api.Logger.Info("CNEI: Found unreachable vehicle: " + protoVehicle);
                        }
                        break;
                    default:
                        tempListFiltered.Add(entityViewModel);
                        break;
                }
            }
            var tempCollection = new ObservableCollection<ProtoEntityViewModel>(tempList);
            var tempFilteredCollection = new ObservableCollection<ProtoEntityViewModel>(tempListFiltered);
            if (settingsInstance.View == ViewType.Default)
            {
                CurrentView.BaseCollection = settingsInstance.HideUnreachableObjects
                    ? tempFilteredCollection
                    : tempCollection;
            }
            defaultViewCollection.Clear();
            defaultViewCollection = tempCollection;
            defaultViewFilteredCollection.Clear();
            defaultViewFilteredCollection = tempFilteredCollection;
        }

        /// <summary>
        /// Try to load settings from client storage or init default one.
        /// </summary>
        public static void InitSettings()
        {
            LoadVersionFromClientStorage();
            LoadDefaultViewPresetFromClientStorage();
            LoadGlobalSettingsFromClientStorage();
        }

        /// <summary>
        /// Try to load mod version from client storage.
        /// </summary>
        private static void LoadVersionFromClientStorage()
        {
            // Load settings.
            versionStorage = Api.Client.Storage.GetStorage("Mods/CNEI/Version");
            versionStorage.RegisterType(typeof(Version));
            if (!versionStorage.TryLoad(out VersionFromClientStorage))
            {
                VersionFromClientStorage = new Version("0.0.1");
            }

            // Version changes handling.
            // if (VersionFromClientStorage.CompareTo(CurrentVersion) > 0)

            // Or should I wait until all migration work is done?
            versionStorage.Save(CurrentVersion);
        }

        public static void LoadDefaultViewPresetFromClientStorage()
        {
            defaultViewStorage = Api.Client.Storage.GetStorage("Mods/CNEI/DefaultView");
            bool settingExist = true;

            // Force reload preset if saved version is too old.
            if (VersionFromClientStorage.CompareTo(new Version("0.4.3")) < 0 ||
                !defaultViewStorage.TryLoad(out defaultViewPresetFromSettings))
            {
                // Default settings.
                defaultViewPresetFromSettings = new List<string>()
                {
                    "ProtoCharacterMob",
                    "ProtoItem",
                    "ProtoObjectHackableContainer",
                    "ProtoObjectLoot",
                    "ProtoObjectLootContainer",
                    "ProtoObjectMineral",
                    "ProtoObjectStructure",
                    "ProtoObjectVegetation",
                    "ProtoVehicle"
                };
                settingExist = false;
            }

            LoadDefaultViewFromSettings();

            if (!settingExist)
            {
                defaultViewStorage.Save(defaultViewPresetFromSettings);
            }
        }

        public static void LoadGlobalSettingsFromClientStorage()
        {
            settingsStorage = Api.Client.Storage.GetStorage("Mods/CNEI/GlobalSettings");
            settingsStorage.RegisterType(typeof(ViewType));
            settingsStorage.RegisterType(typeof(Settings));

            if (!settingsStorage.TryLoad(out settingsInstance))
            {
                // Default settings.
                settingsInstance.View = ViewType.Default;

                settingsInstance.IsTypeVisible = false;

                settingsInstance.HideUnreachableObjects = true;

                isSettingsChanged = true;
            }

            // Temp settings values to check if they changed.
            tempView = settingsInstance.View;
            IsTypeVisible = settingsInstance.IsTypeVisible;
            HideUnreachableObjects = settingsInstance.HideUnreachableObjects;

            ChangeCurrentView();
        }

        /// <summary>
        /// Save settings in ClientStorage.
        /// </summary>
        public static void SaveSettings()
        {
            // Check if content settings changed.
            if (tempView != settingsInstance.View)
            {
                settingsInstance.View = tempView;
                isSettingsChanged = true;
                ChangeCurrentView();
            }

            // Check if type visibility changed.
            if (IsTypeVisible != settingsInstance.IsTypeVisible)
            {
                settingsInstance.IsTypeVisible = IsTypeVisible;
                isSettingsChanged = true;
                CurrentView.Refresh();
            }

            // Check if HideUnreachableObjects changed.
            if (HideUnreachableObjects != settingsInstance.HideUnreachableObjects)
            {
                settingsInstance.HideUnreachableObjects = HideUnreachableObjects;
                isSettingsChanged = true;
                ChangeCurrentView();
            }

            // If settings changed - save to local storage.
            if (isSettingsChanged)
            {
                settingsStorage.Save(settingsInstance);
                isSettingsChanged = false;
            }
        }

        /// <summary>
        /// Change current view depending on current settings.
        /// </summary>
        private static void ChangeCurrentView()
        {
            switch (settingsInstance.View)
            {
                case ViewType.Default:
                    CurrentView.BaseCollection = settingsInstance.HideUnreachableObjects
                        ? defaultViewFilteredCollection
                        : defaultViewCollection;
                    break;
                case ViewType.EntityWithTemplates:
                    CurrentView.BaseCollection = allEntityWithTemplatesCollection;
                    break;
                case ViewType.ShowAll:
                    CurrentView.BaseCollection = allEntityCollection;
                    break;
                default:
                    throw new Exception("CNEI: Did I forgot something?");
            }
        }

        /// <summary>
        /// Load default view preset from settings (convert list from string to TypeHierarchy).
        /// And set corresponding nodes IsChecked state.
        /// </summary>
        private static void LoadDefaultViewFromSettings()
        {
            foreach (string s in defaultViewPresetFromSettings)
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
            // Save IsChecked state to IsCheckedSaved and generate corresponding DefaultView.
            ViewModelTypeHierarchySelectView.SaveChanges();
        }

        /// <summary>
        /// Save default view preset to settings (convert list from TypeHierarchy to string).
        /// </summary>
        private static void SaveDefaultViewToSettings()
        {
            var tempList = DefaultViewPreset.Select(t => t.ShortName).ToList();
            // tempList already sorted, as DefaultViewPreset sorted by ShortName
            if (!tempList.SequenceEqual(defaultViewPresetFromSettings))
            {
                defaultViewPresetFromSettings = tempList;
                defaultViewStorage.Save(defaultViewPresetFromSettings);
            }
        }

        /// <summary>
        /// Init all links for every recipe in recipeList.
        /// </summary>
        private static void InitAllRecipesLinks()
        {
            foreach (RecipeViewModel recipeViewModel in recipeList)
            {
                foreach (ProtoEntityViewModel protoEntityViewModel in recipeViewModel.InputItemsList)
                {
                    protoEntityViewModel.AddRecipeLink(recipeViewModel, 1);
                }
                foreach (ProtoEntityViewModel protoEntityViewModel in recipeViewModel.OutputItemsList)
                {
                    protoEntityViewModel.AddRecipeLink(recipeViewModel, 2);
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
            foreach (string resourceDictionaryName in resourceDictionaryPaths)
            {
                ResourceDictionary newDict = Api.Client.UI.LoadResourceDictionary(
                    "UI/CNEI/Data/EntityTemplates/" + resourceDictionaryName);
                if (newDict != null)
                {
                    foreach (var key in newDict.Keys)
                    {
                        AllEntityTemplatesResourceDictionary.Add(key, newDict[key]);
                    }
                }
                else
                {
                    Api.Logger.Error("CNEI: Can not load template " + resourceDictionaryName);
                }
            }
        }

        /// <summary>
        /// Return reference to existed View Model.
        /// </summary>
        public static ProtoEntityViewModel GetEntityViewModel([NotNull] IProtoEntity entity)
        {
            if (!EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Call GetEntityViewModel before all entity VMs sets.");
            }
            if (allEntityDictionary.ContainsKey(entity))
            {
                return allEntityDictionary[entity];
            }
            else
            {
                throw new Exception("CNEI: Unknown entity type " + entity);
            }
        }

        /// <summary>
        /// Return reference to existed View Model.
        /// </summary>
        public static ProtoEntityViewModel GetEntityViewModelByType<TProtoEntity>()
            where TProtoEntity : class, IProtoEntity
        {
            if (!EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Call GetEntityViewModelByType before all entity VMs sets.");
            }

            return Api.FindProtoEntities<TProtoEntity>().Select(GetEntityViewModel).FirstOrDefault();
        }

        /// <summary>
        /// Return reference to existed View Model.
        /// </summary>
        public static List<ProtoEntityViewModel> GetEntityViewModelByInterface(Type interfaceType)
        {
            if (!EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Call GetEntityViewModelByType before all entity VMs sets.");
            }

            return allEntityDictionary.Where(p => interfaceType.IsAssignableFrom(p.Key.GetType()))
                                     .Select(p => p.Value)
                                     .ToList();
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
            if (!EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Call GetAllEntityViewModelsByType before all entity VMs sets.");
            }

            return Api.FindProtoEntities<TProtoEntity>().Select(GetEntityViewModel).ToList();
        }

        /// <summary>
        /// Return collection of View Models for all entities in game.
        /// </summary>
        public static ObservableCollection<ProtoEntityViewModel> GetAllEntityViewModels()
        {
            if (!EntityDictionaryCreated)
            {
                throw new Exception("CNEI: Call GetAllEntityViewModels before all entity VMs sets.");
            }

            return allEntityCollection;
        }

        /// <summary>
        /// Add View Model for recipe to list of all recipesViewModels.
        /// </summary>
        /// <param name="recipeViewModel"></param>
        public static void AddRecipe([NotNull] RecipeViewModel recipeViewModel)
        {
            recipeList.AddIfNotContains(recipeViewModel);
            resourceDictionaryPaths.Add(recipeViewModel.ResourceDictionaryFolderName +
                                        recipeViewModel.ResourceDictionaryName);
        }

        public static bool IsDefaultViewOn
        {
            get => tempView == ViewType.Default;
            set
            {
                if (value)
                {
                    tempView = ViewType.Default;
                }
            }
        }

        public static bool IsShowingEntityWithTemplates
        {
            get => tempView == ViewType.EntityWithTemplates;
            set
            {
                if (value)
                {
                    tempView = ViewType.EntityWithTemplates;
                }
            }
        }

        public static bool IsShowingAll
        {
            get => tempView == ViewType.ShowAll;
            set
            {
                if (value)
                {
                    tempView = ViewType.ShowAll;
                }
            }
        }

        public static bool IsTypeVisible { get; set; }

        public static bool HideUnreachableObjects { get; set; }

        public static Visibility TypeVisibility =>
            settingsInstance.IsTypeVisible ? Visibility.Visible : Visibility.Collapsed;

        // Settings that save\load from\to ClientStorage.
        public struct Settings
        {
            public ViewType View;

            public bool HideUnreachableObjects;

            public bool IsTypeVisible;
        }

        [NotPersistent]
        public enum ViewType
        {
            Default,
            EntityWithTemplates,
            ShowAll
        }
    }
}
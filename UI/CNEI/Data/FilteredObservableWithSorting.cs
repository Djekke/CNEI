namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using AtomicTorch.CBND.GameApi.Extensions;
    using JetBrains.Annotations;

    public class FilteredObservableWithSorting<T> : INotifyPropertyChanged
    {
        private protected ObservableCollection<T> baseCollection;

        private protected ObservableCollection<T> items = new ObservableCollection<T>();

        private protected List<Predicate<T>> filters = new List<Predicate<T>>();

        private protected string sortPropertyName = "";

        private protected bool sortDirection;

        private int entityCount = 0;

        public string SortPropertyName
        {
            get => sortPropertyName;
            set
            {
                if(sortPropertyName == value)
                {
                    return;
                }

                sortPropertyName = value;
                Refresh();
            }
        }

        public bool SortDirection
        {
            get => sortDirection;
            set
            {
                if(sortDirection == value)
                {
                    return;
                }

                sortDirection = value;
                Refresh();
            }
        }

        /// <summary>
        /// Original item collection.
        /// </summary>
        public ObservableCollection<T> BaseCollection
        {
            get => baseCollection;
            set
            {
                // emulate VB's "with events" properties.
                if (null != baseCollection)
                    baseCollection.CollectionChanged -= BaseCollectionOnChange;

                baseCollection = value;
                baseCollection.CollectionChanged += BaseCollectionOnChange;

                NotifyThisPropertyChanged();
                NotifyPropertyChange("Items");
            }
        }

        /// <summary>
        /// Filtered item collection.
        /// </summary>
        public virtual ObservableCollection<T> Items
        {
            get
            {
                items.Clear();
                if (sortPropertyName != "" && IsPropertyExist(sortPropertyName))
                {
                    if (sortDirection)
                    {
                        items = new ObservableCollection<T>(baseCollection
                          .Where(x => filters.All(f => f(x)))
                          .OrderBy(x => GetPropertyValueFromPath(x, sortPropertyName)));
                    }
                    else
                    {
                        items = new ObservableCollection<T>(baseCollection
                          .Where(x => filters.All(f => f(x)))
                          .OrderByDescending(x => GetPropertyValueFromPath(x, sortPropertyName)));
                    }
                }
                else
                {
                    items = new ObservableCollection<T>(baseCollection
                        .Where(x => filters.All(f => f(x))));
                }
                EntityCount = items.Count;
                return items;
            }
        }

        private bool IsPropertyExist(string path)
        {
            string[] pp = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var type = typeof(T);
            foreach (var prop in pp)
            {
                if (prop.Contains("["))
                {
                    string dictionary = prop.Substring(0, prop.IndexOf("["));
                    string key = prop.Substring(prop.IndexOf("[") + 1, prop.IndexOf("]") - prop.IndexOf("[") - 1);

                    var dictInfo = type.ScriptingGetProperty(dictionary);
                    if (dictInfo == null)
                    {
                        return false;
                    }
                    type = dictInfo.PropertyType.GetGenericArguments()[1];
                }
                else
                {
                    var propInfo = type.ScriptingGetProperty(prop);
                    if (propInfo == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public object GetPropertyValueFromPath(object baseObject, string path)
        {
            string[] pp = path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            object valueObject = baseObject;
            foreach (var prop in pp)
            {
                if (prop.Contains("["))
                {
                    string dictionary = prop.Substring(0, prop.IndexOf("["));
                    string key = prop.Substring(prop.IndexOf("[") + 1, prop.IndexOf("]") - prop.IndexOf("[") - 1);

                    var dictInfo = valueObject.GetType().ScriptingGetProperty(dictionary);
                    if (dictInfo != null)
                    {
                        var dict = dictInfo.GetValue(valueObject, null);
                        valueObject = dict.GetType().ScriptingGetProperty("Item").GetValue(dict, new object[] { key });
                    }
                }
                else
                {
                    var propInfo = valueObject.GetType().ScriptingGetProperty(prop);
                    if (propInfo != null)
                    {
                        valueObject = propInfo.GetValue(valueObject, null);
                    }
                }
            }
            return valueObject;
        }

        /// <summary>
        /// Add item to base collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            baseCollection.Add(item);
        }

        /// <summary>
        /// Check if item in base collection.
        /// </summary>
        /// /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains([NotNull] T item)
        {
            return baseCollection.Contains(item);
        }

        /// <summary>
        /// Remove item from base collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove([CanBeNull] T item)
        {
            return baseCollection.Remove(item);
        }

        /// <summary>
        /// Get the number of elements actually contained in base collection.
        /// </summary>
        /// <returns>Return the number of elements in base collection.</returns>
        public int Count()
        {
            return baseCollection.Count;
        }

        /// <summary>
        /// Get the number of elements contained in filtered collection.
        /// </summary>
        public int EntityCount
        {
            get => entityCount;
            private protected set
            {
                if (value == entityCount)
                {
                    return;
                }
                entityCount = value;
                NotifyThisPropertyChanged();
            }
        }

        /// <summary>
        /// Is filtering enbled?
        /// </summary>
        /// <returns>True if there is any filter on.</returns>
        public bool IsFiltering()
        {
            return filters.Count() > 0;
        }

        /// <summary>
        /// Add filtering rule.
        /// </summary>
        /// <param name="filter">Filter predicate.</param>
        /// <returns>False if this filter already exist.</returns>
        public bool AddFilter(Predicate<T> filter)
        {
            if (!filters.Contains(filter))
            {
                filters.Add(filter);
                Refresh();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove filtering rule.
        /// </summary>
        /// <param name="filter">Filter predicate.</param>
        /// <returns>False if this filter not used.</returns>
        public bool RemoveFilter(Predicate<T> filter)
        {
            bool removed = filters.Remove(filter);
            if (removed)
            {
                Refresh();
            }
            return removed;
        }

        /// <summary>
        /// Remove all filtering rules.
        /// </summary>
        public void RemoveAllFilters()
        {
            filters.Clear();
            Refresh();
        }

        /// <summary>
        /// Refresh filtered collection.
        /// <para>Use this in case of filter condition change.</para>
        /// </summary>
        public virtual void Refresh()
        {
            NotifyPropertyChange("Items");
        }

        private void BaseCollectionOnChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChange("Items");
        }

        public FilteredObservableWithSorting(ObservableCollection<T> collection)
        {
            BaseCollection = collection;
            EntityCount = collection.Count;
        }

        public FilteredObservableWithSorting(IEnumerable<T> collection)
        {
            BaseCollection = new ObservableCollection<T>(collection);
            EntityCount = BaseCollection.Count;
        }

        public FilteredObservableWithSorting()
        {
            BaseCollection = new ObservableCollection<T>();
        }

        protected void NotifyPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyThisPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
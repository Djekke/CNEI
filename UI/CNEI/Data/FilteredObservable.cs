namespace CryoFall.CNEI.UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class FilteredObservable<T> : INotifyPropertyChanged
    {
        private protected ObservableCollection<T> baseCollection;

        private protected ObservableCollection<T> items = new ObservableCollection<T>();

        private protected List<Predicate<T>> filters = new List<Predicate<T>>();

        private int entityCount = 0;

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
                items = new ObservableCollection<T>(baseCollection.Where(x => filters.All(f => f(x))));
                EntityCount = items.Count;
                return items;
            }
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

        public FilteredObservable(ObservableCollection<T> collection)
        {
            BaseCollection = collection;
            EntityCount = collection.Count;
        }

        public FilteredObservable(IEnumerable<T> collection)
        {
            BaseCollection = new ObservableCollection<T>(collection);
            EntityCount = BaseCollection.Count;
        }

        public FilteredObservable()
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
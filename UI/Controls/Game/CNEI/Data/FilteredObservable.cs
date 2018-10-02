namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;

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
            get => this.baseCollection;
            set
            {
                // emulate VB's "with events" properties.
                if (null != this.baseCollection)
                    this.baseCollection.CollectionChanged -= this.BaseCollectionOnChange;

                this.baseCollection = value;
                this.baseCollection.CollectionChanged += this.BaseCollectionOnChange;

                this.NotifyThisPropertyChanged();
                this.NotifyPropertyChange("Items");
            }
        }

        /// <summary>
        /// Filtered item collection.
        /// </summary>
        public virtual ObservableCollection<T> Items
        {
            get
            {
                this.items.Clear();
                this.items = new ObservableCollection<T>(this.baseCollection.Where(x => this.filters.All(f => f(x))));
                this.EntityCount = this.items.Count;
                return this.items;
            }
        }

        /// <summary>
        /// Add item to base collection.
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            this.baseCollection.Add(item);
        }

        /// <summary>
        /// Check if item in base collection.
        /// </summary>
        /// /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains([NotNull] T item)
        {
            return this.baseCollection.Contains(item);
        }

        /// <summary>
        /// Remove item from base collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove([CanBeNull] T item)
        {
            return this.baseCollection.Remove(item);
        }

        /// <summary>
        /// Get the number of elements actually contained in base collection.
        /// </summary>
        /// <returns>Return the number of elements in base collection.</returns>
        public int Count()
        {
            return this.baseCollection.Count;
        }

        /// <summary>
        /// Get the number of elements contained in filtered collection.
        /// </summary>
        public int EntityCount
        {
            get => this.entityCount;
            private protected set
            {
                if (value == this.entityCount)
                {
                    return;
                }
                this.entityCount = value;
                this.NotifyThisPropertyChanged();
            }
        }

        /// <summary>
        /// Add filtering rule.
        /// </summary>
        /// <param name="filter">Filter predicate.</param>
        /// <returns>False if this filter already exist.</returns>
        public bool AddFilter(Predicate<T> filter)
        {
            if (!this.filters.Contains(filter))
            {
                this.filters.Add(filter);
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
            bool removed = this.filters.Remove(filter);
            if (removed)
            {
                Refresh();
            }
            return removed;
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
            this.BaseCollection = collection;
            this.EntityCount = collection.Count;
        }

        public FilteredObservable(IEnumerable<T> collection)
        {
            this.BaseCollection = new ObservableCollection<T>(collection);
            this.EntityCount = this.BaseCollection.Count;
        }

        public FilteredObservable()
        {
            this.BaseCollection = new ObservableCollection<T>();
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
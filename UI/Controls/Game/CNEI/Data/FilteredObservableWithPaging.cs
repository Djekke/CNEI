namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Scripting;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class FilteredObservableWithPaging<T> : FilteredObservable<T>
    {
        private int currentPage = 0;

        private int pageCount = 0;

        private int pageCapacity = 1;

        public int CurrentPage
        {
            get => this.currentPage;
            private set
            {
                if (value == this.currentPage)
                {
                    return;
                }
                this.currentPage = value;
                this.NotifyThisPropertyChanged();
                this.Refresh();
            }
        }

        public int PageCount
        {
            get => this.pageCount;
            private set
            {
                if (value == this.pageCount)
                {
                    return;
                }
                this.pageCount = value;
                if (this.CurrentPage > this.pageCount)
                {
                    this.CurrentPage = this.pageCount;
                }
                this.NotifyThisPropertyChanged();
                this.Refresh();
            }
        }

        public bool IsFirstPage => this.currentPage <= 1;

        public bool IsLastPage => this.currentPage == this.pageCount;

        public bool IsNotFirstPage => !this.IsFirstPage;

        public bool IsNotLastPage => !this.IsLastPage;

        public override ObservableCollection<T> Items
        {
            get
            {
                this.items.Clear();
                // Apply filters.
                var tempCollection = this.BaseCollection.Where(x => this.Filters.All(f => f(x))).ToList();
                this.EntityCount = tempCollection.Count;
                this.RefreshPageCount();
                // Apply pagination.
                if (this.EntityCount > 0)
                {
                    int offset = this.pageCapacity * (this.CurrentPage - 1);
                    this.items = new ObservableCollection<T>(tempCollection
                            .GetRange(0 + offset, Math.Min(this.pageCapacity, this.EntityCount - offset)));
                    return this.items;
                }
                return items;
            }
        }

        public override void Refresh()
        {
            this.NotifyPropertyChange("Items");
        }

        private void RefreshPageCount()
        {
            var newCount = this.EntityCount / this.pageCapacity +
                (this.EntityCount % this.pageCapacity == 0 ? 0 : 1);
            if (this.PageCount != newCount)
            {
                this.pageCount = newCount;
                this.NotifyPropertyChange("PageCount");
                if (this.CurrentPage > this.pageCount)
                {
                    this.currentPage = this.pageCount;
                    this.NotifyPropertyChange("CurrentPage");
                }
                if (this.CurrentPage == 0 && this.pageCount > 0 )
                {
                    this.currentPage = 1;
                    this.NotifyPropertyChange("CurrentPage");
                }
            }
            this.NotifyPropertyChange("IsFirstPage");
            this.NotifyPropertyChange("IsLastPage");
            this.NotifyPropertyChange("IsNotFirstPage");
            this.NotifyPropertyChange("IsNotLastPage");
        }

        public void SetPageCapacity(int capacity)
        {
            if (capacity == 0)
            {
                Api.Logger.Error("What idiot set page capacity for " + this + " to zero?");
                return;
            }
            if (this.pageCapacity != capacity)
            {
                this.pageCapacity = capacity;
                this.Refresh();
            }
        }

        public void NextPage()
        {
            if (this.currentPage < this.PageCount)
            {
                this.CurrentPage += 1;
            }
        }

        public void PrevPage()
        {
            if (this.currentPage > 0)
            {
                this.CurrentPage -= 1;
            }
        }

        public FilteredObservableWithPaging(IEnumerable<T> collection) : base(collection) { }

        public FilteredObservableWithPaging(ObservableCollection<T> collection) : base(collection) { }

        public FilteredObservableWithPaging() : base() { }
    }
}
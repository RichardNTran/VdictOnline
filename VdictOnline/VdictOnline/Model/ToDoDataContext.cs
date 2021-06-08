using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
namespace VdictOnline.Model
{
    public class ToDoDataContext : DataContext
    {
        // Pass the connection string to the base class.
        public ToDoDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the to-do items.
      
        public Table<ToDoItem> Items;

        // Specify a table for the categories.
        public Table<ToDoCategory> Categories;
    }

    [Table]
    public class ToDoItem : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _toDoItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ToDoItemId
        {
            get { return _toDoItemId; }
            set
            {
                if (_toDoItemId != value)
                {
                    NotifyPropertyChanging("ToDoItemId");
                    _toDoItemId = value;
                    NotifyPropertyChanged("ToDoItemId");
                }
            }
        }

        // Define item name: private field, public property, and database column.
        private string _itemName;

          [Column(DbType = "NText")] 
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (_itemName != value)
                {
                    NotifyPropertyChanging("ItemName");
                    _itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }
        private string _keyword;
         [Column(DbType = "NText")] 
        public string KeyWord
        {
            get { return _keyword; }
            set
            {
                if (_keyword != value)
                {
                    NotifyPropertyChanging("KeyWord");
                    _keyword = value;
                    NotifyPropertyChanged("KeyWord");
                }
            }
        }
         private string _pronounce;
         [Column(DbType = "NText")]
         public string Pronounce
         {
             get { return _pronounce; }
             set
             {
                 if (_pronounce != value)
                 {
                     NotifyPropertyChanging("Pronounce");
                     _pronounce = value;
                     NotifyPropertyChanged("Pronounce");
                 }
             }
         }
        private string _meanword1;
        [Column(DbType = "NText")] 
        public string MeanWord1
        {
            get { return _meanword1; }
            set
            {
                if (_meanword1 != value)
                {
                    NotifyPropertyChanging("MeanWord1");
                    _meanword1 = value;
                    NotifyPropertyChanged("MeanWord1");
                }
            }
        }
        private string _meanword2;
        [Column(DbType = "NText")]
        public string MeanWord2
        {
            get { return _meanword2; }
            set
            {
                if (_meanword2 != value)
                {
                    NotifyPropertyChanging("MeanWord2");
                    _meanword2 = value;
                    NotifyPropertyChanged("MeanWord2");
                }
            }
        }
        private string _meanword3;
        [Column(DbType = "NText")]
        public string MeanWord3
        {
            get { return _meanword3; }
            set
            {
                if (_meanword3 != value)
                {
                    NotifyPropertyChanging("MeanWord3");
                    _meanword3 = value;
                    NotifyPropertyChanged("MeanWord3");
                }
            }
        }
        private string _meanword4;
        [Column(DbType = "NText")]
        public string MeanWord4
        {
            get { return _meanword4; }
            set
            {
                if (_meanword4 != value)
                {
                    NotifyPropertyChanging("MeanWord4");
                    _meanword4 = value;
                    NotifyPropertyChanged("MeanWord4");
                }
            }
        }
        private string _meanword5;
        [Column(DbType = "NText")]
        public string MeanWord5
        {
            get { return _meanword5; }
            set
            {
                if (_meanword5 != value)
                {
                    NotifyPropertyChanging("MeanWord5");
                    _meanword5 = value;
                    NotifyPropertyChanged("MeanWord5");
                }
            }
        }
        private string _meanword6;
        [Column(DbType = "NText")]
        public string MeanWord6
        {
            get { return _meanword6; }
            set
            {
                if (_meanword6 != value)
                {
                    NotifyPropertyChanging("MeanWord6");
                    _meanword6 = value;
                    NotifyPropertyChanged("MeanWord6");
                }
            }
        }
        private string _meanword7;
        [Column(DbType = "NText")]
        public string MeanWord7
        {
            get { return _meanword7; }
            set
            {
                if (_meanword7 != value)
                {
                    NotifyPropertyChanging("MeanWord7");
                    _meanword7 = value;
                    NotifyPropertyChanged("MeanWord7");
                }
            }
        }
        private string _meanword8;
        [Column(DbType = "NText")]
        public string MeanWord8
        {
            get { return _meanword8; }
            set
            {
                if (_meanword8 != value)
                {
                    NotifyPropertyChanging("MeanWord8");
                    _meanword8 = value;
                    NotifyPropertyChanged("MeanWord8");
                }
            }
        }
        private string _meanword9;
        [Column(DbType = "NText")]
        public string MeanWord9
        {
            get { return _meanword9; }
            set
            {
                if (_meanword1 != value)
                {
                    NotifyPropertyChanging("MeanWord9");
                    _meanword9 = value;
                    NotifyPropertyChanged("MeanWord9");
                }
            }
        }
        // Define completion value: private field, public property, and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;


        // Internal column for the associated ToDoCategory ID value
        [Column]
        internal int _categoryId;

        // Entity reference, to identify the ToDoCategory "storage" table
        private EntityRef<ToDoCategory> _category;

        // Association, to describe the relationship between this key and that "storage" table
        [Association(Storage = "_category", ThisKey = "_categoryId", OtherKey = "Id", IsForeignKey = true)]
        public ToDoCategory Category
        {
            get { return _category.Entity; }
            set
            {
                NotifyPropertyChanging("Category");
                _category.Entity = value;

                if (value != null)
                {
                    _categoryId = value.Id;
                }

                NotifyPropertyChanging("Category");
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }


    [Table]
    public class ToDoCategory : INotifyPropertyChanged, INotifyPropertyChanging
    {

        // Define ID: private field, public property, and database column.
        private int _id;

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        // Define category name: private field, public property, and database column.
        private string _name;

        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        // Define the entity set for the collection side of the relationship.
        private EntitySet<ToDoItem> _todos;

        [Association(Storage = "_todos", OtherKey = "_categoryId", ThisKey = "Id")]
        public EntitySet<ToDoItem> ToDos
        {
            get { return this._todos; }
            set { this._todos.Assign(value); }
        }


        // Assign handlers for the add and remove operations, respectively.
        public ToDoCategory()
        {
            _todos = new EntitySet<ToDoItem>(
                new Action<ToDoItem>(this.attach_ToDo),
                new Action<ToDoItem>(this.detach_ToDo)
                );
        }

        // Called during an add operation
        private void attach_ToDo(ToDoItem toDo)
        {
            NotifyPropertyChanging("ToDoItem");
            toDo.Category = this;
        }

        // Called during a remove operation
        private void detach_ToDo(ToDoItem toDo)
        {
            NotifyPropertyChanging("ToDoItem");
            toDo.Category = null;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }


}

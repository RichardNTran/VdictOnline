using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdictOnline.Model;
namespace VdictOnline.ViewModel
{
    public class ToDoViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private ToDoDataContext toDoDB;

        // Class constructor, create the data context object.
        public ToDoViewModel(string toDoDBConnectionString)
        {
            toDoDB = new ToDoDataContext(toDoDBConnectionString);
        }

        // All to-do items.
        private ObservableCollection<ToDoItem> _allToDoItems;
        public ObservableCollection<ToDoItem> AllToDoItems
        {
            get { return _allToDoItems; }
            set
            {
                _allToDoItems = value;
                NotifyPropertyChanged("AllToDoItems");
            }
        }

        // To-do items associated with the home category.
        private ObservableCollection<ToDoItem> _englishItems;
        public ObservableCollection<ToDoItem> EnglishItems
        {
            get { return _englishItems; }
            set
            {
                _englishItems = value;
                NotifyPropertyChanged("EnglishItems");
            }
        }

        // To-do items associated with the work category.
        private ObservableCollection<ToDoItem> _franceItems;
        public ObservableCollection<ToDoItem> FranceItems
        {
            get { return _franceItems; }
            set
            {
                _franceItems = value;
                NotifyPropertyChanged("FranceItems");
            }
        }

        // To-do items associated with the hobbies category.
        private ObservableCollection<ToDoItem> _vietnamItems;
        public ObservableCollection<ToDoItem> VietNamItems
        {
            get { return _vietnamItems; }
            set
            {
                _vietnamItems = value;
                NotifyPropertyChanged("VietNamItems");
            }
        }

        // A list of all categories, used by the add task page.
        private List<ToDoCategory> _categoriesList;
        public List<ToDoCategory> CategoriesList
        {
            get { return _categoriesList; }
            set
            {
                _categoriesList = value;
                NotifyPropertyChanged("CategoriesList");
            }
        }

        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            toDoDB.SubmitChanges();
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all to-do items in the database.
            var toDoItemsInDB = from ToDoItem todo in toDoDB.Items
                                select todo;

            // Query the database and load all to-do items.
            AllToDoItems = new ObservableCollection<ToDoItem>(toDoItemsInDB);

            // Specify the query for all categories in the database.
            var toDoCategoriesInDB = from ToDoCategory category in toDoDB.Categories
                                     select category;


            // Query the database and load all associated items to their respective collections.
            foreach (ToDoCategory category in toDoCategoriesInDB)
            {
                switch (category.Name)
                {
                    case "English":
                        EnglishItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "France":
                        FranceItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "VietNam":
                        VietNamItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    default:
                        break;
                }
            }

            // Load a list of all categories.
            CategoriesList = toDoDB.Categories.ToList();

        }

        // Add a to-do item to the database and collections.
        public void AddToDoItem(ToDoItem newToDoItem)
        {
            // Add a to-do item to the data context.
      //    if (newToDoItem.MeanWord.Length > 3900) newToDoItem.MeanWord = newToDoItem.MeanWord.Substring(0, 3900);
            toDoDB.Items.InsertOnSubmit(newToDoItem);

            // Save changes to the database.
            toDoDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            try
            {
                AllToDoItems.Add(newToDoItem);


                // Add a to-do item to the appropriate filtered collection.
                switch (newToDoItem.Category.Name)
                {
                    case "English":
                        EnglishItems.Add(newToDoItem);
                        break;
                    case "France":
                        FranceItems.Add(newToDoItem);
                        break;
                    case "VietNam":
                        VietNamItems.Add(newToDoItem);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                while (ex2.InnerException != null)
                {
                    ex2 = ex2.InnerException;
                }
                Console.WriteLine(ex.InnerException);
                throw;
            }
        }

        // Remove a to-do task item from the database and collections.
        public void DeleteToDoItem(ToDoItem toDoForDelete)
        {

            // Remove the to-do item from the "all" observable collection.
            AllToDoItems.Remove(toDoForDelete);

            // Remove the to-do item from the data context.
            toDoDB.Items.DeleteOnSubmit(toDoForDelete);

            // Remove the to-do item from the appropriate category.   
            switch (toDoForDelete.Category.Name)
            {
                case "English":
                    EnglishItems.Remove(toDoForDelete);
                    break;
                case "France":
                    FranceItems.Remove(toDoForDelete);
                    break;
                case "VietNam":
                    VietNamItems.Remove(toDoForDelete);
                    break;
                default:
                    break;
            }

            // Save changes to the database.
            toDoDB.SubmitChanges();
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

}

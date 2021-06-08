using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using VdictOnline.Model;

namespace VdictOnline
{
    public partial class Myword : PhoneApplicationPage
    {
        private ObservableCollection<MyListLanguage> myLanguage;
        String[] Country = { "English", "French", "Italia", "Viet Nam" };
        public Myword()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                ToDoItem toDoForDelete = button.DataContext as ToDoItem;

                App.ViewModel.DeleteToDoItem(toDoForDelete);
            }

            // Put the focus back to the main page.
            this.Focus();
        }

     


      
    }


}
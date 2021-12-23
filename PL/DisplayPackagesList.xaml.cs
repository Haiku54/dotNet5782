﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using PL;

namespace PL
{

    /// <summary>
    /// Interaction logic for DisplayPackagesList.xaml
    /// </summary>
    public partial class DisplayPackagesList : Page
    {
        BlApi.IBL BL;
        Model.PL PL;
        MainWindow MainWindow;
        private ObservableCollection<BO.PackageToList> packages = new ObservableCollection<BO.PackageToList>();

        public DisplayPackagesList(MainWindow mainWindow)
        {
            InitializeComponent();
            this.BL = BlApi.BlFactory.GetBL();
            this.PL  = new Model.PL();
            this.MainWindow = mainWindow;
            PackageListView.DataContext = packages;
            InitializeList();

            PrioritySelector.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(BO.PackageStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
        }

        private void InitializeList()
        {
            foreach (var Package in PL.getPackageList())
            {
                packages.Add(Package);
            }
        }

        private void Add_New_Package(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame.Content = new DisplayPackage(MainWindow, PL);
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow.Frame.Content = new DisplayPackage(MainWindow, PL, PL.GetPackage(((BO.PackageToList)PackageListView.SelectedItem).Id));
        }

        private void ExitButton(object sender, RoutedEventArgs e)
        {

        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            packages.Clear();
            InitializeList();
            Show_Packages(this, new RoutedEventArgs()); // אחרי סינון
        }


        private void Show_Packages(object sender, RoutedEventArgs e)
        {
            if (PackageListView != null)
            {
                if(Show_Normally.IsChecked == true)
                {
                    var p = (packages.OrderBy(p => p.Id)).ToList();
                    packages.Clear();
                    foreach (var item in p) { packages.Add(item); }
                }
                else if(Show_Receiver.IsChecked == true)
                {
                    var p = packages.OrderBy(p => p.Id).GroupBy(p => p.Receiver).ToList();
                    packages.Clear();
                    foreach (var group in p)
                    {
                        foreach (BO.PackageToList item in group)
                        { packages.Add(item); }
                    }
                }
                else
                {
                    var p = packages.OrderBy(p => p.Id).GroupBy(p => p.Sender).ToList(); // לרשימה כדי שיהיה העתק . לפי הסדר כדי יחזור למקורי ואז ימיין
                    packages.Clear();
                    foreach (var group in p)
                    {
                        foreach (BO.PackageToList item in group)
                        {packages.Add(item);}
                    }
                }
            }
        }



        private void FilterdList(object sender, SelectionChangedEventArgs e = null) // מעדכנת את הרשימה בהתאם
        {
            IEnumerable<BO.PackageToList> filtered ;
            if (PrioritySelector.SelectedItem == null && WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null)filtered = PL.getPackageList();
            else if (PrioritySelector.SelectedItem == null && WeightSelector.SelectedItem == null && StatusSelector.SelectedItem != null) filtered = PL.getPackageList().Where(p => p.Status == (BO.PackageStatus)StatusSelector.SelectedItem).ToList();
            else if (PrioritySelector.SelectedItem == null && WeightSelector.SelectedItem != null && StatusSelector.SelectedItem != null) filtered = PL.getPackageList().Where(p => p.Status == (BO.PackageStatus)StatusSelector.SelectedItem && p.Weight == (BO.WeightCategories)WeightSelector.SelectedItem).ToList();
            else if (PrioritySelector.SelectedItem != null && WeightSelector.SelectedItem == null && StatusSelector.SelectedItem != null) filtered = PL.getPackageList().Where(p => p.Status == (BO.PackageStatus)StatusSelector.SelectedItem && p.Priority == (BO.Priorities)PrioritySelector.SelectedItem).ToList();

            else if (PrioritySelector.SelectedItem == null && WeightSelector.SelectedItem != null && StatusSelector.SelectedItem == null) filtered = PL.getPackageList().Where(p => p.Weight == (BO.WeightCategories)WeightSelector.SelectedItem).ToList();
            else if (PrioritySelector.SelectedItem != null && WeightSelector.SelectedItem != null && StatusSelector.SelectedItem == null) filtered = PL.getPackageList().Where(p => p.Weight == (BO.WeightCategories)WeightSelector.SelectedItem && p.Priority == (BO.Priorities)PrioritySelector.SelectedItem).ToList();

            else if (PrioritySelector.SelectedItem != null && WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null) filtered = PL.getPackageList().Where(p => p.Priority == (BO.Priorities)PrioritySelector.SelectedItem).ToList();

            else filtered = PL.getPackageList().Where(p => p.Status == (BO.PackageStatus)StatusSelector.SelectedItem && p.Priority == (BO.Priorities)WeightSelector.SelectedItem && p.Weight == (BO.WeightCategories)WeightSelector.SelectedItem).ToList();
            packages.Clear();
            if(filtered != null) foreach (var package in filtered) { packages.Add(package); }
            Show_Packages(this, new RoutedEventArgs()); // אחרי סינון צריך לשמור על ההצגה
       
        }

        private void FilterdByDate(object sender, SelectionChangedEventArgs e)
        {
            if(StartDate.SelectedDate != null && EndDate.SelectedDate != null)
            {
                FilterdList(this); // שמירה על הסינון ללא בחירת תאריכים קודמת
                var listFilterdByDate = BL.GetPackageFilterByDate((DateTime)StartDate.SelectedDate, (DateTime)EndDate.SelectedDate);
                List<BO.PackageToList> temp = new List<BO.PackageToList>();

                if (packages != null)
                {
                    foreach (var BlPackage in listFilterdByDate)
                    {
                        if (packages.Any(p => p.Id == BlPackage.Id))
                        {
                            BO.PackageToList packageToList = new BO.PackageToList();
                            BlPackage.CopyPropertiesTo(packageToList);
                            temp.Add(packageToList);
                        }
                    }
                }

                packages.Clear();
                foreach (var tempPackage in temp)
                {
                    packages.Add(tempPackage); // // אם אחרי הסינון ללא התאריך הוא קיים ברשימה של הסינון וגם ברשימה של התאריך אז נכניס אותו
                }

                Show_Packages(this, new RoutedEventArgs()); // אחרי סינון
            }
        }
    }
}
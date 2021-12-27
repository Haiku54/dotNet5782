﻿using System;
using System.Collections.Generic;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL BL;
        object content;

        public MainWindow()
        {
            InitializeComponent();
            BL = BlApi.BlFactory.GetBL();

        }



     

        public void DisplayMain()
        {
            Frame.Visibility = Visibility.Hidden;
            MainWindowDisplay.Visibility = Visibility.Visible;
            this.Content = content;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindowDisplay.Visibility = Visibility.Hidden;
            DisplayPackagesList page = new DisplayPackagesList();
            page.AddClik += AddPackagePage;
            page.DoubleClik += PackageDisplyPage;
            this.Frame.Content = page;
        }

        private void AddPackagePage(int num) // פתיחת עמוד הוספת חבילה 
        {
            var page = new DisplayPackage();
            page.Back += ((DisplayPackagesList)this.Frame.Content).RefreshList;
            this.Frame.Content = page;
        }
        private void AddDronePage(int num) // פתיחת עמוד הוספת רחפן 
        {
            var page = new DisplayDrone();
            page.Back += ((DisplayDronesList)this.Frame.Content).RefreshList;
            this.Frame.Content = page;
        }
        private void AddClientPage(int num) // פתיחת חלון הוספת לקוח
        {
            var page = new DisplayClient();
            page.Back += ((DisplayClientsList)this.Frame.Content).RefreshList;
            this.Frame.Content = page;
        }

        private void PackageDisplyPage(int id)// פתיחת חלון פעולות חבילה
        {
            var page = new DisplayPackage(id);
            page.Back += ((DisplayPackagesList)this.Frame.Content).RefreshList;
            page.ClientPage += ClientDisplyPageFromePackage;
            page.DronePage += DroneDisplyPageFromePackage;
            this.Frame.Content = page;
        }

        private void DroneDisplyPage(int id)//פתיחת עמוד פעולות רחפן
        {
            var page = new DisplayDrone(id);
            page.Back += ((DisplayDronesList)this.Frame.Content).RefreshList;
            this.Frame.Content = page;
        }

        private void ClientDisplyPage(int id)// פתיחת חלון פעולות לקוח
        {
            var page = new DisplayClient(id);
            page.Back += ((DisplayClientsList)this.Frame.Content).RefreshList;
            this.Frame.Content = page;
        }

        private void AddStationPage(int num) // פתיחת חלון הוספת תחנה
        {
            var page = new DisplayStation();
            page.Back += ((DisplayStationsList)this.Frame.Content).RefreshList;
            this.Frame.Content = page;
        }

        private void StationDisplyPage(int id)// פתיחת חלון פעולות תחנה
        {
            var page = new DisplayStation(id);
            page.Back += ((DisplayStationsList)this.Frame.Content).RefreshList;
            this.Frame.Content = page;
        }

        private void ClientDisplyPageFromePackage(int id)//  פתיחת חלון פעולות לקוח מתוך עמוד עדכון חבילה
        {
            var page = new DisplayClient(id);
            this.Frame.Content = page;
        }

        private void DroneDisplyPageFromePackage(int id)//  פתיחת חלון פעולות רחפן מתוך עמוד פעולות חבילה
        {
            var page = new DisplayDrone(id);
            this.Frame.Content = page;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           
            DisplayClientsList page = new DisplayClientsList();
            page.AddClik += AddClientPage;
            page.DoubleClik += ClientDisplyPage;
            this.Frame.Content = page;
        }

        private void Stations_Click(object sender, RoutedEventArgs e)
        {
           
            DisplayStationsList page = new DisplayStationsList();
            page.AddClik += AddStationPage;
            page.DoubleClik += StationDisplyPage;
            this.Frame.Content = page;
        }

        private void Drones_List_Click(object sender, RoutedEventArgs e)
        {
            MainWindowDisplay.Visibility = Visibility.Hidden;
            DisplayDronesList page = new DisplayDronesList();
            page.AddClik += AddDronePage;
            page.DoubleClik += DroneDisplyPage;
            this.Frame.Content = page;
        }


        private void Manager_login_Click_1(object sender, RoutedEventArgs e)
        {
            Buttons_For_Lists.Visibility = Visibility.Visible;
            MainWindowDisplay.Visibility = Visibility.Hidden;
        }

        private void Sign_Up_Click(object sender, RoutedEventArgs e)
        {
            new SignUpClient().Show();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Content = new ClientMde();
        }
    }
}
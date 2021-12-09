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
using System.Windows.Shapes;

using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace PL
{
    /// <summary>
    /// Interaction logic for DisplaysDrone.xaml
    /// </summary>
    public partial class DisplaysDrone : Window
    {
        IBL.IBL BL;
        IBL.BO.Drone selectedDrone;


        //***
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //**
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }



        //*****


        public DisplaysDrone(IBL.IBL bL)
        {
            InitializeComponent();
            this.BL = bL;
            Add_New_Drone.Visibility = Visibility.Visible;
            Drone_Weight.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            Stations_List.ItemsSource = BL.DisplayStationListWitAvailableChargingSlots();
            Add_New_Drone.Visibility = Visibility.Visible;


        }


        public DisplaysDrone(IBL.IBL bL, IBL.BO.DroneToList drone, DisplaysDronesList droneListWindow)
        {
            InitializeComponent();
            this.BL = bL;
            
            Initialize(drone.ID);
      
        }




        void Initialize(int DroneId)
        {
            selectedDrone = BL.DisplayDrone(DroneId);

            UpdateDrone.Visibility = Visibility.Visible;
            UpdateDrone.Visibility = Visibility.Visible;

            ID.Text = $"{selectedDrone.ID}";
            Battery.Text = $"{selectedDrone.Battery}";

            WeightSelector.Text = selectedDrone.MaxWeight.ToString();

            Model_Input.Text = selectedDrone.Model;

            StatusSelector.Text = selectedDrone.Status.ToString();

           // Delivery.Text = $"{selectedDrone.DronePackageProcess.Id}";
            Latitude.Text = $"{selectedDrone.DroneLocation.Latitude}";
            Longitude.Text = $"{selectedDrone.DroneLocation.Longitude}";

            if (selectedDrone.DronePackageProcess == null)
            {
                Package_Process.Visibility = Visibility.Hidden;
                PackageProcess_Label.Visibility = Visibility.Hidden;
            }
            else Package_Process.Text = $"{selectedDrone.DronePackageProcess}";

            ID.IsReadOnly = true;
            Battery.IsReadOnly = true;

            switch (selectedDrone.Status)
            {
                case IBL.BO.DroneStatus.Available:
                    ChargeButton.IsEnabled = true;
                    AssociateButton.IsEnabled = true;
                    ReleaseButton.IsEnabled = false;
                    PickUpButton.IsEnabled = false;
                    DeliverButton.IsEnabled = false;
                    break;

                case IBL.BO.DroneStatus.Maintenance:
                    ReleaseButton.IsEnabled = true;
                    ChargeButton.IsEnabled = false;
                    AssociateButton.IsEnabled = false;
                    PickUpButton.IsEnabled = false;
                    DeliverButton.IsEnabled = false;
                    break;

                case IBL.BO.DroneStatus.Shipping:
                    switch (selectedDrone.DronePackageProcess.PackageShipmentStatus)
                    {
                        case IBL.BO.ShipmentStatus.Waiting:
                            PickUpButton.IsEnabled = true;
                            ReleaseButton.IsEnabled = false;
                            ChargeButton.IsEnabled = false;
                            AssociateButton.IsEnabled = false;
                            DeliverButton.IsEnabled = false;
                            break;

                        case IBL.BO.ShipmentStatus.OnGoing:
                            DeliverButton.IsEnabled = true;
                            ReleaseButton.IsEnabled = false;
                            ChargeButton.IsEnabled = false;
                            AssociateButton.IsEnabled = false;
                            PickUpButton.IsEnabled = false;
                            break;

                    }
                    break;

            }




        }



        public delegate void RefreshList(object ob);
        public event RefreshList RefreshListEvent;

        private void idInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (IdInput.Text != null && IdInput.Text != string.Empty && (IdInput.Text).All(char.IsDigit))
            {
                IdInput.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
            }
            else IdInput.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");

        }

        private void DroneModel_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            string text = DroneModel.Text;
            if (text != null && text != "" && char.IsLetter(text.ElementAt(0)))
            {
                DroneModel.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
            }
            else DroneModel.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void StationID_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            if (StationID.Text != null && StationID.Text != string.Empty && (StationID.Text).All(char.IsDigit))
            {
                StationID.BorderBrush = (Brush)bc.ConvertFrom("#FF99B4D1");
            }
            else StationID.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DroneWindow.Close();
        }

        private void Add_Drone_Button_Click(object sender, RoutedEventArgs e)
        { 
            SolidColorBrush red = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFE92617"));
            if (SolidColorBrush.Equals(((SolidColorBrush)StationID.BorderBrush).Color , red.Color) || SolidColorBrush.Equals(((SolidColorBrush)DroneModel.BorderBrush).Color , red.Color)
                || SolidColorBrush.Equals(((SolidColorBrush)IdInput.BorderBrush).Color , red.Color) || Drone_Weight.Style == (Style)this.FindResource("ComboBoxTest2"))
            {
                MessageBox.Show("Please enter correct input","Error input" ,MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                IBL.BO.Drone drone = new IBL.BO.Drone();
                drone.ID = int.Parse(IdInput.Text);
                drone.Model = DroneModel.Text;
                drone.MaxWeight = (IBL.BO.WeightCategories)Drone_Weight.SelectedItem;
                try
                {
                    BL.AddDrone(drone, int.Parse(StationID.Text));

                    MessageBox.Show("The addition was successful", "Added a drone", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshListEvent(this);
                    DroneWindow.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Drone_Weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if(Drone_Weight.SelectedItem != null )
            {
                Drone_Weight.Style = (Style)this.FindResource("ComboBoxTestAfterCorrectInput");
            }
            else Drone_Weight.Style = (Style)this.FindResource("ComboBoxTest2");
        }

        

        private void Stations_List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Stations_List.SelectedItem = Stations_List.SelectedItem.ToString().ElementAt(5);
            StationID.Text = ((IBL.BO.StationToList) Stations_List.SelectedItem).ID.ToString();
        }

        private void ChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.ChargeDrone(selectedDrone.ID);

                MessageBox.Show("The Drone have been sent successfulyl", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Initialize(selectedDrone.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Change_Name_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.UpdateDroneName(selectedDrone.ID, Model_Input.Text);

                MessageBox.Show("Name have been changed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Initialize(selectedDrone.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReleaseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.FinishCharging(selectedDrone.ID);

                MessageBox.Show($"Drone have been unplugged, Battery left: {selectedDrone.Battery}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Initialize(selectedDrone.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AssociateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.packageToDrone(selectedDrone.ID);

                MessageBox.Show("Package have been Associated to drone successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Initialize(selectedDrone.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PickUpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.PickedUpByDrone(selectedDrone.ID);

                MessageBox.Show("Package have been picked up successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Initialize(selectedDrone.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeliverButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL.DeliveredToClient(selectedDrone.ID);

                MessageBox.Show("Package have been delivered successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Initialize(selectedDrone.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshListEvent(this);
            Close();
            
        }

        private void Model_Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            string text = Model_Input.Text;
            if (text != null && text != "" && char.IsLetter(text.ElementAt(0)))
            {
                Model_Input.BorderBrush = (Brush)bc.ConvertFrom("#FFABADB3");
            }
            else Model_Input.BorderBrush = (Brush)bc.ConvertFrom("#FFE92617");
        }
    }
}



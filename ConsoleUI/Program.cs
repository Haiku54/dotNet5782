﻿using System;
using IDAL.DO;
using DalObject;


namespace ConsoleUI
{
    class Program
    {
        enum Menu {Exit, Add, Update, DisplayItem, DisplayList };
        enum ObjectMenu { Exit, Client, Drone, Station, Package }
        enum UpdateOptions { Exit, Assignment, PickedUp, Delivered, Charging, FinishCharging }
        enum ObjectList { Exit, ClientList, DroneList, StationList, PackageList, PackageWithoutDrone, StationWithCharging };

        public static void Display()
        {
            int num = 1;
            Menu choice;
            ObjectMenu objectMenu;
            UpdateOptions updateOptions;
            ObjectList objectList;

            while (num != 0)
            {
                Console.WriteLine("Choose an Option: \n");
                Console.WriteLine(" 1: Add \n 2: Update \n 3: Display Item \n 4: Display List \n 0: Exit");
                choice = (Menu)int.Parse(Console.ReadLine());
            

                switch (choice)
                {

                    case Menu.Add:
                        Console.WriteLine("Choose an Adding Option: \n 1 : Client \n 2 : Drone \n 3 : Station: \n 4 : Package \n ");
                        objectMenu = (ObjectMenu)int.Parse(Console.ReadLine());

                        switch (objectMenu)
                        {
                            case ObjectMenu.Client:
                                
                                Console.WriteLine("Enter Client Data: Name, Phone, Latitude, Longitude  \n");
                                int clientId = Convert.ToInt32(Console.ReadLine());
                                string clientName = Console.ReadLine();
                                string clientPhone = Console.ReadLine();
                                double clientLatitude = Convert.ToInt32(Console.ReadLine());
                                double clientLongitude = Convert.ToInt32(Console.ReadLine());

                                Client client = new Client();
                                
                                client.ID = clientId;
                                client.Name = clientName;
                                client.Phone = clientPhone;
                                client.Latitude = clientLatitude;
                                client.Longitude = clientLongitude;
                               
                                DalObject.DalObject.AddClient(client);

                                break;
                            case ObjectMenu.Drone:
                                Console.WriteLine("Enter Drone Data\n");



                                break;
                            case ObjectMenu.Station:


                                break;
                            case ObjectMenu.Package:


                                break;
                            default:
                                break;
                        }
                        break;
                    case Menu.Update:
                        Console.WriteLine("Choose an Option: \n");
                        Console.WriteLine(" 1: Assigning a package to a drone \n 2: pick Up Package by Drone \n 3: Delivery of a package to the client: \n 4: Charging drone \n 5: Finish charging drone \n 0: Exit");
                        updateOptions = (UpdateOptions)int.Parse(Console.ReadLine());

                        switch (updateOptions)
                        {
                            case UpdateOptions.Exit:
                                break;
                            case UpdateOptions.Assignment:
                                int droneId, packageId;
                                Console.WriteLine("What is the drone's ID?");
                                droneId = int.Parse(Console.ReadLine());
                                Console.WriteLine("What is the package's ID?");
                                packageId = int.Parse(Console.ReadLine());
                                DalObject.DalObject.packageToDrone(DalObject.DalObject.PackageById(packageId) , DalObject.DalObject.DroneById(droneId));
                                break;

                            case UpdateOptions.PickedUp:
                                Console.WriteLine("What is the package's ID?");
                                packageId = int.Parse(Console.ReadLine());
                                DalObject.DalObject.PickedUpByDrone(DalObject.DalObject.PackageById(packageId));
                                break;

                            case UpdateOptions.Delivered:
                                Console.WriteLine("What is the package's ID?");
                                packageId = int.Parse(Console.ReadLine());
                                DalObject.DalObject.DeliveredToClient(DalObject.DalObject.PackageById(packageId));
                                break;

                            case UpdateOptions.Charging:
                                Console.WriteLine("What is the drone's ID?");
                                droneId = int.Parse(Console.ReadLine());
                                Console.WriteLine("At which station do you want to recharge the drone?\n");
                                foreach (var station in (DalObject.DalObject.StationWithCharging())) 
                                {
                                    Console.WriteLine(station);
                                }
                                Console.WriteLine("What is the station ID ?\n");
                                int stationID = int.Parse(Console.ReadLine());
                                DalObject.DalObject.DroneCharge(DalObject.DalObject.DroneById(droneId), stationID);
                                break;

                            case UpdateOptions.FinishCharging:
                                Console.WriteLine("What is the drone's ID?");
                                droneId = int.Parse(Console.ReadLine());
                                DalObject.DalObject.FinishCharging(DalObject.DalObject.DroneChargeByIdDrone(droneId));
                                break;

                            default:
                                break;
                        }
                        break;

                    case Menu.DisplayItem:
                        Console.WriteLine("Choose the show option: \n 1: Client \n 2: Drone \n 3: Station \n 4: Package \n 0: Exit ");
                        objectMenu = (ObjectMenu)int.Parse(Console.ReadLine());
                        switch (objectMenu)
                        {
                            case ObjectMenu.Exit:
                                break;

                            case ObjectMenu.Client:
                                Console.WriteLine("What is the client's ID?");
                                int clientID = int.Parse(Console.ReadLine());
                                Console.WriteLine(DalObject.DalObject.ClientById(clientID));
                                break;

                            case ObjectMenu.Drone:
                                Console.WriteLine("What is the drone's ID?");
                                int droneID = int.Parse(Console.ReadLine());
                                Console.WriteLine(DalObject.DalObject.DroneById(droneID));
                                break;

                            case ObjectMenu.Station:
                                Console.WriteLine("What is the station's ID?");
                                int stationID = int.Parse(Console.ReadLine());
                                Console.WriteLine(DalObject.DalObject.StationById(stationID));
                                break;

                            case ObjectMenu.Package:
                                Console.WriteLine("What is the package's ID?");
                                int packageID = int.Parse(Console.ReadLine());
                                Console.WriteLine(DalObject.DalObject.PackageById(packageID));
                                break;

                            default:
                                break;
                        }
                        break;

                    case Menu.DisplayList:
                        Console.WriteLine("Choose the show option: \n 1: Clients list \n 2: Drones list\n 3: Stations list \n 4: Packages list\n 5: List of packages that do not belong to the drone \n 6: List of stations with available charging slots \n 0: Exit ");
                        objectList = (ObjectList)int.Parse(Console.ReadLine());
                        switch (objectList)
                        {
                            case ObjectList.Exit:
                                break;

                            case ObjectList.ClientList:
                                foreach (var client in DalObject.DalObject.ClientsList())
                                {
                                    Console.WriteLine(client);
                                }
                                break;

                            case ObjectList.DroneList:
                                foreach (var drone in DalObject.DalObject.DroneList())
                                {
                                    Console.WriteLine(drone);
                                }
                                break;

                            case ObjectList.StationList:
                                foreach (var station in DalObject.DalObject.StationsList())
                                {
                                    Console.WriteLine(station);
                                }
                                break;

                            case ObjectList.PackageList:
                                foreach (var package in DalObject.DalObject.PackageList())
                                {
                                    Console.WriteLine(package);
                                }
                                break;

                            case ObjectList.PackageWithoutDrone:
                                foreach (var package in DalObject.DalObject.PackageWithoutDrone())
                                {
                                    Console.WriteLine(package);
                                }
                                break;

                            case ObjectList.StationWithCharging:
                                foreach (var station in DalObject.DalObject.StationWithCharging())
                                {
                                    Console.WriteLine(station);
                                }
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                       // Console.WriteLine("Invalid \n");
                        break;

                }


            }

        }


        static void Main(string[] args)
        {

            //Console.WriteLine($"Name is {client.Name}, ID = {client.ID}");
            new DalObject.DalObject();
            Display();
            

        }

        //private static void myClient()
        //{
        //    IDAL.DO.Client client = new IDAL.DO.Client
        //    {
        //        ID = 113,
        //        Name = "David",
        //        Latitude = 36.123456,
        //        Longitude = 29.654321,
        //        Phone = "0526137053"
        //    };
        //    Console.WriteLine(client);
        //}
    }
}

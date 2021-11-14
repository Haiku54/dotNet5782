﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        static Random rand = new Random();
        public List<DroneToList> DroneList;
        public IDAL.IDAL dal;
        public double PowerVacantDrone;
        public double PowerLightDrone;
        public double PowerMediumDrone;
        public double PowerHeavyDrone;
        public double ChargeRate;


        public BL()
        { 
            dal = new DalObject.DalObject();

            PowerVacantDrone = (dal.PowerConsumptionByDrone())[0];
            PowerLightDrone = (dal.PowerConsumptionByDrone())[1];
            PowerMediumDrone = (dal.PowerConsumptionByDrone())[2];
            PowerHeavyDrone = (dal.PowerConsumptionByDrone())[3];
            ChargeRate = (dal.PowerConsumptionByDrone())[4];


            foreach (var drone in dal.DroneList())
            {
                DroneToList droneToList = new DroneToList();
                droneToList.ID = drone.ID;
                droneToList.Model = drone.Model;
                droneToList.MaxWeight = (WeightCategories)drone.MaxWeight;

                bool flag = false;
                foreach (var package in dal.PackageList())
                {

                    if ((package.DroneId == drone.ID) && (package.Delivered == DateTime.MinValue)) // אם שוייך אך לא נמסר
                    {
                        flag = true;
                        droneToList.Status = DroneStatus.Shipping;
                        droneToList.PackageID = package.ID;

                        if (package.PickedUp == DateTime.MinValue) // מיקום הרחפן אם עדיין לא נאסף
                        {
                            Location location = NearestStationToClient(package.SenderId);
                            droneToList.DroneLocation.Latitude = location.Latitude;
                            droneToList.DroneLocation.Longitude = location.Longitude;
                        }
                        else // מיקום הרחפן אם נאסף 
                        {
                            droneToList.DroneLocation.Latitude = dal.ClientById(package.SenderId).Latitude;
                            droneToList.DroneLocation.Longitude = dal.ClientById(package.SenderId).Longitude;
                        }

                        // מצב סוללת הרחפן אם שוייך אך לא נמסר
                        Location targetLocation = new Location(); 
                        targetLocation.Latitude = dal.ClientById(dal.PackageById(droneToList.PackageID).TargetId).Latitude;
                        targetLocation.Longitude = dal.ClientById(dal.PackageById(droneToList.PackageID).TargetId).Longitude;

                        int minBattery;
                        double KM = DalObject.DalObject.distance(droneToList.DroneLocation.Latitude, droneToList.DroneLocation.Longitude, targetLocation.Latitude, targetLocation.Longitude);
                        minBattery = BatteryByKM((int)package.Weight, KM);

                        Location stationLocation = NearestStationToClient(package.TargetId);
                        KM = DalObject.DalObject.distance(targetLocation.Latitude, targetLocation.Longitude, stationLocation.Latitude, stationLocation.Longitude);
                        minBattery += BatteryByKM(3, KM);

                        if (minBattery > 100) throw new Exception ();
                        droneToList.Battery = rand.Next(minBattery, 101); // בין הצריכה המינמלית ל100

                        break;
                    }
                }

                if (!flag)
                {
                    droneToList.Status = (DroneStatus)(rand.Next(1, 3));
                    if (droneToList.Status == DroneStatus.Maintenance)
                    {
                        IDAL.DO.Station station = dal.StationsList().ElementAt(rand.Next(0, dal.StationsList().Count()));
                        droneToList.DroneLocation.Latitude = station.Latitude;
                        droneToList.DroneLocation.Longitude = station.Longitude;
                        droneToList.Battery = rand.Next(0, 21);
                    }
                    else
                    {
                        int index = rand.Next(0, 10);
                        while (dal.PackageList().ElementAt(index).Delivered == DateTime.MinValue)
                        {
                            index = rand.Next(0, 10);
                        }
                        int clientID = dal.PackageList().ElementAt(index).TargetId;

                        droneToList.DroneLocation.Latitude = dal.ClientById(clientID).Latitude;
                        droneToList.DroneLocation.Longitude = dal.ClientById(clientID).Longitude;


                        int minBattery;
                        Location stationLocation = NearestStationToClient(dal.ClientById(index).ID);
                        double KM = DalObject.DalObject.distance(droneToList.DroneLocation.Latitude, droneToList.DroneLocation.Longitude, stationLocation.Latitude, stationLocation.Longitude);
                        minBattery = BatteryByKM(3, KM);
                        droneToList.Battery = rand.Next(minBattery, 101);
                      
                    }
                }

                DroneList.Add(droneToList);
            }

        }


        void AddDrone(Drone drone, int stationNumToCharge)
        {
            try // חריגה מהשכבה הלוגית
            {
                if (drone.ID < 0) throw new IBL.BO.Exceptions.IDException("Drone ID can not be negative", drone.ID);
                if (!dal.StationsList().Any(x => x.ID == stationNumToCharge)) throw new IBL.BO.Exceptions.IDException("Station ID not found", stationNumToCharge);
                if (dal.StationById(stationNumToCharge).ChargeSlots <= 0) throw new IBL.BO.Exceptions.StationException("There are no charging slots available at the station", stationNumToCharge);
            }
            catch (IBL.BO.Exceptions.IDException ex)
            {
                if(ex.Message == "Drone ID can not be negative") {throw ;}
                else if (ex.Message == "Station ID not found") { throw; }
            }
            catch(IBL.BO.Exceptions.StationException ex) {throw; }


            IDAL.DO.Drone droneDAL = new IDAL.DO.Drone(); // הוספה לרשימה ב DAL
            droneDAL.ID = drone.ID;
            droneDAL.Model = drone.Model;
            droneDAL.MaxWeight = (IDAL.DO.WeightCategories)(int)(drone.MaxWeight);
            try // חריגה משכבת הנתונם
            {
                dal.AddDrone(droneDAL);
            }
            catch ( IDAL.DO.Exceptions.IDException ex )
            {
                throw new Exceptions.IDException("A Drone ID already exists", ex , droneDAL.ID);
            }
          

            DroneToList droneToList = new DroneToList();
            droneToList.ID = drone.ID;
            droneToList.Model = drone.Model;
            droneToList.MaxWeight = drone.MaxWeight;
            droneToList.Battery = rand.Next(20, 41);
            droneToList.Status = DroneStatus.Maintenance;
            droneToList.DroneLocation.Latitude = dal.StationById(stationNumToCharge).Latitude;
            droneToList.DroneLocation.Longitude = dal.StationById(stationNumToCharge).Longitude;
            droneToList.PackageID = 0;
            DroneList.Add(droneToList);
        }


        void UpdateDroneName(Drone drone)
        {
            
        }



        public int BatteryByKM(int weight, double KM) // חישוב צריכת חשמל לקילומטר
        {
            
            double power;
            if (weight == 0) power = PowerLightDrone;
            if (weight == 1) power = PowerMediumDrone;
            if (weight == 2) power = PowerHeavyDrone;
            else  power = PowerVacantDrone;
            int temp = (int) (KM * power);
            return temp;
        }






    }





}

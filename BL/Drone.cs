﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    namespace BO
    {
        public class Drone
        {
            public int ID { get; set; }
            public DroneModel Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatus Status { get; set; }
            public PackageProcess DronePackageProcess { get; set; }
            public Location DroneLocation { get; set; }


            public override string ToString()
            {
                string result = "";
                result += $"ID is {ID}, \n";
                result += $"Model name is {Model},\n";
                result += $"MaxWeight is {MaxWeight}, \n";
                result += $"Battery is {Battery}, \n";
                result += $"Status is {Status}, \n";
                result += $"PackageProcess of drone is {DronePackageProcess}, \n";
                result += $"DroneLocation is {DroneLocation}, \n";

                return result;
            }
        }
    }
}

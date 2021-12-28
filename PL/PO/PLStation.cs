﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public partial class PL
    {
        
        static IEnumerable<BO.StationToList> stations = new List<BO.StationToList>();

        

        public IEnumerable<BO.StationToList> GetStationList()
        {
            return BL.DisplayStationList();
        }

        public BO.Station GetStation(int id)
        {
            return BL.DisplayStation(id);
        }

        public void DeleteStation(int id)
        {
            BL.DeleteStation(id);
        }

        public void UpdateStationName(int id, string name)
        {
            BL.UpdateStationName(id, name);
            
        }

        public void UpdateSlotNumber(int id, int num)
        {
            BL.UpdateStationNumCharge(id, num);

        }

        public void AddStation(BO.Station station)
        {
            BL.AddStation(station);

        }

        public IEnumerable<BO.StationToList> DiplayStationWithChargSlot()
        {
            return BL.DisplayStationListWitAvailableChargingSlots();
        }
    }

    //public static class CopyProperties
    //{
    //    public static void CopyPropertiesTo<T, S>(this S from, T to)
    //    {
    //        foreach (PropertyInfo propTo in to.GetType().GetProperties())
    //        {
    //            var propFrom = typeof(S).GetProperty(propTo.Name);
    //            if (propFrom == null)
    //                continue;
    //            var value = propFrom.GetValue(from, null);
    //            if (value is ValueType || value is string)
    //                propTo.SetValue(to, value);
    //        }
    //    }
    //    public static object CopyPropertiesToNew<S>(this S from, Type type)
    //    {
    //        object to = Activator.CreateInstance(type); // new object of Type
    //        from.CopyPropertiesTo(to);
    //        return to;
    //    }
    //}



}



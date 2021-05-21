﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarManager
{
    class DataManager
    {
        public static List<ParkingCar> Cars = new List<ParkingCar>();
        static DataManager()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                //select문으로 해당 테이블의 전체 데이터들을 가져옴
                DBHelper.selectQuery();
                Cars.Clear();   //새로 조회할때마다 Clear해주긔
                foreach (DataRow item in DBHelper.ds.Tables[0].Rows)
                {
                    ParkingCar car = new ParkingCar();
                    car.ParkingSpot = int.Parse(item["ParkingSpot"].ToString());
                    car.CarNumber = item["CarNumber"].ToString();
                    car.DriverName = item["DriverName"].ToString();
                    car.PhoneNumber = item["PhoneNumber"].ToString();
                    car.ParkingTime = item["ParkingTime"].ToString() == "" ? new DateTime() : DateTime.Parse(item["ParkingTime"].ToString());
                    Cars.Add(car);
                }
            }
            catch (Exception e)
            {

                System.Windows.Forms.MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public static void Save(string parkingSpotText, string carNumberText, string driverNameText, string phoneNumber, bool isRemove = false)
        {
            try
            {
                //주차 or 출차로 인하여 상태가 변하였으므로 update문을 호출하여 db table에도 값이 바뀔 수 있도록 한다.
                DBHelper.updateQuery(parkingSpotText, carNumberText, driverNameText, phoneNumber, isRemove);
            }
            catch (Exception e)
            {

                System.Windows.Forms.MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
            }
        }

        public static void PrintLog(string contents)
        {
            DirectoryInfo di = new DirectoryInfo("ParkingHistory");
            if (!di.Exists)
            {
                di.Create();
            }

            using (StreamWriter writer = new StreamWriter("ParkingHistory" + "\\" +"ParkingHistory" + ".txt", true))
            {
                writer.WriteLine(contents);
            }
        }

    }
}

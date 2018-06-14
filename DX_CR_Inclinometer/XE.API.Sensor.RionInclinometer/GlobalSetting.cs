using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XE.API.Sensor.RionInclinometer
{
    public static class GlobalSetting
    {
        private static byte _Address =  0x01 ;
        private static AngleTypeEnum _AngleType = AngleTypeEnum.A60;
        private static int _TimeOut = 1000;
        public static byte Address { get => _Address; set => _Address = value; }
        public static AngleTypeEnum AngleType { get => _AngleType; set => _AngleType = value; }
        public static int TimeOut { get => _TimeOut; set => _TimeOut = value; }

        public static bool IsClosed = false;
    }
}

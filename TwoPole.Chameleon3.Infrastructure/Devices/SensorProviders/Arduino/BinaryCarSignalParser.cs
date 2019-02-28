using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TwoPole.Chameleon3.Domain;

namespace TwoPole.Chameleon3.Infrastructure.Devices
{
   public class BinaryCarSignalParser
    {
  
        
        public CarSignalInfo Parse(byte[] data)
        {
            if (data == null || data.Length == 0 || data.Length < 87)
                return null;

            var c = crc(data, data.Length - 1);
            if (data[data.Length - 1] != c)
            {
                return null;
            }
            if (data[0] != 0xAA)
            {
                return null;
            }

            var carSignalInfo = new CarSignalInfo();
            carSignalInfo.Sensor = new CarSensorInfo();
            carSignalInfo.ObdSensor = new ObdCarSensorInfo();
            carSignalInfo.Gps = new GpsInfo();

            int index = 1;
            carSignalInfo.Count = BitConverter.ToUInt32(data, index);
            index += 4;
            carSignalInfo.Time = BitConverter.ToUInt32(data, index);
            index += 4;
            
            carSignalInfo.Gps.UtcTime= DateTime.Now.Date.Add(new TimeSpan(0, data[index], data[index + 1], data[index + 2], data[index + 3] * 10));
            index += 4;
            carSignalInfo.Gps.LatitudeDegrees = BitConverter.ToSingle(data, index);
            index += 4;
            var gps_lat_dir = data[index];
            index += 1;
            carSignalInfo.Gps.LongitudeDegrees= BitConverter.ToSingle(data, index);
            index += 4;
            var gps_lon_dir = data[index];
            index += 1;
            carSignalInfo.Gps.AltitudeMeters = BitConverter.ToSingle(data, index);
            index += 4;
            carSignalInfo.Gps.FixedSatelliteCount = data[index];
            index += 1;
            var gps_quality = data[index];
            index += 1;
            carSignalInfo.Gps.AngleDegrees = BitConverter.ToSingle(data, index);
            index += 4;
            carSignalInfo.Gps.SpeedInKmh= BitConverter.ToSingle(data, index);
            index += 4;

            #region OBD18 bytes

            //carSignalInfo.SpeedInKmh = data[index + 3];
            //carSignalInfo.EngineRpm = data[index+4] * 255 + data[index+5];
            //carSignalInfo.Sensor.OBDGear = (Gear)data[index + 7];
            //var d1 = data[index+12];
            //var d2 = data[index+13];
            //carSignalInfo.ObdSensor.Clutch = (d1 & (byte)128) == (byte)128;
            ////obdInfo.Clutch 
            //carSignalInfo.ObdSensor.SafetyBelt = (d1 & (byte)64) == (byte)64;
            //carSignalInfo.ObdSensor.Door = (d1 & (byte)32) == (byte)32;
            //carSignalInfo.ObdSensor.Handbrake = (d1 & (byte)16) == (byte)16;
            //carSignalInfo.ObdSensor.Brake = (d1 & (byte)8) == (byte)8;
            //carSignalInfo.ObdSensor.Loudspeaker = (d1 & (byte)4) == (byte)4;
            //carSignalInfo.ObdSensor.HighBeam = (d2 & (byte)128) == (byte)128;
            //carSignalInfo.ObdSensor.LowBeam = (d2 & (byte)64) == (byte)64;
            //carSignalInfo.ObdSensor.FogLight = (d2 & (byte)32) == (byte)32;
            //carSignalInfo.ObdSensor.OutlineLight = (d2 & (byte)16) == (byte)16;
            //carSignalInfo.ObdSensor.CautionLight = (d2 & (byte)8) == (byte)8;
            //carSignalInfo.ObdSensor.LeftIndicatorLight = (d2 & (byte)4) == (byte)4;
            //carSignalInfo.ObdSensor.RightIndicatorLight = (d2 & (byte)2) == (byte)2;

            //index += 13;

            var obdType = data[index];
            index += 1;
            carSignalInfo.SpeedInKmh = data[index];
            index += 1;
            carSignalInfo.EngineRpm = BitConverter.ToInt16(data, index);
            index += 2;
            var CarBand = data[index];
            index += 1;
            var Pos = data[index];
            index += 1;
            carSignalInfo.Sensor.OBDGear = (Gear)data[index];
            index += 1;
            var angle = BitConverter.ToSingle(data, index);
            index += 4;
            var Voltage = BitConverter.ToInt16(data, index);
            index += 4;
            var io = data[index];
            index += 1;

            var d1 = data[index];
            index += 1;
            var d2 = data[index];
            index += 1;
            carSignalInfo.ObdSensor.Clutch = (d1 & (byte)128) == (byte)128;
            //obdInfo.Clutch 
            carSignalInfo.ObdSensor.SafetyBelt = (d1 & (byte)64) == (byte)64;
            carSignalInfo.ObdSensor.Door = (d1 & (byte)32) == (byte)32;
            carSignalInfo.ObdSensor.Handbrake = (d1 & (byte)16) == (byte)16;
            carSignalInfo.ObdSensor.Brake = (d1 & (byte)8) == (byte)8;
            carSignalInfo.ObdSensor.Loudspeaker = (d1 & (byte)4) == (byte)4;
            carSignalInfo.ObdSensor.HighBeam = (d2 & (byte)128) == (byte)128;
            carSignalInfo.ObdSensor.LowBeam = (d2 & (byte)64) == (byte)64;
            carSignalInfo.ObdSensor.FogLight = (d2 & (byte)32) == (byte)32;
            carSignalInfo.ObdSensor.OutlineLight = (d2 & (byte)16) == (byte)16;
            carSignalInfo.ObdSensor.CautionLight = (d2 & (byte)8) == (byte)8;
            carSignalInfo.ObdSensor.LeftIndicatorLight = (d2 & (byte)4) == (byte)4;
            carSignalInfo.ObdSensor.RightIndicatorLight = (d2 & (byte)2) == (byte)2;
            #endregion

            //´¦ÀíObdÐÅºÅ
            //gyro£¨18 bytes£©
            carSignalInfo.AccelerationX = (float)BitConverter.ToInt16(data, index) / 32768 * 16;
            index += 2;
            carSignalInfo.AccelerationY = (float)BitConverter.ToInt16(data, index) / 32768 * 16;
            index += 2;
            carSignalInfo.AccelerationZ = (float)BitConverter.ToInt16(data, index) / 32768 * 16;
            index += 2;
            carSignalInfo.AngleSpeedX = (float)BitConverter.ToInt16(data, index) / 32768 * 2000;
            index += 2;
            carSignalInfo.AngleSpeedY= (float)BitConverter.ToInt16(data, index) / 32768 * 2000;
            index += 2;
            carSignalInfo.AngleSpeedZ= (float)BitConverter.ToInt16(data, index) / 32768 * 2000;
            index += 2;
            carSignalInfo.AngleX = (float)BitConverter.ToInt16(data, index) / 32768 * 180;
            index += 2;
            carSignalInfo.AngleY = (float)BitConverter.ToInt16(data, index) / 32768 * 180;
            index += 2;
            carSignalInfo.AngleZ= (float)BitConverter.ToInt16(data, index) / 32768 * 180;//zÖá
            index += 2;
            //sensor
            var sensor_rpm_hz = BitConverter.ToSingle(data, index);
            index += 4;
            var sensor_speed_hz = BitConverter.ToSingle(data, index);
            index += 4;

            carSignalInfo.EngineRpm = BitConverter.ToInt16(data, index);
            index += 2;

            carSignalInfo.SpeedInKmh = BitConverter.ToInt16(data, index);
            index += 2;

            var sensor_a0 = BitConverter.ToInt16(data, index) * 5.0 / 1023;
            index += 2;
            var sensor_a1 = BitConverter.ToInt16(data, index) * 5.0 / 1023;
            index += 2;
            var sessor_io0 = data[index];
            index += 1;
            carSignalInfo.ObdSensor.LowBeam = (sessor_io0 & (byte)2)==(byte)2;
            carSignalInfo.Sensor.LowBeam = (sessor_io0 & (byte)2) == (byte)2;

            var sensor_io1 = data[index];
            index += 1;
            var sensor_io2 = data[index];
            index += 1;
            return carSignalInfo;
        }

        private byte crc(byte[] data, int len)
        {
            byte c = 0;
            while (len-- > 0)
            {
                c ^= data[len];
            }
            return c;
        }
    }
}
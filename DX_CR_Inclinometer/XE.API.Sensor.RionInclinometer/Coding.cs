using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XE.API.Sensor.RionInclinometer
{
    /// <summary>
    /// 使用modbus协议
    /// </summary>
    public class Coding
    {
        /// <summary>
        /// 获取角度
        /// </summary>
        /// <returns></returns>
        public byte[] GetAngleCommand()
        {
            byte[] ret= new byte[8];
            ret[0] = GlobalSetting.Address;
            ret[1] = 0x03;
            ret[2] = 0x00;
            ret[3] = 0x02;
            ret[4] = 0x00;
            ret[5] = 0x04;
            byte[] crc = ret.CRC(6);
            ret[6] = crc[1];
            ret[7] = crc[0];
            return ret;
        }

        public byte[] SetZeroCommand(ZeroTypeEnum ZeroType)
        {
            byte[] ret = new byte[8];
            ret[0] = GlobalSetting.Address;
            ret[1] = 0x06;
            ret[2] = 0x00;
            ret[3] = 0x10;
            ret[4] = 0x00;
            ret[5] = (byte)ZeroType;
            byte[] crc = ret.CRC(6);
            ret[6] = crc[1];
            ret[7] = crc[0];
            return ret;
        }

        /// <summary>
        /// 设置新地址
        /// </summary>
        /// <param name="newAddress">两位地址</param>
        /// <returns>命令值</returns>
        public byte[] SetAddressCommand(byte newAddress)
        {
            byte[] ret = new byte[8];
            ret[0] = GlobalSetting.Address;
            ret[1] = 0x06;
            ret[2] = 0x00;
            ret[3] = 0x11;
            ret[4] = 0x00;
            ret[5] = newAddress;
            byte[] crc = ret.CRC(6);
            ret[6] = crc[1];
            ret[7] = crc[0];
            return ret;
        }

        public byte[] SetBaudRateCommand(BaudRateTypeEnum baudRate)
        {
            byte[] ret = new byte[8];
            ret[0] = GlobalSetting.Address;
            ret[1] = 0x06;
            ret[2] = 0x00;
            ret[3] = 0x12;
            ret[4] = 0x00;
            ret[5] = (byte)baudRate;
            byte[] crc = ret.CRC(6);
            ret[6] = crc[1];
            ret[7] = crc[0];
            return ret;
        }

        public byte[] SetAutoOutputCommand(AutoOutputTypeEnum outputtype)
        {
            byte[] ret = new byte[8];
            ret[0] = GlobalSetting.Address;
            ret[1] = 0x06;
            ret[2] = 0x00;
            ret[3] = 0x13;
            ret[4] = 0x00;
            ret[5] = (byte)outputtype;
            byte[] crc = ret.CRC(6);
            ret[6] = crc[1];
            ret[7] = crc[0];
            return ret;
        }
    }
}

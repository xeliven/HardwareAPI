using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class ExtensionMethods
{
    public static byte[] CRC(this byte[] data, int length)
    {
        if (length > 0)
        {
            ushort crc = 0xFFFF;

            for (int i = 0; i < length; i++)
            {
                crc = (ushort)(crc ^ (data[i]));
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                }
            }
            byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
            byte lo = (byte)(crc & 0x00FF);         //低位置

            return new byte[] { hi, lo };
        }
        return new byte[] { 0, 0 };
    }
}


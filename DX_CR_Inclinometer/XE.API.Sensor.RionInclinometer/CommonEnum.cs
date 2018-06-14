using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XE.API.Sensor.RionInclinometer
{
    /// <summary>
    /// 测量范围类型
    /// </summary>
    public enum AngleTypeEnum : int
    {
        A30 = 3000,
        A45 = 4500,
        A60 = 6000,
        A90 = 9000,
    }
    /// <summary>
    /// 0点类型
    /// </summary>
    public enum ZeroTypeEnum : byte
    {
        /// <summary>
        /// 绝对0点
        /// </summary>
        Absolute = 0x00,
        /// <summary>
        /// 相对0点
        /// </summary>
        Relative = 0xFF,
    }
    /// <summary>
    /// 波特率类型
    /// </summary>
    public enum BaudRateTypeEnum : byte
    {
        B4800 = 0xA0,
        B9600 = 0xA1,
        B19200 = 0xA2,
        B38400 = 0xA3,
        B115200 = 0xA4,
    }
    /// <summary>
    /// 自动输出类型
    /// </summary>
    public enum AutoOutputTypeEnum : byte
    {
        /// <summary>
        /// 问答模式
        /// </summary>
        Interlocution = 0x00,
        Rate10Hz = 0x01,
        Rate25Hz = 0x02,
        Rate50Hz = 0x03,
    }
}

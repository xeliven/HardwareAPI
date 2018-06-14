using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XE.API.Sensor.RionInclinometer
{
    public class Decoding
    {
        public event Action<bool, string> DecodingErr;
        public event Action<float, float> AngleValueEvent;
        public event Action<ZeroTypeEnum> ZeroSettingEvent;

        private Queue<byte> _TempQueue = new Queue<byte>();
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="recdata">收到的数据</param>
        public void DeCode(byte[] recdata)
        {
            byte[] rec = new byte[13];
            foreach (byte b in recdata) { _TempQueue.Enqueue(b); }
            byte[] temp = _TempQueue.ToArray();
            for (int i = 0; i <= temp.Length - 13; i++)
            {
                if (temp[i] == GlobalSetting.Address && temp[i + 1] == 0x03 && temp[i + 2] == 0x08)
                {
                    Array.Copy(temp, i, rec, 0, 13);
                    _TempQueue.Clear();
                    break;
                }
                else
                {
                    continue;
                }
            }
            if (rec[0] == 0x00) { return; }
            //长度检查
            int len = rec.Length;
            if (len < 8) { DecodingErr?.Invoke(false,$"长度不够，长度:{len}");  return; }
            //校验检查
            byte[] crc = rec.CRC(len - 2);
            if (crc[1] != rec[len - 2] || crc[0] != rec[len - 1]) { DecodingErr?.Invoke(false, $"校验位错误"); return; }
            //分类处理
            switch (rec[1])
            {
                case 0x03:
                    if (len < 13) { DecodingErr?.Invoke(false, $"长度不够，长度:{len}"); return; }
                    float x = ((rec[4] << 8) + rec[3] - (int)GlobalSetting.AngleType) * 0.01f; //x轴数据解析
                    float y = ((rec[8] << 8) + rec[7] - (int)GlobalSetting.AngleType) * 0.01f; //y轴数据解析
                    AngleValueEvent?.Invoke(x, y);
                    break;
                case 0x06:
                    switch (rec[3])
                    {
                        case 0x10:
                            ZeroSettingEvent?.Invoke((ZeroTypeEnum)rec[5]);
                            break;
                        case 0x11:
                            break;
                        case 0x12:
                            break;
                        case 0x13:
                            break;
                    }
                    break;
            }
        }
    }
}

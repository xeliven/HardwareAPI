using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XE.API.Sensor.RionInclinometer
{
    public class SynMethod
    {
        public SynMethod()
        {
            //Decoding.ZeroSettingEvent += (zeroType) => { _SetZeroEventWait.Set(); };
        }

        //EventWaitHandle _SetZeroEventWait;
        Decoding _Decoding = new Decoding();
        Coding _coding = new Coding();

        public Decoding Decoding { get => _Decoding; set => _Decoding = value; }

        public void SetZero(ZeroTypeEnum zeroType,Action<byte[]> sendhandel)
        {
            //_SetZeroEventWait = new AutoResetEvent(false);
            sendhandel(_coding.SetZeroCommand(zeroType));
            Thread.Sleep(20);
            sendhandel(_coding.SetZeroCommand(zeroType));
        }
        public void GetValue(Action<byte[]> sendhandel)
        {
            sendhandel(_coding.GetAngleCommand());
        }

        public void SetAutoOutputType(AutoOutputTypeEnum autoOutputType, Action<byte[]> sendhandle)
        {
            sendhandle(_coding.SetAutoOutputCommand(autoOutputType));
            Thread.Sleep(20);
            sendhandle(_coding.SetAutoOutputCommand(autoOutputType));
        }

        public void SetBaudRate(BaudRateTypeEnum baudRateType, Action<byte[]> sendhandle)
        {
            sendhandle(_coding.SetBaudRateCommand(baudRateType));
            Thread.Sleep(20);
            sendhandle(_coding.SetBaudRateCommand(baudRateType));
        }

        public void SetAddress(byte address, Action<byte[]> sendhandle)
        {
            sendhandle(_coding.SetAddressCommand(address));
            Thread.Sleep(20);
            sendhandle(_coding.SetAddressCommand(address));
        }
    }
}

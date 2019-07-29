using System;

namespace MoqSut
{
    public class Machine : IMachine
    {
        public IWheelChanger WheelChanger { get; }

        public string Name { get; set; }

        public Machine(IWheelChanger wc) => WheelChanger = wc;

        public void ExchangeWheel(int wheelChangerPort)
        {
            if (wheelChangerPort < 1 || wheelChangerPort > WheelChanger.NumberOfPorts)
            {
                throw new InvalidOperationException("Port number invalid");
            }

            ExecuteExchangeWheel(wheelChangerPort);
        }

        protected virtual void ExecuteExchangeWheel(int wheelChangerPort)
        {
            WheelChanger.OpenDoor();
            WheelChanger.ExchangeNewWheelFromPort(wheelChangerPort);
            WheelChanger.CloseDoor();
        }
    }
}

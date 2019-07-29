namespace MoqSut
{
    public interface IWheelPack
    {
        string Name { get; set; }

        int NumberOfWheels { get; set;  }
    };

    public interface IWheelChanger
    {
        int NumberOfPorts { get; }

        bool IsDoorClosed { get; }

        void OpenDoor();

        void CloseDoor();

        bool ExchangeNewWheelFromPort(int portNumber);

        IWheelPack GetWheelPack(int portNumber);

        void GetWheelPackName(int portNumber, out string wheelPackName);

        double GetMaxDiameterOfWheelPack(int portNumber);
    }

    public interface IMachine
    {
        IWheelChanger WheelChanger { get; }
    }
}

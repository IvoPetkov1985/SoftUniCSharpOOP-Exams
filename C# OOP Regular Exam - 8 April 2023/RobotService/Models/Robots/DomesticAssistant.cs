namespace RobotService.Models.Robots
{
    public class DomesticAssistant : Robot
    {
        private const int DomesticAssistantBatteryCapacity = 20000;
        private const int DomesticAssistantCapacityIndex = 2000;

        public DomesticAssistant(string model)
            : base(model, DomesticAssistantBatteryCapacity, DomesticAssistantCapacityIndex)
        {
        }
    }
}

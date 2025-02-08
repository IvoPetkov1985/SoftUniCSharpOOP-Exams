namespace RobotService.Models.Robots
{
    public class IndustrialAssistant : Robot
    {
        private const int IndustrialAssistantBatteryCapacity = 40000;
        private const int IndustrialAssistantCapacityIndex = 5000;

        public IndustrialAssistant(string model)
            : base(model, IndustrialAssistantBatteryCapacity, IndustrialAssistantCapacityIndex)
        {
        }
    }
}

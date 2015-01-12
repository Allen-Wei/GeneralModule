using GeneralModule.Interface;

namespace GeneralModule.Module
{
    public class Payment : IModule
    {
        public string GetModuleName()
        {
            return "Payment";
        }
    }
}
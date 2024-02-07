namespace BaseConstructors
{
    public class Beverage
    {
        public string Name { get; set; }
        public bool IsFairTrade { get; set; }

        protected int servingTemperature;
        public virtual int GetServingTemperature()
        {
            return servingTemperature;
        }

        public Beverage()
        {
            // This is the default constructor.
            // You can use this to set default values.
            IsFairTrade = false;
            servingTemperature = 175;
            Name = "Not known";
        }

        public Beverage(string name, bool isFairTrade, int servingTemp)
        {
            // This is the alternative constructor.
            this.Name = name;
            this.IsFairTrade = isFairTrade;
            this.servingTemperature = servingTemp;
        }
    }
}

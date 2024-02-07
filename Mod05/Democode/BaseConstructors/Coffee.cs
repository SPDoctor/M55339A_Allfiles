namespace BaseConstructors
{
    class Coffee : Beverage
    {
        public string Bean { get; set; }
        public string Roast { get; set; }
        public string CountryOfOrigin { get; set; }

        public Coffee()
        {
            // Use the default constructor to set default values.
            Bean = "Not known";
            Roast = "Medium";
            CountryOfOrigin = "Not known";
        }

        public Coffee(string name, bool isFairTrade, int servingTemp, string bean, string roast, string countryOfOrigin)
            : base(name, isFairTrade, servingTemp)
        {
            Bean = bean;
            Roast = roast;
            CountryOfOrigin = countryOfOrigin;
        }
    }
}

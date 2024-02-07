namespace FourthCoffee
{
    public class SalesPerson
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Area { get; set; }
        public string EmailAddress { get; set; }

        public override string ToString()
        {
            var person = "Name: " + FirstName + " " + Surname + "\r\n";
            person += "Area: " + Area + "\r\n";
            person += "Email: " + EmailAddress + "\r\n";
            return person;
        }
    }
}

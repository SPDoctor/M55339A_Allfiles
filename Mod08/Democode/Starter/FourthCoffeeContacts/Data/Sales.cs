﻿using FourthCoffee;

namespace FourthCoffeeContacts.Data
{
    public static class Sales
    {
        public static SalesPerson GetSalesPerson(string emailAddress)
        {
            return GetSalesPeople()
                .Where(s => s.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
        }

        public static IQueryable<SalesPerson> GetSalesPeople()
        {
            return new List<SalesPerson>
            {
                new SalesPerson
                {
                    Area = "snacks",
                    FirstName = "Jesper",
                    Surname = "Herp",
                    EmailAddress = "jesper@fourthcoffee.com"
                },new SalesPerson
                {
                    Area = "snacks",
                    FirstName = "Eran",
                    Surname = "Harel",
                    EmailAddress = "eran@fourthcoffee.com"
                },new SalesPerson
                {
                    Area = "snacks",
                    FirstName = "David",
                    Surname = "Pelton",
                    EmailAddress = "david@fourthcoffee.com"
                },new SalesPerson
                {
                    Area = "snacks",
                    FirstName = "Jan",
                    Surname = "Kotas",
                    EmailAddress = "jan@fourthcoffee.com"
                }
            }.AsQueryable();
        }
    }
}

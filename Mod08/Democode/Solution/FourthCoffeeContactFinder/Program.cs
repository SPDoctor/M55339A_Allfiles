using FourthCoffee;
using System.Text.Json;

var client = new HttpClient();
Console.WriteLine("Welcome to the Fourth Coffee Contact Finder!");
Console.WriteLine("Please enter the email address of the sales person whom you seek:");
var salesPersonEmail = Console.ReadLine();

while (salesPersonEmail != null && salesPersonEmail.Length > 0)
{
    var json = await client.GetStringAsync("http://localhost:5731/salespeople/" + salesPersonEmail);
    SalesPerson person = null;
    person = JsonSerializer.Deserialize<SalesPerson>(json);
    if (person != null)
    {

        Console.WriteLine("Here are the details:");
        Console.WriteLine(person.ToString());
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Sorry, we couldn't find a sales person with that email :-(");
    }
    Console.WriteLine("Please enter another email address, or return to quit:");
    salesPersonEmail = Console.ReadLine();
}
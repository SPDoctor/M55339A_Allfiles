using FourthCoffee;
using System.ComponentModel;
using System.Net;
// TODO: 01: Bring the System.Text.Json namespace into scope

// TODO: 02: Create an HttpClient object to make HTTP requests 
Console.WriteLine("Welcome to the Fourth Coffee Contact Finder!");
Console.WriteLine("Please enter the email address of the sales person whom you seek:");
var salesPersonEmail = Console.ReadLine();

while (salesPersonEmail != null && salesPersonEmail.Length > 0)
{
    // TODO: 03: Make a request to the REST endpoint
    SalesPerson person = null;
    // TODO: 04: Deserialize the JSON data
    if (person != null)
    {

        Console.WriteLine("Here are the details:");
        // TODO: 05: Write data to the output
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Sorry, we couldn't find a sales person with that email :-(");
    }
    Console.WriteLine("Please enter another email address, or return to quit:");
    salesPersonEmail = Console.ReadLine();
}
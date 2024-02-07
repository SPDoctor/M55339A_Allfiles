Console.WriteLine("Fetching web page");
showWebSite("http://www.spdoctor.com/");
Console.WriteLine("Waiting...");
for (int i = 0; i < 10; i++)
{
    Console.ReadLine();
    Console.WriteLine("Waiting...");
}

static async void showWebSite(string Uri)
{
    var client = new HttpClient();
    var msg = await client.GetStringAsync(Uri);
    Console.WriteLine(msg);
}


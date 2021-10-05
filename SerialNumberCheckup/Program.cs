using RestEase;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;

namespace SerialNumberCheckup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var apiSr = new StreamReader("apitoken.csv");
            var urlSr = new StreamReader("url.csv");
            string? url = await urlSr.ReadLineAsync();
            var api = new RestClient(url).For<ISnipeItApi>();
            string? jwt = await apiSr.ReadLineAsync();


            api.Authorization = $"Bearer {jwt}";


            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                var input = Console.ReadLine();
                var creationSucceeded = StickerContent.TryCreateInstanceFromString(input!, out var stickerContent);
                if (!creationSucceeded)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid input; please try again.");
                    continue;
                }

                var response = await api.GetHardwareBySerialnumber(stickerContent!.SerialNumber.Value);
                if (response.ResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var responseObject = JObject.Parse(response.StringContent!);
                    var total = responseObject["total"]!.Value<int>();

                    if (total != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Hardware is already known.");


                        var id = responseObject["rows"][0].Value<int>("id");
                        //Should parse id of object, didnt

                        var uri = "<BASE_URL_SLASH_HARDWARE>" + id;
                        var psi = new System.Diagnostics.ProcessStartInfo();
                        psi.UseShellExecute = true;
                        psi.FileName = uri;
                        Process.Start(psi);
                        continue;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Unknown hardware; Consider adding hardware to SnipeIT.");
            }
        }
    }
}

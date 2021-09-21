﻿using RestEase;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SerialNumberCheckup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var jwt = "<API_TOKEN>";
            var api = new RestClient("<API_BASE_URL>").For<ISnipeItApi>();

            api.Authorization = $"Bearer {jwt}";


            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                var input = Console.ReadLine();
                var creationSucceeded = StickerContent.TryCreateInstanceFromString(input, out var stickerContent);
                if (!creationSucceeded)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Invalid input; please try again.");
                    continue;
                }

                var response = await api.GetHardwareBySerialnumber(stickerContent.SerialNumber.Value);
                if (response.ResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var responseObject = JObject.Parse(response.StringContent!);
                    var total = responseObject["total"]!.Value<int>();

                    if (total != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Hardware is already known.");
                        
                        
                        var id = responseObject["id"]!.Value<int>();
                        //Should parse id of object, didnt
                        
                        var idLink = "<BASE_URL>/hardware/" + id;
                        System.Diagnostics.Process.Start(idLink);
                        continue;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Unknown hardware; Consider adding hardware to SnipeIT.");
            }
        }
    }
}

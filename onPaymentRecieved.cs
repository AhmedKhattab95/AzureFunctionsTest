using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace courseFirstFunction
{
    public static class onPaymentRecieved
    {
        [FunctionName("onPaymentRecieved")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Recieved a payment.");


                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Order>(requestBody);

                log.LogInformation(data.ToString());

                string responseMessage = "Thank you for your purchase";

                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                log.LogError(null, ex, ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }

    public class Order
    {
        public string OrderId { get; set; }
        [JsonRequired]
        public string ProducetId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Price { get; set; }


        public override String ToString()
        {
            return $"Order: {OrderId} recieved from {Name} with Email: {Email}, Phone: {Phone}\nfor product {ProducetId}.";
        }
    }
}

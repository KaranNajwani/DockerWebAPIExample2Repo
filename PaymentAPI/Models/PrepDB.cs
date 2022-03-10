using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace PaymentAPI.Models
{
    public class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SendData(serviceScope.ServiceProvider.GetService<PaymentDetailContext>());
            }
        }

        private static void SendData(PaymentDetailContext context)
        {
            Console.WriteLine("Applying migration...");
            context.Database.Migrate();
            if (context.PaymentDetails.Any())
            {
                context.PaymentDetails.AddRange(new PaymentDetail()
                {
                    CardNumber = "1234567891234567",
                    CardOwnerName = "John Doe",
                    ExpirationDate = "12/25",
                    SecurityCode = "123"
                });

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Already have data -- not seeding");
            }
        }
    }
}

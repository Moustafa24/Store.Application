using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace peresistence.Data.Configrations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {


            builder.OwnsOne(O => O.ShippingAddress, addres => addres.WithOwner());

            builder.HasMany(O => O.OrderItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
                
            builder.HasOne(O=>O.DeliveryMethod)
                .WithMany()
                .HasForeignKey(O => O.DeleveryMethodId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(O => O.paymentStatus)
                .HasConversion(s => s.ToString(), s => Enum.Parse<OrderPaymentStatus>(s));

            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,4)");
        
        }
    }
}

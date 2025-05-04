using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.order;
using Services.Abstractions;
using Services.Specifications;
using Shared.OrderModels;

namespace Services
{
    public class OrederService(IMapper mapper,
        IBasketRepository basketRepository,
        IUnitOfWork unitOfWork
        ) : IOrderService
    {
        public async Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequestDto, string userEmail)
        {
            //1. Address
            var address =  mapper.Map<Address>(orderRequestDto.ShipToAddress);

            //2. order item
           var basket = await basketRepository.GetBasketAsync(orderRequestDto.BasketId);
            if (basket is null) throw new BasketNotFoundException(orderRequestDto.BasketId);
            var orderItems =new List<OrderItem>();
            foreach (var item in basket.Items) 
            {
                var product = await unitOfWork.GetRepository<Product, int>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundExceptions(item.Id);
                var orderItem = new OrderItem(new ProductInOrderItem(product.Id , product.Name , product.PictureUrl),item.Quantity ,product.Price);
           orderItems.Add(orderItem);
            }

            // 3. Get Delevery Method
            var deliveryMethod=  await unitOfWork.GetRepository<DeliveryMethod, int>().GetAsync(orderRequestDto.DeliverMethodId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFOundExcpetion(orderRequestDto.DeliverMethodId);

            var subTotal = orderItems.Sum(I=>I.Price * I.Quantity);
            //ToDo :: Create Payment Intent Id ----

            //CreateOrderAsync Order
            var Order = new Order(userEmail,address , orderItems , deliveryMethod, subTotal ,"");
        
            await unitOfWork.GetRepository<Order,Guid>().AddAsync(Order);
            var count = await unitOfWork.SaveChangesAsync();
            if (count == 0) throw new OrderBadRequestException();

            var result = mapper.Map<OrderResultDto>(Order);
            return result;

        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethod()
        {
            var delevryMethods = await unitOfWork.GetRepository<DeliveryMethod , int>().GetAllAsync();
           var result = mapper.Map<IEnumerable<DeliveryMethodDto>>(delevryMethods);
            return result;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id)
        {
            var spec = new OrderSpecifications(id);
          var order = await  unitOfWork.GetRepository<Order,Guid>().GetAsync(spec);
            if (order is null) throw new OrderNotFoundException(id);

            var result  =mapper.Map<OrderResultDto>(order);
            return result;
        }

        public async Task<IEnumerable<OrderResultDto>> GetOrderByUserEmailAsync(string userEmail)
        {

            var spec = new OrderSpecifications(userEmail);
            
            var orders = await unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
           
            var result = mapper.Map<IEnumerable<OrderResultDto>>(orders);
           
            return result;
        }
    }
}

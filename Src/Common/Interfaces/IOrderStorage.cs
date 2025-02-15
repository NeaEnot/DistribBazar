﻿using Kurier.Common.Models.Events;
using Kurier.Common.Models.Requests;
using Kurier.Common.Models.Responses;

namespace Kurier.Common.Interfaces
{
    public interface IOrderStorage
    {
        Task<Guid> CreateOrder(CreateOrderInStorageRequest request);
        Task<GetOrderResponse> GetOrderById (Guid id);
        Task<OrderUpdatedEvent> UpdateOrderStatus(UpdateOrderStatusRequest request);
    }
}

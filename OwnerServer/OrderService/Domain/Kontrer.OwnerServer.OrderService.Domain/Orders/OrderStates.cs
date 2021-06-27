namespace Kontrer.OwnerServer.OrderService.Domain.Orders
{
    public enum OrderStates
    {
        New = 1,
        Processed = 2,
        Completed = 3,
        CanceledByCustomer = 4,
        CanceledByOwner = 5,
    }
}
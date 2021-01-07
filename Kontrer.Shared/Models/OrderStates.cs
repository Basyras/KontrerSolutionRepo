namespace Kontrer.Shared.Models
{
    public enum OrderStates
    {
       Unsaved = 0,     
       WaitingForCustomerResponse = 1,
       Ready = 2,
       Completed = 3,
       CanceledByCustomer = 4,
       CanceledByOwner = 5,
    }
}

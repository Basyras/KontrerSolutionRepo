namespace Kontrer.Shared.Models
{
    public enum AccommodationState
    {
       Unsaved = 0,     
       WaitingForConfirmation = 1,
       Ready = 2,
       Completed = 3,
       CanceledByCustomer = 4,
       CanceledByOwner = 5,
    }
}

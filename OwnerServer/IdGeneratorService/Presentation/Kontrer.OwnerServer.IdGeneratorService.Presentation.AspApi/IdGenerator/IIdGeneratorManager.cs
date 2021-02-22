namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.IdGenerator
{
    public interface IIdGeneratorManager
    {
        public const string OrdersGroupName = "orders";
        public const string CustomersGroupName = "customers";
        int CacheSize { get; set; }

        /// <summary>
        /// Returns unique ID across specific group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        int CreateNewId(string groupName);
    }
}
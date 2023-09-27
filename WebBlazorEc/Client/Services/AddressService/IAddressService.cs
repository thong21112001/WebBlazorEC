namespace WebBlazorEc.Client.Services.AddressService
{
    public interface IAddressService
    {
        Task<Address> GetAddres();
        Task<Address> AddOrUpdateAddres(Address address);
    }
}

namespace GoogleMapsApi.FE.endpoints.WichitaWisdom.Service
{
    public interface IWichitaWisdomService
    {
        Task<string> GetWichitaWisdom(string prompt);
    }
}

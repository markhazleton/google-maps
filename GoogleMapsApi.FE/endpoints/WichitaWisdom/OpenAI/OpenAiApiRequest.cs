namespace GoogleMapsApi.FE.endpoints.WichitaWisdom.OpenAI
{
    public class OpenAiApiRequest
    {
        public string model { get; set; }
        public List<Message> messages { get; set; }
        public double temperature { get; set; }
    }
}

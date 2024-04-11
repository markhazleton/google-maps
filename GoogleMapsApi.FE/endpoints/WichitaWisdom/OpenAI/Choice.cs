namespace GoogleMapsApi.FE.endpoints.WichitaWisdom.OpenAI
{
    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        // Assuming logprobs can be various types, we're leaving it as dynamic. In a real implementation, you may want to define a specific type or structure.
        public dynamic Logprobs { get; set; }
        public string FinishReason { get; set; }
    }
}

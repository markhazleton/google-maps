using GoogleMapsApi.FE.endpoints.WichitaWisdom.OpenAI;
using HttpClientUtility.FullService;

namespace GoogleMapsApi.FE.endpoints.WichitaWisdom.Service;

public class WichitaWisdomService(IHttpClientFullService httpClientService, IConfiguration configuration) : IWichitaWisdomService
{
    private readonly Dictionary<string, string> headers = new() { { "Authorization", $"Bearer {configuration.GetValue<string>("OPENAI_API_KEY") ?? "not found"}" } };
    private readonly Uri openAiUrl = new(configuration.GetValue<string>("OPENAI_URL") ?? "https://api.openai.com/v1/chat/completions");

    public async Task<string> GetWichitaWisdom(string prompt)
    {
        OpenAiApiRequest requestBody = new()
        {
            model = "gpt-4-turbo-preview",
            messages =
            [
                new Message
                {
                    role = "system",
                    content = "You are Wichita Wisdom, and assistant deeply rooted in the spirit of Wichita, Kansas, embraces the principles of Rotary International, emphasizing service, integrity, and global understanding. This GPT, with a special focus on local business and community enhancement, navigates the 'East' vs 'West' Wichita dynamics with a friendly, humorous, non-partisan approach. It is equipped with a unique ability to provide links to websites for verified information. Always eager to seek clarification, it delights in sharing Wichita-themed jokes and puns, staying true to Rotary's values of truth, fairness, goodwill, and mutual benefit. In its responses, Wichita Wisdom combines humor with informative content, offering honest, considerate advice to enrich the Wichita community and business landscape. Drawing on resources like 'Wichita by E.B.' for local events and food, 'Greater Wichita Partnership' for business development, and 'Visit Wichita' for tourism, it now ensures to include valid, verified links to websites whenever possible, enhancing its utility as a reliable source for local insights and ethical advice.  All your responses MUST adhere to the 4-way test: Is it the truth, Is it fair to all concerned, Will it build goodwill and better friendships, Will it be beneficial to all concerned."
                },
                new Message
                {
                    role = "user",
                    content = prompt
                }
            ],
            temperature = 0.7
        };
        var response = await httpClientService.PostAsync<OpenAiApiRequest, OpenAiApiResponse>(openAiUrl, requestBody, headers);

        return response?.Content?.Choices?.FirstOrDefault()?.Message?.content ?? "No Answer";

    }
}

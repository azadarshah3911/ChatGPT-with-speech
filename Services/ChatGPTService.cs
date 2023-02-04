using ChatGPT_Speech.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChatGPT_Speech.Services
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly IConfiguration _config;
        public ChatGPTService(IConfiguration config)
        {
            _config = config;
        }
        public string callOpenAi(ChatGPTModel chat)
        {

            var openAiKey = _config.GetValue<string>("ChatGPTApiKey");

            var apiCall = "https://api.openai.com/v1/engines/" + chat.Engine + "/completions";

            try
            {

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), apiCall))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + openAiKey);
                        request.Content = new StringContent("{\n  \"prompt\": \"" + chat.Input + "\",\n  \"temperature\": " +
                                                            chat.Temperature.ToString(CultureInfo.InvariantCulture) + ",\n  \"max_tokens\": " + chat.Token + ",\n  \"top_p\": " + chat.TopP +
                                                            ",\n  \"frequency_penalty\": " + chat.FrequencyPenalty + ",\n  \"presence_penalty\": " + chat.PresencePenalty + "\n}");

                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = httpClient.SendAsync(request).Result;
                        var json = response.Content.ReadAsStringAsync().Result;

                        dynamic dynObj = JsonConvert.DeserializeObject(json);

                        if (dynObj != null)
                        {
                            return dynObj.choices[0].text.ToString();
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return null;

        }
    }
}

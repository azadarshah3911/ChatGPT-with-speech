using ChatGPT_Speech.Models;
using ChatGPT_Speech.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace ChatGPT_Speech.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatGPTService _chatGPTService;
        public HomeController(IChatGPTService chatGPTService, ILogger<HomeController> logger)
        {
            _chatGPTService = chatGPTService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            Response response = new Response();
         
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Response response)
        {
           
            if (response.Question != null)
            {
                response.ID += 1;
                ChatGPTModel chat = new ChatGPTModel()
                {
                    Token = 1000,
                    Input = response.Question,
                    TopP = 0,
                    Engine = "text-davinci-002",
                    Temperature = 0.7,
                    PresencePenalty = 0,
                    FrequencyPenalty = 0
                };
                response.Answer = _chatGPTService.callOpenAi(chat);
                // creating the object of SpeechSynthesizer class  
                SpeechSynthesizer sp = new SpeechSynthesizer();
                //setting volume   
                sp.Volume = 100;
                //ing text box text to SpeakAsync method   
                sp.SpeakAsync(response.Answer);
                return View(response);
            }
            return View(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

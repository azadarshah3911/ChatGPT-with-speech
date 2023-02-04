using ChatGPT_Speech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatGPT_Speech.Services
{
    public interface IChatGPTService
    {
        string callOpenAi(ChatGPTModel chat);
    }
}

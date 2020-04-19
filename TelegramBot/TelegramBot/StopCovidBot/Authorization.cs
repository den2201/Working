using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace StopCovidBot
{
   public class Authorization
    {
       static MihaZupan.HttpToSocks5Proxy HttpToSocks5Proxy;
       public static TelegramBotClient InitBot()
        {
            HttpToSocks5Proxy = new MihaZupan.HttpToSocks5Proxy("127.0.0.1", 9150);
            string token ="";
            return new TelegramBotClient(token, HttpToSocks5Proxy);
        }
    }
}

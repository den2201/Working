using System;
using Telegram.Bot;
using MihaZupan;
using System.Net;
using System.Net.Http;

namespace TelegramBot
{
    class Program
    {
        static TelegramBotClient Bot;
        static void Main(string[] args)
        {
            WebProxy proxy = new WebProxy(Address: new Uri( "http://163.182.179.32:8811"));
            HttpClientHandler httpProxy = new HttpClientHandler() { Proxy = proxy };
            HttpClient hc = new HttpClient(handler: httpProxy, disposeHandler: true);
            Bot = new TelegramBotClient("1207416695:AAHwctcnV7E088k3ScrLo9L5lTClJpI--FQ", hc);
        
         
            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
            Console.ReadLine();
           
        }

        private static async  void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string text = e?.Message?.Text;
            if (text != null)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, e.Message.Text);
            }
        }
    }
}

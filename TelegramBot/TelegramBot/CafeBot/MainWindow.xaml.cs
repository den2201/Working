using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Telegram.Bot;
using MihaZupan.Dns;
using Telegram.Bot.Types.ReplyMarkups;

namespace CafeBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TelegramBotClient bot;
        MihaZupan.HttpToSocks5Proxy HttpToSocks5Proxy;
        
        public MainWindow()
        {
            InitializeComponent();
            HttpToSocks5Proxy = new MihaZupan.HttpToSocks5Proxy("127.0.0.1", 9150);

            bot = new TelegramBotClient("1207416695:AAHwctcnV7E088k3ScrLo9L5lTClJpI--FQ",HttpToSocks5Proxy);
            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += Bot_OnCallbackQuery;
            var t= bot.GetMeAsync().Result;
            bot.StartReceiving();
        }
        
        private void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.CallbackQuery.Data == "cd1")
                {
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "DOOM");
                    bot.SendAudioAsync(e.CallbackQuery.Message.Chat.Id);
                }
                if(e.CallbackQuery.Data=="cd2")
                {
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,"OOOOOPS");
                }
            });
        }

        private  void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string senderName = e.Message.Chat.FirstName;
            Dispatcher.Invoke(() => 
            {
                if ((e.Message.Text.ToLower() == "привет") || (e.Message.Text.ToLower() == "hello"))
                {
                    bot.SendTextMessageAsync(e.Message.Chat.Id, $"Привет, {senderName}");
                    var keyboard = new InlineKeyboardMarkup(new[]
                            {
                                new[] {InlineKeyboardButton.WithCallbackData("name1","cd1") },
                                new[] {InlineKeyboardButton.WithCallbackData("name2","cd2") },
                                new[] {InlineKeyboardButton.WithCallbackData("name3","cd3") }
                            });
                     bot.SendTextMessageAsync(e.Message.Chat.Id, e.Message.Chat.Id.ToString(), replyMarkup: keyboard);
                }
            });
        }
    }
}

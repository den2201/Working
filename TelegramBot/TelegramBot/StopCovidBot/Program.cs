using StopCovidBot.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace StopCovidBot
{
    class Program
    {
       static TelegramBotClient bot;
        static InlineKeyboardMarkup keyboard;
        static InlineKeyboardMarkup CodesKeyboard;
        static Dictionary<string, string> dicCountries;
        static string codeOfCountries;
        public static Dictionary<long, int> lastChatMessageIdDic;
        static void Main(string[] args)
        {
            bot = Authorization.InitBot();
            InitKeyboard();
            lastChatMessageIdDic = new Dictionary<long, int>();
            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += Bot_OnCallbackQuery;
            bot.StartReceiving();
            Console.ReadKey();
            bot.StopReceiving();
        }

        private static void InitKeyboard()
        {
            keyboard = new InlineKeyboardMarkup(new[]
                         {
                                new[] {InlineKeyboardButton.WithCallbackData("Информация по Covid19","info") },
                                new[] {InlineKeyboardButton.WithCallbackData("Основные симптомы Covid19","symptoms") },
                                new[] {InlineKeyboardButton.WithCallbackData("Профилактика","profil") },
                                new[] {InlineKeyboardButton.WithCallbackData(string.Format("Статистика по миру на сегодня"),"statAll") },
                                new[] {InlineKeyboardButton.WithCallbackData(string.Format("Статистика по России на сегодня"),"statRU") },
                                new[] {InlineKeyboardButton.WithCallbackData(string.Format("Остальные страны"),"statOther") }
                           });
        }
                               
        private static void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            Task.Run(() =>
            {
                DeleteLastMessageInChat(e.CallbackQuery.Message.Chat.Id);
                Task<Telegram.Bot.Types.Message> message=null;
                if (e.CallbackQuery.Data == "info")
                {
                     message = bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, CovidInfo.GetInfoCovid());
                    Thread.Sleep(1000);
                }
                if (e.CallbackQuery.Data == "symptoms")
                {
                    message = bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, CovidInfo.GetSympCovid());
                    Thread.Sleep(1000);
                }
               
                if (e.CallbackQuery.Data == "profil")
                {
                   message = bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, CovidInfo.GetProfCovid());
                }
                if (e.CallbackQuery.Data == "statAll")
                {
                    message = bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, CovidInfo.GetStatCovidTotalToday());
                }
                if (e.CallbackQuery.Data == "statRU")
                {
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, CovidInfo.GetStatCovidCountryToday("ru"));
                }
                if (e.CallbackQuery.Data == "statOther")
                {
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.Chat.FirstName + ", для вывода информации введи #+код страны (#UA)");
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id,  CovidInfo.GetCountriesCodes());
                }
                SetLastMessageIdInDictionaty(e.CallbackQuery.Message.Chat.Id, message.Id);
                bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, string.Empty, replyMarkup: keyboard);
            });
           
        }
        private static void SetLastMessageIdInDictionaty(long chatId, int messageId)
        {
            if(lastChatMessageIdDic.ContainsKey(chatId))
            {
                lastChatMessageIdDic[chatId] = messageId;
            }
            else
            {
                lastChatMessageIdDic.Add(chatId, messageId);
            }
        }

        private static  void DeleteLastMessageInChat(long chatId)
        {
            if(lastChatMessageIdDic.ContainsKey(chatId))
            {
                int messageId;
                if((lastChatMessageIdDic.TryGetValue(chatId, out messageId))&&(messageId!=0))
                {
                    bot.DeleteMessageAsync(chatId, messageId);
                    lastChatMessageIdDic[chatId] = 0;
                }
            }
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string senderName = e.Message.Chat.FirstName;
            Task<Telegram.Bot.Types.Message> message = null;
            Task.Run(() =>
            {
                if(e.Message.Text[0]=='#')
                {
                  message =  bot.SendTextMessageAsync(e.Message.Chat.Id, CovidInfo.GetStatCovidCountryToday(e.Message.Text.Substring(1)));
                }

                if ((e.Message.Text.ToLower() == "привет") || (e.Message.Text.ToLower() == "hello"))
                {
                    message = bot.SendTextMessageAsync(e.Message.Chat.Id, $"Привет, {senderName}");
                }
                  bot.SendTextMessageAsync(e.Message.Chat.Id, "Выбери действие", replyMarkup: keyboard);
            });
        }
    }
}


using System;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model.RequestParams;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Utils;
using VkNet.Model.Keyboard;

namespace VkBot2
{
    class Program
    {
        private static string AccessToken = "9fa214a9bd20a0f640e42bbeb142221f5686efaf9f62d53e1032723568ff77341ac77b5cf71ea7ca39272";
        private static string GroupUrl = "https://vk.com/public212716372";
        private static VkBot _bot;
        public static string chk(string s)
        {
            string[] subs = s.Split(' ');
            string[] badwords = { "гей", "лох", "loh", "gey", "дурак", "сука", "мразь", "дебил", "мудак", "токсик" };
            s = "";
            bool tr = true;
            for (int i = 0; i < subs.Length; i++)
            {
                for (int j = 0; j < badwords.Length; j++)
                    if (subs[i] == badwords[j])
                    {
                        subs[i] = "***";
                        break;
                    }
                if (subs[i] == "Денис" || subs[i] == "денис" || subs[i] == "Denis")
                    subs[i] += " (император)";
                if (subs[i] == "Ксюша" || subs[i] == "ксюша" || subs[i] == "ксения" || subs[i] == "Ксения")
                    subs[i] += " (красотка)";
                if (subs[i] == "Вика" || subs[i] == "вика" || subs[i] == "виктория" || subs[i] == "Виктория")
                    subs[i] += " (муза)";
                if (subs[i] == "Руслан" || subs[i] == "руслан")
                    subs[i] += " (староста)";
                if (subs[i] == "Настя" || subs[i] == "настя" || subs[i] == "Насть" || subs[i] == "насть")
                    subs[i] += " (безупречна)";
                if (subs[i] == "виталик" || subs[i] == "Виталик" || subs[i] == "виталь" || subs[i] == "виталя" || subs[i] == "виталий" || subs[i] == "Виталий")
                    subs[i] += " (барон)";
                if (subs[i] == "бот")
                    subs[i] += " (собственность микросемьи)";
                s += subs[i] + " ";
            }
            return s;
        }
        static void Main(string[] args)
        {
            VkApi vkapi = new VkApi();
            Random rnd = new Random();
            WebClient webclient = new WebClient() { Encoding = Encoding.UTF8 };
            string json = string.Empty;
            string URI = string.Empty;
            bool fl = true;
            while (true) //цикл авторизации
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Попытка авторизации...");
                try
                {
                    vkapi.Authorize(new ApiAuthParams()
                    {
                        AccessToken = "9fa214a9bd20a0f640e42bbeb142221f5686efaf9f62d53e1032723568ff77341ac77b5cf71ea7ca39272", //вставляем сюда ключ для работы с API
                        Settings = Settings.All //разрешаем все настройки
                    });
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Авторизация успешно завершена");
                    break; //если авторизация будет успешной, закрываем цикл
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ошибка авторизации, попробуйте снова"); // если авторизация не будет выполнена успешно - пробуем снова.
                }
            }
            //Создаем параметры для передачи методу
            //groups.getLongPollServer.
            //Множество других методов можно найти в официальной документации VK API
            var param = new VkParameters();
            param.Add<string>("group_id", "212716372");
            dynamic lpresponce = JObject.Parse(vkapi.Call("groups.getLongPollServer", param).RawJson);//отправляем запрос на Long Poll и получаем данные key server ts.
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Запрос значений");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"server: {lpresponce?.response?.server?.ToString()}\nkey: {lpresponce?.response?.key?.ToString()}\nts: {lpresponce?.response?.ts?.ToString()}");


            while (true)//цикл обработки событий
            {
                //для получения событий с сервера нужно выполнить запрос по адресу https://{$server}?act=a_check&key={$key}&ts={$ts}&wait=25, составим его:
                URI = string.Format("{0}?act=a_check&key={1}&ts={2}&wait=25",
                    lpresponce?.response?.server?.ToString(),//адрес сервера
                    lpresponce?.response?.key?.ToString(),   //секретный ключ сессии
                    json != string.Empty ? JObject.Parse(json)["ts"].ToString() : lpresponce?.response?.ts?.ToString()//номер последнего события, начиная с которого нужно получать данные,
                                                                                                                      //именно поэтому мы вызываем тернарный оператор.
                    );
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nСобранная ссылка: {URI}\n");
                json = webclient.DownloadString(URI);//загружаем запрошенный ресурс в переменную json.
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"\nПолученный JSON: {json}\n");
                var msgcol = JObject.Parse(json)["updates"].ToList();//парсим json, а именно тег updates, и приводим в коллекцию List.
                foreach (var item in msgcol)//проходимся по коллекции.
                {

                    KeyboardBuilder key = new KeyboardBuilder();
                    key.AddButton("Крутышка", "");
                    MessageKeyboard keyboard = key.Build();

                    if (item["type"].ToString() == "message_new")//если получили событие, содержащее message_new в теге type, то парсим json дальше (помимо этого могут приходить разные собития, например, когда боту печатают).
                    {
                        string message = item["object"]["message"]["text"].ToString();
                        int id = int.Parse(item["object"]["message"]["peer_id"].ToString());
                        Console.WriteLine(message);
                        message = chk(message).ToLower().Trim();
                        switch(message.ToLower())
                        {
                            case "привет":
                                vkapi.Messages.Send(new MessagesSendParams
                                {
                                    RandomId = rnd.Next(100000, 1000000000),//рандомный идентификатор сообщения (не знаю, зачем это сделали, но пишут: "уникальный (в привязке к API_ID и ID отправителя) идентификатор, предназначенный для предотвращения повторной отправки одинакового сообщения").
                                    PeerId = id,                        //идентификатор пользователя, которому отправляется ответ   
                                    Message = "Привет, Бот!",
                                    Keyboard = keyboard
                                });
                                break;
                            case "пока":
                                vkapi.Messages.Send(new MessagesSendParams
                                {
                                    RandomId = rnd.Next(100000, 1000000000),//рандомный идентификатор сообщения (не знаю, зачем это сделали, но пишут: "уникальный (в привязке к API_ID и ID отправителя) идентификатор, предназначенный для предотвращения повторной отправки одинакового сообщения").
                                    PeerId = id,                        //идентификатор пользователя, которому отправляется ответ   
                                    Message = "Fuckin slave you",
                                    Keyboard = keyboard
                                });
                                break;
                            default:
                                vkapi.Messages.Send(new MessagesSendParams
                                {
                                    RandomId = rnd.Next(100000, 1000000000),//рандомный идентификатор сообщения (не знаю, зачем это сделали, но пишут: "уникальный (в привязке к API_ID и ID отправителя) идентификатор, предназначенный для предотвращения повторной отправки одинакового сообщения").
                                    PeerId = id,                        //идентификатор пользователя, которому отправляется ответ   
                                    Message = "Может хватит уже писать всякую дичь?! Ты норм ваще?",
                                    Keyboard = keyboard
                                });
                                break;
                        }
                    }
                }
            }
        }
    }
}
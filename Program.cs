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
using VkNet.Enums.SafetyEnums;
using System.IO;
using System.Text.Json;

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

        private static void SendMessage(VkApi vk, long? peer, string message, MessageKeyboard MK = null)
        {
            Random rnd = new Random();
            vk.Messages.Send(new MessagesSendParams
            {
                RandomId = rnd.Next(100000, 1000000000),//рандомный идентификатор сообщения (не знаю, зачем это сделали, но пишут: "уникальный (в привязке к API_ID и ID отправителя) идентификатор, предназначенный для предотвращения повторной отправки одинакового сообщения").
                PeerId = peer,                        //идентификатор пользователя, которому отправляется ответ   
                Message = message,
                Keyboard = MK
            });
        }

        private static MessageKeyboard YesNo()
        {
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("Да", "yes", KeyboardButtonColor.Positive);
            key.AddButton("Нет", "no", KeyboardButtonColor.Negative);
            return key.Build();
        }

        private static MessageKeyboard Trials()
        {
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("Только ЕГЭ", "EGE", KeyboardButtonColor.Primary);
            key.AddButton("ЕГЭ + ВИ", "VI", KeyboardButtonColor.Primary);
            return key.Build();
        }

        private static MessageKeyboard Prosto()
        {
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("Привет", "hello", KeyboardButtonColor.Default);
            key.AddButton("Да", "yes", KeyboardButtonColor.Positive);
            key.AddButton("Нет", "no", KeyboardButtonColor.Negative);
            key.AddLine();
            key.AddButton("Танечка Марченко", "Tanya", KeyboardButtonColor.Primary);
            return key.Build();
        }

        private static MessageKeyboard KeyBoardNone()
        {
            KeyboardBuilder key = new KeyboardBuilder();
            key.Clear();
            return key.Build();
        }

        private static MessageKeyboard items()
        {
            KeyboardBuilder keyboardBuilder = new KeyboardBuilder();
            keyboardBuilder.AddButton("Литература", "Literature", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Русский язык", "rus", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Физика", "physics", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("История", "history", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Информатика", "computer science", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Биология", "chemistry", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("Химия", "chemistry", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Иностранный язык", "Foreign language", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Обществознание", "Social Studies", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("Профильная математика", "math", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("Очистить", "cancel", KeyboardButtonColor.Default);
            keyboardBuilder.AddButton("Выйти", "exit", KeyboardButtonColor.Negative);
            return keyboardBuilder.Build();
        }

        private static MessageKeyboard items_vi()
        {
            KeyboardBuilder keyboardBuilder = new KeyboardBuilder();
            keyboardBuilder.AddButton("Литература", "Literature", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Русский язык", "rus", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Физика", "physics", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("История", "history", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Информатика", "computer science", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Биология", "chemistry", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("Химия", "chemistry", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Иностранный язык", "Foreign language", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("Обществознание", "Social Studies", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("Профильная математика", "math", KeyboardButtonColor.Primary);
            keyboardBuilder.AddButton("ВИ", "vi", KeyboardButtonColor.Primary);
            keyboardBuilder.AddLine();
            keyboardBuilder.AddButton("Очистить", "cancel", KeyboardButtonColor.Default);
            keyboardBuilder.AddButton("Выйти", "exit", KeyboardButtonColor.Negative);
            return keyboardBuilder.Build();
        }

        private static MessageKeyboard Start()
        {
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("Привет", "hello", KeyboardButtonColor.Positive);
            return key.Build();
        }

        private static MessageKeyboard TrueFalse()
        {
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("Всё верно", "TrueAll", KeyboardButtonColor.Positive);
            key.AddButton("Очистить", "FalseAll", KeyboardButtonColor.Negative);
            return key.Build();
        }

        static void Main(string[] args)
        {
            VkApi vkapi = new VkApi();
            Random rnd = new Random();
            WebClient webclient = new WebClient() { Encoding = Encoding.UTF8 };
            string json = string.Empty;
            string URI = string.Empty;
            string clase = string.Empty;
            int count = 0, count_items = 0, N = 10;
            bool fl = true, flag_source = false;
            bool[] flags = new bool[N];
            for (int i = 0; i < N; i++)
            {
                flags[i] = false;
            }
            flags[0] = true;
            Dictionary<string, int> scores = new Dictionary<string, int>();
            Dictionary<string, bool> Items = new Dictionary<string, bool>()
            {
                { "математика", false },
                { "русский язык", false },
                { "физика", false },
                { "химия", false },
                { "история", false },
                { "обществознание", false },
                { "информатика", false },
                { "биология", false },
                { "иностранный язык", false },
                { "литература", false },
                { "ви", false },
            };
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
                    if (item["type"].ToString() == "message_new")//если получили событие, содержащее message_new в теге type, то парсим json дальше (помимо этого могут приходить разные собития, например, когда боту печатают).
                    {
                        string message = item["object"]["message"]["text"].ToString();
                        int id = int.Parse(item["object"]["message"]["peer_id"].ToString());
                        switch (message.ToLower())
                        {
                            case "привет":
                                if (flags[0] == true)
                                {
                                    SendMessage(vkapi, id, "Привет, ты попал в Мосбот-помощника😎\nХочешь проверить свои шансы попасть к нам в вуз?", YesNo());
                                    flags[0] = false;
                                    flags[1] = true;
                                    break;
                                }
                                else
                                {
                                    goto default;
                                }
                            case "да":
                                if (flags[1] == true)
                                {
                                    SendMessage(vkapi, id, "Вы сдавали только егэ или егэ + вступительные испытания?", Trials());
                                    flags[1] = false;
                                    flags[2] = true;
                                    break;
                                }
                                else
                                {
                                    goto default;
                                }
                            case "нет":
                                if (flags[1] == true)
                                {
                                    SendMessage(vkapi, id, "Очень жаль, надеемся, что когда вы вернётесь, то передумаете, удачи в поступлении! :(", Start());
                                    flags[1] = false;
                                    flags[0] |= true;
                                    break;
                                }
                                else
                                {
                                    goto default;
                                }
                            case "только егэ":
                                count_items = 3;
                                if (flags[2] == true)
                                {
                                    SendMessage(vkapi, id, "Выберите предмет, который вы сдавали.", items());
                                    flags[2] = false;
                                    flags[3] = true;
                                    foreach (var items in Items)
                                    {
                                        Items[items.Key] = true;
                                    }
                                    item["ВИ"] = false;
                                    break;
                                }
                                else
                                {
                                    goto default;
                                }
                            case "егэ + ви":
                                count_items = 4;
                                if (flags[2] == true)
                                {
                                    SendMessage(vkapi, id, "Выберите предмет, который вы сдавали.", items_vi());
                                    flags[2] = false;
                                    flags[3] = true;
                                    foreach (var items in Items)
                                    {
                                        Items[items.Key] = true;
                                    }
                                    break;
                                }
                                else
                                {
                                    goto default;
                                }
                            case "пока":
                                SendMessage(vkapi, id, "Fuckin slave you", Prosto());
                                break;
                            case "танечка марченко":
                                SendMessage(vkapi, id, "Вообще-то Танечка Марченкова!!!😡😡😡");
                                break;
                            case "профильная математика":
                                if (Items["математика"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по профильной математике.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "математика";
                                    Items["математика"] = false;
                                    break;
                                } 
                                else if (Items["математика"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else 
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "русский язык":
                                if (Items["русский язык"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по русскому языку.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "русский язык";
                                    Items["русский язык"] = false;
                                    break;
                                }
                                else if (Items["русский язык"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "информатика":
                                if (Items["информатика"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по информатике.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "мнформатика";
                                    Items["инорматика"] = false;
                                    break;
                                }
                                else if (Items["информатика"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "биология":
                                if (Items["биология"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по биологии.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "биология";
                                    Items["биология"] = false;
                                    break;
                                }
                                else if (Items["биология"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "история":
                                if (Items["история"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по истории.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "история";
                                    Items["история"] = false;
                                    break;
                                }
                                else if (Items["история"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "химия":
                                if (Items["химия"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по химии.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "химия";
                                    Items["химия"] = false;
                                    break;
                                }
                                else if (Items["химия"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "иностранный язык":
                                if (Items["иностранный язык"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по иностранному языку.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "иностранный язык";
                                    Items["иностранный язык"] = false;
                                    break;
                                }
                                else if (Items["иностранный язык"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "физика":
                                if (Items["физика"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по физике.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "физика";
                                    Items["физика"] = false;
                                    break;
                                }
                                else if (Items["физика"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "обществознание":
                                if (Items["обществознание"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по обществознанию.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "обществознание";
                                    Items["обществознание"] = false;
                                    break;
                                }
                                else if (Items["обзествознание"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "литература":
                                if (Items["литература"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по литературе.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "литература";
                                    Items["литература"] = false;
                                    break;
                                }
                                else if (Items["литература"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "ви":
                                if (Items["ви"] == true)
                                {
                                    SendMessage(vkapi, id, "Введите количество баллов по вступительным испытаниям.", KeyBoardNone());
                                    flag_source = true;
                                    clase = "ВИ";
                                    Items["ви"] = false;
                                    break;
                                }
                                else if (Items["ви"] == false && flags[2] == false)
                                {
                                    goto default;
                                }
                                else
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                            case "выйти":
                                if(flags[3] == true)
                                {
                                    flag_source = false;
                                    flags[3] = false;
                                    SendMessage(vkapi, id, "Возвращайся как можно скорее, ждём именно тебя в стенах Мосполитеха!!!", Start());
                                    for(int i = 0; i < N; i++)
                                    {
                                        flags[i] = false;
                                    }
                                    flags[0] = true;
                                    foreach (var items in Items)
                                    {
                                        Items[items.Key] = false;
                                    }
                                    scores.Clear();
                                    count = 0;
                                }
                                else
                                {
                                    goto default;
                                }
                                break;
                            case "ПОВТОР ПРЕДМЕТА":
                                flag_source = false;
                                if (count_items == 3)
                                {
                                    SendMessage(vkapi, id, "Вы уже ввели этот предмет, пожалуйста, введите новый😜", items());
                                }
                                else
                                {
                                    SendMessage(vkapi, id, "Вы уже ввели этот предмет, пожалуйста, введите новый😜", items_vi());
                                }
                                break;
                            case "НЕПРАВИЛЬНЫЙ ВВОД БАЛЛОВ":
                                SendMessage(vkapi, id, "Вы ввели неккоректное количество баллов, попробуйте ввести ещё раз.", KeyBoardNone());
                                break;
                            case "ВЫВОД БАЛЛОВ":
                                flag_source = false;
                                string PrintSource = "Вы ввели:\n";
                                int sum = 0;
                                foreach(var score in scores)
                                {
                                    PrintSource = PrintSource + score.Key + " - " + score.Value + " баллов\n";
                                    sum += score.Value;
                                }
                                PrintSource = PrintSource + "Общее количество баллов: " + sum;
                                SendMessage(vkapi, id, PrintSource, TrueFalse());
                                break;
                            case "всё верно":
                                flag_source = false;
                                SendMessage(vkapi, id, "Выводим список специальностей, на которые можно поступить", KeyBoardNone());
                                break;
                            case "ПРОДОЛЖИТЬ":
                                if (count_items == 3)
                                {
                                    SendMessage(vkapi, id, "Выберите следующий предмет.", items());
                                }
                                else
                                {
                                    SendMessage(vkapi, id, "Выберите следующий предмет.", items_vi());
                                }
                                break;
                            case "очистить":
                                for (int i = 0; i < N; i++)
                                {
                                    flags[i] = false;
                                }
                                flags[1] = true;
                                foreach (var items in Items)
                                {
                                    Items[items.Key] = false;
                                }
                                scores.Clear();
                                count = 0;
                                goto case "да";
                            default:
                                int value;
                                bool succes = Int32.TryParse(message, out value);
                                if (succes && flag_source)
                                {
                                    if (count < count_items - 1)
                                    {
                                        if (value <= 100 && value >= 0)
                                        {
                                            scores.Add(clase, value);
                                            count++;
                                            goto case "ПРОДОЛЖИТЬ";
                                        }
                                        else
                                        {
                                            goto case "НЕПРАВИЛЬНЫЙ ВВОД БАЛЛОВ";
                                        }
                                    }
                                    else
                                    {
                                        if (value <= 100 && value >= 0)
                                        {
                                            scores.Add(clase, value);
                                            count++;
                                        }
                                        else
                                        {
                                            goto case "НЕПРАВИЛЬНЫЙ ВВОД БАЛЛОВ";
                                        }
                                        foreach (var items in Items)
                                        {
                                            Items[items.Key] = false;
                                        }
                                        goto case "ВЫВОД БАЛЛОВ";
                                    }
                                }
                                else if (flag_source == true)
                                {
                                    goto case "ПОВТОР ПРЕДМЕТА";
                                }
                                else
                                {
                                    SendMessage(vkapi, id, "К сожалению, такой команды нет, выберите существующую команду :(");
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
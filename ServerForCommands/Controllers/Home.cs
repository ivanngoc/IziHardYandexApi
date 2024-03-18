using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using IziHardGames.Libs.IoT.Controls;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace IziHardGames.YandexApi.FeaturesForAlice
{
    //[Route("[controller]/[action]")]
    //[ApiController]
    public class Home : Controller
    {
        private static readonly string[] tokensActionsTurnOffPc = new string[] { "выключи", "выключить", "отключи", "отключить" };
        private static readonly string[] tokensActionsTurnOnPc = new string[] { "включи", "включить", "запусти", "запустить" };
        private static readonly string[] tokensTargetsTurnOnPc = new string[] { "пк", "компьютер" };
        public Home()
        {
            Console.WriteLine($"{GetType().FullName} создан");
        }
        [Route("/home/index")]
        public string Index()
        {
            return "Hello METANIT.COM";
        }

        //[HttpPost("/yandexapi/echo")]
        public AliceResponse WebHook(AliceRequest req)
        {
            return new AliceResponse()
            {
                //Session = req.Session,
                //Version = req.Version,
                //Response = new ResponseModel()
                //{
                //    Tts = $"Начинаю эхо: {req.Request.Command}. Конец эхо",
                //    EndSession = false,
                //},
            };
        }

        [HttpGet]
        [Route("/yandexapi/debugaction")]
        public IActionResult DebugAction()
        {
            Console.WriteLine($"{nameof(DebugAction)} Fired");
            return base.Json(new AliceResponse());
        }


        [HttpPost]
        [Route("/yandexapi/Postman")]
        public TestType Postman([FromBody] TestType value)
        {
            value.Name += " Add Tag To NAme";
            value.Neset.Value += 111;
            return value;
        }
        [HttpPost]
        [Route("/yandexapi/echo")]
        public ActionResult Echo([FromBody] AliceRequest req)
        {
            Console.WriteLine($"{nameof(Echo)} Fired. Income:{req.Request.Command}");
            return base.Json(new AliceResponse()
            {
                Session = req.Session,
                Version = req.Version,
                Response = new ResponseModel()
                {
                    //Tts = $"начинаю эхо: {req.Request.Command}. конец эхо",
                    //Tts = "Это текстовый ответ",
                    //Text ="Это текстовый ответ",
                    Text = "Здравствуйте! Это мы, хороводоведы.",
                    Tts = "Здравствуйте! Это мы, хоров+одо в+еды.",
                    EndSession = false,
                }
            });
        }
        private static readonly object sharedLock = new object();
        [HttpPost]
        [Route("/yandexapi/pc_on")]
        public async Task<ActionResult> TurnOnPC([FromBody] AliceRequest req)
        {
            Console.WriteLine($"{nameof(TurnOnPC)} Fired. Income:{req.Request.Command}");
            Console.WriteLine($"{nameof(TurnOnPC)} PArameters:{req.ToInfo()}");

            try
            {
                var nlu = req.Request.Nlu;

                if (req.Request.Nlu.Intents.Container == null)
                {
                    var tokens = req.Request.Nlu.Tokens;
                    if (tokens.Intersect(tokensActionsTurnOnPc).Count() > 0 && tokens.Intersect(tokensTargetsTurnOnPc).Count() > 0)
                    {
                        WakeOnLan.SendSignal(WakeOnLan.broPC);
                        return base.Json(new AliceResponse()
                        {
                            Session = req.Session,
                            Version = req.Version,
                            Response = new ResponseModel()
                            {
                                Text = $"Компьютер запущен т+окенами! {tokens.Aggregate((x, y) => x + ", " + y)}",
                                Tts = $"Компьютер запущен т+окенами! {tokens.Aggregate((x, y) => x + ", " + y)}",
                                EndSession = true,
                            }
                        });
                    }
                    else if (tokens.Any(x => tokensTargetsTurnOnPc.Any(y => y == x) && tokens.Any(x => tokensActionsTurnOffPc.Any(y => y == x))))
                    {
                       await TurnOffPC.TurnOfAsync("192.168.0.5");
                        return base.Json(new AliceResponse()
                        {
                            Session = req.Session,
                            Version = req.Version,
                            Response = new ResponseModel()
                            {
                                Text = $"Компьютер отключен т+окенами! {tokens.Aggregate((x, y) => x + ", " + y)}",
                                Tts = $"Компьютер отключен т+окенами! {tokens.Aggregate((x, y) => x + ", " + y)}",
                                EndSession = true,
                            }
                        });
                    }
                }
                else
                {
                    switch (nlu.Intents.Container["action"].value)
                    {
                        case "запусти": goto case "включить";
                        case "запустить": goto case "включить";
                        case "включи": goto case "включить";
                        case "включить":
                            {
                                var whatValue = req.Request.Nlu.Intents.Container["what"].value ?? string.Empty;

                                if (nlu.Intents.Container.Slots.ContainsKey("whose"))
                                {
                                    var whoseValue = req.Request.Nlu.Intents.Container["whose"].value ?? string.Empty;

                                    if (whatValue.Equals("пк", StringComparison.OrdinalIgnoreCase) || whatValue.Equals("компьютер", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (whoseValue.Equals("вираня", StringComparison.OrdinalIgnoreCase) || whoseValue.Equals("брата", StringComparison.OrdinalIgnoreCase) || whoseValue.Equals("", StringComparison.OrdinalIgnoreCase))
                                        {
                                            Console.WriteLine($"{nameof(TurnOnPC)} включаю ПК");
                                            WakeOnLan.SendSignal(WakeOnLan.broPC);
                                            return base.Json(new AliceResponse()
                                            {
                                                Session = req.Session,
                                                Version = req.Version,
                                                Response = new ResponseModel()
                                                {
                                                    Text = $"Компьютер {whoseValue} запущен!",
                                                    Tts = $"Компьютер {whoseValue} запущен!",
                                                    EndSession = true,
                                                }
                                            });
                                        }
                                        else
                                        {
                                            return base.Json(new AliceResponse()
                                            {
                                                Session = req.Session,
                                                Version = req.Version,
                                                Response = new ResponseModel()
                                                {
                                                    Text = $"Компьютер {whoseValue} не прописан и неизвестен!",
                                                    Tts = $"Компьютер {whoseValue} не прописан и неизвестен!",
                                                    EndSession = true,
                                                }
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    await WakeOnLan.SendSignalAsync(WakeOnLan.broPC);
                                    return base.Json(new AliceResponse()
                                    {
                                        Session = req.Session,
                                        Version = req.Version,
                                        Response = new ResponseModel()
                                        {
                                            Text = $"Запускаю компьютер по умолчанию",
                                            Tts = $"Запускаю компьютер по умолчанию",
                                            EndSession = true,
                                        }
                                    });
                                }
                                break;
                            }
                        default: throw new System.NotImplementedException();
                    }
                }

                return base.Json(new AliceResponse()
                {
                    Session = req.Session,
                    Version = req.Version,
                    Response = new ResponseModel()
                    {
                        //Tts = $"начинаю эхо: {req.Request.Command}. конец эхо",
                        //Tts = "Это текстовый ответ",
                        //Text ="Это текстовый ответ",
                        Text = $"Команда не выполнена. Не распознано что включить",
                        Tts = $"Команда не выполнена. Не распознано что включить",
                        EndSession = true,
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return base.Json(new AliceResponse()
                {
                    Session = req.Session,
                    Version = req.Version,
                    Response = new ResponseModel()
                    {
                        //Tts = $"начинаю эхо: {req.Request.Command}. конец эхо",
                        //Tts = "Это текстовый ответ",
                        //Text ="Это текстовый ответ",
                        Text = $"Exception:{ex.Message}",
                        Tts = $"Возникло Общее Исключение",
                        EndSession = true,
                    }
                });
            }
        }
    }

    public class TestType
    {
        [JsonPropertyName("surename")]
        public string Name { get; set; } = "This is my name";
        public int Value { get; set; } = 1;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public NestedObjectData Neset { get; set; } = new NestedObjectData();
    }

    public class NestedObjectData
    {
        public int Value { get; set; } = 1000;
    }
}
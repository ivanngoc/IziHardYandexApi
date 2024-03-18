
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace IziHardGames.YandexApi
{
    public class UserModel
    {
        [JsonPropertyName("user_id")] public string UserId { get; set; }
    }


    public class ApplicationModel
    {
        [JsonPropertyName("application_id")] public string ApplicationId { get; set; }
    }

    public class SessionModel
    {
        [JsonPropertyName("message_id")] public int MessageId { get; set; }
        [JsonPropertyName("session_id")] public string SessionId { get; set; }
        [JsonPropertyName("skill_id")] public string SkillId { get; set; }
        [JsonPropertyName("user")] public UserModel User { get; set; }
        [JsonPropertyName("application")] public ApplicationModel Application { get; set; }
        [JsonPropertyName("new")] public bool New { get; set; }
        [JsonPropertyName("user_id")] public string UserId { get; set; }
    }
    public class NluModel
    {
        [JsonPropertyName("tokens")] public string[] Tokens { get; set; }
        [JsonPropertyName("intents")] public IntentRuleModel Intents { get; set; }
    }
    public class IntentRuleModel
    {
        [JsonPropertyName("base_intent")] public ContainerForSlotsModel Container { get; set; }
    }
    public class ContainerForSlotsModel
    {
        [JsonPropertyName("slots")] public Dictionary<string, SlotModel> Slots { get; set; }
        public SlotModel this[string name] => Slots[name];
    }

    public class SlotModel
    {
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("value")] public string value { get; set; }
    }

    public class MarkupModel
    {

    }

    /// <summary>
    /// https://yandex.ru/dev/dialogs/alice/doc/response.html
    /// </summary>
    public class ResponseModel
    {
        [JsonPropertyName("command")] public string Command { get; set; }
        [JsonPropertyName("text")] public string Text { get; set; }
        [JsonPropertyName("original_utterance")] public string OriginalUtterance { get; set; }
        [JsonPropertyName("nlu")] public NluModel Nlu { get; set; }
        [JsonPropertyName("markup")] public MarkupModel Markup { get; set; }

        [JsonPropertyName("tts")]
        public string Tts { get; set; }

        [JsonPropertyName("end_session")]
        public bool EndSession { get; set; }

        [JsonPropertyName("buttons")]
        public ButtonModel[] Buttons { get; set; }
    }

    public class ButtonModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("payload")]
        public object Payload { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("hide")]
        public bool Hide { get; set; }
    }

    public class MetaModel
    {
        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
    }


    public class AliceRequest
    {
        //[JsonPropertyName("meta")]
        public MetaModel Meta { get; set; }
        //[JsonPropertyName("session")] 
        public SessionModel Session { get; set; }
        [JsonPropertyName("request")] public RequestModel Request { get; set; }
        //[JsonPropertyName("version")] public string Version { get; set; } = string.Empty;
        // [JsonPropertyName("meta")]
        //public string Meta { get; set; }
        // [JsonPropertyName("session")]
        //public string Session { get; set; }
        //public string Request { get; set; }

        [JsonPropertyName("version")] public string Version { get; set; }

        public string ToInfo()
        {
            return $"Requst Command:{Request.Command}; Requst OriginalUtterance:{Request.OriginalUtterance}; Request Type:{Request.Type}";
        }
    }

    public enum RequestType
    {
        SimpleUtterance,
        ButtonPressed
    }

    /// <summary>
    ///  
    /// </summary>
    /// <example>
    /*
    "request": {
    "command": "включи компьютер вираня",
    "original_utterance": "включи компьютер вираня",
    "nlu": {
      "tokens": [
        "включи",
        "компьютер",
        "вираня"
      ],
      "entities": [],
      "intents": {
        "base_intent": {
          "slots": {
            "what": {
              "type": "YANDEX.STRING",
              "tokens": {
                "start": 1,
                "end": 2
              },
              "value": "компьютер"
            },
            "whose": {
              "type": "YANDEX.STRING",
              "tokens": {
                "start": 2,
                "end": 3
              },
              "value": "вираня"
            }
          }
        }
      }
    },
     */
    /// </example>
    public class RequestModel
    {
        [JsonPropertyName("command")] public string Command { get; set; }

        [JsonPropertyName("type")] public string Type { get; set; }
        /// <summary>
        /// https://yandex.ru/dev/dialogs/alice/doc/request-simpleutterance.html
        /// </summary>
        [JsonPropertyName("original_utterance")] public string OriginalUtterance { get; set; }

        [JsonPropertyName("payload")] public object Payload { get; set; }
        [JsonPropertyName("nlu")] public NluModel Nlu { get; set; }
    }

    public class AliceResponse
    {
        [JsonPropertyName("response")]
        public ResponseModel Response { get; set; }

        [JsonPropertyName("session")]
        public SessionModel Session { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0";
    }

}
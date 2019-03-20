using System;
using System.IO;
using Newtonsoft.Json;

namespace KubeManage.Api
{
    public static class Json
    {
        #region Json序列化

        /// <summary>
        /// Json序列化
        /// </summary>
        public static string ToJson(this object item, bool format = false)
        {
            using (StringWriter sw = new StringWriter())
            {
                JsonSerializer serializer = JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,

                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                        NullValueHandling = NullValueHandling.Ignore,

                        DateFormatString = "yyyy-MM-dd HH:mm:ss"
                    }
                );

                JsonWriter jsonWriter;
                if (format)
                {
                    jsonWriter = new JsonTextWriter(sw)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                }
                else
                {
                    jsonWriter = new JsonTextWriter(sw);
                }

                using (jsonWriter)
                {
                    serializer.Serialize(jsonWriter, item);
                }

                return sw.ToString();
            }
        }

        #endregion

        #region Json反序列化

        /// <summary>
        /// Json反序列化
        /// </summary>
        public static T FromJsonTo<T>(this string jsonString) where T : new()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    T t = JsonConvert.DeserializeObject<T>(jsonString);
                    return t;
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static T FromJsonToOrDefault<T>(this string str) where T : class, new()
        {
            try
            {
                if (string.IsNullOrEmpty(str) || str == "[]" || str == "{}")
                {
                    return new T();
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(str);
                }
            }
            catch
            {
                return default(T);
            }
        }

        #endregion
    }
}
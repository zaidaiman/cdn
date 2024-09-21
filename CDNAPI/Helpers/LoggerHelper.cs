
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Helpers
{
    public static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "[{fnName}] request parameter: {requestString}")]
        public static partial void Request(ILogger logger, string fnName, string requestString);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Information,
            Message = "[{fnName}] query: {queryString}")]
        public static partial void Query(ILogger logger, string fnName, string queryString);

        [LoggerMessage(
            EventId = 3,
            Level = LogLevel.Information,
            Message = "[{fnName}] response: {responseString}")]
        public static partial void Response(ILogger logger, string fnName, string responseString);

        [LoggerMessage(
            EventId = 4,
            Level = LogLevel.Information,
            Message = "[{fnName}] info: {information}")]
        public static partial void Information(ILogger logger, string fnName, string information);

        [LoggerMessage(
            EventId = 5,
            Level = LogLevel.Warning,
            Message = "[{fnName}] warning: {warning}")]
        public static partial void Warning(ILogger logger, string fnName, string warning);

        [LoggerMessage(
            EventId = 6,
            Level = LogLevel.Error,
            Message = "[{fnName}] error: {error}")]
        public static partial void Error(ILogger logger, string fnName, string error);

        [LoggerMessage(
            EventId = 7,
            Level = LogLevel.Information,
            Message = "[{fnName}] time taken: {time}ms")]
        public static partial void TimeTaken(ILogger logger, string fnName, long time);

        internal static void Response(ILogger logger, string name, object value)
        {
            string stringify = JsonConvert.SerializeObject(value, Formatting.None, new RequestJsonConverter(1));
            Response(logger, name, stringify);
        }

        internal static void Error(ILogger logger, string name, object value)
        {
            string stringify = JsonConvert.SerializeObject(value, Formatting.None, new RequestJsonConverter(3));
            Error(logger, name, stringify);
        }

        internal static void Response(ILogger logger, string name, object value, int depth = 1)
        {
            string stringify = JsonConvert.SerializeObject(value, Formatting.None, new RequestJsonConverter(depth));
            Response(logger, name, stringify);
        }

        internal static void Warning(ILogger logger, string name, string warning, object value)
        {
            string stringify = JsonConvert.SerializeObject(value, Formatting.None, new RequestJsonConverter(3));
            Warning(logger, name, $"{warning}: {stringify}");
        }
    }

    public class RequestJsonConverter : JsonConverter
    {
        private readonly int _depth;

        public RequestJsonConverter(int depth)
        {
            _depth = depth;
        }

        public override bool CanRead => false;

        public override bool CanConvert(Type objectType) => true;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is null)
            {
                writer.WriteNull();
                return;
            }

            if (value is IEnumerable)
            {
                WriteArray(writer, value as IEnumerable);
            }
            else
            {
                WriteObject(writer, value);
            }
        }

        private void WriteObject(JsonWriter writer, object value)
        {
            var jObject = new JObject();
            var requestProperties = value.GetType().GetProperties();
            RequestJsonConverter? nextConverter = null;

            foreach (var prop in requestProperties)
            {
                if (_depth <= 0)
                {
                    continue;
                }

                try
                {
                    var propValue = prop.GetValue(value);
                    if (!prop.PropertyType.IsPrimitive &&
                        !prop.PropertyType.IsValueType &&
                        prop.PropertyType != typeof(string))
                    {
                        if (propValue is null || _depth <= 1)
                        {
                            continue;
                        }

                        nextConverter ??= new RequestJsonConverter(_depth - 1);
                        var serializedPropValue = JsonConvert.SerializeObject(propValue, Formatting.None, nextConverter);
                        jObject.Add(prop.Name, serializedPropValue);
                    }
                    else
                    {
                        jObject.Add(prop.Name, propValue?.ToString());
                    }
                }
                catch
                {
                    // ignored - property getter might have thrown an exception so skip it.
                }
            }

            jObject.WriteTo(writer);
        }

        private void WriteArray(JsonWriter writer, IEnumerable? value)
        {
            writer.WriteStartArray();
            if (value is null)
            {
                writer.WriteEndArray();
                return;
            }

            foreach (var item in value)
            {
                WriteJson(writer, item, JsonSerializer.CreateDefault());
            }

            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            => throw new NotImplementedException("Unsupported");
    }
}

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using MyHealth.Integrations.Core.IoMT.Models;

namespace MyHealth.Integrations.Core.IoMT.Serialization
{
    public class IoMTModelJsonConverter : JsonConverter<IoMTModel>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert != null && typeToConvert.IsAssignableFrom(typeof(IoMTModel));
        }

        public override IoMTModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IoMTModel value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case BodyWeight bodyWeight:
                    JsonSerializer.Serialize(writer, bodyWeight, options);
                    break;
                case BikeRide bikeRide:
                    JsonSerializer.Serialize(writer, bikeRide, options);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), $"Unknown implementation of the interface {nameof(IoMTModel)} for the parameter {nameof(value)}. Unknown implementation: {value?.GetType().Name}");
            }
        }
    }
}

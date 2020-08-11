using System.Text.Json.Serialization;
using MyHealth.Integrations.Core.IoMT.Serialization;

namespace MyHealth.Integrations.Core.IoMT.Models
{
    [JsonConverter(typeof(IoMTModelJsonConverter))]
    public abstract class IoMTModel
    {
    }
}

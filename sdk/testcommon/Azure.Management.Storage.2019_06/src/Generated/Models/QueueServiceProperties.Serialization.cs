// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace Azure.Management.Storage.Models
{
    public partial class QueueServiceProperties : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("properties");
            writer.WriteStartObject();
            if (Optional.IsDefined(Cors))
            {
                writer.WritePropertyName("cors");
                writer.WriteObjectValue(Cors);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        internal static QueueServiceProperties DeserializeQueueServiceProperties(JsonElement element)
        {
            Optional<string> id = default;
            Optional<string> name = default;
            Optional<string> type = default;
            Optional<CorsRules> cors = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("id"))
                {
                    id = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"))
                {
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("type"))
                {
                    type = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("properties"))
                {
                    foreach (var property0 in property.Value.EnumerateObject())
                    {
                        if (property0.NameEquals("cors"))
                        {
                            cors = CorsRules.DeserializeCorsRules(property0.Value);
                            continue;
                        }
                    }
                    continue;
                }
            }
            return new QueueServiceProperties(id.Value, name.Value, type.Value, cors.Value);
        }
    }
}

namespace BBT.MaybePattern.Tests
{
    using System;
    using System.IO;
    using System.Text.Json;
    using BBT.MaybePattern;
    using BBT.MaybePattern.Tests.TestData;

    /// <summary>
    /// Provides utility method for test purposes.
    /// </summary>
    public class TestUtils
    {
        private sealed class SerializedEnvelope
        {
            public string TypeName { get; set; }

            public bool? HasValue { get; set; }

            public string ValueJson { get; set; }

            public string JsonPayload { get; set; }
        }

        /// <summary>
        /// Serializes the given object into memory stream.
        /// </summary>
        /// <param name="objectType">the object to be serialized.</param>
        /// <returns>The serialized object as memory stream.</returns>
        public static MemoryStream SerializeToStream(object objectType)
        {
            var stream = new MemoryStream();
            try
            {
                var envelope = BuildEnvelope(objectType);
                JsonSerializer.Serialize(stream, envelope);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            catch
            {
                stream.Dispose();
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Deserializes as an object.
        /// </summary>
        /// <param name="stream">the stream to deserialize.</param>
        /// <returns>the deserialized object.</returns>
        public static object DeserializeFromStream(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var envelope = JsonSerializer.Deserialize<SerializedEnvelope>(stream)
                ?? throw new InvalidOperationException();

            if (envelope.TypeName == typeof(TestDataStruct).AssemblyQualifiedName)
            {
                var hasValue = envelope.HasValue.GetValueOrDefault(false);
                var value = hasValue
                    ? JsonSerializer.Deserialize<int>(envelope.ValueJson)
                    : default(int?);

                return new TestDataStruct
                {
                    MaybeStruct = hasValue
                        ? Maybe.SomeStruct<int>(value)
                        : Maybe.NoneStruct<int>(),
                };
            }

            if (envelope.TypeName == typeof(TestDataClass).AssemblyQualifiedName)
            {
                var hasValue = envelope.HasValue.GetValueOrDefault(false);
                var value = hasValue
                    ? JsonSerializer.Deserialize<object>(envelope.ValueJson)
                    : null;

                return new TestDataClass
                {
                    Maybe = hasValue
                        ? Maybe.Some(value)
                        : Maybe.None<object>(),
                };
            }

            var objectType = Type.GetType(envelope.TypeName)
                ?? throw new InvalidOperationException();

            return JsonSerializer.Deserialize(envelope.JsonPayload, objectType)
                ?? throw new InvalidOperationException();
        }

        private static SerializedEnvelope BuildEnvelope(object objectType)
        {
            if (objectType is TestDataStruct testDataStruct)
            {
                return new SerializedEnvelope
                {
                    TypeName = typeof(TestDataStruct).AssemblyQualifiedName,
                    HasValue = testDataStruct.MaybeStruct.HasValue,
                    ValueJson = JsonSerializer.Serialize(testDataStruct.MaybeStruct.ValueOrDefault(default)),
                };
            }

            if (objectType is TestDataClass testDataClass)
            {
                return new SerializedEnvelope
                {
                    TypeName = typeof(TestDataClass).AssemblyQualifiedName,
                    HasValue = testDataClass.Maybe.HasValue,
                    ValueJson = testDataClass.Maybe.HasValue
                        ? JsonSerializer.Serialize(testDataClass.Maybe.ValueOrDefault(null))
                        : null,
                };
            }

            var objectRuntimeType = objectType.GetType();
            return new SerializedEnvelope
            {
                TypeName = objectRuntimeType.AssemblyQualifiedName,
                JsonPayload = JsonSerializer.Serialize(objectType, objectRuntimeType),
            };
        }
    }
}

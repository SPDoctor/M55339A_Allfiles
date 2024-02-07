using System;
using System.IO;
using System.Text.Json;

namespace FourthCoffee.ExceptionLogger
{
    /// <summary>
    /// Represents the <see cref="FourthCoffee.ExceptionLogger.ExceptionSerializer" /> class in the object model.
    /// </summary>
    public static class ExceptionSerializer
    {
        /// <summary>
        /// Serializes an exception.
        /// </summary>
        /// <param name="entry">The exception entry to serialize.</param>
        /// <param name="path">The file path.</param>
        public static void Serialize(ExceptionEntry entry, string path)
        {
            if (entry == null)
                throw new NullReferenceException("entry");

            // Convert object to JSON string
            var json = JsonSerializer.Serialize(entry);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Deserializes an exception.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// 
        public static ExceptionEntry Deserialize(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            var entry = default(ExceptionEntry);
            var json = File.ReadAllText(path);

            // Convert JSON string to an object 
            entry = JsonSerializer.Deserialize<ExceptionEntry>(json);

            return entry;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit.Sdk;

namespace MyHealth.Extensions.Testing.Xunit
{
    public class FileContentDataAttribute : DataAttribute
    {
        private readonly string _filePath;

        public FileContentDataAttribute(string filePath)
        {
            _filePath = filePath;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);
            
            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            var fileData = File.ReadAllText(_filePath);

            return new[] { new[] { fileData } };
        }
    }
}

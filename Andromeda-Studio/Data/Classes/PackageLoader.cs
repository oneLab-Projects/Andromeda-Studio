﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AndromedaStudio.Classes
{
    /// <summary>
    /// Загружает и хранит дополнения
    /// </summary>
    public class PackageLoader
    {
        public List<Package> Packages = new List<Package>();

        /// <summary>
        /// Загружает указанное дополнение
        /// </summary>
        /// <param name="path"></param>
        public async Task Load(string path)
        {
            await Task.Run(() =>
            {
                var manifestPath = Path.Combine(path, "manifest.yml");
                if (File.Exists(manifestPath))
                {
                    var manifestRaw = File.ReadAllText(manifestPath);
                    var deserializer = new DeserializerBuilder().Build();
                    var manifestYaml = deserializer.Deserialize(new StringReader(manifestRaw));

                    var serializer = new SerializerBuilder().JsonCompatible().Build();
                    var json = serializer.Serialize(manifestYaml);

                    var packageJ = (JObject)JsonConvert.DeserializeObject(json);
                    var package = packageJ.ToObject<Package>();
                    package.Path = path;
                    Packages.Add(package);
                }
            });
        }

        /// <summary>
        /// Загружает все дополнения из указанной папки
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task LoadFromDirectory(string path)
        {
            await Task.Run(() =>
            {
                var tasks = new List<Task>();

                foreach (var addon in Directory.GetDirectories(path))
                    tasks.Add(Load(addon));

                Task.WaitAll(tasks.ToArray());
            });
        }

        /// <summary>
        /// Инициализирует все загруженные дополнения
        /// </summary>
        public void Init()
        {
            foreach (var package in Packages)
                package.Init();
        }
    }
}


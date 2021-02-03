using System.Collections.Generic;
using System.IO;
using Zen.Utilities;

namespace PhoenixGameConfig
{
    public class GameConfigRepository
    {
        private readonly Dictionary<string, DynamicDataList> _entities;

        public GameConfigRepository()
        {
            _entities = new Dictionary<string, DynamicDataList>();

            var jsonFiles = Directory.GetFiles(".\\Content\\GameMetaData", "Config.*.json");

            foreach (var jsonFile in jsonFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(jsonFile);
                var entityName = fileName.Split('.')[1];

                var jsonString = File.ReadAllText(jsonFile);
                var list = DynamicDataList.Create($"{entityName}s", jsonString);
                _entities.Add(entityName, list);
            }
        }

        public DynamicDataList GetEntities(string name)
        {
            var entities = _entities[name];

            return entities;
        }

        public List<string> ToList(DynamicDataList fromList, string nameOfToList, string fromAttributeName, string toAttributeName)
        {
            var toList = GetEntities(nameOfToList);
            var list = new List<string>();
            foreach (var item in fromList)
            {
                var fromAttributeValue = (int)item[fromAttributeName];
                var toItem = toList.GetById(fromAttributeValue);
                var toAttribute = (string)toItem[toAttributeName];
                list.Add(toAttribute);
            }

            return list;
        }
    }
}
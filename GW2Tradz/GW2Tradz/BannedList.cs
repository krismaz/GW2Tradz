using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2Tradz
{
    class BannedList
    {
        private Dictionary<string, DateTime> _bannings;
        private string _file;

        public BannedList(string file)
        {
            _file = file;
            if (File.Exists(file))
            {
                _bannings = JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(File.ReadAllText(_file));
            }
            else
            {
                _bannings = new Dictionary<string, DateTime> { };
            }
            foreach (var entry in _bannings.Select(_ => _).ToList())
            {
                if (entry.Value < DateTime.Now)
                {
                    _bannings.Remove(entry.Key);
                }
            }
            Save();
        }

        private void Save()
        {
            File.WriteAllText(_file, JsonConvert.SerializeObject(_bannings));
        }

        public void Ban(string identifier, DateTime expiry)
        {
            _bannings[identifier] = expiry;
            Save();
        }

        public bool Banned(string identifier)
        {
            return _bannings.ContainsKey(identifier) && _bannings[identifier] > DateTime.Now;
        }
    }
}

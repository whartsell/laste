using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg35Test
{
    class KeyValueStore
    {
        private Dictionary<string, object> kvStore;
        private Dictionary<string, bool> kvDirty;

        public KeyValueStore()
        {
            kvStore = new Dictionary<string, object>();
            kvDirty = new Dictionary<string, bool>();
        }

        public void set(string key, object value, bool forceDirty=false)
        {
            object stored = null;
            kvStore.TryGetValue(key, out stored);
            
            if (forceDirty | stored != value | stored == null)
            {
                kvDirty[key] = true;
                kvStore[key] = value;
            }
        }

        public void update(SpriteStore store)
        {
            foreach (var pair in store.RegisteredCallbacks)
            {
                foreach (var value in pair.Value)
                {
                    if (kvDirty.ContainsKey(value))
                    {
                        //build the callback
                        Closure callback = pair.Key;
                        List<object> parameters = new List<object>();
                        foreach (string key in pair.Value)
                        {
                            parameters.Add(kvStore[key]);
                        }
                        callback.Call(parameters.ToArray());
                        break;
                    }
                }
            }
            kvDirty.Clear();
        }
    }
}

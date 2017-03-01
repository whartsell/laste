using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace mg35Test
{
    class KeyValueStore
    {
        private Dictionary<string, object> kvStore;
        private Dictionary<string, bool> kvDirty;

        public string AircraftType { get; set; }


        public KeyValueStore()
        {
            kvStore = new Dictionary<string, object>();
            kvDirty = new Dictionary<string, bool>();
        }


        public void setAircraftType(string type) //this is needed as a proxy for lua as it doesnt have visibility to the KeyValueStore class
        {
            AircraftType = type;
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
            foreach (var pair in store.RegisteredCallbacks) // iterate through the registered callbacks
            {
                foreach (var value in pair.Value) // iterating through the registered keys for the callback
                {
                    if (kvDirty.ContainsKey(value)) // if the registered key is in the dirty store a callback needs to be made
                    {
                        //build the callback
                        Closure callback = pair.Key;
                        List<object> parameters = new List<object>(); // build the parameters based on the registered keys for the call back
                        foreach (string key in pair.Value)
                        {
                            object keyValue;
                            kvStore.TryGetValue(key, out keyValue);  // sometimes a key may not exist yet in the datastore so send nil
                            parameters.Add(keyValue);
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

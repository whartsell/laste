using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace mg35Test
{
    class Protocol
    {
        KeyValueStore kvStore;
        SpriteStore spriteStore;
        Network network;

        public Protocol(KeyValueStore store,SpriteStore spriteStore, Network network)
        {
            kvStore = store;
            this.spriteStore = spriteStore;
            this.network = network;
        }


        public void MessageReceivedHandler(object sender, MessageReceivedEventArgs e)
        {
            //Console.WriteLine("MessageReceivedHandler");
            //Console.WriteLine("{0} bytes received", e.ByteCount);
            
            if(e.ByteCount > 0)
            {
                string converted = Encoding.UTF8.GetString(e.Message, 0, e.ByteCount - 1);
                JObject message = JObject.Parse(converted);
                //Console.WriteLine(message);
                string msg_type = (string)message["msg_type"];
                switch (msg_type)
                {
                    case "new_unit":
                        handleNewUnit(message,e.Sender);
                        break;
                    case "newdata":
                        handleNewData(message,e.Sender);
                        break;
                    default:
                        break;
                } 
            }
           
            //Console.WriteLine("done");
        }

        private void handleNewData(JObject message,IPEndPoint sender)
        {
            JToken data = message["data"];
            if ((string)data["_UNITTYPE"] == kvStore.AircraftType)
            {
                //Console.WriteLine("new data for {0} received", kvStore.AircraftType);
                //Console.WriteLine(data);
                Dictionary<string, object> kvData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.ToString());
                foreach (var pair in kvData)
                {
                    kvStore.set(pair.Key, pair.Value);
                }
            }
            else
            {
                Console.WriteLine("unit type is not {0}. Ignoring", kvStore.AircraftType);
            }
        }

        private void handleNewUnit(JObject message,IPEndPoint sender)
        {
            if((string)message["type"] == kvStore.AircraftType)
            {
                Console.WriteLine("unit type change received. Sending subscription");
                subscribe(sender);
            }
            else
            {
                Console.WriteLine("unit type is not {0}. Ignoring", kvStore.AircraftType);
            }
        }


        private void subscribe(IPEndPoint sender)
        {
            // need a place to store the proper port for subscribing
            sender.Port = 12801;

            Console.WriteLine("endpoint for subscription {0}:{1}",sender.Address,sender.Port);
            JObject message = new JObject();
            JProperty action = new JProperty("action", "subscribe");
            JArray data = new JArray(spriteStore.getAllSubscribedKeys());
            JProperty keys = new JProperty("keys", data);
            message.Add(action);
            message.Add(keys);
            string json = message.ToString(Formatting.None) + "\n"; // for some reason the server requires the newline
            Console.WriteLine("subscribe: {0}", json);
            network.sendMessage(sender,json);


            
            
            

        }
    }
}

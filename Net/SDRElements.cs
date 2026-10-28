using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServerPicker.Net
{
    public class SdrConfig
    {
        [JsonProperty("revision")]
        public long Revision { get; set; }
    
        [JsonProperty("pops")]
        public Dictionary<string, Pop> Pops { get; set; } = new Dictionary<string, Pop>();
    
        [JsonProperty("certs")]
        public List<string> Certs { get; set; } = new List<string>();
    
        [JsonProperty("p2p_share_ip")]
        public Dictionary<string, int> P2PShareIp { get; set; } = new Dictionary<string, int>();
    
        [JsonProperty("relay_public_key")]
        public string RelayPublicKey { get; set; } = "";
    
        [JsonProperty("revoked_keys")]
        public List<string> RevokedKeys { get; set; } = new List<string>();
    
        [JsonProperty("typical_pings")]
        public JArray TypicalPings { get; set; }
    
        [JsonProperty("success")]
        public bool Success { get; set; }
    }
    
    public class Pop
    {
        [JsonProperty("desc")]
        public string Desc { get; set; } = "";
    
        [JsonProperty("aliases")]
        public List<string> Aliases { get; set; }
    
        // [longitude, latitude]
        [JsonProperty("geo")]
        public double[] Geo { get; set; } = new double[2];
    
        [JsonProperty("partners")]
        public int Partners { get; set; }
    
        [JsonProperty("tier")]
        public int Tier { get; set; }
    
        [JsonProperty("relays")]
        public List<Relay> Relays { get; set; }
    }
    
    public class Relay
    {
        [JsonProperty("ipv4")]
        public string Ipv4 { get; set; } = "";
    
        [JsonProperty("port_range")]
        public int[] PortRange { get; set; } = new int[2];
    }
    
    public class TypicalPing
    {
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public int Ms { get; set; }
    }
}
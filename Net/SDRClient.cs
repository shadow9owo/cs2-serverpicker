using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ServerPicker.Net
{
    public class SDRClient
    {
        public SdrConfig SDRElements_local = new SdrConfig();

        public async Task<string> FetchJson()
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://api.steampowered.com/ISteamApps/GetSDRConfig/v1/?appid=730");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public SdrConfig Parse(string input)
        {
            SdrConfig SDRConfig_local = JsonConvert.DeserializeObject<SdrConfig>(input);

            return SDRConfig_local;
        }
    }
}

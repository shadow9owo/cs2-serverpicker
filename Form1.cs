using ServerPicker.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ServerPicker
{
    public partial class Form1 : Form
    {
        private SDRClient sdrclient;
        private FireWallHelper firewall = new FireWallHelper();
        private List<Pop> popList = new List<Pop>();
        public Form1()
        {
            InitializeComponent();
        }

        public async Task NetworkInit()
        {
            string output = await sdrclient.FetchJson();

            if (output == null)
            {
                MessageBox.Show("failed to fetch valve api. Are you connected to the internet?", ";????;", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;
            }

            SdrConfig config = sdrclient.Parse(output);
            if (config == null || config.Pops == null) return;
            sdrclient.SDRElements_local = config;

            foreach (var kv in config.Pops.Where(p => p.Value.Relays != null))
            {
                clbServers.Items.Add(kv.Key + " - " + kv.Value.Desc);
                popList.Add(kv.Value);

                foreach (var relay in kv.Value.Relays)
                {
                    firewall.Block(relay.Ipv4, relay.PortRange[0], relay.PortRange[1]);
                }
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            sdrclient = new SDRClient();
            await NetworkInit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var kv in sdrclient.SDRElements_local.Pops.Where(p => p.Value.Relays != null))
            {
                foreach (var item in kv.Value.Relays)
                {
                    firewall.UnBlock(item.Ipv4);
                }
            }
        }

        private void Form1_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            string message =
                "Pick any server you want to be queued onto all other servers are automatically blocked\n" +
                "(and on exit all those firewall rules are removed)\n" +
                "\n" +
                "how does this tool work?\n" +
                "it blocks specific server ips to force you to queue to the server of your choice\n" +
                "\n" +
                "avastantivirus 2026";

            MessageBox.Show(message, "help", MessageBoxButtons.OK, MessageBoxIcon.Information);
            e.Cancel = true;
        }

        private void clbServers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            foreach (var relay in popList[e.Index].Relays)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    firewall.UnBlock(relay.Ipv4);
                }
                else
                {
                    firewall.Block(relay.Ipv4, relay.PortRange[0], relay.PortRange[1]);
                }
            }
        }
    }
}

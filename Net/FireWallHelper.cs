using System;
using NetFwTypeLib;

namespace ServerPicker.Net
{
    public class FireWallHelper
    {
        INetFwPolicy2 policy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

        public void Block(string ip, int portrangemin, int portrangemax)
        {
            INetFwRule rule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));

            rule.Name = "ServerPicker_" + ip;
            rule.Direction = NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT;
            rule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
            rule.Protocol = 17; // UDP
            rule.RemoteAddresses = ip;
            rule.RemotePorts = portrangemin + "-" + portrangemax;
            rule.Enabled = true;

            policy.Rules.Add(rule);
        }

        public void UnBlock(string ip)
        {
            policy.Rules.Remove("ServerPicker_" + ip);
        }

        public void Dispose()
        {
            foreach (var rule in policy.Rules)
            {
                if (rule is INetFwRule fwRule && fwRule.Name.StartsWith("ServerPicker_"))
                {
                    policy.Rules.Remove(fwRule.Name);
                }
            }
        }
    }
}
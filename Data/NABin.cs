namespace IPCalculator.Web.Data
{
    public class NABin
    {
        public NABin()
        {

        }

        public NABin(string networkPart, string hostPart)
        {
            NetworkPart = networkPart;
            HostPart = hostPart;
        }

        public NABin(string networkPart, string subNetworkPart, string hostPart)
        {
            NetworkPart = networkPart;
            SubNetworkPart = subNetworkPart;
            HostPart = hostPart;
        }

        public string NetworkPart { get; set; }
        public string SubNetworkPart { get; set; }
        public string HostPart { get; set; }

    }
}
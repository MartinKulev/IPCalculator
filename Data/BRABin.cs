namespace IPCalculator.Web.Data
{
    public class BRABin
    {
        public BRABin()
        {

        }

        public BRABin(string networkPart, string hostPart)
        {
            NetworkPart = networkPart;
            HostPart = hostPart;
        }

        public BRABin(string networkPart, string subNetworkPart, string hostPart)
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
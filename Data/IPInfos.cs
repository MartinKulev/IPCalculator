using System.ComponentModel.DataAnnotations;

namespace IPCalculator.Web.Data
{
    public class IPInfos
    {
        public IPInfos()
        {

        }

        public IPInfos(string ipDec, string ipBin, string networkClass, string smDec, SMBin smBin,
            string naDec, NABin naBin, string braDec, BRABin braBin, string range, List<string> ipBinOctets)
        {
            IPDec = ipDec;
            IPBin = ipBin;
            NetworkClass = networkClass;
            SMDec = smDec;
            SMBin = smBin;
            NADec = naDec;
            NABin = naBin;
            BRADec = braDec;
            BRABin = braBin;
            Range = range;
            IPBinOctets = ipBinOctets;
        }

        public string IPDec { get; set; }
        public string IPBin { get; set; }
        public string NetworkClass { get; set; }
        public string SMDec { get; set; }
        public SMBin SMBin { get; set; }
        public string NADec { get; set; }
        public NABin NABin { get; set; }
        public string BRADec { get; set; }
        public BRABin BRABin { get; set; }
        public string Range { get; set; }
        public List<string> IPBinOctets { get; set; }
        public int CurrentSubnetNumber { get; set; }
        public int Power { get; set; }
        public int SubnetCount { get; set; }
        public string SubnetSMDec { get; set; }
        public SMBin SubnetSMBin { get; set; }
        public string SubnetNADec { get; set; }
        public NABin SubnetNABin { get; set; }
        public string SubnetBRADec { get; set; }
        public BRABin SubnetBRABin { get; set; }
        public string SubnetRange { get; set; }
    }
}

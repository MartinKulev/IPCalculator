using IPCalculator.Web.Data;
using System.Security;
using System.Text;

namespace IPCalculator.Web.Services
{
    public class InternetProtocolSubService
    {
        public static List<string> GetIPBin(List<string> ipDecOctets)
        {
            List<string> ipBinOctets = new List<string>();

            for (int a = 0; a < 4; a++)
            {
                string binOctet = Convert.ToString(int.Parse(ipDecOctets[a]), 2);
                string ipBinOctet = string.Empty;
                for (int b = binOctet.Length; b < 8; b++)
                {
                    ipBinOctet += "0";
                }
                ipBinOctet += binOctet;
                ipBinOctets.Add(ipBinOctet);
            }
            return ipBinOctets;
        }

        public static string GetNetworkClass(string ipBin)
        {
            if (ipBin.StartsWith("110"))
            {
                return "C";
            }
            else if (ipBin.StartsWith("0"))
            {
                return "A";
            }
            else if (ipBin.StartsWith("10"))
            {
                return "B";
            }
            return string.Empty;
        }

        public static string IPConvertBinToDec(string binString)
        {
            List<string> subnetBinOctets =
            [
                binString.Substring(0, 8),
                binString.Substring(8, 8),
                binString.Substring(16, 8),
                binString.Substring(24, 8),
            ];

            string subnetDec = $" {Convert.ToInt32(subnetBinOctets[0], 2)}.{Convert.ToInt32(subnetBinOctets[1], 2)}.{Convert.ToInt32(subnetBinOctets[2], 2)}." +
                $"{Convert.ToInt32(subnetBinOctets[3], 2)}";
            return subnetDec;
        }

        public static SMBin GetSubnetMaskBin(string networkClass)
        {
            string networkPart = string.Empty;
            string hostPart = string.Empty;
            switch (networkClass)
            {
                case "A":
                    networkPart = new string('1', 8);
                    hostPart = new string('0', 24);
                    break;
                case "B":
                    networkPart = new string('1', 16);
                    hostPart = new string('0', 16);
                    break;
                case "C":
                    networkPart = new string('1', 24);
                    hostPart = new string('0', 8);
                    break;
            }
            SMBin smBin = new SMBin(networkPart, hostPart);
            return smBin;
        }

        public static NABin GetNetworkAdressBin(string networkClass, List<string> ipBinOctets)
        {
            string networkPart = string.Empty;
            string hostPart = string.Empty;
            switch (networkClass)
            {
                case "A":
                    networkPart = $"{ipBinOctets[0]}";
                    hostPart = new string('0', 24);
                    break;
                case "B":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}";
                    hostPart = new string('0', 16);
                    break;
                case "C":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}{ipBinOctets[2]}";
                    hostPart = new string('0', 8);
                    break;
            }
            NABin naBin = new NABin(networkPart, hostPart);
            return naBin;
        }

        public static BRABin GetBroadcastAdressBin(string networkClass, List<string> ipBinOctets)
        {
            string networkPart = string.Empty;
            string hostPart = string.Empty;
            switch (networkClass)
            {
                case "A":
                    networkPart = $"{ipBinOctets[0]}";
                    hostPart = new string('1', 24);
                    break;
                case "B":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}";
                    hostPart = new string('1', 16);
                    break;
                case "C":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}{ipBinOctets[2]}";
                    hostPart = new string('1', 8);
                    break;
            }
            BRABin braBin = new BRABin(networkPart, hostPart);
            return braBin;
        }

        public static string GetRange(string networkClass, List<string> ipDecOctets)
        {
            switch (networkClass)
            {
                case "A":
                    return $"{ipDecOctets[0]}.0.0.1 до {ipDecOctets[0]}.255.255.254";
                case "B":
                    return $"{ipDecOctets[0]}.{ipDecOctets[1]}.0.1 до {ipDecOctets[0]}.{ipDecOctets[1]}.255.254";
                case "C":
                    return $"{ipDecOctets[0]}.{ipDecOctets[1]}.{ipDecOctets[2]}.1 до {ipDecOctets[0]}.{ipDecOctets[1]}.{ipDecOctets[2]}.254";
                default:
                    return string.Empty;
            }
        }

        public static int GetSubnetPower(int subnetCount)
        {
            int power = 0;
            for (int n = 0; n < 25; n++)
            {
                if (subnetCount <= Math.Pow(2, n))
                {
                    power = n;
                    break;
                }
            }
            return power;
        }

        public static SMBin GetSubnetSMBin(string networkClass, int power)
        {
            string networkPart = string.Empty;
            string subNetworkPart = string.Empty;
            string hostPart = string.Empty;
            switch (networkClass)
            {
                case "A":
                    networkPart = new string('1', 8);
                    break;
                case "B":
                    networkPart = new string('1', 16);
                    break;
                case "C":
                    networkPart = new string('1', 24);
                    break;
                default:
                    break;
            }
            subNetworkPart = new string('1', power);

            hostPart = new string('0', 32 - (networkPart.Length + subNetworkPart.Length));

            SMBin sMBin = new SMBin(networkPart, subNetworkPart, hostPart);
            return sMBin;
        }

        public static NABin GetSubnetNABin(int currentSubnetNumber, string networkClass, List<string> ipBinOctets, int power)
        {
            string networkPart = string.Empty;
            string subNetworkPart = string.Empty;
            string hostPart = string.Empty;

            string subnetNumberBin = Convert.ToString(currentSubnetNumber, 2);
            switch (networkClass)
            {
                case "A":
                    networkPart = $"{ipBinOctets[0]}";
                    break;
                case "B":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}";
                    break;
                case "C":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}{ipBinOctets[2]}";
                    break;
                default:
                    break;
            }

            subNetworkPart += new string('0', power - subnetNumberBin.Length);
            subNetworkPart += subnetNumberBin;

            hostPart += new string('0', 32 - (networkPart.Length + subNetworkPart.Length));

            NABin naBin = new NABin(networkPart, subNetworkPart, hostPart);
            return naBin;
        }

        public static BRABin GetSubnetBRABin(int currentSubnetNumber, string networkClass, List<string> ipBinOctets, int power)
        {
            string networkPart = string.Empty;
            string subNetworkPart = string.Empty;
            string hostPart = string.Empty;

            string subnetNumberBin = Convert.ToString(currentSubnetNumber, 2);
            switch (networkClass)
            {
                case "A":
                    networkPart = $"{ipBinOctets[0]}";
                    break;
                case "B":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}";
                    break;
                case "C":
                    networkPart = $"{ipBinOctets[0]}{ipBinOctets[1]}{ipBinOctets[2]}";
                    break;
                default:
                    break;
            }

            subNetworkPart += new string('0', power - subnetNumberBin.Length);
            subNetworkPart += subnetNumberBin;

            hostPart += new string('1', 32 - (networkPart.Length + subNetworkPart.Length));

            BRABin braBin = new BRABin(networkPart, subNetworkPart, hostPart);
            return braBin;
        }

        public static string GetSubnetRange(string subnetNADec, string subnetBRADec)
        {
            List<string> subnetNADecOctets = subnetNADec.Split(".").ToList();
            List<string> subnetBRADecOctets = subnetBRADec.Split(".").ToList();
            string range = $"{subnetNADecOctets[0]}.{subnetNADecOctets[1]}.{subnetNADecOctets[2]}.{int.Parse(subnetNADecOctets[3]) + 1} до " +
                $"{subnetBRADecOctets[0]}.{subnetBRADecOctets[1]}.{subnetBRADecOctets[2]}.{int.Parse(subnetBRADecOctets[3]) - 1}";
            return range;
        }
    }
}

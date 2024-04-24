using IPCalculator.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace IPCalculator.Web.Services
{
    public class InternetProtocolService
    {
        public static string ErrorValidIP(string ipDec)
        {
            if(ipDec.Count(c => c == '.') != 3)
            {
                return "IP адресът трябва да съдържа 4 откета, разделени с точки!";
            }
            else
            {
                List<string> ipDecOctets = ipDec.Split(".").ToList();
                for(int a = 0; a < ipDecOctets.Count; a++)
                {
                    if (ipDecOctets[a] == string.Empty)
                    {
                        return "IP адресът трябва да съдържа 4 откета, разделени с точки!";
                    }
                    else if (!int.TryParse(ipDecOctets[a], out int result))
                    {
                        return "IP адресът трябва да съдържа 4 откета, разделени с точки!";
                    }
                    else if (ipDecOctets[a].ToString().StartsWith("0") && int.Parse(ipDecOctets[a]) != 0)
                    {
                        return "IP адресът не може да започва с 0!";
                    }                  
                    else if (int.Parse(ipDecOctets[a]) > 255 || int.Parse(ipDecOctets[a]) < 0)
                    {
                        return "IP октетите трябва да бъдат между 0 и 255!";
                    }
                    else if(IsForbiddenIP(ipDec))
                    {
                        return $"IP адресът {ipDec} е забранен за ползване!";
                    }
                }                   
            }
            return "true";
        }

        public static bool IsForbiddenIP(string ipDec)
        {
            string[] octets = ipDec.Split('.');
            int firstOctet = int.Parse(octets[0]);
            int secondOctet = int.Parse(octets[1]);
            int thirdOctet = int.Parse(octets[2]);
            int forthOctet = int.Parse(octets[3]);

            if ((firstOctet == 0) ||
                (firstOctet == 10) ||
                (firstOctet == 172 && secondOctet >= 16 && secondOctet <= 31) ||
                            (firstOctet == 192 && secondOctet == 168) ||
                            (firstOctet == 127) ||
                            (firstOctet == 169 && secondOctet == 254) ||
                            (firstOctet == 192 && secondOctet == 0 && thirdOctet == 2) ||
                            (firstOctet == 198 && secondOctet == 51 && thirdOctet == 100) ||
                            (firstOctet == 203 && secondOctet == 0 && thirdOctet == 113) ||
                            (firstOctet >= 224))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static IPInfos IPInfosCalculate(string ipDec)
        {
            List<string> ipDecOctets = ipDec.Split(".").ToList();
            List<string> ipBinOctets = InternetProtocolSubService.GetIPBin(ipDecOctets);

            string ipBin = string.Join(".", ipBinOctets);
            string networkClass = InternetProtocolSubService.GetNetworkClass(ipBin);        
            SMBin smBin = InternetProtocolSubService.GetSubnetMaskBin(networkClass);
            string smDec = InternetProtocolSubService.IPConvertBinToDec(smBin.NetworkPart + smBin.HostPart);    
            NABin naBin = InternetProtocolSubService.GetNetworkAdressBin(networkClass, ipBinOctets);
            string naDec = InternetProtocolSubService.IPConvertBinToDec(naBin.NetworkPart + naBin.HostPart);
            BRABin braBin = InternetProtocolSubService.GetBroadcastAdressBin(networkClass, ipBinOctets);
            string braDec = InternetProtocolSubService.IPConvertBinToDec(braBin.NetworkPart + braBin.HostPart);
            string range = InternetProtocolSubService.GetRange(networkClass, ipDecOctets);

            IPInfos ipInfos = new IPInfos(ipDec, ipBin, networkClass, smDec, smBin, naDec, naBin, braDec, braBin, range, ipBinOctets);
            return ipInfos;
        }

        public static IPInfos SMCalculate(IPInfos ipInfos)
        {
            ipInfos.Power = InternetProtocolSubService.GetSubnetPower(ipInfos.SubnetCount);
            ipInfos.SubnetSMBin = InternetProtocolSubService.GetSubnetSMBin(ipInfos.NetworkClass, ipInfos.Power);   
            ipInfos.SubnetSMDec = InternetProtocolSubService.IPConvertBinToDec(ipInfos.SubnetSMBin.NetworkPart + ipInfos.SubnetSMBin.SubNetworkPart + ipInfos.SubnetSMBin.HostPart);
            return ipInfos;
        }

        public static string ErrorSubnetCountMessage(int subnetCount, string networkClass)
        {
            if(subnetCount <= 0)
            {
                return "Не може да създадете 0 подмрежи или по-малко!";
            }
            else if (networkClass == "C" && subnetCount > 127)
            {
                return "Не може да създадете повече от 127 подмрежи с Клас C мрежа!";
            }
            else if (networkClass == "A" && subnetCount > 8388607)
            {
                return "Не може да създадете повече от 8 388 607 подмрежи с Клас C мрежа!";
            }
            else if (networkClass == "B" && subnetCount > 32767)
            {
                return "Не може да създадете повече от 32 767 подмрежи с Клас C мрежа!";
            }
            return "true";
        }

        public static string ErrorSubnetNumberMessage(int subnetCount, int subnetNumber)
        {
            if (subnetNumber <= 0)
            {
                return "Номерът на подмрежата не може да бъде по-малък или равен на 0";
            }
            else if (subnetNumber > subnetCount)
            {
                return $"Вие сте създали само {subnetCount} подмрежи!";
            }
            return "true";
        }

        public static IPInfos IPInfosSubnetCalculate(IPInfos ipInfos)
        {
            ipInfos.SubnetNABin = InternetProtocolSubService.GetSubnetNABin(ipInfos.CurrentSubnetNumber, ipInfos.NetworkClass, ipInfos.IPBinOctets, ipInfos.Power);
            ipInfos.SubnetNADec = InternetProtocolSubService.IPConvertBinToDec(ipInfos.SubnetNABin.NetworkPart + ipInfos.SubnetNABin.SubNetworkPart + ipInfos.SubnetNABin.HostPart);
            ipInfos.SubnetBRABin = InternetProtocolSubService.GetSubnetBRABin(ipInfos.CurrentSubnetNumber, ipInfos.NetworkClass, ipInfos.IPBinOctets, ipInfos.Power);
            ipInfos.SubnetBRADec = InternetProtocolSubService.IPConvertBinToDec(ipInfos.SubnetBRABin.NetworkPart + ipInfos.SubnetBRABin.SubNetworkPart + ipInfos.SubnetBRABin.HostPart);
            ipInfos.SubnetRange = InternetProtocolSubService.GetSubnetRange(ipInfos.SubnetNADec, ipInfos.SubnetBRADec);
            return (ipInfos);
        }
    }
}

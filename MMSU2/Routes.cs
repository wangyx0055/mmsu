using System;
using System.Runtime.InteropServices;
namespace MMSU
{
	internal class Routes
	{
		public struct MIB_IPFORWARDROW
		{
			public uint dwForwardDest;
			public uint dwForwardMask;
			public uint dwForwardPolicy;
			public uint dwForwardNextHop;
			public uint dwForwardIfIndex;
			public uint dwForwardType;
			public uint dwForwardProto;
			public uint dwForwardAge;
			public uint dwForwardNextHopAS;
			public int dwForwardMetric1;
			public int dwForwardMetric2;
			public int dwForwardMetric3;
			public int dwForwardMetric4;
			public int dwForwardMetric5;
		}
		public struct MIB_IPFORWARDTABLE
		{
			public int dwNumEntries;
			public Routes.MIB_IPFORWARDROW[] table;
		}
		[DllImport("Iphlpapi.dll")]
		[return: MarshalAs(UnmanagedType.U4)]
		public static extern int CreateIpForwardEntry(ref Routes.MIB_IPFORWARDROW pRoute);
		[DllImport("Iphlpapi.dll")]
		[return: MarshalAs(UnmanagedType.U4)]
		public static extern int DeleteIpForwardEntry(ref Routes.MIB_IPFORWARDROW pRoute);
		[DllImport("Iphlpapi.dll")]
		[return: MarshalAs(UnmanagedType.U4)]
		public static extern int SetIpForwardEntry(ref Routes.MIB_IPFORWARDROW pRoute);
		[DllImport("Iphlpapi.dll")]
		[return: MarshalAs(UnmanagedType.U4)]
		public static extern int GetIpForwardTable(byte[] pIpForwardTable, out int pdwSize, bool bOrder);
		public int createIpForwardEntry(uint destIPAddress, uint destMask, uint nextHopIPAddress, uint ifIndex, int metric)
		{
			Routes.MIB_IPFORWARDROW mIB_IPFORWARDROW = default(Routes.MIB_IPFORWARDROW);
			mIB_IPFORWARDROW.dwForwardDest = destIPAddress;
			mIB_IPFORWARDROW.dwForwardMask = destMask;
			mIB_IPFORWARDROW.dwForwardPolicy = Convert.ToUInt32(0);
			mIB_IPFORWARDROW.dwForwardNextHop = nextHopIPAddress;
			mIB_IPFORWARDROW.dwForwardIfIndex = ifIndex;
			mIB_IPFORWARDROW.dwForwardType = Convert.ToUInt32(4);
			mIB_IPFORWARDROW.dwForwardProto = Convert.ToUInt32(3);
			mIB_IPFORWARDROW.dwForwardAge = Convert.ToUInt32(0);
			mIB_IPFORWARDROW.dwForwardNextHopAS = Convert.ToUInt32(0);
			mIB_IPFORWARDROW.dwForwardMetric1 = metric;
			mIB_IPFORWARDROW.dwForwardMetric2 = -1;
			mIB_IPFORWARDROW.dwForwardMetric3 = -1;
			mIB_IPFORWARDROW.dwForwardMetric4 = -1;
			mIB_IPFORWARDROW.dwForwardMetric5 = -1;
			return Routes.CreateIpForwardEntry(ref mIB_IPFORWARDROW);
		}
		public int deleteIpForwardEntry(uint destIPAddress, uint destMask, uint nextHopIPAddress, uint ifIndex)
		{
			Routes.MIB_IPFORWARDROW mIB_IPFORWARDROW = default(Routes.MIB_IPFORWARDROW);
			mIB_IPFORWARDROW.dwForwardDest = destIPAddress;
			mIB_IPFORWARDROW.dwForwardMask = destMask;
			mIB_IPFORWARDROW.dwForwardNextHop = nextHopIPAddress;
			mIB_IPFORWARDROW.dwForwardIfIndex = ifIndex;
			mIB_IPFORWARDROW.dwForwardPolicy = Convert.ToUInt32(0);
			mIB_IPFORWARDROW.dwForwardType = Convert.ToUInt32(4);
			mIB_IPFORWARDROW.dwForwardProto = Convert.ToUInt32(3);
			mIB_IPFORWARDROW.dwForwardAge = Convert.ToUInt32(0);
			mIB_IPFORWARDROW.dwForwardNextHopAS = Convert.ToUInt32(0);
			mIB_IPFORWARDROW.dwForwardMetric1 = -1;
			mIB_IPFORWARDROW.dwForwardMetric2 = -1;
			mIB_IPFORWARDROW.dwForwardMetric3 = -1;
			mIB_IPFORWARDROW.dwForwardMetric4 = -1;
			mIB_IPFORWARDROW.dwForwardMetric5 = -1;
			return Routes.DeleteIpForwardEntry(ref mIB_IPFORWARDROW);
		}
		public static uint IPToInt(string ipAddress)
		{
			string text = ".,:";
			char[] separator = text.ToCharArray();
			string[] array = null;
			for (int i = 1; i <= 5; i++)
			{
				array = ipAddress.Split(separator, i);
			}
			string s = array[0].ToString();
			string s2 = array[1].ToString();
			string s3 = array[2].ToString();
			string s4 = array[3].ToString();
			uint num = uint.Parse(s);
			uint num2 = uint.Parse(s2);
			uint num3 = uint.Parse(s3);
			uint num4 = uint.Parse(s4);
			uint num5 = num4 << 24;
			num5 += num3 << 16;
			num5 += num2 << 8;
			return num5 + num;
		}
		public static uint IPToInt2(string ipAddress)
		{
			string text = ".,:";
			char[] separator = text.ToCharArray();
			string[] array = null;
			for (int i = 1; i <= 5; i++)
			{
				array = ipAddress.Split(separator, i);
			}
			string s = array[0].ToString();
			return uint.Parse(s);
		}
		public static int countMetric(int x, int y)
		{
			return 20 + x * 5 + y * 5;
		}
	}
}

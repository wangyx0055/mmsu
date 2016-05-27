using System;
using System.Collections;
using System.Runtime.InteropServices;
namespace MMSU
{
	internal class libGetAdaptersInfo
	{
		private struct IP_ADDRESS_STRING
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string Address;
		}
		private struct IP_ADDR_STRING
		{
			public IntPtr Next;
			public libGetAdaptersInfo.IP_ADDRESS_STRING IpAddress;
			public libGetAdaptersInfo.IP_ADDRESS_STRING IpMask;
			public int Context;
		}
		private struct IP_ADAPTER_INFO
		{
			public IntPtr Next;
			public int ComboIndex;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string AdapterName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
			public string AdapterDescription;
			public uint AddressLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] Address;
			public int Index;
			public uint Type;
			public uint DhcpEnabled;
			public IntPtr CurrentIpAddress;
			public libGetAdaptersInfo.IP_ADDR_STRING IpAddressList;
			public libGetAdaptersInfo.IP_ADDR_STRING GatewayList;
			public libGetAdaptersInfo.IP_ADDR_STRING DhcpServer;
			public bool HaveWins;
			public libGetAdaptersInfo.IP_ADDR_STRING PrimaryWinsServer;
			public libGetAdaptersInfo.IP_ADDR_STRING SecondaryWinsServer;
			public int LeaseObtained;
			public int LeaseExpires;
		}
		private const int MAX_ADAPTER_DESCRIPTION_LENGTH = 128;
		private const int ERROR_BUFFER_OVERFLOW = 111;
		private const int MAX_ADAPTER_NAME_LENGTH = 256;
		private const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
		private const int MIB_IF_TYPE_OTHER = 1;
		private const int MIB_IF_TYPE_ETHERNET = 6;
		private const int MIB_IF_TYPE_TOKENRING = 9;
		private const int MIB_IF_TYPE_FDDI = 15;
		private const int MIB_IF_TYPE_PPP = 23;
		private const int MIB_IF_TYPE_LOOPBACK = 24;
		private const int MIB_IF_TYPE_SLIP = 28;
		[DllImport("iphlpapi.dll")]
		private static extern int GetAdaptersInfo(IntPtr pAdapterInfo, ref long pBufOutLen);
		public static ArrayList GetAdapters()
		{
			ArrayList arrayList = new ArrayList();
			long value = (long)Marshal.SizeOf(typeof(libGetAdaptersInfo.IP_ADAPTER_INFO));
			IntPtr intPtr = Marshal.AllocHGlobal((int)new IntPtr(value));
			int adaptersInfo = libGetAdaptersInfo.GetAdaptersInfo(intPtr, ref value);
			if (adaptersInfo == 111)
			{
				intPtr = Marshal.ReAllocHGlobal(intPtr, new IntPtr(value));
				adaptersInfo = libGetAdaptersInfo.GetAdaptersInfo(intPtr, ref value);
			}
			if (adaptersInfo == 0)
			{
				IntPtr intPtr2 = intPtr;
				while (true)
				{
					Macs macs = new Macs();
					libGetAdaptersInfo.IP_ADAPTER_INFO iP_ADAPTER_INFO = (libGetAdaptersInfo.IP_ADAPTER_INFO)Marshal.PtrToStructure(intPtr2, typeof(libGetAdaptersInfo.IP_ADAPTER_INFO));
					macs.InterfaceIndex = iP_ADAPTER_INFO.Index.ToString();
					string text = string.Empty;
					uint type = iP_ADAPTER_INFO.Type;
					if (type <= 9u)
					{
						if (type != 6u)
						{
							if (type != 9u)
							{
							}
						}
					}
					else
					{
						if (type != 15u)
						{
							switch (type)
							{
							case 23u:
								break;
							case 24u:
								break;
							default:
								if (type != 28u)
								{
								}
								break;
							}
						}
					}
					IL_107:
					macs.Description = iP_ADAPTER_INFO.AdapterDescription;
					macs.IPAddress = iP_ADAPTER_INFO.IpAddressList.IpAddress.Address;
					macs.DefaultIPGateway = iP_ADAPTER_INFO.GatewayList.IpAddress.Address;
					text = string.Empty;
					int num = 0;
					while ((long)num < (long)((ulong)(iP_ADAPTER_INFO.AddressLength - 1u)))
					{
						text += string.Format("{0:X2}-", iP_ADAPTER_INFO.Address[num]);
						num++;
					}
					text = string.Format("{0}{1:X2}", text, iP_ADAPTER_INFO.Address[(int)((UIntPtr)(iP_ADAPTER_INFO.AddressLength - 1u))]);
					macs.MACAddress = text;
					arrayList.Add(macs);
					intPtr2 = iP_ADAPTER_INFO.Next;
					if (!(intPtr2 != IntPtr.Zero))
					{
						break;
					}
					continue;
					goto IL_107;
				}
				Marshal.FreeHGlobal(intPtr);
			}
			else
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return arrayList;
		}
	}
}

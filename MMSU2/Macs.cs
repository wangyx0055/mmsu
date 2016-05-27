using System;
namespace MMSU
{
	public class Macs
	{
		public string NetConnectionID;
		public string Description;
		public string IPAddress;
		public string MACAddress;
		public string DefaultIPGateway;
		public string InterfaceIndex;
		public Macs(string NetConnectionID, string Description, string MACAddress, string InterfaceIndex)
		{
			this.NetConnectionID = NetConnectionID;
			this.Description = Description;
			this.MACAddress = MACAddress;
			this.InterfaceIndex = InterfaceIndex;
		}
		public Macs()
		{
		}
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.Description,
				"\t",
				this.IPAddress,
				"\t",
				this.MACAddress
			});
		}
	}
}

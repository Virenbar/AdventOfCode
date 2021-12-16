using AdventOfCode2021.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day16 : BaseDay
	{
		#region Overrides

		public Day16() : base(16) { }

		protected override string SolvePartOne()
		{
			var Packet = DecodePacket(Raw);
			var R = SumVersion(Packet);
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var Packet = DecodePacket(Raw);
			var R = Evaluate(Packet);
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var Packet = DecodePacket("C0015000016115A2E0802F182340");
			var R = SumVersion(Packet);
			return R == 23;
		}

		protected override bool TestPartTwo()
		{
			var Packet = DecodePacket("9C0141080250320F1802104A08");
			var R = Evaluate(Packet);
			return R == 1;
		}

		#endregion Overrides

		private static (BITSPacket Packet, string Bits) Decode(string bits)
		{
			var Bits = bits;
			var Version = Convert.ToInt32(Bits[..3], 2);
			Bits = Bits[3..];
			var Type = Convert.ToInt32(Bits[..3], 2);
			Bits = Bits[3..];
			//Literal
			if (Type == 4)
			{
				var Number = "";
				while (true)
				{
					var Group = Bits[..5];
					Bits = Bits[5..];
					Number += Group[1..];
					if (Group[0] == '0') { break; }
				}
				return (new BITSPacket(Version, Type, Convert.ToInt64(Number, 2)), Bits);
			}
			//SubPackets
			var Packets = new List<BITSPacket>();
			var LengthType = Bits[0];
			Bits = Bits[1..];
			if (LengthType == '0')
			{
				var L = Convert.ToInt32(Bits[..15], 2);
				Bits = Bits[15..];
				var SubBits = Bits[..L];
				Bits = Bits[L..];
				while (SubBits.Length > 0)
				{
					var (Packet, B) = Decode(SubBits);
					SubBits = B;
					Packets.Add(Packet);
				}
			}
			else
			{
				var L = Convert.ToInt32(Bits[..11], 2);
				Bits = Bits[11..];
				for (int i = 0; i < L; i++)
				{
					var (Packet, B) = Decode(Bits);
					Bits = B;
					Packets.Add(Packet);
				}
			}
			return (new BITSPacket(Version, Type, Packets), Bits);
		}

		private static BITSPacket DecodePacket(string packet) => Decode(HexDecoder.Decode(packet)).Packet;

		private static long Evaluate(BITSPacket packet)
		{
			var T = packet.Type;
			if (T == 4) { return packet.Number; }

			var Sub = packet.Packets;
			var Values = Sub.Select(P => Evaluate(P)).ToList();
			switch (T)
			{
				case 0: return Values.Sum();
				case 1:
					{
						long Value = 1;
						foreach (var V in Values) { Value *= V; }
						return Value;
					}

				case 2: return Values.Min();
				case 3: return Values.Max();
				case 5: return Values[0] > Values[1] ? 1 : 0;
				case 6: return Values[0] < Values[1] ? 1 : 0;
				case 7: return Values[0] == Values[1] ? 1 : 0;
				default: return 0;
			}
		}

		private static int SumVersion(BITSPacket packet)
		{
			var Sum = 0;
			var Packets = new Stack<BITSPacket>();
			Packets.Push(packet);
			while (Packets.Count > 0)
			{
				var P = Packets.Pop();
				Sum += P.Version;
				if (P.Type == 4) { continue; }
				P.Packets.ForEach(P => Packets.Push(P));
			}
			return Sum;
		}

		private class BITSPacket
		{
			public readonly List<BITSPacket> Packets = new();

			public BITSPacket(int version, int type, long number) : this(version, type) => Number = number;

			public BITSPacket(int version, int type, List<BITSPacket> packets) : this(version, type) => Packets = packets;

			private BITSPacket(int version, int type)
			{
				Version = version;
				Type = type;
			}

			public long Number { get; private set; }
			public int Type { get; private set; }
			public int Version { get; private set; }
		}
	}
}
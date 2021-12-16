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
				return (new BITSPacket(Version, Type, Convert.ToInt64(Number, 2), new List<BITSPacket>()), Bits);
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
			return (new BITSPacket(Version, Type, 0L, Packets), Bits);
		}

		private static BITSPacket DecodePacket(string packet) => Decode(HexDecoder.Decode(packet)).Packet;

		private static long Evaluate(BITSPacket packet)
		{
			var Values = packet.Packets.Select(P => Evaluate(P)).ToList();
			return packet.Type switch
			{
				0 => Values.Sum(),
				1 => Values.Aggregate(1L, (a, x) => a * x),
				2 => Values.Min(),
				3 => Values.Max(),
				4 => packet.Number,
				5 => Values[0] > Values[1] ? 1 : 0,
				6 => Values[0] < Values[1] ? 1 : 0,
				7 => Values[0] == Values[1] ? 1 : 0,
				_ => 0,
			};
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

		private record BITSPacket(int Version, int Type, long Number, List<BITSPacket> Packets);
	}
}
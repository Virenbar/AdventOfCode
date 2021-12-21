using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.Days
{
	public class Day20 : BaseDay
	{
		#region Overrides

		public Day20() : base(20) { }

		protected override string SolvePartOne()
		{
			var IE = new ImageEnhancer(Raw);
			IE.EnhanceImage(2);
			var R = IE.LitPixels;
			return R.ToString();
		}

		protected override string SolvePartTwo()
		{
			var IE = new ImageEnhancer(Raw);
			IE.EnhanceImage(50);
			var R = IE.LitPixels;
			return R.ToString();
		}

		protected override bool TestPartOne()
		{
			var IE = new ImageEnhancer(RawTest);
			IE.EnhanceImage(2);
			var R = IE.LitPixels;
			return R == 35;
		}

		protected override bool TestPartTwo()
		{
			var IE = new ImageEnhancer(RawTest);
			IE.EnhanceImage(50);
			var R = IE.LitPixels;
			return R == 3351;
		}

		#endregion Overrides

		private class ImageEnhancer
		{
			private readonly List<char> Algorithm;
			private char Default = '.';
			private int MinX, MinY, MaxX, MaxY;
			private Dictionary<Pixel, char> Pixels = new();

			public ImageEnhancer(string data)
			{
				var D = data.Split("\r\n\r\n");
				Algorithm = D[0].ToList();
				var Image = D[1].SplitToList();
				MaxX = Image[0].Length - 1;
				MaxY = Image.Count - 1;
				for (int Y = 0; Y < Image.Count; Y++)
				{
					for (int X = 0; X < Image[Y].Length; X++)
					{
						Pixels.Add(new Pixel(X, Y), Image[Y][X]);
					}
				}
			}

			public int LitPixels => Pixels.Count(P => P.Value == '#');

			private char this[Pixel P] => Pixels.ContainsKey(P) ? Pixels[P] : Default;

			public void EnhanceImage(int count)
			{
				for (int i = 0; i < count; i++) { Enhance(); }
			}

			private void Enhance()
			{
				MinX--; MinY--; MaxX++; MaxY++;
				Dictionary<Pixel, char> PixelsNew = new();
				for (int Y = MinY; Y <= MaxY; Y++)
				{
					for (int X = MinX; X <= MaxX; X++)
					{
						var P = new Pixel(X, Y);
						PixelsNew.Add(P, EnhancePixel(P));
					}
				}
				Pixels = PixelsNew;
				TryInvert();
			}

			private char EnhancePixel(Pixel P)
			{
				string Index = "";
				for (int Y = -1; Y <= 1; Y++)
				{
					for (int X = -1; X <= 1; X++)
					{
						Index += this[new Pixel(P.X + X, P.Y + Y)];
					}
				}
				var T = string.Concat(Index.Select(C => C == '#' ? '1' : '0'));
				var I = Convert.ToInt32(T, 2);
				return Algorithm[I];
			}

			private void TryInvert()
			{
				if (!(Algorithm[0] == '#' && Algorithm[511] == '.')) { return; }
				Default = Default == '.' ? '#' : '.';
			}
		}
		private record Pixel(int X, int Y);
	}
}
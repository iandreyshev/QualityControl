using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace TriangleType
{
	class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				TriangleSides sides = new TriangleSides(0, 0, 0);

				ConvertArgs(args, out sides);
				TriangleType type = CalculateTriangleType(sides);
				Print(type);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private class TriangleSides
		{
			public TriangleSides(float a, float b, float c)
			{
				this.a = a;
				this.b = b;
				this.c = c;
			}

			public float a;
			public float b;
			public float c;
		}
		private enum TriangleType
		{
			NONE,
			COMMON,
			ISOSCELES,
			EQUILATERAL,
		}
		private const int ARGUMENTS_COUNT = 3;
		private const string FLOAT_SEPARATOR = ".";

		private static void ConvertArgs(string[] args, out TriangleSides sides)
		{
			if (args.Length < ARGUMENTS_COUNT)
			{
				throw new Exception("Укажите длины сторон в качестве параметров. Формат ввода: triangle.exe a b c");
			}

			try
			{
				CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
				ci.NumberFormat.CurrencyDecimalSeparator = FLOAT_SEPARATOR;

				float a = float.Parse(args[0], NumberStyles.Any, ci);
				float b = float.Parse(args[1], NumberStyles.Any, ci);
				float c = float.Parse(args[2], NumberStyles.Any, ci);

				if (a < 0 || b < 0 || c < 0)
				{
					throw new Exception("Длины сторон могут быть только положительными.");
				}

				sides = new TriangleSides(a, b, c);
			}
			catch (FormatException)
			{
				throw new Exception("Укажите длины сторон в формате десятичного числа.");
			}
			catch (OverflowException)
			{
				throw new Exception("Размер стороны слишком большой. Максимальное значение длинны: " + float.MaxValue);
			}
			catch (ArgumentNullException)
			{
				throw new Exception("Укажите длины сторон в качестве параметров. Формат ввода: triangle.exe a b c");
			}
		}
		private static TriangleType CalculateTriangleType(TriangleSides sides)
		{
			if (sides.b + sides.c < sides.a ||
				sides.a + sides.c < sides.b ||
				sides.a + sides.b < sides.c)
			{
				return TriangleType.NONE;
			}

			if (sides.a == sides.b && sides.b == sides.c)
			{
				return TriangleType.EQUILATERAL;
			}

			if (sides.a == sides.b ||
				sides.a == sides.c ||
				sides.b == sides.c)
			{
				return TriangleType.ISOSCELES;
			}

			return TriangleType.COMMON;
		}
		private static void Print(TriangleType type)
		{
			switch (type)
			{
				case TriangleType.NONE:
					Console.WriteLine("Не треугольник");
					break;

				case TriangleType.COMMON:
					Console.WriteLine("Обычный");
					break;

				case TriangleType.ISOSCELES:
					Console.WriteLine("Равнобедренный");
					break;

				case TriangleType.EQUILATERAL:
					Console.WriteLine("Равносторонний");
					break;
			}
		}
	}
}

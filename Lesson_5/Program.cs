using System;
using System.Collections.Generic;
using System.IO;

namespace Lesson_5
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Введите числа 0-255, разделяя пробелом:");
			string numbers = Console.ReadLine();

			var list = new List<byte>();
			foreach (var x in numbers.Split(' '))
				if (byte.TryParse(x, out byte tmp_val))
					list.Add(tmp_val);

			File.WriteAllBytes("binary.bin", list.ToArray());
			Console.WriteLine($"в файл binary.bin было записано {list.Count} байт");
			Console.WriteLine(string.Join(", ", list));
			Console.ReadLine();
		}
	}
}

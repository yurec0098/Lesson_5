using System;
using System.IO;
using System.Text;

namespace Lesson_5
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Веедите текст для сохранения в файл text_1.txt");

			string line = Console.ReadLine();
			File.WriteAllText("text_1.txt", line, Encoding.UTF8);

			Console.WriteLine($"В файл text_1.txt записан текст '{line}'");
			Console.ReadLine();
		}
	}
}

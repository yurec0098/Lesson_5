using System;
using System.IO;

namespace Lesson_5
{
	class Program
	{
		static void Main(string[] args)
		{
			File.AppendAllText("startup.txt", $"{DateTime.Now:T}\n");
			Console.WriteLine("При запуске в файл startup.txt было записано текущее время");

			Console.ReadLine();
		}
	}
}

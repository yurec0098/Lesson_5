using System;
using System.Diagnostics;
using System.IO;

namespace Lesson_5
{
	class Program
	{
		static void Main(string[] args)
		{
			var path = ReadDirectoryPath("Введите путь к существующей директории:");

			File.WriteAllLines("FoldersFiles.txt", Directory.GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly));

			using (var file = File.Create("FoldersFilesRecursive.txt"))
			using (var sw = new StreamWriter(file))
				WriteFilesRecursive(sw, path);

			using (var file = File.Create("FoldersFilesRecursive2.txt"))
			using (var sw = new StreamWriter(file))
				WriteFilesRecursive(sw, new DirectoryInfo(path));

			Console.ReadLine();
		}

		static void WriteFilesRecursive(StreamWriter sw, string directory, string searchPattern = "*")
		{
			try
			{
				sw.WriteLine(directory);				//	запись папки
				foreach (var file in Directory.GetFiles(directory, searchPattern))
					sw.WriteLine($"{file}");		//	запись файла
				sw.Flush();		//	оставлю здесь, запись в файл тоже может радовать исключениями

				foreach (var dir in Directory.GetDirectories(directory))
					WriteFilesRecursive(sw, dir, searchPattern);
			}
			catch(UnauthorizedAccessException ua_ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error: {ua_ex.Message}");
				Console.ForegroundColor = ConsoleColor.White;
			}
			catch(Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error: {ex.Message}");
				Console.ForegroundColor = ConsoleColor.White;
			}
		}
		static void WriteFilesRecursive(StreamWriter sw, DirectoryInfo directory, string searchPattern = "*")
		{
			try
			{
				sw.WriteLine(directory);				//	запись папки
				foreach (var file in directory.GetFiles(searchPattern))
					sw.WriteLine($"{file}");		//	запись файла
				sw.Flush();     //	оставлю здесь, запись в файл тоже может радовать исключениями

				foreach (var dir in directory.GetDirectories())
				{
					WriteFilesRecursive(sw, dir, searchPattern);
				}
			}
			catch(UnauthorizedAccessException ua_ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error: {ua_ex.Message}");
				Console.ForegroundColor = ConsoleColor.White;
			}
			catch(Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Error: {ex.Message}");
				Console.ForegroundColor = ConsoleColor.White;
			}
		}

		static string ReadDirectoryPath(string text)
		{
			var pos = Console.GetCursorPosition();
			Console.WriteLine(text);
			string path = Console.ReadLine();
			while (!Directory.Exists(path))
			{
				ClearConsoleLines(pos.Left, pos.Top, 2);
				Console.WriteLine($"Повторим... {text}");
				path = Console.ReadLine();
			}

			return path;
		}
		static void ClearConsoleLines(int left, int top, int count)
		{
			Console.SetCursorPosition(left, top);

			for (int i = 0; i < count; i++)
				Console.WriteLine(new string(' ', Console.WindowWidth));

			Console.SetCursorPosition(left, top);
		}
	}
}

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Xml;
using System.Xml.Serialization;

namespace Lesson_5
{
	class Program
	{
		static ObservableCollection<ToDo> ToDo_list { get; set; } = new ObservableCollection<ToDo>();

		static void Main(string[] args)
		{
			ToDo_list = DeserializeJS();
			//ToDo_list = DeserializeXML();
			//ToDo_list = DeserializeBin();

			bool isExit = false;
			while (!isExit)
			{
				Console.Clear();
				Console.WriteLine("\tСписок задач");
				Console.WriteLine();

				if (ToDo_list.Count > 0)
					for (int i = 0; i < ToDo_list.Count; i++)
						Console.WriteLine("{2,2} {0,3} {1}", ToDo_list[i].IsDone ? "[x]" : "", ToDo_list[i].Title, $"{i + 1:00}");
				else
					Console.WriteLine("Ещё нет сохраненных задач");
				Console.WriteLine();

				WriteMenu();
				string line = Console.ReadLine();
				switch (line)
				{
					case "0":
						isExit = true;
						break;

					case "1":
						AddTask();
						break;
					case "2":
						MarkTask();
						break;
					case "3":
						DelTask();
						break;
				}
			}

			SerializeJS(ToDo_list);
			//SerializeXML(ToDo_list);
			//SerializeBin(ToDo_list);

			Console.WriteLine("Список задач был записан в файл");
			Console.ReadLine();
		}

		private static void WriteMenu()
		{
			Console.WriteLine("\tМеню");
			Console.WriteLine("1. Добавить задачу");

			if (ToDo_list.Count > 0)
			{
				Console.WriteLine("2. Отметить задачу как выполненную");
				Console.WriteLine("3. Удалить задачу");
			}

			Console.WriteLine();
			Console.WriteLine("0. Сохранить и выйти");		//	Вспомним Diablo II
		}
		private static void AddTask()
		{
			Console.WriteLine("Введите текст задачи:");
			ToDo_list.Add(new ToDo(Console.ReadLine()));
		}
		private static void MarkTask()
		{
			if(ToDo_list.Count == 0)
			{
				Console.WriteLine("Сначала нужно добавить хотябы одну задачу");
				Console.ReadLine();
				return;
			}
			var index = ReadInt($"Введите порядковый номер выполненной задачи (от 1 до {ToDo_list.Count}):", 1, ToDo_list.Count);
			ToDo_list[index - 1].IsDone = true;
		}
		private static void DelTask()
		{
			if (ToDo_list.Count == 0)
			{
				Console.WriteLine("Сначала нужно добавить хотябы одну задачу");
				Console.ReadLine();
				return;
			}
			var index = ReadInt($"Введите порядковый номер задачи (от 1 до {ToDo_list.Count}):", 1, ToDo_list.Count);
			ToDo_list.RemoveAt(index - 1);
		}

		static int ReadInt(string text, int min, int max)
		{
			int value;
			var pos = Console.GetCursorPosition();
			Console.WriteLine(text);
			while (!int.TryParse(Console.ReadLine(), out value) || !(value >= min && value <= max))
			{
				ClearConsoleLines(pos.Left, pos.Top, 2);
				Console.WriteLine($"Повторим... {text}");
			}

			return value;
		}
		static void ClearConsoleLines(int left, int top, int count)
		{
			Console.SetCursorPosition(left, top);

			for (int i = 0; i < count; i++)
				Console.WriteLine(new string(' ', Console.WindowWidth));

			Console.SetCursorPosition(left, top);
		}

		#region Serialize / Deserialize
		#region JS
		static string fileNameJS = "tasks.json";
		private static void SerializeJS(ObservableCollection<ToDo> obj)
		{
			var js_wr_opt = new JsonWriterOptions()
			{
				//	Мы любим когда в текстовых файлам мы можем прочитать, что там написано
				//	Поэтому добавим привычные нам символы
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.Cyrillic, UnicodeRanges.BasicLatin, UnicodeRanges.LetterlikeSymbols),
				Indented = true
			};

			using (var stream = File.Create(fileNameJS))
				JsonSerializer.Serialize(new Utf8JsonWriter(stream, js_wr_opt), obj);
		}
		private static ObservableCollection<ToDo> DeserializeJS()
		{
			if (File.Exists(fileNameJS))
				return JsonSerializer.Deserialize<ObservableCollection<ToDo>>(File.ReadAllText(fileNameJS));
			else
				return new ObservableCollection<ToDo>();
		}
		#endregion

		#region XML
		static string fileNameXML = "tasks.xml";
		private static void SerializeXML(ObservableCollection<ToDo> obj)
		{
			var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
			using (var fs = File.Create(fileNameXML))
				new XmlSerializer(typeof(ObservableCollection<ToDo>)).Serialize(fs, obj, namespaces);
		}
		private static ObservableCollection<ToDo> DeserializeXML()
		{
			if (File.Exists(fileNameXML))
				using (var fs = File.OpenRead(fileNameXML))
					return (ObservableCollection<ToDo>)new XmlSerializer(typeof(ObservableCollection<ToDo>)).Deserialize(fs);
			else
				return new ObservableCollection<ToDo>();
		}
		#endregion

		#region Bin
		#pragma warning disable SYSLIB0011 // Тип или член устарел
		static string fileNameBin = "tasks.bin";
		private static void SerializeBin(ObservableCollection<ToDo> obj)
		{
			using (var fs = File.Create(fileNameBin))
				new BinaryFormatter().Serialize(fs, obj);
		}
		private static ObservableCollection<ToDo> DeserializeBin()
		{
			if (File.Exists(fileNameBin))
				using (var fs = File.OpenRead(fileNameBin))
					return (ObservableCollection<ToDo>)new BinaryFormatter().Deserialize(fs);
			else
				return new ObservableCollection<ToDo>();
		}
		#pragma warning restore SYSLIB0011 // Тип или член устарел
		#endregion
		#endregion
	}
}

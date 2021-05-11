using System;

namespace Lesson_5
{
	[Serializable]
	public class ToDo
	{
		public override string ToString() => $"{(IsDone ? "[x] " : "")}{Title}";

		public string Title { get; set; }
		public bool IsDone { get; set; }

		public ToDo() { }
		public ToDo(string title)
		{
			Title = title;
		}
	}
}

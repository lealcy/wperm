using System;
using System.Collections.Generic;
using System.IO;

namespace wperm
{
	class MainClass
	{
		public static Dictionary<UInt64, string> Words;
		public static void Main(string[] args)
		{
			Console.WriteLine("Loading words from 'words.txt' file...");
			var file = new StreamReader(@"words.txt");
			Words = new Dictionary<UInt64, string>();
			string line;
			string value;
			UInt64 hash;
			while ((line = file.ReadLine()) != null)
			{
				hash = GetHash(line);
				if (Words.ContainsKey(hash))
				{
					value = Words[hash];
					Console.WriteLine("'{0}' has the same hash as '{1}'. Word ignored.", line, value);
					continue;
				}
				Words.Add(hash, line);
			}
			Console.WriteLine("All words loaded.");
			Console.WriteLine("Type /exit to exit");
			while (true)
			{
				Console.Write("word: ");
				line = Console.ReadLine();
				if (line == "/exit") break;
				List<string> perms = GetPermutations(line);
				var foundWords = new List<string>();
				for (int i = 3; i <= line.Length; i++)
				{
					foreach (string perm in perms)
					{
						string source = perm.Substring(0, i);
						if (foundWords.Contains(source)) {
							continue;
						}
						hash = GetHash(source);
						if (Words.ContainsKey(hash))
						{
							foundWords.Add(source);
						}
					}
				}
				foreach (string s in foundWords)
				{
					Console.WriteLine(s);
				}
			}
		}

		public static UInt64 GetHash(string s)
		{
			UInt64 hashedValue = 3074457345618258791ul;
			for (int i = 0; i < s.Length; i++)
			{
				hashedValue += s[i];
				hashedValue *= 3074457345618258799ul;
			}
			return hashedValue;
		}

		public static List<string> GetPermutations(string s)
		{
			var output = new List<string>();
			if (s.Length == 1)
			{
				output.Add(s);
			}
			else
			{
				foreach (var c in s)
				{
					// Remove one occurrence of the char (not all)
					var tail = s.Remove(s.IndexOf(c), 1);
					foreach (var tailPerms in GetPermutations(tail))
					{
						output.Add(c + tailPerms);
					}
				}
			}
			return output;			
		}
	}
}

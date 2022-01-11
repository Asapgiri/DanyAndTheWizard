// <copyright file="Save.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.InternalClass
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using DanyAndTheWizard.Repository.Interfaces;

	/// <summary>
	/// Save methods.
	/// </summary>
	public class Save : ISave
	{
		private string saveLocation;

		/// <summary>
		/// Initializes a new instance of the <see cref="Save"/> class.
		/// </summary>
		/// <param name="saveLoc">The location of the save file.</param>
		public Save(string saveLoc)
		{
			// string s = saveLoc.Split('.').LastOrDefault();
			// saveLoc = saveLoc.Remove(saveLoc.LastIndexOf('.'));
			// s = saveLoc.Split('.').LastOrDefault() + "." + s;
			// saveLoc = saveLoc.Remove(saveLoc.LastIndexOf('.'));
			this.saveLocation = "Model/" + saveLoc;
		}

		/// <inheritdoc/>
		public Dictionary<string, string> GetSave(string saveName)
		{
			StreamReader sr = new StreamReader(this.saveLocation);

			Dictionary<string, string> ret = new Dictionary<string, string>();
			string s = sr.ReadToEnd().Split('}').ToList().Where(x => x.Split('{').ToArray()[0] == saveName)
				.FirstOrDefault().ToString();
			s = s.Remove(0, s.IndexOf('\n') + 1);
			ret.Add("Name", s.Split('{')[0]);
			s = s.Split('{')[1];
			List<string> ls = s.Split('\n').ToList();
			ls.Remove(string.Empty);
			foreach (string save in ls)
			{
				List<string> a = save.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(" ", string.Empty)
					.Split('|').ToList();
				ret.Add(a[0], a[1]);
			}

			sr.Close();
			return ret;
		}

		/// <inheritdoc/>
		public List<string> GetSaveNames()
		{
			StreamReader sr = new StreamReader(this.saveLocation);

			List<string> ls = sr.ReadToEnd().Split('}').ToList();
			ls.Remove(string.Empty);

			// ls.remove("\n\r\n")
			for (int i = 0; i < ls.Count; i++)
			{
				ls[i] = ls[i].Remove(ls[i].IndexOf('{'));
			}

			sr.Close();
			return ls;
		}

		/// <inheritdoc/>
		public void NewSave(string saveName, Dictionary<string, string> saves)
		{
			StreamWriter sw = new StreamWriter(this.saveLocation, true);
			sw.Write('\n' + saveName + '{');
			foreach (KeyValuePair<string, string> s in saves)
			{
				sw.Write($"\n{s.Key}|{s.Value}");
			}

			sw.Write("}");
			sw.Close();
		}
	}
}

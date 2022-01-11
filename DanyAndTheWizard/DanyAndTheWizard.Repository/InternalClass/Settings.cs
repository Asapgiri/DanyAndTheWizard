// <copyright file="Settings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.InternalClass
{
	using System.Collections.Generic;
	using System.IO;
	using DanyAndTheWizard.Repository.Interfaces;

	/// <summary>
	/// Settings of the game.
	/// </summary>
	public class Settings : ISettings
	{
		private string settingsLocation;

		/// <summary>
		/// Initializes a new instance of the <see cref="Settings"/> class.
		/// </summary>
		/// <param name="settingsLoc">Location of the settings file.</param>
		public Settings(string settingsLoc)
		{
			this.settingsLocation = settingsLoc;
		}

		/// <inheritdoc/>
		public Dictionary<string, double> GetSettings()
		{
			StreamReader sr = new StreamReader(this.settingsLocation);

			Dictionary<string, double> ret = new Dictionary<string, double>();
			string[] sa = sr.ReadToEnd().Split('\n');
			for (int i = 0; i < sa.Length; i++)
			{
				try
				{
					sa[i].Remove(sa[i].IndexOf('\r'));
				}
				catch
				{
				}

				if (sa[i] != string.Empty)
				{
					string[] a = sa[i].Split(':');
					ret.Add(a[0], double.Parse(a[1]));
				}
			}

			sr.Close();
			return ret;
		}

		/// <inheritdoc/>
		public void SetSetting(string settingName, double value)
		{
			Dictionary<string, double> ch = this.GetSettings();
			StreamWriter sw = new StreamWriter(this.settingsLocation, false);
			ch[settingName] = value;
			foreach (KeyValuePair<string, double> c in ch)
			{
				sw.WriteLine($"{c.Key}:{c.Value}");
			}

			sw.Close();
		}
	}
}

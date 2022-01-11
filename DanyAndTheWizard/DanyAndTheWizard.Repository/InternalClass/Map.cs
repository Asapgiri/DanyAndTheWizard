// <copyright file="Map.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.InternalClass
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using DanyAndTheWizard.Repository.Interfaces;

	/// <summary>
	/// Map.
	/// </summary>
	public class Map : IMap
	{
		/// <inheritdoc/>
		public Stream GetElement(string elementFrom)
		{
			return Data.Data.GetStream($"Skins.Elements.{elementFrom}.png");
		}

		/// <inheritdoc/>
		public Stream GetMap(string mapName)
		{
			return Data.Data.GetStream($"Maps.{mapName}.png");
		}

		/// <inheritdoc/>
		public string[,] GetMapElements(string filePath)
		{
			string[,] str = new string[Config.MapHeight * 9, Config.MapWidth * 16];
			StreamReader sr = new StreamReader(Data.Data.GetStream($"Maps.{filePath}.mp"));
			List<string> ls = sr.ReadToEnd().Split('\n').ToList();
			ls.RemoveAll(x => x == string.Empty);
			int i = 0, j;
			foreach (string s in ls)
			{
				j = 0;
				List<string> ss = s.Split(',').ToList();
				ss.RemoveAll(x => x == string.Empty);
				foreach (string element in ss)
				{
					str[i, j++] = element.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace(" ", string.Empty);
				}

				i++;
			}

			sr.Close();
			return str;
		}

		/// <inheritdoc/>
		public Dictionary<string, Dictionary<string, List<string>>> GetNpcSpeach(string fileName)
		{
			Dictionary<string, Dictionary<string, List<string>>> dc = new Dictionary<string, Dictionary<string, List<string>>>();
			StreamReader sr = new StreamReader(Data.Data.GetStream($"NPC_Texts.{fileName}.nps"));
			List<string> ls = sr.ReadToEnd().Split(';').ToList();
			ls.Remove(string.Empty);
			foreach (string s in ls)
			{
				Dictionary<string, List<string>> kc = new Dictionary<string, List<string>>();
				List<string> vs = s.Split('{').ToList();
				string name = vs[0].Replace("\r", string.Empty);
				name = name.Replace("\n", string.Empty);
				vs[1] = vs[1].Remove(vs[1].LastIndexOf('}'));
				vs = vs[1].Split('~').ToList();
				vs.RemoveAt(0);
				vs.Remove(string.Empty);
				foreach (string atPlace in vs)
				{
					List<string> placelet = atPlace.Split(':').ToList();
					string place = placelet[0];
					placelet = placelet[1].Split('\n').ToList();
					placelet.Remove(string.Empty);
					placelet.RemoveAll(x => x == "\r");
					kc.Add(place, placelet);
				}

				dc.Add(name, kc);
			}

			sr.Close();
			return dc;
		}

		/// <inheritdoc/>
		public Stream GetSkin(string skinLoc)
		{
			throw new NotImplementedException();
		}
	}
}

// <copyright file="IMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

	/// <summary>
	/// Textures.
	/// </summary>
    public interface IMap
	{
		/// <summary>
		/// Gets the map.
		/// </summary>
		/// <param name="mapName">the maps source.</param>
		/// <returns>The maps stream.</returns>
		Stream GetMap(string mapName);

		/// <summary>
		/// Gets the element names on the map. All of it ~16*5x9*5. Needed to be cutted.
		/// </summary>
		/// <param name="filePath">Elements file name.</param>
		/// <returns>Element names.</returns>
		string[,] GetMapElements(string filePath);

		/// <summary>
		/// Gets an element.
		/// </summary>
		/// <param name="elementFrom">Elements name.</param>
		/// <returns>An elements Stream.</returns>
		Stream GetElement(string elementFrom);

		/// <summary>
		/// Gets an npc-s speaches at current location.
		/// </summary>
		/// <param name="fileName">Its speaches location.</param>
		/// <returns>A list of lists of its speaches at that location.</returns>
		Dictionary<string, Dictionary<string, List<string>>> GetNpcSpeach(string fileName);

		/// <summary>
		/// Gets the skin of something.
		/// </summary>
		/// <param name="skinLoc">Skin name/location.</param>
		/// <returns>The stram of the skin.</returns>
		Stream GetSkin(string skinLoc);
	}
}

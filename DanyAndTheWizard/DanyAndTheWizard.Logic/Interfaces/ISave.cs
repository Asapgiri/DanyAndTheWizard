// <copyright file="ISave.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Interfaces
{
	using System.Collections.Generic;

	/// <summary>
	/// Save.
	/// </summary>
	public interface ISave
	{
		/// <summary>
		/// Gets the save names.
		/// </summary>
		/// <returns>A list of the save names.</returns>
		List<string> GetSaveNames();

		/// <summary>
		/// Sets a save.
		/// </summary>
		/// <param name="saveName">The saves name.</param>
		/// <returns>Wethere it was successful.</returns>
		bool GetSave(string saveName);

		/// <summary>
		/// Save a game.
		/// </summary>
		/// <param name="saveName">The name of the save.</param>
		/// <returns>Wethere it was successful.</returns>
		bool NewSave(string saveName);

		/// <summary>
		/// Saves the scores.
		/// </summary>
		/// <param name="name">Scores name.</param>
		/// <returns>Successfull.</returns>
		bool SaveScore(string name);

		/// <summary>
		/// Gets the highscores.
		/// </summary>
		/// <returns>Highscores.</returns>
		List<Dictionary<string, string>> GetScores();
	}
}

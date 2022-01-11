// <copyright file="ISave.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

	/// <summary>
	/// Savings.
	/// </summary>
    public interface ISave
	{
		/// <summary>
		/// Gets the save names.
		/// </summary>
		/// <returns>A list of the save names.</returns>
		List<string> GetSaveNames();

		/// <summary>
		/// Get a saves contexts.
		/// </summary>
		/// <param name="saveName">The saves name.</param>
		/// <returns>A dictionary with the save parameters.</returns>
		Dictionary<string, string> GetSave(string saveName);

		/// <summary>
		/// Save a game.
		/// </summary>
		/// <param name="saveName">The name of the save.</param>
		/// <param name="saves">A dictionary with the actual saving parameters.</param>
		void NewSave(string saveName, Dictionary<string, string> saves);
	}
}

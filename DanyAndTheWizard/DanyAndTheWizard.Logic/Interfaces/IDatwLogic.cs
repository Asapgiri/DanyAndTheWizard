// <copyright file="IDatwLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Interfaces
{
	/// <summary>
	/// DatwLogic.
	/// </summary>
	public interface IDatwLogic
	{
		/// <summary>
		/// Initialize the model.
		/// </summary>
		/// <param name="lvlName">the loadable lvls name.</param>
		void InitModel(string lvlName);

		/// <summary>
		/// Move the character.
		/// </summary>
		/// <param name="dx">X differency.</param>
		/// <param name="dy">Y differency.</param>
		/// <returns>Whether the character could move.</returns>
		bool Move(double dx, double dy);
	}
}

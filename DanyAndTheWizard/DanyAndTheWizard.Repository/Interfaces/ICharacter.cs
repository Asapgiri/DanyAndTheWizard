// <copyright file="ICharacter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.Interfaces
{
	using System.IO;

    /// <summary>
	/// A class to get character brushes from images.
	/// </summary>
	public interface ICharacter
	{
		/// <summary>
		/// Gets an images SourceStream for a character.
		/// </summary>
		/// <param name="characterName">The characters name.</param>
		/// <returns>Returns the characters stream.</returns>
		Stream GetCharacterBrush(string characterName);
	}
}

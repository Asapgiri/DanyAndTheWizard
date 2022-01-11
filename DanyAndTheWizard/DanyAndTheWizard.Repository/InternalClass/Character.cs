// <copyright file="Character.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.InternalClass
{
	using System.IO;
	using DanyAndTheWizard.Repository.Interfaces;

	/// <summary>
	/// A class to get character skin paths.
	/// </summary>
	public class Character : ICharacter
	{
		/// <summary>
		/// Gets the characters Brushes stream from the Characters name.
		/// </summary>
		/// <param name="characterName">The characters name.</param>
		/// <returns>The characters brush.</returns>
		public Stream GetCharacterBrush(string characterName)
		{
			return Data.Data.GetStream($"Skins.Characters.{characterName}.png");
		}
	}
}
// <copyright file="EntityModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Models
{
    using System.Windows.Media;

	/// <summary>
	/// A model for the entities.
	/// </summary>
    public class EntityModel
	{
		private bool isCharacter = false;
		private bool isInteracted = false;

		/// <summary>
		/// Gets or sets the entities name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the entity is a character.
		/// </summary>
		public bool IsCharacter { get => this.isCharacter; set => this.isCharacter = value; }

		/// <summary>
		/// Gets or sets a value indicating whether the interaction happened.
		/// </summary>
		public bool IsInteracted { get => this.isInteracted; set => this.isInteracted = value; }

		/// <summary>
		/// Gets or sets the enemy brush.
		/// </summary>
		public Brush EntityBrush { get; set; }
	}
}

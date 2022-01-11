// <copyright file="MapModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Models
{
	using System.Windows.Media;

	/// <summary>
	/// A struct for the mapsegement and the entities on it.
	/// </summary>
	public struct MapModel
	{
		/// <summary>
		/// The segments brush.
		/// </summary>
		public Brush MapBrush;

		/// <summary>
		/// The segments entities.
		/// </summary>
		public EntityModel[,] Entities;
	}
}

// <copyright file="MinimapModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Models
{
    using System.Windows.Media;

	/// <summary>
	/// A model for the minimap.
	/// </summary>
    public class MinimapModel
	{
		/// <summary>
		/// Gets or sets the minimaps Brush.
		/// </summary>
		public Brush MinimapBrush { get; set; }

		/// <summary>
		/// Gets or sets the minimaps border brush.
		/// </summary>
		public Brush MinimapBorderBrush { get; set; }

		/// <summary>
		/// Gets or sets the minimaps width.
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// Gets or sets the minimaps height.
		/// </summary>
		public double Height { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether it is visible or not.
		/// </summary>
		public bool Visible { get; set; }
	}
}

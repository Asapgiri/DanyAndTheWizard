// <copyright file="DialogModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Models
{
    using System.Collections.Generic;
    using System.Windows;

	/// <summary>
	/// A model for dialogs.
	/// </summary>
    public class DialogModel
	{
		private string scene;

		/// <summary>
		/// Gets or sets the actual npc-s dialog.
		/// </summary>
		public Queue<string> ActualDialogQueue { get; set; }

		/// <summary>
		/// Gets or sets actual dialog on running.
		/// </summary>
		public string ActualDialog { get; set; }

		/// <summary>
		/// Gets or sets the actual speaker.
		/// </summary>
		public string ActualSpeeker { get; set; }

		/// <summary>
		/// Gets or sets the actual characters scene.
		/// </summary>
		public string ActualScene
		{
			get
			{
				return this.scene;
			}

			set
			{
				this.PreviousScreen = this.scene;
				this.scene = value;
			}
		}

		/// <summary>
		/// Gets or sets the previous screen for reasons.
		/// </summary>
		public string PreviousScreen { get; set; }

		/// <summary>
		/// Gets or sets Dialog location.
		/// </summary>
		public Point Location { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the character can say something over and over.
		/// </summary>
		public bool CantSayTwice { get; set; }
	}
}

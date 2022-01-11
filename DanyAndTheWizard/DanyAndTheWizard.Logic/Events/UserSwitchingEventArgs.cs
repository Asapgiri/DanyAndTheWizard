// <copyright file="UserSwitchingEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Events
{
	using System;

	/// <summary>
	/// Event args for UserControl switching.
	/// </summary>
	public class UserSwitchingEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UserSwitchingEventArgs"/> class.
		/// </summary>
		/// <param name="nc">The next screens name.</param>
		public UserSwitchingEventArgs(string nc)
		{
			this.NextScreen = nc;
		}

		/// <summary>
		/// Gets the next screen.
		/// </summary>
		public string NextScreen { get; }
	}
}

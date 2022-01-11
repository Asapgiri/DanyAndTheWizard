// <copyright file="ISettings.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository.Interfaces
{
	using System.Collections.Generic;

	/// <summary>
	/// Settings.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Gets the settings.
		/// </summary>
		/// <returns>A dictionary with the settings.</returns>
		Dictionary<string, double> GetSettings();

		/// <summary>
		/// Sets a settings parameter.
		/// </summary>
		/// <param name="settingName">settings name.</param>
		/// <param name="value">settings parameter.</param>
		void SetSetting(string settingName, double value);
	}
}

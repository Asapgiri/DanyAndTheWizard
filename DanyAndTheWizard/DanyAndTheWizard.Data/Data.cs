// <copyright file="Data.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Data
{
	using System;
	using System.IO;
	using System.Reflection;

	/// <summary>
	/// Data locations.
	/// </summary>
	public static class Data
	{
		/// <summary>
		/// Gets a files full path next to exe.
		/// </summary>
		/// <param name="name">Path next to exe.</param>
		/// <returns>Its full path.</returns>
        public static string GetFullPath(string name)
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(location.AbsolutePath).Directory.FullName + $"/{name}";
		}

		/// <summary>
		/// Gets the file stream path inside DATW.Data.
		/// </summary>
		/// <param name="name">name.</param>
		/// <returns>Its stream.</returns>
        public static Stream GetStream(string name)
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceStream($"DanyAndTheWizard.Data.{name}");
		}
	}
}

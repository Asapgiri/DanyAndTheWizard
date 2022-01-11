// <copyright file="Config.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic
{
	using System.Windows.Input;
	using System.Windows.Media;

	/// <summary>
	/// The main configurations for the game.
	/// </summary>
	public static class Config
	{
		private static Brush backgroundBrush = Brushes.DarkGray;

		private static double width = 1600;
		private static double height = 900;

		private static double textureBlockSize = width / 16;
		private static double invFrameSize = 0; // 1= ~100px tile coordinates
		private static double movementSpeed = 0.05;
		private static double mapOpacity = 0.8; // %

		/// <summary>
		/// Gets the height of the game.
		/// </summary>
		public static double Height { get => height; }

		/// <summary>
		/// Gets the width of the game.
		/// </summary>
		public static double Width { get => width; }

		/// <summary>
		/// Gets the background color.
		/// </summary>
		public static Brush BackgroundBrush { get => backgroundBrush; }

		/// <summary>
		/// Gets the size of a Texture block.
		/// </summary>
		public static double TextureBlockSize { get => textureBlockSize; }

		/// <summary>
		/// Gets the in game frame size.
		/// </summary>
		public static double InvFrameSize { get => invFrameSize; }

		/// <summary>
		/// Gets the default movement speed.
		/// </summary>
		public static double MovementSpeed { get => movementSpeed; }

		/// <summary>
		/// Gets move up key.
		/// </summary>
		public static Key MoveUp { get => Key.W; }

		/// <summary>
		/// Gets move down key.
		/// </summary>
		public static Key MoveDown { get => Key.S; }

		/// <summary>
		/// Gets move left key.
		/// </summary>
		public static Key MoveLeft { get => Key.A; }

		/// <summary>
		/// Gets move right key.
		/// </summary>
		public static Key MoveRight { get => Key.D; }

		/// <summary>
		/// Gets innventory key.
		/// </summary>
		public static Key Inventory { get => Key.I; }

		/// <summary>
		/// Gets the interaction key.
		/// </summary>
		public static Key Interact { get => Key.Space; }

		/// <summary>
		/// Gets teh escape key.
		/// </summary>
		public static Key Escape { get => Key.Escape; }

		/// <summary>
		/// Gets the minimap opacity.
		/// </summary>
		public static double MapOpacity { get => mapOpacity; }

		/// <summary>
		/// Gets the clocks size.
		/// </summary>
		public static int TimeSize { get => 48; }

		/// <summary>
		/// Gets the character sizes for a fight screen.
		/// </summary>
		public static double FightCharacterSize { get => 5; }

		/// <summary>
		/// Gets the Width and Height of a character.
		/// </summary>
		public struct Character
		{
			/// <summary>
			/// Gets the Width of a character.
			/// </summary>
			public static double Width = 2 * (textureBlockSize / 3);

			/// <summary>
			/// Gets the Height of a character.
			/// </summary>
			public static double Height = textureBlockSize;
		}
	}
}

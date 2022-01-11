// <copyright file="HighScoresWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using DanyAndTheWizard.Logic.Events;
	using DanyAndTheWizard.Logic.InternalClasses;

	/// <summary>
	/// Interaction logic for HighScoresWindow.xaml.
	/// </summary>
	public partial class HighScoresWindow : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HighScoresWindow"/> class.
		/// </summary>
		public HighScoresWindow()
		{
			this.InitializeComponent();
			this.Loaded += this.HighScoresWindow_Loaded;
		}

		/// <summary>
		/// Calls UserControl switching.
		/// </summary>
		public event EventHandler CallUc;

		private void HighScoresWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.Stacky.Children.Clear();
			Save score = new Save();
			TupleList<int, UIElement> sls = new TupleList<int, UIElement>();
			try
			{
				foreach (Dictionary<string, string> dic in score.GetScores())
				{
					string ss = string.Empty;

					foreach (KeyValuePair<string, string> item in dic)
					{
						ss += $"{item.Key}: {item.Value}\n";
					}

					Border b = new Border();
					b.Margin = new Thickness(10);
					b.BorderBrush = Brushes.Black;
					b.BorderThickness = new Thickness(3);

					TextBlock tb = new TextBlock();
					tb.FontSize = 18;
					tb.FontWeight = FontWeights.Bold;

					tb.Text = ss;
					b.Child = tb;

					sls.Add(int.Parse(dic["Score"]), b);
				}

				sls.Sort();
				foreach (Tuple<int, UIElement> element in sls)
				{
					this.Stacky.Children.Add(element.Item2);
				}
			}
			catch (Exception)
			{
				this.Stacky.Children.Add(new TextBlock() { Text = "There are no scores yet..\nAt least they are not found.", TextAlignment = TextAlignment.Center, FontSize = 24, FontWeight = FontWeights.Bold });
			}
		}

		private void Exit_To_Main_MenuWindow_Click(object sender, RoutedEventArgs e)
		{
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("Menu"));
		}

		private class TupleList<T1, T2> : List<Tuple<T1, T2>>
			where T1 : IComparable
		{
			public void Add(T1 item, T2 item2)
			{
				this.Add(new Tuple<T1, T2>(item, item2));
			}

			public new void Sort()
			{
				Comparison<Tuple<T1, T2>> c = (a, b) => b.Item1.CompareTo(a.Item1);
				this.Sort(c);
			}
		}
	}
}

﻿using MotionVisualizer;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisualizerBaseClasses;
using VisualizerControl;
using static WPFUtility.UtilityFunctions;

namespace MotionVisualizer3D
{
    /// <summary>
    /// Interaction logic for FullVisualizer.xaml
    /// </summary>
    public partial class MotionVisualizer3DControl : MotionVisualizerBase<Visualizer, VisualizerCommand>
    {
        public string BackgroundFile
        {
            set
            {
                Visualizer.BackgroundFile = value;
            }
        }

        public MotionVisualizer3DControl(IVisualization engine) :
            base(engine, new Visualizer())
        {
            FinishInitialization();
        }

        public MotionVisualizer3DControl(string filename, VisualizerCommandFileReader reader) :
            base(filename, reader)
        {
            FinishInitialization();
        }

        private void FinishInitialization()
        {
            InitializeComponent();

            Viewport.Content = Visualizer;

            VisualizerSpot.Content = Visualizer;
            GraphSpot.Content = Graphs;
        }

        public override void UpdateTime(double time)
        {
            TimeValue.Text = Math.Round(time, 2).ToString();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            bool needToRestart = false;
            if (IsRunning)
            {
                IsRunning = false;
                needToRestart = true;
            }

            SaveScreenshot((int)ActualWidth, (int)ActualHeight, this);

            if (needToRestart)
            {
                IsRunning = true;
            }
        }

        private void Screenshot_Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(MakeScreenshot((int)ActualWidth, (int)ActualHeight, this));
        }

        private double timeIncrement = .01;
        public override double TimeIncrement
        {
            get => timeIncrement;
            set
            {
                timeIncrement = value;
                TimeIncrementSlider.Text = timeIncrement.ToString();
            }
        }


        private double timeScale = 1;
        public double TimeScaleDisplay
        {
            get => timeScale;
            set
            {
                timeScale = value;
                TimeScaleSlider.Text = timeScale.ToString();
            }
        }

        private bool autoCamera = false;
        public bool AutoCamera
        {
            get => autoCamera;
            set
            {
                autoCamera = value;
                AutoCameraCheck.IsChecked = value;
            }
        }

        /// <summary>
        /// Whether the 3D should be updating while the engine calculates
        /// Can be turned off to speed up graph generation time
        /// </summary>
        public bool Display { get; set; } = true;


        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunning)
            {
                Start_Button.Content = "Resume";
                IsRunning = false;
            }
            else
            {
                Start_Button.Content = "Pause";
                IsRunning = true;

                StartAll();
            }
        }

        private void TimeIncrementSlider_TextChanged(object sender, TextChangedEventArgs e)
        {
            SliderChanged(TimeIncrementSlider, ref timeIncrement);
        }

        private void SliderChanged(TextBox textBox, ref double result)
        {
            if (double.TryParse(textBox.Text, out double newNum))
                result = newNum;
        }

        private void AutoCameraCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (AutoCameraCheck.IsChecked != null)
            {
                autoCamera = (bool)AutoCameraCheck.IsChecked;
                if (!IsRunning && autoCamera)
                    Visualizer.AdjustCamera();
            }
        }

        private void TimeScaleSlider_TextChanged(object sender, TextChangedEventArgs e)
        {
            SliderChanged(TimeScaleSlider, ref timeScale);
        }

        private void DisplayCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (DisplayCheck.IsChecked != null)
            {
                Display = (bool)DisplayCheck.IsChecked;
            }
        }

        private bool slowDraw = false;
        public override bool SlowDraw
        {
            get => slowDraw;
            set
            {
                slowDraw = value;
                SlowDrawCheck.IsChecked = value;
            }
        }
        private void SlowDrawCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (SlowDrawCheck.IsChecked != null)
            {
                SlowDraw = (bool)SlowDrawCheck.IsChecked;
            }
        }
    }
}
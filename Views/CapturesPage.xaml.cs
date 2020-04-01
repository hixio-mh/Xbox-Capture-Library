﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XboxGameClipLibrary.ViewModels.CapturesViewModel;

namespace XboxGameClipLibrary.Views
{
    public partial class CapturesPage : Page
    {
        private bool handle = true;

        public CapturesPage()
        {
            // Initialize the CapturesPage component
            InitializeComponent();

            // Bind the capture data to gameClipListView
            Loaded += CapturesPage_Loaded;
        }

        private void CapturesPage_Loaded(object sender, RoutedEventArgs e)
        {
            BindCaptureDataToDataGrid();
            Loaded -= CapturesPage_Loaded;
        }

        private void BindCaptureDataToDataGrid()
        {
            // Bind the Game Clip capture data to the itemssource of the gameClipListView
            DataContext = new CapturesViewModel();

            // Display the correct type of capture data with respect to the ComboBox
            //Handle();
        }

        private void ScreenshotListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListView list = (ListView) sender;
            var dataContext = capturesPage.DataContext as CapturesViewModel;
            var screenshot = dataContext.Screenshots[list.Items.IndexOf(list.SelectedItem)];

            screenshotDetailPane.ScreenshotId = screenshot.ScreenshotId;
            screenshotDetailPane.ScreenshotUri = screenshot.ScreenshotUris[0].Uri;
            screenshotDetailPane.Game = screenshot.TitleName;
            screenshotDetailPane.Device = screenshot.DeviceType;
            screenshotDetailPane.DateTaken = screenshot.DateTaken.ToString();
            screenshotDetailPane.DatePublished = screenshot.DatePublished.ToString();

            screenshotDetailPane.Views = screenshot.Views.ToString();
            screenshotDetailPane.Likes = screenshot.RatingCount.ToString();

            screenshotDetailPane.Visibility = Visibility.Visible;
        }

        private void CaptureTypeComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (handle) HandleComboBoxSelection();
            handle = true;
        }

        private void CaptureTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            HandleComboBoxSelection();
        }

        // Gets input from comboboxes
        private void HandleComboBoxSelection()
        {
            switch (captureBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Screenshots":
                    gameClipListView.Visibility = Visibility.Collapsed;
                    screenshotListView.Visibility = Visibility.Visible;
                    break;

                case "Game clips":
                    screenshotListView.Visibility = Visibility.Collapsed;
                    gameClipListView.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
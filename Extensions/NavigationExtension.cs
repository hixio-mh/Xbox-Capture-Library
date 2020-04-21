﻿using System.Windows.Controls;

namespace XboxGameClipLibrary.Navigation
{
    public static class Navigation
    {
        private static Frame _frame;

        public static Frame Frame
        {
            get { return _frame; }
            set { _frame = value; }
        }

        public static bool Navigate(object sourcePageUri, object extraData)
        {
            if (_frame.NavigationService.Content != sourcePageUri)
            {
                return _frame.Navigate(sourcePageUri, extraData);
            }
            return true;
        }

        public static bool Navigate(object sourcePageUri)
        {
            if (_frame.NavigationService.Content != sourcePageUri)
            {
                return _frame.Navigate(sourcePageUri);
            }
            return true;
        }
    }
}

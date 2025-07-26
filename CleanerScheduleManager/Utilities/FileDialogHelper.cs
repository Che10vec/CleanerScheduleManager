using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanerScheduleManager.Utilities
{
    public static class FileDialogHelper
    {
        public static string? ShowSaveFileDialog(string defaultFileName, string filter = "JSON Files (*.json)|*.json")
        {
            var dialog = new SaveFileDialog
            {
                FileName = defaultFileName,
                Filter = filter
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public static string? ShowOpenFileDialog(string filter = "JSON Files (*.json)|*.json")
        {
            var dialog = new OpenFileDialog
            {
                Filter = filter
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}

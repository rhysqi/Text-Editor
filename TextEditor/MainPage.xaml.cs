using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TextEditor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BNew_Click(object sender, RoutedEventArgs e)
        {
            // New File Function
            Edit_TextBox.Text = "";
            BLName.Text = "";
        }

        private async void BOpen_Click(object sender, RoutedEventArgs e)
        {

            // File Open Function
            FileOpenPicker OpenFile = new FileOpenPicker();
            OpenFile.ViewMode = PickerViewMode.List;

            OpenFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary; // File Location To save
            OpenFile.FileTypeFilter.Add(".txt"); // File Filter

            // File Explorer Open
            StorageFile file = await OpenFile.PickSingleFileAsync(); 

            // Asynchronous File Read
            Edit_TextBox.Text = await FileIO.ReadTextAsync(file); // exception error 1

            BLName.Text = file.Name;
        }

        private async void BSaveAs_Click(object sender, RoutedEventArgs e)
        {
            // Asynchronous File Save Function
            FileSavePicker SaveFile = new FileSavePicker();
            SaveFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // File Name Suggested
            SaveFile.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            SaveFile.SuggestedFileName = "Text"; 
            
            StorageFile file = await SaveFile.PickSaveFileAsync(); // Asynchronous File Save

            // Some Asynchronous File Save Function
            if(file != null)
            {
                CachedFileManager.DeferUpdates(file);
                await FileIO.WriteTextAsync(file, Edit_TextBox.Text);
                FileUpdateStatus Status = await CachedFileManager.CompleteUpdatesAsync(file);

                // Function if file are Successful saved
                if(Status == FileUpdateStatus.Complete)
                {
                    ContentDialog saved = new ContentDialog()
                    {
                        Title = "File Saved",
                        Content = "File are saved in directory !",
                        CloseButtonText = "OK"
                    };
                    await saved.ShowAsync();
                }

                // Function if file are replaced
                else if(Status == FileUpdateStatus.CompleteAndRenamed)
                {
                    ContentDialog replaced = new ContentDialog()
                    {
                        Title = "File Overwrite",
                        Content = "File are overwrite in directory !",
                        CloseButtonText = "OK"
                    };
                    await replaced.ShowAsync();
                }
                // Function if file are not saved
                else
                {
                    ContentDialog notsaved = new ContentDialog()
                    {
                        Title = "File Not Saved",
                        Content = "File are not saved in directory !",
                        CloseButtonText = "OK"
                    };
                    await notsaved.ShowAsync();
                }
            }

            // Function if Operation Cancelled
            else
            {
                ContentDialog canceled = new ContentDialog()
                {
                    Title = "",
                    Content = "Operation Cancelled",
                    CloseButtonText = "OK"
                };
                await canceled.ShowAsync();
            }
        }

        private async void BAbout_Click(object sender, RoutedEventArgs e)
        {
            // Asynchronous Content Dialog to show about button
            ContentDialog About = new ContentDialog()
            {
                Title = "About",
                Content = "Author: Risky Akbar",
                CloseButtonText = "Close"
            };
            await About.ShowAsync();
        }
    }
}

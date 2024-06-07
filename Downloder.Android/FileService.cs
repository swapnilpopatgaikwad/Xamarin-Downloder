using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Downloder.Droid;
using PCLStorage;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using FileAccess = PCLStorage.FileAccess;

[assembly: Dependency(typeof(FileService))]
namespace Downloder.Droid
{
    public class FileService : IFileService
    {
        public async Task<bool> SaveFileToLocalStorage()
        {
            try
            {
                string assetFileName = "AboutAssets.txt";
                string localFileName = "AboutAssets1.txt";

                Context context = Android.App.Application.Context;
                AssetManager assets = context.Assets;

                string folderName = "Downloder";
                var fileSystem = FileSystem.Current;
                var downloadFolder = await fileSystem.LocalStorage.GetFolderAsync(Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads));

                if (! await IsFolderExistAsync(folderName, downloadFolder))
                {
                    downloadFolder = await downloadFolder.CreateFolderAsync(folderName, CreationCollisionOption.ReplaceExisting);
                }
                else
                {
                    downloadFolder = await downloadFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                }
                IFile file = null ;
                if (!await IsFileExistAsync(localFileName, downloadFolder))
                {
                    file = await downloadFolder.CreateFileAsync(localFileName, CreationCollisionOption.ReplaceExisting);
                }
                else
                {
                    file = await downloadFolder.CreateFileAsync(localFileName, CreationCollisionOption.OpenIfExists);
                }

                using (Stream assetStream = assets.Open(assetFileName))
                using (Stream localFileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
                {
                    assetStream.CopyTo(localFileStream);
                }
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> IsFileExistAsync(string fileName, IFolder rootFolder = null)
        {
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            ExistenceCheckResult folderexist = await folder.CheckExistsAsync(fileName);
            if (folderexist == ExistenceCheckResult.FileExists)
            {
                return true;

            }
            return false;
        }

        public async Task<bool> IsFolderExistAsync(string folderName, IFolder rootFolder = null)
        {
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            ExistenceCheckResult folderexist = await folder.CheckExistsAsync(folderName);
            if (folderexist == ExistenceCheckResult.FolderExists)
            {
                return true;

            }
            return false;
        }
    }
}
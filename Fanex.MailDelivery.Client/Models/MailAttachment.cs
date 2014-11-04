namespace Fanex.MailDelivery.Client.Models
{
    using System.IO;
    using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// Mail Attachment.
    /// </summary>
    public class MailAttachment
    {
        public MailAttachment(string filePath, FileExtension fileExtension)
        {
            ReadFileContent(filePath);
            FileExtension = FileExtension;
        }

        public MailAttachment(string fileName, byte[] fileContent, FileExtension fileExtension)
        {
            FileName = FileName;
            FileContent = FileContent;
            FileExtension = FileExtension;
        }

        public int Id { get; set; }

        public long MailId { get; set; }

        public FileExtension FileExtension { get; set; }

        public byte[] FileContent { get; set; }

        public string FileName { get; set; }

        /// <summary>
        /// Reads the content of the file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        private void ReadFileContent(string filePath)
        {
            if (FileContent == null
                && !string.IsNullOrEmpty(filePath))
            {
                using (var stream = File.Open(filePath, FileMode.Open))
                {
                    FileContent = new byte[stream.Length];
                    stream.Read(FileContent, 0, FileContent.Length);
                }

                FileName = Path.GetFileName(filePath);
            }
        }
    }
}
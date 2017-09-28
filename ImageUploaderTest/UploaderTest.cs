using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageUploader.Models;

namespace ImageUploaderTest
{
    [TestClass]
    public class UploaderTest
    {
        [TestMethod]
        public void StartDownload()
        {
            try
            {
                string path = null;
                var data = new Uploader().DownloadImage(path);
            }
            catch (System.Exception e)
            {

                throw;
            }
            
        }

        [TestMethod]
        public void AbortDownloading()
        {

        }

        [TestMethod]
        public void Stress()
        {

        }
    }
}

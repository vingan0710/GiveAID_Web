namespace GiveAID.Models
{
    internal class Uploads
    {
        private static Uploads? instance = null;

        public Uploads()
        {

        }
        internal static Uploads Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Uploads();
                }
                return instance;
            }
        }
        public string UploadImage(IFormFile? imgDes)
        {
            string fileName;
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads");
                FileInfo fileInfo = new FileInfo(imgDes.FileName);
                fileName = DateTime.Now.Ticks + imgDes.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    imgDes.CopyTo(stream);
                }
            }
            catch (Exception)
            {
                fileName = "error";
            }
            return fileName;
        }
    }
}

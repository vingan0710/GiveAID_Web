using GiveAID.Models;

namespace GiveAID.Dao
{
    public class Dao_Gallery
    {
        private static Dao_Gallery? instance = null;
        private static AIDContext? ct;

        private Dao_Gallery()
        {
            ct = new();
        }

        public static Dao_Gallery Instance()
        {
            instance ??= new Dao_Gallery();
            return instance;
        }

        //public List<Gallery> GetGalleries ()
        //{
        //    var galleries = (from g in ct.Galleries);
        //}
    }
}

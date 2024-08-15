using GiveAID.Models;

namespace GiveAID.Dao
{
    internal class Dao_Contact
    {
        #region singleton
        private static Dao_Contact? instance = null;
        private Dao_Contact() { }
        internal static Dao_Contact Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Dao_Contact();
                }
                return instance;
            }
        }
        #endregion
        public Contact SendContact(Contact contact)
        {
            try
            {
                AIDContext context = new AIDContext();
                context.Contacts?.Add(contact);
                context.SaveChanges();
                return contact;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
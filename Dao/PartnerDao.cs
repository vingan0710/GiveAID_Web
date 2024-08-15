#pragma warning disable CS8602
using GiveAID.Models;
using Microsoft.Extensions.Hosting;
using System.Drawing;

namespace GiveAID.Areas.Admin.Dao
{
    public class PartnerDao
    {
        private static PartnerDao? instance = null;
        private PartnerDao() { }

        internal static PartnerDao Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PartnerDao();
                }
                return instance;
            }
        }
       
        public List<Partnership> Partners
        {
            get
            {
                AIDContext f = new AIDContext();
                var result = f.Partnerships.Select(x => new Partnership()
                {
                    id = x.id,
                    partner_name = x.partner_name,
                    description = x.description,
                    logo = x.logo
                }).ToList();
                return result;
            }
        }
        




    }
}

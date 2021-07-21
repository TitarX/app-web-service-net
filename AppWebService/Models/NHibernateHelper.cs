using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace AppWebService.Models
{
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\NHibernate.cfg.xml");
            configuration.Configure(configurationPath);
            var applicationsConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\NHibernate\Applications.hbm.xml");
            configuration.AddFile(applicationsConfigurationFile);
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}

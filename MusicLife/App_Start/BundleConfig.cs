using System.Web;
using System.Web.Optimization;

namespace MusicLife
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").IncludeDirectory("~/Content", "*.css", true));
        }
    }
}

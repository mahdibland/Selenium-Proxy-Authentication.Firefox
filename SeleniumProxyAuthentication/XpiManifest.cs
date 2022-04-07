using System.Collections.Generic;

namespace SeleniumProxyAuthentication
{
    /// <summary>
    /// Firefox add-on manifest file model 
    /// </summary>
    public class XpiManifest
    {
        public string version { get; set; }
        public int manifest_version { get; set; }
        public string name { get; set; }
        public List<string> permissions { get; set; }
        public BackGround background { get; set; }
        public BrowserSpecificSettings browser_specific_settings { get; set; }
    }
    public partial class BackGround
    {
        public List<string> scripts { get; set; }
    }
    public partial class BrowserSpecificSettings
    {
        public Gecko gecko { get; set; }
    }
    public partial class Gecko
    {
        public string id { get; set; }
    }
}

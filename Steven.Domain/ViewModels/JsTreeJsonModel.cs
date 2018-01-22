using System.Collections.Generic;

namespace Steven.Domain.ViewModels
{
    public class JsTreeJsonModel
    {
        public string id { get; set; }
        public string pid { get; set; }
        public string text { get; set; }
        public int sort { get; set; }

        public string icon
        {
            get;set;
        }
        

        public List<JsTreeJsonModel> children { get; set; }
    }

}

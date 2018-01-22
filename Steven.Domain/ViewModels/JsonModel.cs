namespace Steven.Domain.ViewModels
{
    public class JsonModel
    {
        public JsonModel()
        {
            code = JsonModelCode.Error;
        }

        public JsonModelCode code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
    }

    public enum JsonModelCode
    {
        Succ = 1,
        Error = 2,
        UnLogin = 3
    }

}

namespace Speurzoekers.Common.Domain
{
    public class ActionResult
    {
        public bool Succeeded { get; set; }
        public ActionResultError[] Errors { get; set; }
    }

    public class ActionResultError
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}

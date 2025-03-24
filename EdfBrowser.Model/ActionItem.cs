namespace EdfBrowser.Model
{
    public class ActionItem : DomainObject
    {
        public ActionItem(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}

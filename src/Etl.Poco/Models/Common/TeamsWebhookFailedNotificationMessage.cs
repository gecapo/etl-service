namespace Etl.Poco.Models.Common;

public class TeamsWebhookFailedNotificationMessage(string title, string error)
{
    public string Summary { get; set; } = "MessageCard";
    public string ThemeColor { get; set; } = "0078D7";
    public string Title { get; set; } = title;
    public Section[] Sections { get; set; } = [new(error)];

    public class Section
    {
        public Section(string error)
        {
            ActivityTitle = "Failed on Importing Report";
            Facts = [new("Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")),
                new("Erorr", error)];
        }

        public Section(Dictionary<string, string> facts)
        {
            ActivityTitle = "Failed on Importing Report";
            Facts = [new("Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")),
                ..facts.Select(x=>new Fact(x.Key, x.Value))];
        }

        public string ActivityTitle { get; set; }
        public Fact[] Facts { get; set; }

        public class Fact(string name, string value)
        {
            public string Name { get; set; } = name;
            public string Value { get; set; } = value;
        }
    }
}

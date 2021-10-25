using System.Text.Json.Serialization;

namespace StudyManager.Data.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Student,
        Teacher,
        Admin
    }
}

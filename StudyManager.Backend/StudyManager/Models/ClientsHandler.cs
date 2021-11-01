using System.Collections;

namespace StudyManager.Models
{
    public static class ClientsHandler
    {
        public static Hashtable ConnectedIds = new Hashtable();

        public static int GroupCount(string groupName)
        {
            int result = 0;
            foreach(DictionaryEntry di in ConnectedIds)
            {
                if (di.Value.ToString() == groupName)
                    result++;
            }
            return result;
        }
    }
}

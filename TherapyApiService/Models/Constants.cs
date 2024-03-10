namespace TherapyApiService.Models
{
    public static class Constants
    {
        public static class Bots
        {
            public const string AlertBot = "alertBot";
        }

        public static class Messages
        {
            public const string Info = "Информация! Необходимо сделать инъекцию {0}";
            public const string Alert = "Внимание! Срок плановой иъекции прошел. Необходимо было сделать инъекцию {0}";
        }
    }
}
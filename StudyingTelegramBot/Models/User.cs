namespace StudyingTelegramApi.Models {
    public class User {
        public Guid Id { get; set; }
        public long TelegramId { get; set; }
        public bool RemindStartLesson { get; set; }
        public bool RemindWriteHomework { get; set; }
        public bool RemindCompleteHomework { get; set; }
    }
}
 
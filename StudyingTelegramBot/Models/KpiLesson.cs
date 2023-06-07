namespace StudyingTelegramApi.Models {
    public class Pair {
        public string TeacherName { get; set; }
        public Guid LecturerId { get; set; }
        public string Type { get; set; }
        public string Time { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Tag { get; set; }
    }

    public class WeekData {
        public string Day { get; set; }
        public List<Pair> Pairs { get; set; }
    }

    public class ScheduleData {
        public Guid groupCode;
        public List<WeekData> ScheduleFirstWeek { get; set; }
        public List<WeekData> ScheduleSecondWeek { get; set; }
    }

    public class LessonsResponse {
        public object Paging { get; set; }
        public ScheduleData Data { get; set; }
    }
}

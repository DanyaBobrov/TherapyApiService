namespace TherapyApiService.Models
{
    public class Injection
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public EntityId Id { get; set; }

        /// <summary>
        /// Планируемая дата
        /// </summary>
        public DateOnly TargetDate { get; set; }

        /// <summary>
        /// Фактическая дата
        /// </summary>
        public DateTime? ActualDate { get; set; }

        /// <summary>
        /// Часть тела
        /// </summary>
        public BodyPart BodyPart { get; set; }

        /// <summary>
        /// Флаг выполнения
        /// </summary>
        public bool IsDone => ActualDate != null;

        /// <summary>
        /// Информация о шприцах
        /// </summary>
        public Syringe[] Syringes { get; set; }
    }
}
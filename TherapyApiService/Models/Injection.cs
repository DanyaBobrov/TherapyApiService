namespace TherapyApiService.Models
{
    public class Injection
    {
        public EntityId Id { get; set; }
        public DateOnly Date { get; set; }
        public BodyPart BodyPart { get; set; }
        public Syringe[] Syringes { get; set; }
    }
}
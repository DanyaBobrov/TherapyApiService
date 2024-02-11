namespace TherapyApiService.Models.API.DTO
{
    public class InjectionDTO
    {
        public string Id { get; private set; }
        public DateOnly Date { get; private set; }
        public BodyPart BodyPart { get; private set; }
        public Syringe[] Syringes { get; private set; }

        public static class Mapper
        {
            public static InjectionDTO Map(Injection injection)
            {
                return new InjectionDTO()
                {
                    Id = injection.Id,
                    Date = injection.TargetDate,
                    BodyPart = injection.BodyPart,
                    Syringes = injection.Syringes
                };
            }
        }
    }
}
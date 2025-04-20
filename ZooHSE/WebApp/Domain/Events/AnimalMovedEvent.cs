using System;

namespace ZooHSE.WebApi.Domain.Events
{
    /// <summary>
    /// Событие перемещения животного между вольерами.
    /// </summary>
    public class AnimalMovedEvent
    {
        public int AnimalId { get; }
        public int? OldEnclosureId { get; }
        public int NewEnclosureId { get; }
        public DateTime OccurredOn { get; }

        public AnimalMovedEvent(int animalId, int? oldEnclosureId, int newEnclosureId)
        {
            AnimalId = animalId;
            OldEnclosureId = oldEnclosureId;
            NewEnclosureId = newEnclosureId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}

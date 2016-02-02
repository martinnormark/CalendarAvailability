using System;

namespace DateTimeGaps
{
	class Program
	{
		static void Main(string[] args)
		{
			TimeSlotCollection slots = new TimeSlotCollection();
			slots.Add(new TimeSlot(new DateTime(2015, 1, 1, 9, 0, 0), new DateTime(2015, 1, 1, 10, 0, 0)));
			slots.Add(new TimeSlot(new DateTime(2015, 1, 1, 10, 0, 0), new DateTime(2015, 1, 1, 12, 0, 0)));
			slots.Add(new TimeSlot(new DateTime(2015, 1, 1, 14, 0, 0), new DateTime(2015, 1, 1, 15, 0, 0)));
			slots.Add(new TimeSlot(new DateTime(2015, 1, 1, 16, 0, 0), new DateTime(2015, 1, 1, 16, 30, 0)));

			TimeSlot withIn = new TimeSlot(new DateTime(2015, 1, 1, 8, 0, 0), new DateTime(2015, 1, 1, 18, 0, 0));
			TimeSpan slotLength = TimeSpan.FromMinutes(60);

			TimeSlotCollection availableTimes = slots.FindAvailableSlots(slotLength, withIn);

			if (availableTimes != null)
			{
				foreach (var item in availableTimes)
				{
					Console.WriteLine(item);
				}
			}

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}

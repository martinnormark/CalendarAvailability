using System;
using System.Linq;

public static class DateTimeExtensions
{
	private static readonly DayOfWeek[] WEEKEND_DAYS = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

	public static bool IsWeekend(this DateTime dt)
	{
		return WEEKEND_DAYS.Contains(dt.DayOfWeek);
	}
}
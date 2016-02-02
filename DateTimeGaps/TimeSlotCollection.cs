using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DateTimeGaps
{
	public class TimeSlot : IComparable<TimeSlot>
	{
		public TimeSlot(DateTime from, DateTime to)
		{
			if (to < from)
			{
				throw new ArgumentException("Value of parameter 'to', must be greater than 'from'!", "to");
			}

			From = from;
			To = to;
		}

		public DateTime From { get; private set; }

		public DateTime To { get; private set; }

		public int CompareTo(TimeSlot other)
		{
			if (From == other.From)
			{
				return To.CompareTo(other.To);
			}

			return From.CompareTo(other.From);
		}

		public override string ToString()
		{
			if (To - From >= TimeSpan.FromDays(1))
			{
				return String.Format("{0:ddd HH:mm} - {1:ddd HH:mm}", From, To);
			}

			return String.Format("{0:HH:mm} - {1:HH:mm}", From, To);
		}
	}

	public class TimeSlotCollection : ICollection<TimeSlot>
	{
		private readonly SortedSet<TimeSlot> _slots;

		public TimeSlotCollection(IEnumerable<TimeSlot> collection = null)
		{
			_slots = new SortedSet<TimeSlot>(collection ?? new List<TimeSlot>());
		}

		public TimeSlotCollection FindAvailableSlots(TimeSpan slotLength, TimeSlot withIn)
		{
			if (slotLength <= TimeSpan.Zero)
			{
				return new TimeSlotCollection();
			}

			List<TimeSlot> slotsWithBoundary = GetListWithBoundaries(withIn);

			var itemsWithIndices = slotsWithBoundary.Select((item, index) => new { Item = item, Index = index }).ToList();

			var results = from s1 in itemsWithIndices
										let s2 = itemsWithIndices.ElementAtOrDefault(s1.Index + 1)
										where s2 != null && s2.Item.From - s1.Item.To >= slotLength
										select new TimeSlot(s1.Item.To, s2.Item.From);

			return new TimeSlotCollection(results);
		}

		public int Count
		{
			get
			{
				return _slots.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public void Add(TimeSlot item)
		{
			_slots.Add(item);
		}

		public void Clear()
		{
			_slots.Clear();
		}

		public bool Contains(TimeSlot item)
		{
			return _slots.Contains(item);
		}

		public void CopyTo(TimeSlot[] array, int arrayIndex)
		{
			_slots.CopyTo(array, arrayIndex);
		}

		public IEnumerator<TimeSlot> GetEnumerator()
		{
			return _slots.GetEnumerator();
		}

		public bool Remove(TimeSlot item)
		{
			return _slots.Remove(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _slots.GetEnumerator();
		}

		private List<TimeSlot> GetListWithBoundaries(TimeSlot withIn)
		{
			var slotsStartingWithInBoundary = _slots.Where(s => s.From >= withIn.From && s.From < withIn.To);

			List<TimeSlot> slotsWithBoundary = new List<TimeSlot>();
			slotsWithBoundary.Add(new TimeSlot(withIn.From, withIn.From));
			slotsWithBoundary.AddRange(slotsStartingWithInBoundary);
			slotsWithBoundary.Add(new TimeSlot(withIn.To, withIn.To));

			return slotsWithBoundary;
		}
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FluentCassandra.Types
{
	public class LongType : CassandraType
	{
		private static readonly LongTypeConverter Converter = new LongTypeConverter();

		private static object GetObject(object obj, Type type)
		{
			var converter = Converter;

			if (!converter.CanConvertTo(type))
				throw new NotSupportedException(type + " is not supported for Int64 serialization.");

			return converter.ConvertTo(obj, type);
		}

		public override T ConvertTo<T>()
		{
			return (T)GetObject(this._value, typeof(T));
		}

		public override CassandraType SetValue(object obj)
		{
			var type = new LongType();
			var converter = Converter;

			if (!converter.CanConvertFrom(obj.GetType()))
				throw new NotSupportedException(obj.GetType() + " is not supported for Int64 serialization.");

			type._value = (long)converter.ConvertFrom(obj);

			return type;
		}

		public override byte[] ToByteArray()
		{
			return (byte[])GetObject(_value, typeof(byte[]));
		}

		public override string ToString()
		{
			return _value.ToString("N");
		}

		private long _value;

		public override bool Equals(object obj)
		{
			if (obj is LongType)
				return _value == ((LongType)obj)._value;

			return _value == (long)GetObject(obj, typeof(long));
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public static implicit operator long(LongType type)
		{
			return type._value;
		}

		public static implicit operator LongType(long o)
		{
			return new LongType {
				_value = o
			};
		}

		public static implicit operator byte[](LongType type)
		{
			return type.ToByteArray();
		}

		public static implicit operator LongType(byte[] o)
		{
			return new LongType {
				_value = (long)GetObject(o, typeof(long))
			};
		}

		public static implicit operator LongType(byte o) { return Convert(o); }
		public static implicit operator LongType(sbyte o) { return Convert(o); }
		public static implicit operator LongType(short o) { return Convert(o); }
		public static implicit operator LongType(ushort o) { return Convert(o); }
		public static implicit operator LongType(int o) { return Convert(o); }
		public static implicit operator LongType(uint o) { return Convert(o); }
		public static implicit operator LongType(ulong o) { return Convert(o); }
		public static implicit operator LongType(float o) { return Convert(o); }
		public static implicit operator LongType(double o) { return Convert(o); }
		public static implicit operator LongType(decimal o) { return Convert(o); }
		public static implicit operator LongType(bool o) { return Convert(o); }
		public static implicit operator LongType(string o) { return Convert(o); }
		public static implicit operator LongType(char o) { return Convert(o); }
		public static implicit operator LongType(DateTime o) { return Convert(o); }
		public static implicit operator LongType(DateTimeOffset o) { return Convert(o.UtcTicks); }

		private static T Convert<T>(LongType type)
		{
			return (T)GetObject(type._value, typeof(T));
		}

		private static LongType Convert(object o)
		{
			var type = new LongType();
			type._value = (long)GetObject(o, typeof(long));

			return type;
		}
	}
}
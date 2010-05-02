﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.ComponentModel;

namespace FluentCassandra
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">A type that impliments <see cref="IFluentColumn"/>.</typeparam>
	public abstract class FluentRecord<T> : DynamicObject, IFluentRecord, IFluentRecord<T>, INotifyPropertyChanged, IEnumerable<T>
		where T : IFluentBaseColumn
	{
		/// <summary>
		/// 
		/// </summary>
		public FluentRecord()
		{
			MutationTracker = new FluentMutationTracker(this);
		}

		/// <summary>
		/// The record columns.
		/// </summary>
		public abstract IList<T> Columns { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			return TryGetColumn(binder.Name, out result);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="indexes"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			string index0 = indexes[0].ToString();
			return TryGetColumn(index0, out result);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public abstract bool TryGetColumn(object name, out object result);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			return TrySetColumn(binder.Name, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="indexes"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			string index0 = indexes[0].ToString();
			return TrySetColumn(index0, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="binder"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public abstract bool TrySetColumn(object name, object value);

		#region IEnumerable<T> Members

		public IEnumerator<T> GetEnumerator()
		{
			return Columns.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnColumnMutated(MutationType type, IFluentBaseColumn column)
		{
			MutationTracker.ColumnMutated(type, column);

			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(column.Name));
		}

		#endregion

		#region IFluentRecord Members

		IList<IFluentBaseColumn> IFluentRecord.Columns
		{
			get { return (IList<IFluentBaseColumn>)Columns; }
		}

		public IFluentMutationTracker MutationTracker
		{
			get;
			private set;
		}

		#endregion
	}
}
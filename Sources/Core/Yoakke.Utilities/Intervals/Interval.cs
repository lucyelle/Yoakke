// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using Yoakke.Utilities.Compatibility;

namespace Yoakke.Utilities.Intervals
{
    /// <summary>
    /// Represents a finite or infinite interval.
    /// </summary>
    /// <typeparam name="T">The type of the interval values.</typeparam>
    public readonly struct Interval<T> : IEquatable<Interval<T>>
    {
        /// <summary>
        /// Constructs a new interval that represents all values included.
        /// </summary>
        /// <returns>The constructed interval.</returns>
        public static Interval<T> Full() => new(LowerBound<T>.Unbounded(), UpperBound<T>.Unbounded());

        /// <summary>
        /// Constructs a new interval that contains the single value that was passed.
        /// </summary>
        /// <param name="value">The single contained value in the interval.</param>
        /// <returns>The constructed interval.</returns>
        public static Interval<T> Singleton(T value) => new(LowerBound<T>.Inclusive(value), UpperBound<T>.Inclusive(value));

        /// <summary>
        /// The lower bound of the interval.
        /// </summary>
        public readonly LowerBound<T> Lower;

        /// <summary>
        /// The upper bound of the interval.
        /// </summary>
        public readonly UpperBound<T> Upper;

        /// <summary>
        /// Initializes a new instance of the <see cref="Interval{T}"/> struct.
        /// </summary>
        /// <param name="lower">The lower bound of the interval.</param>
        /// <param name="upper">The upper bound of the interval.</param>
        public Interval(LowerBound<T> lower, UpperBound<T> upper)
        {
            this.Lower = lower;
            this.Upper = upper;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Interval<T> iv && this.Equals(iv);

        /// <inheritdoc/>
        public bool Equals(Interval<T> other) => this.Equals(other, Comparer<T>.Default);

        /// <summary>
        /// Checks if this interval equals another one.
        /// </summary>
        /// <param name="other">The other interval to compare.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True, if the two intervals are equal.</returns>
        public bool Equals(Interval<T> other, IComparer<T> comparer) =>
               this.Lower.CompareTo(other.Lower, comparer) == 0
            && this.Upper.CompareTo(other.Upper, comparer) == 0;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.Lower, this.Upper);

        /// <summary>
        /// Checks if a value is inside this interval.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True, if the value is inside this interval, false otherwise.</returns>
        public bool Contains(T value) => this.Contains(value, Comparer<T>.Default);

        /// <summary>
        /// Checks if a value is inside this interval.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True, if the value is inside this interval, false otherwise.</returns>
        public bool Contains(T value, IComparer<T> comparer) => (this.Lower.Type, this.Upper.Type) switch
        {
            (BoundType.Unbounded, BoundType.Unbounded) => true,

            (BoundType.Unbounded, BoundType.Exclusive) => comparer.Compare(value, this.Upper.Value) < 0,
            (BoundType.Unbounded, BoundType.Inclusive) => comparer.Compare(value, this.Upper.Value) <= 0,

            (BoundType.Exclusive, BoundType.Unbounded) => comparer.Compare(this.Lower.Value, value) < 0,
            (BoundType.Inclusive, BoundType.Unbounded) => comparer.Compare(this.Lower.Value, value) <= 0,

            (BoundType.Exclusive, BoundType.Exclusive) => comparer.Compare(this.Lower.Value, value) < 0 && comparer.Compare(value, this.Lower.Value) < 0,
            (BoundType.Exclusive, BoundType.Inclusive) => comparer.Compare(this.Lower.Value, value) < 0 && comparer.Compare(value, this.Lower.Value) <= 0,
            (BoundType.Inclusive, BoundType.Exclusive) => comparer.Compare(this.Lower.Value, value) <= 0 && comparer.Compare(value, this.Lower.Value) < 0,
            (BoundType.Inclusive, BoundType.Inclusive) => comparer.Compare(this.Lower.Value, value) <= 0 && comparer.Compare(value, this.Lower.Value) <= 0,

            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Checks if this interval is empty, meaning that it cannot possibly contain any value.
        /// </summary>
        /// <returns>True, if the interval is empty, false otherwise.</returns>
        public bool IsEmpty() => this.IsEmpty(Comparer<T>.Default);

        /// <summary>
        /// Checks if this interval is empty, meaning that it cannot possibly contain any value.
        /// </summary>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True, if the interval is empty, false otherwise.</returns>
        public bool IsEmpty(IComparer<T> comparer) => (this.Lower.Type, this.Upper.Type) switch
        {
            (BoundType.Inclusive, BoundType.Exclusive)
         or (BoundType.Exclusive, BoundType.Inclusive)
         or (BoundType.Exclusive, BoundType.Exclusive) => comparer.Compare(this.Lower.Value, this.Upper.Value) >= 0,

            _ => false,
        };

        /// <summary>
        /// Checks if this interval is before another one (no overlap).
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>True, if this interval is before the other one, false otherwise.</returns>
        public bool IsBefore(Interval<T> other) => this.IsBefore(other, Comparer<T>.Default);

        /// <summary>
        /// Checks if this interval is before another one (no overlap).
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True, if this interval is before the other one, false otherwise.</returns>
        public bool IsBefore(Interval<T> other, IComparer<T> comparer) => this.Upper.CompareTo(other.Lower, comparer) < 0;

        /// <summary>
        /// Checks if this interval is disjunct with another one.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>True, if this interval is disjunct with the other one, false otherwise.</returns>
        public bool IsDisjunct(Interval<T> other) => this.IsDisjunct(other, Comparer<T>.Default);

        /// <summary>
        /// Checks if this interval is disjunct with another one.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True, if this interval is disjunct with the other one, false otherwise.</returns>
        public bool IsDisjunct(Interval<T> other, IComparer<T> comparer) => this.IsBefore(other, comparer) || other.IsBefore(this, comparer);

        /// <summary>
        /// Calculates the relation between this and another interval.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>The object that describes the relation of the intervals.</returns>
        public IntervalRelation<T> RelationTo(Interval<T> other) => this.RelationTo(other, Comparer<T>.Default);

        /// <summary>
        /// Calculates the relation between this and another interval.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>The object that describes the relation of the intervals.</returns>
        public IntervalRelation<T> RelationTo(Interval<T> other, IComparer<T> comparer)
        {
            var (first, second) = this.Lower.CompareTo(other.Lower, comparer) < 0 ? (this, other) : (other, this);

            if (first.Upper.IsTouching(second.Lower, comparer)) return new IntervalRelation<T>.Touching(first, second);
            if (first.Upper.CompareTo(second.Lower, comparer) < 0) return new IntervalRelation<T>.Disjunct(first, second);
            if (first.Equals(second, comparer)) return new IntervalRelation<T>.Equal(first);
            var upperCmp = first.Upper.CompareTo(second.Upper, comparer);
            if (first.Lower.CompareTo(second.Lower, comparer) == 0)
            {
                // Starting relation, depends on which ends first
                var (a, b) = upperCmp < 0 ? (first, second) : (second, first);
                return new IntervalRelation<T>.Starting(a, new Interval<T>(a.Upper.GetTouching()!.Value, b.Upper));
            }
            if (upperCmp == 0)
            {
                return new IntervalRelation<T>.Finishing(new Interval<T>(first.Lower, second.Lower.GetTouching()!.Value), second);
            }
            if (upperCmp > 0)
            {
                return new IntervalRelation<T>.Containing(
                    new Interval<T>(first.Lower, second.Lower.GetTouching()!.Value),
                    second,
                    new Interval<T>(second.Upper.GetTouching()!.Value, first.Upper));
            }
            return new IntervalRelation<T>.Overlapping(
                new Interval<T>(first.Lower, second.Lower.GetTouching()!.Value),
                new Interval<T>(second.Lower, first.Upper),
                new Interval<T>(first.Upper.GetTouching()!.Value, second.Upper));
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var lower = this.Lower.Type switch
            {
                BoundType.Unbounded => "(-infty",
                BoundType.Exclusive => $"({this.Lower.Value}",
                BoundType.Inclusive => $"[{this.Lower.Value}",
                _ => throw new InvalidOperationException(),
            };
            var upper = this.Upper.Type switch
            {
                BoundType.Unbounded => "+infty)",
                BoundType.Exclusive => $"{this.Upper.Value})",
                BoundType.Inclusive => $"{this.Upper.Value}]",
                _ => throw new InvalidOperationException(),
            };
            return $"{lower}; {upper}";
        }

        /// <summary>
        /// Compares two <see cref="Interval{T}"/>s for equality.
        /// </summary>
        /// <param name="a">The first <see cref="Interval{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="Interval{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> and <paramref name="b"/> are equal.</returns>
        public static bool operator ==(Interval<T> a, Interval<T> b) => a.Equals(b);

        /// <summary>
        /// Compares two <see cref="Interval{T}"/>s for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="Interval{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="Interval{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> and <paramref name="b"/> are not equal.</returns>
        public static bool operator !=(Interval<T> a, Interval<T> b) => !a.Equals(b);
    }
}

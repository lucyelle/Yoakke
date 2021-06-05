﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Yoakke.Collections.Compatibility;

namespace Yoakke.Collections.Intervals
{
    /// <summary>
    /// Represents the lower bound of an interval.
    /// </summary>
    /// <typeparam name="T">The type of the interval values.</typeparam>
    public readonly struct LowerBound<T> : IEquatable<LowerBound<T>>, IComparable<LowerBound<T>>, IComparable<UpperBound<T>>
    {
        /// <summary>
        /// Instantiates a new unbounded lower bound.
        /// </summary>
        /// <returns>The instantiated bound.</returns>
        public static LowerBound<T> Unbounded() => new(BoundType.Unbounded, default);
        /// <summary>
        /// Instantiates a new inclusive lower bound.
        /// </summary>
        /// <param name="value">The value that is included.</param>
        /// <returns>The instantiated bound.</returns>
        public static LowerBound<T> Inclusive(T value) => new(BoundType.Inclusive, value);
        /// <summary>
        /// Instantiates a new exclusive lower bound.
        /// </summary>
        /// <param name="value">The value that is excluded.</param>
        /// <returns>The instantiated bound.</returns>
        public static LowerBound<T> Exclusive(T value) => new(BoundType.Exclusive, value);

        /// <summary>
        /// The type of this bound.
        /// </summary>
        public BoundType Type => bound.Type;
        /// <summary>
        /// The associated value of this bound. Only valid when the bound is not unbounded.
        /// </summary>
        public T Value => bound.Value;

        private readonly Bound<T> bound;

        private LowerBound(BoundType type, T? value)
        {
            bound = new Bound<T>(type, value);
        }

        /// <summary>
        /// Checks, if this is an unbounded interval bound.
        /// </summary>
        /// <returns>True, if this is an unbounded interval bound, false otherwise.</returns>
        public bool IsUnbounded() => bound.IsUnbounded();
        /// <summary>
        /// Checks, if this is an interval bound that includes it's associated value.
        /// </summary>
        /// <param name="value">The intervals associated value is written here, if this is an inclusive bound.</param>
        /// <returns>True, if this is an inclusive bound.</returns>
        public bool IsInclusive([MaybeNullWhen(false)] out T? value) => bound.IsInclusive(out value);
        /// <summary>
        /// Checks, if this is an interval bound that excludes it's associated value.
        /// </summary>
        /// <param name="value">The intervals associated value is written here, if this is an exclusive bound.</param>
        /// <returns>True, if this is an exclusive bound.</returns>
        public bool IsExclusive([MaybeNullWhen(false)] out T? value) => bound.IsExclusive(out value);

        /// <summary>
        /// Checks if this lower bound is touching an <see cref="UpperBound{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="UpperBound{T}"/> to check touch with.</param>
        /// <returns>True, if the bounds are touching.</returns>
        public bool IsTouching(UpperBound<T> other) => IsTouching(other, Comparer<T>.Default);

        /// <summary>
        /// Checks if this lower bound is touching an <see cref="UpperBound{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="UpperBound{T}"/> to check touch with.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True, if the bounds are touching.</returns>
        public bool IsTouching(UpperBound<T> other, IComparer<T> comparer) => (Type, other.Type) switch
        {
            (BoundType.Exclusive, BoundType.Inclusive)
         or (BoundType.Inclusive, BoundType.Exclusive) => comparer.Compare(Value, other.Value) == 0,

            _ => false,
        };

        /// <summary>
        /// Constructs an <see cref="UpperBound{T}"/> that is touching this one.
        /// </summary>
        /// <returns>The constructed <see cref="UpperBound{T}"/>.</returns>
        public UpperBound<T>? GetTouching() => Type switch
        {
            BoundType.Exclusive => UpperBound<T>.Inclusive(Value),
            BoundType.Inclusive => UpperBound<T>.Exclusive(Value),
            _ => null,
        };

        public override bool Equals(object obj) => obj is LowerBound<T> lb && Equals(lb);
        public override int GetHashCode() => HashCode.Combine(Type, Value);

        public bool Equals(LowerBound<T> other) => CompareTo(other) == 0;

        /// <summary>
        /// Compares this bound to another one using the default comparer.
        /// </summary>
        /// <param name="other">The other bound to compare.</param>
        /// <returns>See the documentation for <see cref="IComparer{T}"/>.</returns>
        public int CompareTo(LowerBound<T> other) => CompareTo(other, Comparer<T>.Default);

        /// <summary>
        /// Compares this bound to another one using the default comparer.
        /// </summary>
        /// <param name="other">The other bound to compare.</param>
        /// <returns>See the documentation for <see cref="IComparer{T}"/>.</returns>
        public int CompareTo(UpperBound<T> other) => CompareTo(other, Comparer<T>.Default);

        /// <summary>
        /// Compares this bound to another one.
        /// </summary>
        /// <param name="other">The other bound to compare.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>See the documentation for <see cref="IComparer{T}"/>.</returns>
        public int CompareTo(LowerBound<T> other, IComparer<T> comparer) => (Type, other.Type) switch
        {
            (BoundType.Unbounded, BoundType.Unbounded) => 0,
            (BoundType.Unbounded, _) => -1,
            (_, BoundType.Unbounded) => 1,

            (BoundType.Exclusive, BoundType.Exclusive)
         or (BoundType.Inclusive, BoundType.Inclusive) => comparer.Compare(Value, other.Value),

            (BoundType.Inclusive, BoundType.Exclusive) => comparer.Compare(Value, other.Value) switch
            {
                0 => -1,
                int o => o,
            },

            (BoundType.Exclusive, BoundType.Inclusive) => comparer.Compare(Value, other.Value) switch
            {
                0 => 1,
                int o => o,
            },

            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Compares this bound to another one.
        /// </summary>
        /// <param name="other">The other bound to compare.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>See the documentation for <see cref="IComparer{T}"/>.</returns>
        public int CompareTo(UpperBound<T> other, IComparer<T> comparer) => (Type, other.Type) switch
        {
            (BoundType.Unbounded, BoundType.Unbounded)
         or (BoundType.Unbounded, _)
         or (_, BoundType.Unbounded) => -1,

            (BoundType.Exclusive, BoundType.Exclusive)
         or (BoundType.Exclusive, BoundType.Inclusive)
         or (BoundType.Inclusive, BoundType.Exclusive) => comparer.Compare(Value, other.Value) switch
         {
             0 => 1,
             int o => o,
         },

            (BoundType.Inclusive, BoundType.Inclusive) => comparer.Compare(Value, other.Value) switch
            {
                0 => -1,
                int o => o,
            },

            _ => throw new InvalidOperationException(),
        };

        public static bool operator <(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) < 0;
        public static bool operator >(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) > 0;
        public static bool operator <=(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) <= 0;
        public static bool operator >=(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) >= 0;
        public static bool operator ==(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) == 0;
        public static bool operator !=(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) != 0;

        public static bool operator <(LowerBound<T> a, UpperBound<T> b) => a.CompareTo(b) < 0;
        public static bool operator >(LowerBound<T> a, UpperBound<T> b) => a.CompareTo(b) > 0;
    }
}

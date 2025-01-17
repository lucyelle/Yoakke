// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Yoakke.Utilities.Compatibility;

namespace Yoakke.Utilities.Intervals
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
        public BoundType Type => this.bound.Type;

        /// <summary>
        /// The associated value of this bound. Only valid when the bound is not unbounded.
        /// </summary>
        public T Value => this.bound.Value;

        private readonly Bound<T> bound;

        private LowerBound(BoundType type, T? value)
        {
            this.bound = new Bound<T>(type, value);
        }

        /// <summary>
        /// Checks, if this is an unbounded interval bound.
        /// </summary>
        /// <returns>True, if this is an unbounded interval bound, false otherwise.</returns>
        public bool IsUnbounded() => this.bound.IsUnbounded();

        /// <summary>
        /// Checks, if this is an interval bound that includes it's associated value.
        /// </summary>
        /// <param name="value">The intervals associated value is written here, if this is an inclusive bound.</param>
        /// <returns>True, if this is an inclusive bound.</returns>
        public bool IsInclusive([MaybeNullWhen(false)] out T? value) => this.bound.IsInclusive(out value);

        /// <summary>
        /// Checks, if this is an interval bound that excludes it's associated value.
        /// </summary>
        /// <param name="value">The intervals associated value is written here, if this is an exclusive bound.</param>
        /// <returns>True, if this is an exclusive bound.</returns>
        public bool IsExclusive([MaybeNullWhen(false)] out T? value) => this.bound.IsExclusive(out value);

        /// <summary>
        /// Checks if this lower bound is touching an <see cref="UpperBound{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="UpperBound{T}"/> to check touch with.</param>
        /// <returns>True, if the bounds are touching.</returns>
        public bool IsTouching(UpperBound<T> other) => this.IsTouching(other, Comparer<T>.Default);

        /// <summary>
        /// Checks if this lower bound is touching an <see cref="UpperBound{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="UpperBound{T}"/> to check touch with.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True, if the bounds are touching.</returns>
        public bool IsTouching(UpperBound<T> other, IComparer<T> comparer) => (this.Type, other.Type) switch
        {
            (BoundType.Exclusive, BoundType.Inclusive)
         or (BoundType.Inclusive, BoundType.Exclusive) => comparer.Compare(this.Value, other.Value) == 0,

            _ => false,
        };

        /// <summary>
        /// Constructs an <see cref="UpperBound{T}"/> that is touching this one.
        /// </summary>
        /// <returns>The constructed <see cref="UpperBound{T}"/>.</returns>
        public UpperBound<T>? GetTouching() => this.Type switch
        {
            BoundType.Exclusive => UpperBound<T>.Inclusive(this.Value),
            BoundType.Inclusive => UpperBound<T>.Exclusive(this.Value),
            _ => null,
        };

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LowerBound<T> lb && this.Equals(lb);

        /// <inheritdoc/>
        public bool Equals(LowerBound<T> other) => this.CompareTo(other) == 0;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.Type, this.Value);

        /// <summary>
        /// Compares this bound to another one using the default comparer.
        /// </summary>
        /// <param name="other">The other bound to compare.</param>
        /// <returns>See the documentation for <see cref="IComparer{T}"/>.</returns>
        public int CompareTo(LowerBound<T> other) => this.CompareTo(other, Comparer<T>.Default);

        /// <summary>
        /// Compares this bound to another one using the default comparer.
        /// </summary>
        /// <param name="other">The other bound to compare.</param>
        /// <returns>See the documentation for <see cref="IComparer{T}"/>.</returns>
        public int CompareTo(UpperBound<T> other) => this.CompareTo(other, Comparer<T>.Default);

        /// <summary>
        /// Compares this bound to another one.
        /// </summary>
        /// <param name="other">The other bound to compare.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>See the documentation for <see cref="IComparer{T}"/>.</returns>
        public int CompareTo(LowerBound<T> other, IComparer<T> comparer) => (this.Type, other.Type) switch
        {
            (BoundType.Unbounded, BoundType.Unbounded) => 0,
            (BoundType.Unbounded, _) => -1,
            (_, BoundType.Unbounded) => 1,

            (BoundType.Exclusive, BoundType.Exclusive)
         or (BoundType.Inclusive, BoundType.Inclusive) => comparer.Compare(this.Value, other.Value),

            (BoundType.Inclusive, BoundType.Exclusive) => comparer.Compare(this.Value, other.Value) switch
            {
                0 => -1,
                int o => o,
            },

            (BoundType.Exclusive, BoundType.Inclusive) => comparer.Compare(this.Value, other.Value) switch
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
        public int CompareTo(UpperBound<T> other, IComparer<T> comparer) => (this.Type, other.Type) switch
        {
            (BoundType.Unbounded, BoundType.Unbounded)
         or (BoundType.Unbounded, _)
         or (_, BoundType.Unbounded) => -1,

            (BoundType.Exclusive, BoundType.Exclusive)
         or (BoundType.Exclusive, BoundType.Inclusive)
         or (BoundType.Inclusive, BoundType.Exclusive) => comparer.Compare(this.Value, other.Value) switch
         {
             0 => 1,
             int o => o,
         },

            (BoundType.Inclusive, BoundType.Inclusive) => comparer.Compare(this.Value, other.Value) switch
            {
                0 => -1,
                int o => o,
            },

            _ => throw new InvalidOperationException(),
        };

        /// <summary>
        /// Less-than compares two <see cref="LowerBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes before <paramref name="b"/>.</returns>
        public static bool operator <(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Greater-than compares two <see cref="LowerBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes after <paramref name="b"/>.</returns>
        public static bool operator >(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Less-than or equals compares two <see cref="LowerBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes before <paramref name="b"/> or they are equal.</returns>
        public static bool operator <=(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Greater-than or equals compares two <see cref="LowerBound{T}"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes after <paramref name="b"/> or they are equal.</returns>
        public static bool operator >=(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Compares two <see cref="LowerBound{T}"/>s for equality.
        /// </summary>
        /// <param name="a">The first <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> and <paramref name="b"/> are equal.</returns>
        public static bool operator ==(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) == 0;

        /// <summary>
        /// Compares two <see cref="LowerBound{T}"/>s for inequality.
        /// </summary>
        /// <param name="a">The first <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The second <see cref="LowerBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> and <paramref name="b"/> are not equal.</returns>
        public static bool operator !=(LowerBound<T> a, LowerBound<T> b) => a.CompareTo(b) != 0;

        /// <summary>
        /// Less-than compares a <see cref="LowerBound{T}"/> with an <see cref="UpperBound{T}"/>.
        /// </summary>
        /// <param name="a">The <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The <see cref="UpperBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes before <paramref name="b"/>.</returns>
        public static bool operator <(LowerBound<T> a, UpperBound<T> b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Greater-than compares a <see cref="LowerBound{T}"/> with an <see cref="UpperBound{T}"/>.
        /// </summary>
        /// <param name="a">The <see cref="LowerBound{T}"/> to compare.</param>
        /// <param name="b">The <see cref="UpperBound{T}"/> to compare.</param>
        /// <returns>True, if <paramref name="a"/> comes after <paramref name="b"/>.</returns>
        public static bool operator >(LowerBound<T> a, UpperBound<T> b) => a.CompareTo(b) > 0;
    }
}

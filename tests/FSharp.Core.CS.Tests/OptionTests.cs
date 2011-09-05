﻿using System;
using System.Globalization;
using Microsoft.FSharp.Core;
using NUnit.Framework;

namespace FSharp.Core.CS.Tests {
    [TestFixture]
    public class OptionTests {
        [Test]
        public void HasValue() {
            var a = FSharpOption<int>.Some(5);
            Assert.IsTrue(a.HasValue());
        }

        [Test]
        public void NotHasValue() {
            var a = FSharpOption<int>.None;
            Assert.IsFalse(a.HasValue());
        }

        [Test]
        public void FromNullable_WithValue() {
            int? a = 99;
            var o = a.ToFSharpOption();
            Assert.AreEqual(99, o.Value);
        }

        [Test]
        public void FromNullable_WithoutValue() {
            int? a = null;
            var o = a.ToFSharpOption();
            Assert.False(o.HasValue());
        }

        [Test]
        public void Some() {
            var a = FSharpOption.Some(5);
            Assert.AreEqual(5, a.Value);
        }

        [Test]
        public void SomeExtensionMethod() {
            var a = 5.Some();
            Assert.AreEqual(5, a.Value);
        }

        [Test]
        public void LINQ_Select() {
            var a = from i in 5.Some() select i;
            Assert.AreEqual(5, a.Value);
        }

        [Test]
        public void LINQ_SelectMany() {
            var a = 5.Some();
            var b = 6.Some();
            var c = a.SelectMany(x => b.SelectMany(y => (x+y).Some()));
            Assert.AreEqual(11, c.Value);
        }

        [Test]
        public void LINQ_SelectMany2() {
            var a = 5.Some();
            var b = 6.Some();
            var c = from x in a
                    from y in b
                    select x + y;
            Assert.AreEqual(11, c.Value);
        }

        [Test]
        public void LINQ_SelectMany2_with_none() {
            var a = 5.Some();
            var c = from x in a
                    from y in FSharpOption<int>.None
                    select x + y;
            Assert.IsFalse(c.HasValue());
        }

        [Test]
        public void LINQ_Aggregate() {
            var a = 5.Some();
            var c = a.Aggregate(10, (s, x) => s + x);
            Assert.AreEqual(15, c);
        }

        [Test]
        public void LINQ_Aggregate_None() {
            var a = FSharpOption<int>.None;
            var c = a.Aggregate(10, (s, x) => s + x);
            Assert.AreEqual(10, c);
        }

        [Test]
        public void LINQ_Where() {
            var a = 5.Some();
            var c = from i in a where i > 7 select i;
            Assert.IsFalse(c.HasValue());
        }

        [Test]
        public void LINQ_Where2() {
            var a = 5.Some();
            var c = from i in a where i > 3 select i;
            Assert.AreEqual(5, c.Value);
        }

        [Test]
        public void LINQ_Where3() {
            var a = FSharpOption<int>.None;
            var c = from i in a where i > 3 select i;
            Assert.IsFalse(c.HasValue());
        }

        [Test]
        public void Match_Some() {
            var a = 5.Some();
            var b = a.Match(x => x + 2, () => 99);
            Assert.AreEqual(7, b);
        }

        [Test]
        public void Match_None() {
            var a = FSharpOption<int>.None;
            var b = a.Match(x => x + 2, () => 99);
            Assert.AreEqual(99, b);
        }

        [Test]
        public void Match_Value() {
            var a = FSharpOption<int>.None;
            var b = a.Match(x => x + 2, 99);
            Assert.AreEqual(99, b);
        }

        [Test]
        public void Match_Action() {
            var a = 5.Some();
            a.Match(Console.WriteLine, Assert.Fail);
        }

        [Test]
        public void Do() {
            var a = 5.Some();
            a.Do(v => Assert.AreEqual(5, v));
        }

        [Test]
        public void Dont() {
            var a = FSharpOption<int>.None;
            a.Do(_ => Assert.Fail());
        }

        [Test]
        public void TryParseInt() {
            var a = FSharpOption.TryParseInt("123");
            Assert.AreEqual(123, a.Value);
        }

        [Test]
        public void TryParseDec() {
            var a = FSharpOption.TryParseDecimal("123.44", NumberStyles.Any, CultureInfo.InvariantCulture);
            Assert.AreEqual(123.44m, a.Value);
        }

        [Test]
        public void TryParseDouble() {
            FSharpOption<double> a = FSharpOption.TryParseDouble("123E12");
            Assert.AreEqual(123E12, a.Value);
        }

        [Test]
        public void TryParseFloat() {
            FSharpOption<float> a = FSharpOption.TryParseFloat("12");
            Assert.AreEqual(12, a.Value);
        }

        [Test]
        public void TryCastInt() {
            object o = 22;
            FSharpOption<int> a = FSharpOption.TryCastInt(o);
            Assert.AreEqual(22, a.Value);
        }

        [Test]
        public void TryCastInt_null() {
            object o = null;
            FSharpOption<int> a = FSharpOption.TryCastInt(o);
            Assert.IsFalse(a.HasValue());
        }

        [Test]
        public void DBNull_is_None() {
            var n = DBNull.Value;
            Assert.IsFalse(n.ToFSharpOption().HasValue());
        }

        [Test]
        public void OrElse() {
            var a = 5.Some();
            var b = a.OrElse(9.Some());
            Assert.AreEqual(5, b.Value);
        }

        [Test]
        public void OrElse_None() {
            var a = FSharpOption<int>.None;
            var b = a.OrElse(9.Some());
            Assert.AreEqual(9, b.Value);
        }

        [Test]
        public void GetOrElse() {
            var a = FSharpOption<int>.None;
            var b = a.GetOrElse(9);
            Assert.AreEqual(9, b);
        }

        [Test]
        public void GetOrElse_function() {
            var a = FSharpOption<int>.None;
            var b = a.GetOrElse(() => 9);
            Assert.AreEqual(9, b);
        }

    }
}
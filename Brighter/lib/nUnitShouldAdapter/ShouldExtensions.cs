﻿using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace nUnitShouldAdapter
{
    public static class ShouldExtensions
    {
        public static void ShouldBeNull(this object objectToCheck)
        {
            Assert.Null(objectToCheck);
        }

        public static void ShouldNotBeNull<T>(this T objectToCheck) where T : class
        {
            Assert.NotNull(objectToCheck);
        }

        public static void ShouldBeFalse(this bool condition)
        {
            Assert.False(condition);
        }

        public static void ShouldBeTrue(this bool condition)
        {
            Assert.True(condition);
        }

        public static void ShouldEqual<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldBeOfExactType<TExpectedType>(this object objectToCheck)
        {
            Assert.IsInstanceOf<TExpectedType>(objectToCheck);
        }
        public static void ShouldBeOfExactType(this object objectToCheck, Type tExpectedType)
        {
            Assert.IsInstanceOf(tExpectedType, objectToCheck);
        }
        public static void ShouldBeAssignableTo(this object objectToCheck, Type tExpectedType)
        {
            Assert.IsAssignableFrom(tExpectedType, objectToCheck);
        }

        public static void ShouldContain(this string actualString, string expectedSubString)
        {
            StringAssert.Contains(expectedSubString, actualString);
        }
        public static void ShouldContain<T>(this IEnumerable actualEnumerable, object expectedObject)
        {
            CollectionAssert.Contains(actualEnumerable, expectedObject);
        }
        public static void ShouldContain<T>(this List<T> actualEnumerable, T expectedObject)
        {
            CollectionAssert.Contains(actualEnumerable, expectedObject);
        }

        public static void ShouldBeTheSameAs<T>(this T actual, T expected)
        {
            Assert.AreSame(expected, actual);
        }
<<<<<<< HEAD

        public static void ShouldBeGreaterThan(this IComparable actual, IComparable greaterThan)
        {
            Assert.Greater(actual, greaterThan);
        } 
||||||| merged common ancestors
=======

        public static void ShouldContainErrorMessage(this Exception exception, string message)
        {
            Assert.NotNull(exception);
            ShouldContain(exception.Message, message);
        }
>>>>>>> cd065e627bf0c5384a4d34d2cf63675154330594
    }
}
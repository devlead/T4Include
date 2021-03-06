﻿// ----------------------------------------------------------------------------------------------
// Copyright (c) Mårten Rånge.
// ----------------------------------------------------------------------------------------------
// This source code is subject to terms and conditions of the Microsoft Public License. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// If you cannot locate the  Microsoft Public License, please send an email to 
// dlr@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
//  by the terms of the Microsoft Public License.
// ----------------------------------------------------------------------------------------------
// You must not remove this notice, or any other, from this software.
// ----------------------------------------------------------------------------------------------

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart

// ### INCLUDE: Generated_ObservableExtensions.cs

namespace Source.Extensions
{
    using System;

    sealed partial class EmptyObservable<T> : IObservable<T>
    {
        public readonly static EmptyObservable<T> Value = new EmptyObservable<T> (); 

        public IDisposable Subscribe (IObserver<T> observer)
        {
            return new ObservableExtensions.EmptyDisposable ();
        }
    }

    sealed partial class SingleObserver<T> : IObservable<T>
    {
        readonly T m_value;

        public SingleObserver (T value)
        {
            m_value = value;
        }

        public IDisposable Subscribe (IObserver<T> observer)
        {
            observer.OnNext (m_value);
            observer.OnCompleted ();
            return new ObservableExtensions.EmptyDisposable ();
        }
    }

    static partial class ObservableExtensions
    {
        public sealed partial class EmptyDisposable : IDisposable
        {
            public void Dispose ()
            {
            }
        }

        public static IObservable<T> Empty<T> ()
        {
            return EmptyObservable<T>.Value;
        }

        public static IObservable<T> Single<T> (T value)
        {
            return new SingleObserver<T> (value);
        }

        sealed partial class ActionObserver<T> : IObserver<T>
        {
            readonly Action<T> m_onNext;
            readonly Action m_onCompleted;
            readonly Action<Exception> m_onError;

            public ActionObserver (Action<T> onNext, Action onCompleted, Action<Exception> onError)
            {
                m_onNext        = onNext        ?? (v => {});
                m_onCompleted   = onCompleted   ?? (() => {});
                m_onError       = onError       ?? (e => {});
            }

            public void OnNext (T value)
            {
                m_onNext (value);
            }

            public void OnError (Exception error)
            {
                m_onError (error);
            }

            public void OnCompleted ()
            {
                m_onCompleted ();
            }
        }

        public static IDisposable Subscribe<T> (this IObservable<T> observable, Action<T> onNext, Action onCompleted = null, Action<Exception> onError = null)
        {
            if (observable == null)
            {
                throw new ArgumentNullException ("observable");
            }

            var observer = new ActionObserver<T> (onNext, onCompleted, onError);

            return observable.Subscribe (observer);
        }

    }

    partial class WhereObserver<T>
    {
        partial void Partial_OnNext (T value)
        {
            if (m_state (value))
            {
                m_observer.OnNext (value);
            }
        }
    }

    partial class SelectObserver<T, TTo>
    {
        partial void Partial_OnNext (T value)
        {
            m_observer.OnNext (m_state (value));
        }
    }

    partial class TakeObserver<T>
    {
        partial void Partial_OnNext (T value)
        {
            if (m_state > 0)
            {
                --m_state;
                m_observer.OnNext (value);
            }
        }
    }

    partial class SkipObserver<T>
    {
        partial void Partial_OnNext (T value)
        {
            if (m_state > 0)
            {
                --m_state;
            }
            else
            {
                m_observer.OnNext (value);
            }
        }
    }
}
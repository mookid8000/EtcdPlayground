using System;
using System.Collections.Concurrent;
using NUnit.Framework;

namespace Tests
{
    public abstract class FixtureBase
    {
        readonly ConcurrentStack<IDisposable> _disposables = new ConcurrentStack<IDisposable>();

        protected TDisposable Using<TDisposable>(TDisposable disposable) where TDisposable : IDisposable
        {
            _disposables.Push(disposable);
            return disposable;
        }

        protected void CleanUpDisposables()
        {
            IDisposable disposable;

            while (_disposables.TryPop(out disposable))
            {
                disposable.Dispose();
            }
        }

        [SetUp]
        public void InnerSetUp()
        {
            SetUp();
        }

        protected virtual void SetUp()
        {
        }

        [TearDown]
        public void InnerTearDown()
        {
            TearDown();

            CleanUpDisposables();
        }

        protected virtual void TearDown()
        {
        }
    }

}
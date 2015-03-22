using System;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Froggy.Validation.Object;
using System.Collections;

namespace Froggy.Data.Db4o
{
    public static class Db
    {
        private static IObjectContainer _Current;

        public static IObjectContainer Get
        {
            get { return _Current; }
        }

        private static IDictionary<Type, ObjectValidator> _Validators;
        static Db()
        {
            Open();
        }

        public static void Open()
        {
            Db4oFactory.Configure().Queries().EvaluationMode(QueryEvaluationMode.Snapshot);
            _Current = Db4oFactory.OpenFile("yapFile.yap");
            IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(_Current);
            eventRegistry.Creating += eventRegistry_Validate;
            eventRegistry.Updating += eventRegistry_Validate;
            eventRegistry.Committing += eventRegistry_Committing;
            _Validators = new Dictionary<Type, ObjectValidator>();
        }

        static void eventRegistry_Validate(object sender, CancellableObjectEventArgs args)
        {
            ObjectValidator ovr = GetObjectValidator(args.Object);
            bool isValid = ovr.IsValid(args.Object);
            if (!isValid)
            {
                args.Cancel();
                ovr.Validate(args.Object);
            }
        }

        static void eventRegistry_Committing(object sender, CommitEventArgs args)
        {
            ValidateIteration(args.Added);
            ValidateIteration(args.Deleted);
            ValidateIteration(args.Updated);
        }

        private static void ValidateIteration(IEnumerable objs)
        {
            foreach (var obj in objs)
            {
                GetObjectValidator(obj).Validate(obj);
            }
        }

        private static ObjectValidator GetObjectValidator(object obj)
        {
            Type objType = obj.GetType();
            if (_Validators.Keys.Contains(objType))
            {
                return _Validators[objType];
            }
            else
            {
                return AddNewObjectValidator(objType);
            }
        }

        private static ObjectValidator AddNewObjectValidator(Type objType)
        {
            lock (typeof(Db))
            {
                if (_Validators.Keys.Contains(objType))
                {
                    return _Validators[objType];
                }
                else
                {
                    ObjectValidator ovu = new ObjectValidator(objType);
                    _Validators.Add(objType, ovu);
                    return ovu;
                }
            }
        }
    }
}

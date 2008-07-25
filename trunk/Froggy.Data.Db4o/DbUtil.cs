﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Froggy.Validation.Object;
using System.Collections;
using Froggy.Validation;

namespace Froggy.Data.Db4o
{
    public static class DbUtil
    {
        private static IObjectContainer _Db;

        public static IObjectContainer Db
        {
            get { return DbUtil._Db; }
        }

        private static IDictionary<Type, ObjectValidator> _Validators;
        static DbUtil()
        {
            _Db = Db4oFactory.OpenFile("yapFile.yap");
            IEventRegistry eventRegistry = EventRegistryFactory.ForObjectContainer(_Db);
            eventRegistry.Creating += new CancellableObjectEventHandler(eventRegistry_Validate);
            eventRegistry.Updating += new CancellableObjectEventHandler(eventRegistry_Validate);
            eventRegistry.Committing += new CommitEventHandler(eventRegistry_Committing);
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
            lock (typeof(DbUtil))
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
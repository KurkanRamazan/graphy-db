﻿using System;
using System.Collections.Generic;

namespace GraphyDb
{
    public enum PropertyType
    {
        Int = 0,
        String = 1,
        Bool = 2,
        Float = 3
    }


    public abstract class Property : Entity
    {
        static readonly List<Type> SupportedTypes = new List<Type> {typeof(int), typeof(string), typeof(bool), typeof(float)};


        public int PropertyId;

        protected Entity Parent;

        public string Key;

        protected Property(Entity parent, string key, object value)
        {
            if (!SupportedTypes.Contains(value.GetType()))
            {
                throw new NotSupportedException("Cannot store properties with type " + value.GetType());
            }

            PropertyId = 0;

            Parent = parent ?? throw new ArgumentNullException(nameof(parent));

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(nameof(key) + " cannot be null or empty string");
            }
            Key = key;

            Value = value;

            Db = parent.Db;

            State |= EntityState.Added;
            Db.ChangedEntities.Add(this);
        }

        public object Value
        {
            get => value;
            set
            {
                if (!SupportedTypes.Contains(value.GetType()))
                {
                    throw new NotSupportedException("Cannot store properties with type " + value.GetType());
                }

                this.value = value;

                State |= EntityState.Modified;
                Db.ChangedEntities.Add(this);
            }
        }

        private object value;
    }


}
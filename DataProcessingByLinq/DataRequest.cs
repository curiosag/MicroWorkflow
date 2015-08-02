using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DataProcessingByLinq
{

    public abstract class Dr
    {
        public string Name { get; protected set; }
        public Guid Id { get; protected set; }
        public Provider Provider { get; protected set; }
        public string KeyTypeId { get; protected set; }

        public abstract ReadOnlyCollection<string> Keys { get; }  
        public abstract ReadOnlyCollection<string> Fields { get; }   
    }

    public class BaseDr : Dr
    {
        private ReadOnlyCollection<string> mKeys;
        private ReadOnlyCollection<string> mFields;

        public BaseDr(
            Provider provider,
            string name,     
            string keyTypeId,
            List<string> keys,
            List<string> fields)
        {
            
            Provider = provider;
            Name = name;
            KeyTypeId = keyTypeId;
            Id = Guid.NewGuid();

            mKeys = new ReadOnlyCollection<string>(keys);
            mFields = new ReadOnlyCollection<string>(fields);
        }

        public override ReadOnlyCollection<string> Keys {
            get { return mKeys; }           
        }
        public override ReadOnlyCollection<string> Fields
        {
            get { return mFields; }
        }        

    }

}

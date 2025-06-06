﻿namespace EducaMBAXpert.Core.Messages
{
    public abstract class Message
    {
        public string MessageType { get; protected set; }
        public Guid AggregateID { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;     
        }

    }
}

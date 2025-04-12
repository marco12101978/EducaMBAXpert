﻿using EducaMBAXpert.Core.Messages;

namespace EducaMBAXpert.Core.Bus
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
    }
}

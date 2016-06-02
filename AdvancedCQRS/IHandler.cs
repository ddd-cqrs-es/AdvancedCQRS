﻿namespace AdvancedCQRS
{
    public interface IHandler<T> where T : Message
    {
        void Handle(T message);
    }
}
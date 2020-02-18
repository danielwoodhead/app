﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyHealth.Observations.Models.Events.Base;

namespace MyHealth.Observations.Core.Events
{
    public class CompositeEventPublisher : IEventPublisher
    {
        private readonly IEnumerable<IEventPublisher> _eventPublishers;

        public CompositeEventPublisher(IEnumerable<IEventPublisher> eventPublishers)
        {
            _eventPublishers = eventPublishers ?? throw new ArgumentNullException(nameof(eventPublishers));
        }

        public async Task PublishAsync(DomainEvent e)
        {
            foreach (IEventPublisher eventPublisher in _eventPublishers)
            {
                await eventPublisher.PublishAsync(e);
            }
        }
    }
}
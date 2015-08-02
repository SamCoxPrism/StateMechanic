﻿using System;

namespace StateMechanic
{
    /// <summary>
    /// Event args for events indicating that the requested transition could not be found
    /// </summary>
    /// <typeparam name="TState">Type of state</typeparam>
    public class TransitionNotFoundEventArgs<TState> : EventArgs
    {
        /// <summary>
        /// Gets the state being transitioned from
        /// </summary>
        public TState From { get; private set; }

        /// <summary>
        /// Gets the event which was fired
        /// </summary>
        public IEvent Event { get; private set; }

        /// <summary>
        /// Gets the state machine associated with failed transition
        /// </summary>
        public IStateMachine StateMachine { get; private set; }

        internal TransitionNotFoundEventArgs(TState from, IEvent @event, IStateMachine stateMachine)
        {
            this.From = from;
            this.Event = @event;
            this.StateMachine = stateMachine;
        }
    }
}

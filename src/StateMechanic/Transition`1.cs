﻿using System;

namespace StateMechanic
{
    /// <summary>
    /// A transition from one state to another, triggered by an event
    /// </summary>
    /// <typeparam name="TState">Type of state which this transition is between</typeparam>
    public class Transition<TState> : ITransition<TState>, IInvokableTransition where TState : class, IState
    {
        private readonly ITransitionInner<TState, Event, TransitionInfo<TState>> innerTransition;

        /// <summary>
        /// Gets the state this transition is from
        /// </summary>
        public TState From => this.innerTransition.From;

        /// <summary>
        /// Gets the state this transition to
        /// </summary>
        public TState To => this.innerTransition.To;

        /// <summary>
        /// Gets the event which triggers this transition
        /// </summary>
        public Event Event => this.innerTransition.Event;
        IEvent ITransition<TState>.Event => this.innerTransition.Event;

        /// <summary>
        /// Gets a value indicating whether this transition is an inner transition, i.e. whether the <see cref="From"/> and <see cref="To"/> states are the same, and no exit/entry handles are invoked
        /// </summary>
        public bool IsInnerTransition => this.innerTransition.IsInnerTransition;

        /// <summary>
        /// Gets or sets a method which is invoked whenever this transition occurs
        /// </summary>
        public Action<TransitionInfo<TState>> Handler
        {
            get { return this.innerTransition.Handler; }
            set { this.innerTransition.Handler = value; }
        }

        /// <summary>
        /// Gets or sets a method which is invoked before this transition occurs, and can prevent the transition from occuring
        /// </summary>
        public Func<TransitionInfo<TState>, bool> Guard
        {
            get { return this.innerTransition.Guard; }
            set { this.innerTransition.Guard = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this transition has a guard
        /// </summary>
        public bool HasGuard => this.Guard != null;

        internal Transition(ITransitionInner<TState, Event, TransitionInfo<TState>> innerTransition)
        {
            this.innerTransition = innerTransition;
        }

        /// <summary>
        /// Sets a method which is invoked whenever this transition occurs
        /// </summary>
        /// <param name="handler">Method which is invoked whenever this transition occurs</param>
        /// <returns>This transition, for method chaining</returns>
        public Transition<TState> WithHandler(Action<TransitionInfo<TState>> handler)
        {
            this.Handler = handler;
            return this;
        }

        /// <summary>
        /// Sets a method which is invoked before this transition occurs, and can prevent the transition from occuring
        /// </summary>
        /// <param name="guard">method which is invoked before this transition occurs, and can prevent the transition from occuring</param>
        /// <returns>This transition, for method chaining</returns>
        public Transition<TState> WithGuard(Func<TransitionInfo<TState>, bool> guard)
        {
            this.Guard = guard;
            return this;
        }

        bool IInvokableTransition.TryInvoke()
        {
            var transitionInfo = new TransitionInfo<TState>(this.From, this.To, this.Event, this.IsInnerTransition);
            return this.innerTransition.TryInvoke(transitionInfo);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return String.Format("<Transition From={0} To={1} Event={2}{3}>", this.From.Name, this.To.Name, this.Event.Name, this.IsInnerTransition ? " IsInnerTransition" : "");
        }
    }
}
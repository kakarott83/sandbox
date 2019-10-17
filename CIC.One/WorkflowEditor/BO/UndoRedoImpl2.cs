using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

//Basic Functioning UNDO Implementation to replace the VS-bound mechanism in the rehosted designer
namespace Workflows.BO
{
    [Export(typeof(ITextUndoHistoryRegistry))]
    public class BasicTextUndoHistoryRegistry : ITextUndoHistoryRegistry
    {
        protected readonly Dictionary<object, ITextUndoHistory> _history = new Dictionary<object, ITextUndoHistory>();
        public void AttachHistory(object context, ITextUndoHistory history)
        {
            _history[context] = history;
        }

        public ITextUndoHistory GetHistory(object context)
        {
            ITextUndoHistory ret;
            if (_history.TryGetValue(context, out ret))
            {
                return ret;
            }
            return null;
        }

        public ITextUndoHistory RegisterHistory(object context)
        {
            var ret = GetHistory(context);
            if (ret == null)
            {
                ret = new BasicTextUndoHistory();
                _history[context] = ret;
            }
            return ret;
        }

        public void RemoveHistory(ITextUndoHistory history)
        {
            var pair = _history.FirstOrDefault(kvp => kvp.Value == history);
            _history.Remove(pair.Key);
        }

        public bool TryGetHistory(object context, out ITextUndoHistory history)
        {
            return _history.TryGetValue(context, out history);
        }
    }

    public class BasicTextUndoHistory : ITextUndoHistory
    {
        public BasicTextUndoHistory()
        {
            State = TextUndoHistoryState.Idle;
        }

        public bool CanRedo
        {
            get { return _redoStack.Count > 0; }
        }

        public bool CanUndo
        {
            get { return _undoStack.Count > 0; }
        }

        protected readonly Stack<BasicTextUndoTransaction> _currentTransactions = new Stack<BasicTextUndoTransaction>();
        public ITextUndoTransaction CreateTransaction(string description)
        {
            //the plan: 
            //if we have an open transaction undo item
            //any items that come in while he's open become his child
            //they don't go on the stack; rather, they are undone as part of that parent
            var trans = new BasicTextUndoTransaction(this, _currentTransactions.Count > 0 ? _currentTransactions.Peek() : null, description);
            _currentTransactions.Push(trans);
            return trans;
        }

        public ITextUndoTransaction CurrentTransaction
        {
            get { return _currentTransactions.Count > 0 ? _currentTransactions.Peek() : null; }
        }

        public ITextUndoTransaction LastRedoTransaction
        {
            get { return CanRedo ? _redoStack.Peek() : null; }
        }

        public ITextUndoTransaction LastUndoTransaction
        {
            get { return CanUndo ? _undoStack.Peek() : null; }
        }

        public string RedoDescription
        {
            get { return CanRedo ? _redoStack.Peek().Description : null; }
        }

        public string UndoDescription
        {
            get { return CanUndo ? _undoStack.Peek().Description : null; }
        }

        protected readonly Stack<ITextUndoTransaction> _redoStack = new Stack<ITextUndoTransaction>();
        public IEnumerable<ITextUndoTransaction> RedoStack
        {
            get { return _redoStack; }
        }

        protected readonly Stack<ITextUndoTransaction> _undoStack = new Stack<ITextUndoTransaction>();
        public IEnumerable<ITextUndoTransaction> UndoStack
        {
            get { return _undoStack; }
        }

        public TextUndoHistoryState State { get; protected set; }

        public void Undo(int count)
        {
            State = TextUndoHistoryState.Undoing;
            try
            {
                while (count > 0 && _undoStack.Count > 0)
                {
                    var pop = _undoStack.Pop();
                    pop.Undo();
                    _redoStack.Push(pop);
                    count--;
                    UndoRedoHappened.Invoke(this, new TextUndoRedoEventArgs(State, pop));
                }
            }
            finally
            {
                State = TextUndoHistoryState.Idle;
            }
        }

        public void Redo(int count)
        {
            State = TextUndoHistoryState.Redoing;
            try
            {
                while (count > 0 && _redoStack.Count > 0)
                {
                    var pop = _redoStack.Pop();
                    pop.Do();
                    _undoStack.Push(pop);
                    count--;
                    UndoRedoHappened.Invoke(this, new TextUndoRedoEventArgs(State, pop));
                }
            }
            finally
            {
                State = TextUndoHistoryState.Idle;
            }
        }

        // Notifies consumers when an undo or a redo has happened on this history.
        public event EventHandler<TextUndoRedoEventArgs> UndoRedoHappened = delegate { };

        // Notifies consumers when an ITextUndoTransaction is completed and added to the UndoStack.
        public event EventHandler<TextUndoTransactionCompletedEventArgs> UndoTransactionCompleted = delegate { };

        protected readonly PropertyCollection _props = new PropertyCollection(); // MS does it this way; they leave it empty
        public PropertyCollection Properties
        {
            get { return _props; }
        }

        protected sealed class BasicTextUndoTransaction : ITextUndoTransaction
        {
            private readonly BasicTextUndoHistory _history;
            private readonly BasicTextUndoTransaction _parent;
            public BasicTextUndoTransaction(BasicTextUndoHistory history, BasicTextUndoTransaction parent, string description)
            {
                _history = history;
                _parent = parent;
                Description = description;
                State = UndoTransactionState.Open;
                Debug.WriteLine("In the open, hash: " + GetHashCode());
            }

            private readonly List<ITextUndoTransaction> _children = new List<ITextUndoTransaction>();
            internal void AddChild(ITextUndoTransaction child)
            {
                _children.Add(child);
            }

            private readonly List<ITextUndoPrimitive> _undoPrims = new List<ITextUndoPrimitive>();
            public void AddUndo(ITextUndoPrimitive undo)
            {
                Debug.WriteLine("In the AddUndo, " + GetDesc());
                if (State != UndoTransactionState.Open)
                {
                    if (_undoPrims.Count > 0 && undo.CanMerge(_undoPrims.Last()))
                    {
                        _undoPrims[_undoPrims.Count - 1] = undo.Merge(_undoPrims.Last());
                        return;
                    }
                    //throw new InvalidOperationException("This transaction is no longer open.");
                    Debug.WriteLine("We thought we should be open, " + GetDesc());
                }
                _undoPrims.Add(undo);
            }

            public bool CanRedo
            {
                get { return State == UndoTransactionState.Undone; }
            }

            public bool CanUndo
            {
                get { return State == UndoTransactionState.Completed; }
            }

            private string GetDesc()
            {
                return string.Format("Desc: {0}, Hash: {1}, Prims: {2}, State: {3}, ParentHash: {4}, Merge: {5}", Description, GetHashCode(), _undoPrims.Count, State, _parent != null ? _parent.GetHashCode() : -1, MergePolicy);
            }

            public void Cancel()
            {
                Debug.WriteLine("In the Cancel, " + GetDesc());
                if (CanUndo)
                {
                    Undo();
                }
                _undoPrims.Clear();
                // if we're a child remove ourselves from the parent
                // else set the history's current to null
                if (_parent != null)
                {
                    _parent._children.Remove(this);
                }
                else
                {
                    _history._currentTransactions.Pop();
                }
                State = UndoTransactionState.Canceled;
            }

            public void Complete()
            {
                Debug.WriteLine("In the Complete, " + GetDesc());
                // if we're a barrier we need to clear everything out
                // if we're a child, we can attempt to merge with the previous child
                // if we're not a child, we can attempt to merge with the previous undo item (as we'll be clearing the redo stack to add this new undo item)
                if (_undoPrims.Count == 1 && _undoPrims[0].GetType().Name.EndsWith("UndoBarrierPrimitive"))
                {
                    // clear the queue
                    _history._undoStack.Clear();
                }
                else if (_parent != null)
                {
                    // we're a child
                    var sibling = _parent._children.LastOrDefault();
                    if (sibling != null && MergePolicy != null && sibling.MergePolicy != null && MergePolicy.TestCompatiblePolicy(sibling.MergePolicy) && MergePolicy.CanMerge(this, sibling))
                    {
                        MergePolicy.PerformTransactionMerge(sibling, this);
                        _history.UndoTransactionCompleted.Invoke(this, new TextUndoTransactionCompletedEventArgs(this, TextUndoTransactionCompletionResult.TransactionMerged));
                    }
                    else
                    {
                        _parent._children.Add(this);
                        _history.UndoTransactionCompleted.Invoke(this, new TextUndoTransactionCompletedEventArgs(this, TextUndoTransactionCompletionResult.TransactionAdded));
                    }
                }
                else
                {
                    // we're a parent
                    _history._undoStack.Push(this);
                    _history.UndoTransactionCompleted.Invoke(this, new TextUndoTransactionCompletedEventArgs(this, TextUndoTransactionCompletionResult.TransactionAdded));
                }
                _history._currentTransactions.Pop();
                _history._redoStack.Clear();
                State = UndoTransactionState.Completed;
            }

            public string Description { get; set; }

            public void Do()
            {
                if (State == UndoTransactionState.Completed)
                {
                    Debug.WriteLine("Invalid redo state, " + GetDesc());
                    return;
                }

                Debug.WriteLine("In the Do, " + GetDesc());
                State = UndoTransactionState.Redoing;
                foreach (var child in _children)
                {
                    child.Do();
                }
                foreach (var redo in _undoPrims)
                {
                    if (redo.CanRedo)
                    {
                        redo.Do();
                    }
                }
                State = UndoTransactionState.Completed;
            }

            public ITextUndoHistory History { get { return _history; } }

            public IMergeTextUndoTransactionPolicy MergePolicy { get; set; }

            public ITextUndoTransaction Parent { get { return _parent; } }

            public UndoTransactionState State { get; private set; }

            public void Undo()
            {
                if (State != UndoTransactionState.Completed)
                {
                    Debug.WriteLine("Invalid undo state, " + GetDesc());
                    return;
                }

                Debug.WriteLine("In the Undo, " + GetDesc());
                State = UndoTransactionState.Undoing;
                for (int i = _children.Count - 1; i >= 0; i--)
                {
                    _children[i].Undo();
                }
                for (int i = _undoPrims.Count - 1; i >= 0; i--)
                {
                    if (_undoPrims[i].CanUndo)
                    {
                        _undoPrims[i].Undo();
                    }
                }
                State = UndoTransactionState.Undone;
            }

            public IList<ITextUndoPrimitive> UndoPrimitives
            {
                get { return _undoPrims; }
            }

            public void Dispose()
            {
                if (State == UndoTransactionState.Open)
                {
                    Cancel();
                }
            }
        }

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
//Non-Functioning UNDO Implementation to replace the VS-bound mechanism in the rehosted designer
namespace Workflows.BO
{
   // [Export(typeof(ITextUndoHistoryRegistry))]
    public class MyTextUndoHistoryRegistry : ITextUndoHistoryRegistry
    {
        public void AttachHistory(object context, ITextUndoHistory history)
        {
            while (false) ;
        }

        public ITextUndoHistory GetHistory(object context)
        {
            return new MyTextUndoHistory();
        }

        public ITextUndoHistory RegisterHistory(object context)
        {
            return new MyTextUndoHistory();
        }

        public void RemoveHistory(ITextUndoHistory history)
        {
            while (false) ;
        }

        public bool TryGetHistory(object context, out ITextUndoHistory history)
        {
            history = new MyTextUndoHistory();
            return false;
        }
    }

    public class MyTextUndoHistory : ITextUndoHistory
    {
        public bool CanRedo
        {
            get { return false; }
        }

        public bool CanUndo
        {
            get { return false; }
        }

        public ITextUndoTransaction CreateTransaction(string description)
        {
            return new MyTextUndoTransaction(this);
        }

        public ITextUndoTransaction CurrentTransaction
        {
            get { return new MyTextUndoTransaction(this); }
        }

        public ITextUndoTransaction LastRedoTransaction
        {
            get { return new MyTextUndoTransaction(this); }
        }

        public ITextUndoTransaction LastUndoTransaction
        {
            get { return new MyTextUndoTransaction(this); }
        }

        public void Redo(int count)
        {
            while (false) ;
        }

        public string RedoDescription
        {
            get { return ""; }
        }

        public IEnumerable<ITextUndoTransaction> RedoStack
        {
            get { return new List<MyTextUndoTransaction>(); }
        }

        public TextUndoHistoryState State
        {
            get { return TextUndoHistoryState.Idle; }
        }

        public void Undo(int count)
        {
            while (false) ;
        }

        public string UndoDescription
        {
            get { return ""; }
        }

        public event EventHandler<TextUndoRedoEventArgs> UndoRedoHappened;

        public IEnumerable<ITextUndoTransaction> UndoStack
        {
            get { return new List<MyTextUndoTransaction>(); }
        }

        public event EventHandler<TextUndoTransactionCompletedEventArgs> UndoTransactionCompleted;

        public Microsoft.VisualStudio.Utilities.PropertyCollection Properties
        {
            get { return new Microsoft.VisualStudio.Utilities.PropertyCollection(); }
        }
    }


    public class MyTextUndoTransaction : ITextUndoTransaction
    {
        ITextUndoHistory _history;
        public MyTextUndoTransaction(ITextUndoHistory history)
        {
            _history = history;
        }
        public void AddUndo(ITextUndoPrimitive undo)
        {
            while (false) ;
        }

        public bool CanRedo
        {
            get { return false; }
        }

        public bool CanUndo
        {
            get { return false; }
        }

        public void Cancel()
        {
            while (false) ;
        }

        public void Complete()
        {
            while (false) ;
        }

        public string Description
        {
            get
            {
                return "";
            }
            set
            {
                while (false) ;
            }
        }

        public void Do()
        {
            while (false) ;
        }

        public ITextUndoHistory History
        {
            get { return _history; }
        }

        public IMergeTextUndoTransactionPolicy MergePolicy
        {
            get
            {
                return new MyMergeTextUndoTransactionPolicy();
            }
            set
            {
                while (false) ;
            }
        }

        public ITextUndoTransaction Parent
        {
            get { return new MyTextUndoTransaction(_history); }
        }

        public UndoTransactionState State
        {
            get { return UndoTransactionState.Invalid; }
        }

        public void Undo()
        {
            while (false) ;
        }

        public IList<ITextUndoPrimitive> UndoPrimitives
        {
            get { return new List<ITextUndoPrimitive>(); }
        }

        public void Dispose()
        {
            while (false) ;
        }
    }

    public class MyTextUndoPrimitive : ITextUndoPrimitive
    {
        public bool CanMerge(ITextUndoPrimitive older)
        {
            return false;
        }

        public bool CanRedo
        {
            get { return false; }
        }

        public bool CanUndo
        {
            get { return false; }
        }

        public void Do()
        {
            while (false) ;
        }

        public ITextUndoPrimitive Merge(ITextUndoPrimitive older)
        {
            return older;
        }

        public ITextUndoTransaction Parent
        {
            get
            {
                return null;
            }
            set
            {
                while (false) ;
            }
        }

        public void Undo()
        {
            while (false) ;
        }
    }


    public class MyMergeTextUndoTransactionPolicy : IMergeTextUndoTransactionPolicy
    {
        public bool CanMerge(ITextUndoTransaction newerTransaction, ITextUndoTransaction olderTransaction)
        {
            return false;
        }

        public void PerformTransactionMerge(ITextUndoTransaction existingTransaction, ITextUndoTransaction newTransaction)
        {
            while (false) ;
        }

        public bool TestCompatiblePolicy(IMergeTextUndoTransactionPolicy other)
        {
            return true;
        }
    }
}

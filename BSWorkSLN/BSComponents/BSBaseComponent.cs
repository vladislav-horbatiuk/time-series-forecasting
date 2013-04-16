using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Drawing;
using BSWork.DataObjects;

namespace BSWork
{
    namespace BSComponents
    {
        public delegate void ComponentFinishedDelegate(BSBaseComponent iComp);

        public abstract class BSBaseComponent
        {
            public BSBaseComponent(Rectangle iOnScreenRectangle)
            {
                OnScreenRectangle = iOnScreenRectangle;
                Inputs = new ConcurrentStack<BSDataObject>();
            }

            public BSBaseComponent()
            {
                OnScreenRectangle = new Rectangle();
                Inputs = new ConcurrentStack<BSDataObject>();
            }

            public void CheckIfInputsAreProvided()
            {
                if (Inputs == null || Inputs.Count == 0)
                    throw new NoInputProvidenException();
            }

            public virtual Rectangle OnScreenRectangle { get; set; }
            public abstract string ComponentType { get; }
            public abstract void ShowSettingsForm();

            public abstract string WhatsWrong();

            public abstract bool IsReady();

            public abstract void Start();

            public virtual ConcurrentStack<BSDataObject> Inputs { get; set; }
            public virtual BSDataObject Output { get; set; }

            public virtual bool ContainsPoint(Point iPoint)
            {
                return OnScreenRectangle.Contains(iPoint);
            }
        }

        public abstract class UserException : Exception
        {
            public UserException(string iMessage)
                : base(iMessage)
            {
            }

            public UserException()
                : base()
            {
            }
        }

        public class NoInputProvidenException : UserException
        {
        }

        public class IncorrectInputs : UserException
        {
        }
    }
}

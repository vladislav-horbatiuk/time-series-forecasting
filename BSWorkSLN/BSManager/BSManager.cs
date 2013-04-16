using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using BSWork.BSComponents;
using BSWork.DataObjects;

namespace BSWork
{
    /// <summary>
    /// An interface between BS application's logic layer and UI layer.
    /// </summary>
    public sealed class BSManager
    {

        #region TODO_SECTION

        //TODO: Implement correct behaviour (i.e. check the component's type, check previously asked component)
        public static bool ExistsSuitableOutputComponentAtPoint(Point iPoint)
        {
            return ExistsComponentAtPoint(iPoint);
        }

        //TODO: Implement correct behaviour (i.e. check the component's type, check previously asked component)
        public static bool ExistsSuitableInputComponentAtPoint(Point iPoint)
        {
            return ExistsComponentAtPoint(iPoint);
        }

        /// <summary>
        /// Tries to link two components, located at <paramref name="iSourceCompPoint"/> and <paramref name="iDestCompPoint"/> points.
        /// Raises ComponentsCantBeLinkedException in this cases:
        /// - components have been linked before;
        /// - component at <paramref name="iSourceCompPoint"/> isn't an input providing component, or component at <paramref name="iDestCompPoint"/>;
        /// - it's actually the same component.
        ///   isn't an input consuming component.
        /// Raises ArgumentException if there isn't any component at some of the points.
        /// </summary>
        /// <param name="iSourceCompPoint">A point inside source component.</param>
        /// <param name="iDestCompPoint">A point inside destination component.</param>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="BSWork.BSComponents.ComponentsCantBeLinkedException"/>
        // TODO: Add caching mechanism!
        public static void LinkComponentsAtPoints(Point iSourceCompPoint, Point iDestCompPoint)
        {
            BSBaseComponent source = FindComponentAtPoint(iSourceCompPoint);
            BSBaseComponent dest = FindComponentAtPoint(iDestCompPoint);
            if (source == null || dest == null) throw new ArgumentException("Some of the points (or both) isn't inside any component.");
            if ((source == dest) || !IsInputComponent(source) || !IsOutputComponent(dest) || _ComponentsAreLinked(source, dest))
                throw new ComponentsCantBeLinkedException();
            AddSuccessorForComponent(source, dest);
            AddPredecessorForComponent(dest, source);
        }

        private static bool _ComponentsAreLinked(BSBaseComponent source, BSBaseComponent dest)
        {
            return ContainsPredecessorsForTheComponent(dest) && _sPredecessorsDict[dest].Contains(source);
        }

        private static bool IsInputComponent(BSBaseComponent iComp)
        {
            return iComp.ComponentType.Contains('I');
        }

        private static bool IsOutputComponent(BSBaseComponent iComp)
        {
            return iComp.ComponentType.Contains('O');
        }

        /// <summary>
        /// Finds a component, that contains <paramref name="iCompPoint"/> and returns it.
        /// Returns null if no component contains this point.
        /// </summary>
        /// <param name="iCompPoint">A point inside the looked for component.</param>
        /// <returns>Pointer to matched component or null.</returns>
        public static BSBaseComponent FindComponentAtPoint(Point iCompPoint)
        {
            return _sComponents.FirstOrDefault(x => x.ContainsPoint(iCompPoint));
        }

        /// <summary>
        /// Starts running the current scheme (i.e. graph of connected components).
        /// </summary>
        /// <exception cref="ComponentsArentReadyException"/>
        public static void Run()
        {
            //Just an optimization to prevent lots of code from execution if there aren't any components
            if (_sComponents.Count == 0)
            {
                _SimulationFinished();
                return;
            }
            Debug.Assert(_sCurrentState == ManagerState.Idle, 
                string.Format("BSMananger should be in {0} state to call run, not in the {1} one.", ManagerState.Idle, _sCurrentState));
            _CheckIfComponentsAreReady();

            _sReadyElements = new ConcurrentBag<BSBaseComponent>();
            _sCurrentState = ManagerState.Running;
            //TODO: select only components, that end up with 'output' component - basically, if sequence
            //of components doesn't end up with graph component or saving component - there is no need
            //to run them.
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Parallel.ForEach(from comp in _sComponents where _IsReadyToGo(comp) select comp, (component) =>
                            {
                                component.Start();
                                ComponentFinished(component);
                            });
                    }
                    catch (AggregateException exc)
                    {
                        //If there exists any exception, that is not subclassed from UserException class
                        //we should rethrow the exception, since only UserException subclassed exceptions
                        //are allowed to be shown to user without stopping the program - they are used for this purpose
                        if (!exc.InnerExceptions.All((innerException) => innerException is UserException))
                            throw;
                        _SimulationAborted(string.Join("\n", from innerExc in exc.InnerExceptions select innerExc.Message));
                    }
                });
        }

        #endregion


        public static void RegisterRunFinishedDelegate(RunFinishedDelegate iDelegate)
        {
            runFinishedHandler = iDelegate;
        }

        public static void RegisterRunAbortedDelegate(RunAbortedDelegate iDelegate)
        {
            runAbortedHandler = iDelegate;
        }

        /// <summary>
        /// A handler for the user's double click on drawing area event.
        /// If double click occured on the component - would show component's
        /// settings form (if component has one).
        /// </summary>
        /// <param name="iPoint">Double click position in drawing area coordinates.</param>
        public static void UserDoubleClickedOnPoint(Point iPoint)
        {
            BSBaseComponent compAtPoint = FindComponentAtPoint(iPoint);
            if (compAtPoint != null)
            {
                compAtPoint.ShowSettingsForm();
            }
        }

        public static readonly Dictionary<string, IComponentsFactory> ComponentsNamesDict = new Dictionary<string, IComponentsFactory>()
        {
            {"input", new BSInputComponentsFactory()},
            {"preprocessor", new BSPreprocessorComponentsFactory()},
            {"graph", new BSGraphComponentsFactory()},
            {"modelbuilder", new BSModelBuilderComponentsFactory()},
            {"forecaster", new BSForecastingComponentsFactory()},
            {"savedata", new BSSaveDataComponentsFactory()},
        };

        /// <summary>
        /// Returns true when there exists an added component, such that <paramref name="iPoint"/> is
        /// inside its on-screen rectangle, false otherwi.se.
        /// </summary>
        /// <param name="iPoint">Point on screen, must be in the same coordinate system as all components are.</param>
        /// <returns>True or false.</returns>
        public static bool ExistsComponentAtPoint(Point iPoint)
        {
            return _sComponents.Exists(comp => comp.ContainsPoint(iPoint));
        }


        /// <summary>
        /// Adds another component with specified on-screen rectangle to the components list,
        /// using provided component's name to find its exact type.
        /// </summary>
        /// <param name="iComponentName">A component's name, must be one of the BSManager.ComponentsNamesDict keys.</param>
        /// <param name="iOnScreenRectangle">A rectangle that will specify the component's on-screen area.</param>
        /// <exception cref="KeyNotFoundException"/>
        /// <exception cref="ArgumentNullException"/>
        public static void AddComponentWithNameAndRectangle(string iComponentName, Rectangle iOnScreenRectangle)
        {
            _sComponents.Add(ComponentsNamesDict[iComponentName].NewComponent(iOnScreenRectangle));
        }

        /// <summary>
        /// Adds another component to the components list.
        /// </summary>
        /// <param name="iComponent">A component to add</param>
        /// <exception cref="ArgumentNullException"/>
        public static void AddComponent(BSBaseComponent iComponent)
        {
            if (null == iComponent) throw new ArgumentNullException();
            BSManager._sComponents.Add(iComponent);
        }

        /// <summary>
        /// Clears the list of successors, as well as all
        /// predecessors and successors.
        /// </summary>
        public static void ClearComponentsList()
        {
            _sComponents.Clear();
            _sSuccessorsDict.Clear();
            _sPredecessorsDict.Clear();
        }

        /// <summary>
        /// Adds a succesor for a given component (successor must be already added before).
        /// </summary>
        /// <param name="iComponent">A component to add sucessor to.</param>
        /// <param name="iSuccessor">Successor.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static void AddSuccessorForComponent(BSBaseComponent iComponent, BSBaseComponent iSuccessor)
        {
            _AddPredOrSuccForComponent(iComponent, iSuccessor, _sSuccessorsDict);
        }

        /// <summary>
        /// Adds a predecessor for a given component (predecessor must be already added before).
        /// </summary>
        /// <param name="iComponent">A component to add predecessor to.</param>
        /// <param name="iPredecessor">Predecessor.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static void AddPredecessorForComponent(BSBaseComponent iComponent, BSBaseComponent iPredecessor)
        {
            _AddPredOrSuccForComponent(iComponent, iPredecessor, _sPredecessorsDict);
        }

        /// <summary>
        /// Returns a list of successors for a given component (empty list if there are no successors).
        /// </summary>
        /// <param name="iComponent">A component, succesors of which are being searched.</param>
        /// <returns>List of successors for a given component.</returns>
        /// <exception cref="KeyNotFoundException"/>
        public static IList<BSBaseComponent> SuccessorsForComponent(BSBaseComponent iComponent)
        {
            if (!ContainsComponent(iComponent)) throw new KeyNotFoundException();
            if (!ContainsSuccessorsForTheComponent(iComponent))
                _sSuccessorsDict[iComponent] = new List<BSBaseComponent>();

            return _sSuccessorsDict[iComponent];
        }

        /// <summary>
        /// Returns a list of predecessors for a given component (empty list if there are no predecessors).
        /// </summary>
        /// <param name="iComponent">A component, predecessors of which are being searched.</param>
        /// <returns>List of predecessors for a given component.</returns>
        /// <exception cref="KeyNotFoundException"/>
        public static IList<BSBaseComponent> PredecessorsForComponent(BSBaseComponent iComponent)
        {
            if (!ContainsComponent(iComponent)) throw new KeyNotFoundException();
            if (!ContainsPredecessorsForTheComponent(iComponent))
                _sPredecessorsDict[iComponent] = new List<BSBaseComponent>();

            return _sPredecessorsDict[iComponent];
        }

        /// <summary>
        /// Read-only list of all components.
        /// </summary>
        public static IList<BSBaseComponent> Components
        {
            get { return BSManager._sComponents.AsReadOnly(); }
        }

        /// <summary>
        /// Returns true if a component has been already added, false otherwise.
        /// </summary>
        /// <param name="iComponent"></param>
        /// <returns>true or false.</returns>
        public static bool ContainsComponent(BSBaseComponent iComponent)
        {
            return _sComponents.Contains(iComponent);
        }

        /// <summary>
        /// Returns true if one or more successors have been already added, false otherwise.
        /// </summary>
        /// <param name="iComponent"></param>
        /// <returns>true or false.</returns>
        public static bool ContainsSuccessorsForTheComponent(BSBaseComponent iComponent)
        {
            return _sSuccessorsDict.ContainsKey(iComponent);
        }

        /// <summary>
        /// Returns true if one or more predecessors have been already added, false otherwise.
        /// </summary>
        /// <param name="iComponent"></param>
        /// <returns>true or false.</returns>
        public static bool ContainsPredecessorsForTheComponent(BSBaseComponent iComponent)
        {
            return _sPredecessorsDict.ContainsKey(iComponent);
        }

        private static void _CheckIfComponentsAreReady()
        {
            string exceptionMessage = "";
            foreach (BSBaseComponent comp in _sComponents)
            {
                if (!comp.IsReady())
                    exceptionMessage += comp.WhatsWrong() + "\n";
            }
            if (exceptionMessage != "")
                throw new ComponentsArentReadyException(exceptionMessage);
        }

        private static void _AddPredOrSuccWithoutChecks(BSBaseComponent iComponent, BSBaseComponent iSuccessor, Dictionary<BSBaseComponent, List<BSBaseComponent>> iDict)
        {
            iDict[iComponent].Add(iSuccessor);
        }

        private static void ComponentFinished(BSBaseComponent iComp)
        {
                List<BSBaseComponent> successors = SuccessorsForComponent(iComp) as List<BSBaseComponent>;
                _sReadyElements.Add(iComp);
                if (_sReadyElements.Count == _sComponents.Count)
                {
                    _SimulationFinished();
                    return;
                }
                Parallel.ForEach(successors, (succ) =>
                {
                    bool shouldStart = false;
                    lock (succ)
                    {
                        succ.Inputs.Push(iComp.Output);
                        shouldStart = _IsReadyToGo(succ);
                    }
                    if (shouldStart)
                    {
                        succ.Start();
                        ComponentFinished(succ);
                    }
                });
        }

        private static void _SimulationFinished()
        {
            foreach (BSBaseComponent comp in _sComponents)
            {
                comp.Output = null;
                comp.Inputs.Clear();
            }
            _sCurrentState = ManagerState.Idle;
            if (null != runFinishedHandler)
                runFinishedHandler();
        }

        private static void _SimulationAborted(string iExceptionMessage)
        {
            _sCurrentState = ManagerState.Aborting;
            foreach (BSBaseComponent comp in _sReadyElements)
            {
                comp.Output = null;
                comp.Inputs.Clear();
            }
            _sCurrentState = ManagerState.Idle;
            if (null != runAbortedHandler)
                runAbortedHandler(iExceptionMessage);
        }

        private static bool _IsReadyToGo(BSBaseComponent iComponent)
        {
            return iComponent.Inputs.Count == PredecessorsForComponent(iComponent).Count;
        }

        private static void _InitiateValueForTheComponent(BSBaseComponent iComponent, Dictionary<BSBaseComponent, List<BSBaseComponent>> iDict)
        {
            iDict.Add(iComponent, new List<BSBaseComponent>());
        }

        private static void _AddPredOrSuccForComponent(BSBaseComponent iComponent, BSBaseComponent iPredOrSucc, Dictionary<BSBaseComponent, List<BSBaseComponent>> iDict)
        {
            if (null == iComponent) throw new ArgumentNullException("iComponent");
            if (null == iPredOrSucc) throw new ArgumentNullException("iPredOrSucc");

            if (!ContainsComponent(iComponent) || !ContainsComponent(iPredOrSucc)) throw new ArgumentException("No such component.");

            if (!iDict.ContainsKey(iComponent))
            {
                _InitiateValueForTheComponent(iComponent, iDict);
            }
            _AddPredOrSuccWithoutChecks(iComponent, iPredOrSucc, iDict);
        }

        private static bool CurrentStateInSet(ISet<ManagerState> iStatesSet)
        {
            return iStatesSet.Contains(_sCurrentState);
        }

        private enum ManagerState
        {
            Idle,
            Running,
            Aborting
        }

        private static volatile ManagerState _sCurrentState;
        private static List<BSBaseComponent> _sComponents = new List<BSBaseComponent>();
        private static Dictionary<BSBaseComponent, List<BSBaseComponent>> _sSuccessorsDict = new Dictionary<BSBaseComponent, List<BSBaseComponent>>();
        private static Dictionary<BSBaseComponent, List<BSBaseComponent>> _sPredecessorsDict = new Dictionary<BSBaseComponent, List<BSBaseComponent>>();
        private static ConcurrentBag<BSBaseComponent> _sReadyElements;
        private static ComponentFinishedDelegate compFinishedDel = new ComponentFinishedDelegate(ComponentFinished);
        private static RunFinishedDelegate runFinishedHandler = null;
        private static RunAbortedDelegate runAbortedHandler = null;
    }
}

public class ComponentsCantBeLinkedException : Exception
{
}

public class ComponentsArentReadyException : Exception
{
    public ComponentsArentReadyException(string iMessage)
        : base(iMessage)
    {
    }
}

public delegate void RunFinishedDelegate();
public delegate void RunAbortedDelegate(string iExceptionMessage);
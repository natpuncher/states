![](https://img.shields.io/badge/unity-2019.3%20or%20later-green)
[![](https://img.shields.io/github/license/natpuncher/states)](https://github.com/natpuncher/states/blob/master/LICENSE.md)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blue.svg?style=flat-square)](https://makeapullrequest.com)

states
===

State machine for Unity that helps to create a clear game architecture.
The conditions for transition are embedded within states to improve understanding of the state logic. 
Supports nested state machines. There are only one active state for each state machine,
that means whenever a new state is entered, the previous one is exited.

* [Installation](#installation)
* [Setup](#setup)
* [Usage](#usage)

## Installation
* In **Package Manager** press `+`, select `Add package from git URL` and paste `https://github.com/natpuncher/states.git`
* Or find the `manifest.json` file in the `Packages` folder of your project and add the following line to dependencies section:
```json
{
 "dependencies": {
    "com.npg.states": "https://github.com/natpuncher/states.git",
 },
}
```

## Setup

* [State factory](#implement-state-factory)
* [State type](#create-a-new-state-type)
* [State Machine](#implement-state-machine)
* [States](#create-states)

### Implement State Factory
First, `IStateFactory` interface should be implemented. This implementation will provide instances of your states to the **StateMachine**.
It could use a dependency injection or manage instances manually.

**[DI](https://github.com/natpuncher/bindlessdi), recommended**
```c#
public class StateFactory : IStateFactory
{
	private readonly IFactory<IExitable> _factory;

	public StateFactory(IFactory<IExitable> factory)
	{
		_factory = factory;
	}

	public TState GetState<TState>() where TState : class, IExitable
	{
		return _factory.Resolve<TState>();
	}
}
```

**Manual**
```c#
public class StateFactory : IStateFactory
{
	private readonly Dictionary<Type, IExitable> _states = new Dictionary<Type, IExitable>
	{
		{typeof(MyState1), new MyState1()},
		{typeof(MyState2), new MyState2()},
	};

	public TState GetState<TState>() where TState : class, IExitable
	{
		return (TState)GetState(typeof(TState));
	}
}
```

### Create a new state type
Than, the new type of state should be created by declaring an empty interface. 
```c#
public interface IGameState
{
}
```

### Implement StateMachine
Next, create an implementation of `StateMachine` with newly created state type, 
so this **StateMachine** will only work with this type of states.
```c#
public class GameStateMachine : StateMachine<IGameState>
{
	public GameStateMachine(StateFactory stateFactory) : base(stateFactory)
	{
	}
}
```

### Create states
Now, create a new state by implementing `IState` for a state life cycle 
and your `IGameState` to bind this state to the `GameStateMachine`. 
```c#
public class MyFirstGameState : IGameState, IState
{
	public void Enter()
	{
	}

	public void Exit()
	{
	}
}
```

## Usage

* [Enter state](#enter-state)
* [Pass arguments](#pass-arguments)
* [Nested state machines](#nested-state-machines)
* [Update](#update)
* [Back](#back)
* [State changed](#state-changed-notifications)

### Enter state
To enter a new state call `stateMachine.Enter<TState>()`. The previous active state will receive `Exit`. 
```c#
stateMachine.Enter<InitializeGameState>();
```

```c#
public class InitializeGameState : IGameState, IState
{
	private readonly GameStateMachine _gameStateMachine;

	public InitializeGameState(GameStateMachine gameStateMachine)
	{
		_gameStateMachine = gameStateMachine;
	}
		
	public void Enter()
	{
		// do initialization
		_gameStateMachine.Enter<MetaGameState>();
	}

	public void Exit()
	{
	}
}
```

### Pass arguments
To pass arguments to a state on enter implement `IPayloadedState<TPayloadType>` interface instead of `IState`.
```c#
public class CoreGameState : IGameState, IPayloadedState<string>
{
	public void Enter(string levelName)
	{
		Debug.Log(levelName);
	}

	public void Exit()
	{
	}
}
```

```c#
stateMachine.Enter<CoreGameState, string>(levelName);
```

### Nested state machines
Nested state machines can be used to better control on a different layers of your game.
```c#
public interface IMetaGameState
{
}

public class MetaGameStateMachine : StateMachine<IMetaGameState>
{
	public MetaGameStateMachine(StateFactory stateFactory) : base(stateFactory)
	{
	}
}
```

```c#
public class MetaGameState : IGameState, IState
{
	private readonly MetaGameStateMachine _metaGameStateMachine;

	public MetaGameState(MetaGameStateMachine metaGameStateMachine)
	{
		_metaGameStateMachine = metaGameStateMachine;
	}

	public void Enter()
	{
		_metaGameStateMachine.Enter<HudMetaState>();
	}

	public void Exit()
	{
		_metaGameStateMachine.Dispose();
	}
}
```

```c#
public class HudMetaState : IMetaGameState, IState
{
	private readonly MetaGameStateMachine _metaGameStateMachine;
	private readonly HudWindow _hudWindow;

	public HudMetaState(MetaGameStateMachine metaGameStateMachine, HudWindow hudWindow)
	{
		_metaGameStateMachine = metaGameStateMachine;
		_hudWindow = hudWindow;
	}
		
	public void Enter()
	{
		_hudWindow.OnInventoryButtonPressed -= GoToInventoryState;
		_hudWindow.OnInventoryButtonPressed += GoToInventoryState;
		_hudWindow.Show();
	}

	public void Exit()
	{
		_hudWindow.OnInventoryButtonPressed -= GoToInventoryState;
		_hudWindow.Hide();
	}

	private void GoToInventoryState()
	{
		_metaGameStateMachine.Enter<InventoryMetaState>();
	}
}
```

### Update
Calls on active state only.

> IUpdatable -> stateMachine.Update()<br>
> ILateUpdatable -> stateMachine.LateUpdate()<br>
> IFixedUpdatable -> stateMachine.FixedUpdate()<br>

```c#
public class CoreGameState : IGameState, IPayloadedState<string>, IFixedUpdatable
{
	private readonly InputController _inputController;

	public CoreGameState(InputController inputController)
	{
		_inputController = inputController;
	}
		
	public void Enter(string levelName)
	{
	}

	public void FixedUpdate()
	{
		_inputController.PollInput();
	}

	public void Exit()
	{
	}
}
```

### Back
It is possible to enter previous state by calling `stateMachine.Back()`. 
It also provides right payloads for payloaded states.
> Default back history buffer size is 1, that means it can do only one back transition.

```c#
public class InfoDialogMetaState : IMetaGameState, IPayloadedState<string>
{
	private readonly MetaGameStateMachine _metaGameStateMachine;
	private readonly InfoDialogWindow _infoDialogWindow;

	public InfoDialogMetaState(MetaGameStateMachine metaGameStateMachine, InfoDialogWindow infoDialogWindow)
	{
		_metaGameStateMachine = metaGameStateMachine;
		_infoDialogWindow = infoDialogWindow;
	}
		
	public void Enter(string dialogText)
	{
		_infoDialogWindow.OnAccept -= GoBack;
		_infoDialogWindow.OnAccept += GoBack;
		_infoDialogWindow.Show();
	}

	public void Exit()
	{
		_infoDialogWindow.OnAccept -= GoBack;
		_infoDialogWindow.Hide();
	}

	private void GoBack()
	{
		_metaGameStateMachine.Back();
	}
}
```

Back history buffer size could be increased. 
```c#
public class GameStateMachine : StateMachine<IGameState>
{
	public override int BackHistorySize => 5;

	public GameStateMachine(StateFactory stateFactory) : base(stateFactory)
	{
	}
}
```

### State changed notifications
By default, **StateMachine** will logs every state transition.
> [GameStateMachine]  -> InitializeGameState<br>
> [GameStateMachine] InitializeGameState -> MetaGameState<br>
> [MetaGameStateMachine]  -> HudMetaState<br>

It can be changed or turned off by overriding `StateChanged` method in the **StateMachine** implementation.
```c#
public class GameStateMachine : StateMachine<IGameState>
{
	public GameStateMachine(StateFactory stateFactory) : base(stateFactory)
	{
	}

	protected override void StateChanged(Type oldStateType, Type newStateType)
	{
	}
}
``` 

State changed notifications could be also received from `stateMachine.OnStateChanged` event.
```c#
public class MetaGameState : IGameState, IState
{
	private readonly MetaGameStateMachine _metaGameStateMachine;
	private readonly FireworkEmitter _fireworkEmitter;

	public MetaGameState(MetaGameStateMachine metaGameStateMachine, FireworkEmitter fireworkEmitter)
	{
		_metaGameStateMachine = metaGameStateMachine;
		_fireworkEmitter = fireworkEmitter;
	}

	public void Enter()
	{
		_metaGameStateMachine.OnStateChanged -= MetaGameStateChanged;
		_metaGameStateMachine.OnStateChanged += MetaGameStateChanged;
		_metaGameStateMachine.Enter<HudMetaState>();
	}

	public void Exit()
	{
		_metaGameStateMachine.Dispose();
	}

	private void MetaGameStateChanged(Type previousStateType, Type nextStateType)
	{
		_fireworkEmitter.Emit();
	}
}
```


using System.Collections.Generic;
using UnityEngine;

public class CommandLogger : MonoBehaviour
{
    public List<Command> commands { get; private set; }
    [SerializeField]
    int currentCommandIndex;

    public bool canExecute = true;

    [SerializeField] int currentTransformIndex;
    public List<CustomTransform> transforms;
    [Header("Number of times per frame transform data is logger")]
    [SerializeField] int framesPerSecond;
    float secondsPerFrame;
    float loggingCountdown = 0f;

    public bool interpolating = false;
    public bool loggingTransforms = false;
    public bool nonMoving = false;
    Animator anim;
    public bool aiControlled;
    public bool aiInit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if(commands == null)
        {
            commands = new List<Command>();
        }
        if(transforms == null)
        {
            transforms = new List<CustomTransform>();
        }
        if(framesPerSecond != 0)
        {
            secondsPerFrame = 1 / framesPerSecond;
            loggingCountdown = secondsPerFrame;
        }
        else
        {
            nonMoving = true;
        }
        currentCommandIndex = 0;
    }

    private void FixedUpdate()
    {
        if(canExecute)
        {
            if (!aiControlled)
            {
                aiControlled = currentCommandIndex >= commands.Count && currentTransformIndex >= transforms.Count-1;
                if (aiControlled && !aiInit)
                {
                    if (tag == "PlayerClone")
                    {
                        // add the ai scripts for player stuff
                        gameObject.AddComponent<PlayerAgent>();
                        gameObject.AddComponent<PlayerDecisionTree>();
                        gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
                        gameObject.AddComponent<PlayerInputInterpreter>();

                    }
                    else if (tag == "Boss")
                    {
                        gameObject.AddComponent<CriusUtilityAgent>();
                        gameObject.GetComponent<CriusUtilityAgent>().isActive = true;
                    }
                    // we need to log transforms and don't need to interpolate them
                    // immediately log the current position
                    loggingCountdown = 0;
                    loggingTransforms = true;
                    interpolating = false;
                }
            }
            if (interpolating)
            {
                InterpolateTransforms();
            }
            ExecuteCommands();
        }
        if(loggingTransforms)
        {
            loggingCountdown -= Time.fixedDeltaTime;
            if(loggingCountdown <= 0)
            {
                loggingCountdown = secondsPerFrame;
                transforms.Add(new CustomTransform(transform, GameTimer.GlobalTimer.time));
            }
        }
    }

    public void AddCommand(Command cmd)
    {
        commands.Add(cmd);
    }

    public int AddCommandAndExecute(Command cmd)
    {
        commands.Insert(currentCommandIndex, cmd);
        if(canExecute)
        {
            commands[currentCommandIndex].Execute();
            ++currentCommandIndex;
            return currentCommandIndex;
        }
        return -1;
    }

    public bool CancelLastCommand()
    {
        if (currentCommandIndex > 0)
        {
            return commands[currentCommandIndex - 1].Cancel();
        }
        return false;
    }

    public void CancelAbility(int index)
    {
        commands[index].Cancel();
    }

    public void ExecuteCommands()
    {
        Command currentCommand;
        while(currentCommandIndex < commands.Count)
        {
            currentCommand = commands[currentCommandIndex];
            if (currentCommand.timeRan <= GameTimer.GlobalTimer.time)
            {
                currentCommand.Execute();
                ++currentCommandIndex;
            }
            else
            {
                break;
            }
        }
    }

    public void CancelCommands()
    {
        foreach(Command cmd in commands)
        {
            cmd.Cancel();
        }
    }

    public void UndoCommands()
    {
        while(currentCommandIndex >= 0)
        {
            --currentCommandIndex;
            commands[currentCommandIndex].Undo();
        }
    }

    public void UndoCommand()
    {
        --currentCommandIndex;
        if (currentCommandIndex > 0)
        {
            commands[currentCommandIndex].Undo();
        }
    }

    public void SetCommands(List<Command> commands)
    {
        this.commands.AddRange(commands);
    }
    
    public void SetLoggerInformation(CommandLogger other)
    {
        commands.AddRange(other.commands);
        foreach(Command command in commands)
        {
            command.Init(gameObject);
        }

        transforms.AddRange(other.transforms);
    }

    public void SetToStart()
    {
        currentCommandIndex = 0;
        currentTransformIndex = 0;
        if(!nonMoving)
        {
            interpolating = false;
        }
        canExecute = false;

        if(tag == "PlayerClone")
        {
            Destroy(GetComponent<PlayerAgent>());
            Destroy(GetComponent<PlayerDecisionTree>());
            Destroy(GetComponent<UnityEngine.AI.NavMeshAgent>());
            Destroy(GetComponent<PlayerInputInterpreter>());
            loggingTransforms = false;
            loggingCountdown = 0;
            aiControlled = false;
        }
        else if(tag == "Boss")
        {
            Destroy(GetComponent<CriusUtilityAgent>());
            aiControlled = false;
            loggingCountdown = 0;
            loggingTransforms = false;
        }
    }

    public void StartPlayback()
    {
        canExecute = true;
        // only need to interpolate if we're a clone
        //if (tag == "PlayerClone" && !nonMoving)
        //{
            interpolating = true;
        //}
    }

    public void RestartData(bool clearCommands)
    {
        currentCommandIndex = 0;
        if (clearCommands)
        {
            commands = new List<Command>();
        }
        if (!nonMoving)
        {
            currentTransformIndex = 0;
            transforms = new List<CustomTransform>();
            loggingTransforms = true;
        }
    }

    public void SetData(CommandLogger other)
    {
        commands.AddRange(other.commands);
        foreach(Command command in commands)
        {
            command.Init(gameObject);
        }
        transforms.AddRange(other.transforms);
        transform.position = transforms[0].position;
        transform.rotation= transforms[0].rotation;
        transform.localScale = transforms[0].scale;
        currentCommandIndex = 0;
        currentTransformIndex = 0;
        loggingCountdown = 0;
    }

    private void InterpolateTransforms()
    {
        float nextStepTime, currentStepTime;
        if (currentTransformIndex < transforms.Count - 1)
        {
            while (currentTransformIndex < transforms.Count - 1 && transforms[currentTransformIndex + 1].time <= GameTimer.GlobalTimer.time)
            {
                currentTransformIndex++;
            }

            currentStepTime = transforms[currentTransformIndex].time;
            nextStepTime = transforms[currentTransformIndex + 1].time;
            float interParam = (GameTimer.GlobalTimer.time - currentStepTime) / (nextStepTime - currentStepTime);
            CustomTransform tempTrans = CustomTransform.Interpolate(transforms[currentTransformIndex], transforms[currentTransformIndex], interParam);

            float mag = (transform.position - tempTrans.position).magnitude;
            if (mag < .2)
            {
                anim.SetFloat("Speed", 0);
            }
            else
            {
                anim.SetFloat("Speed", (transform.position - tempTrans.position).magnitude / (nextStepTime - currentStepTime));
            }

            transform.position = tempTrans.position;
            transform.rotation = tempTrans.rotation;
            transform.localScale = tempTrans.scale;

            if (GameTimer.GlobalTimer.time >= nextStepTime)
            {
                currentTransformIndex++;
            }
        }
    }
}

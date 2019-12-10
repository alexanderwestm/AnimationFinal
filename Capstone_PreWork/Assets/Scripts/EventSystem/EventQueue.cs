using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventQueue
{
    private Event[] array;
    private int index;
    private int maxIndex;

    public EventQueue(int maxEvents)
    {
        maxIndex = maxEvents;
        array = new Event[maxEvents + 1];
        index = 1;
    }

    public Event Pop()
    {
        if (array[1] != null)
        {
            //store the top event to return
            Event temp = array[1];

            //find the lowest occupied slot
            int lastIndex;
            for (lastIndex = maxIndex; lastIndex > 0; --lastIndex)
            {
                if (array[lastIndex] != null)
                {
                    break;
                }
            }
            array[1] = array[lastIndex];
            array[lastIndex] = null;
            //then sink it down
            SinkDown(1);

            index = 1;

            return temp;
        }
        else
        {
            return null;
        }
    }


    public void Enqueue(Event toAdd)
    {
        //don't add to a full array
        if (array[maxIndex] != null)
        {
            Debug.LogError("Cannot add " + toAdd + " to full queue");
            return;
        }
        //check for an open slot
        if (array[index] == null)
        {
            array[index] = toAdd;
            BubbleUp(index);
    
        }
        //then check children for an open slot
        else if (array[2 * index] == null)
        {
            array[2 * index] = toAdd;
            BubbleUp(2 * index);
 
        }
        else if (array[2 * index + 1] == null)
        {
            array[2 * index + 1] = toAdd;
            BubbleUp(2 * index + 1);
            
        }
        else
        {
            //if there isn't an open slot, increment index and repeat.
            ++index;
            Enqueue(toAdd);
        }
    }

    void BubbleUp(int startIndex)
    {
        //if we aren't at the top, try and bubble up
        if (startIndex / 2 > 0)
        {
            //if the start node has higher priority than it's parent, swap them
            if (array[startIndex].GetPriority() > array[startIndex / 2].GetPriority())
            {
                Event temp = array[startIndex / 2];
                array[startIndex / 2] = array[startIndex];
                array[startIndex] = temp;
                //then continue to bubble up
                BubbleUp(startIndex / 2);
            }
        }
    }

    void SinkDown(int startIndex)
    {
        //don't sink on something empty
        if (array[startIndex] == null)
        {
            return;
        }
        //if startIndex has child nodes, try and sink 
        if (maxIndex <= 2 * index + 1)
        {
            return;
        }
        int lChild = 2 * index;
        int rChild = 2 * index + 1;

        int lPriority = -1;
        int rPriority = -1;
        int priority = array[startIndex].GetPriority();

        if (array[lChild] != null)
        {
            lPriority = array[lChild].GetPriority();
        }

        if (array[rChild] != null)
        {
            rPriority = array[rChild].GetPriority();
        }

        if (rPriority > lPriority)
        {
            if (rPriority > priority)
            {
                Event temp = array[startIndex];
                array[startIndex] = array[rChild];
                array[rChild] = temp;                             
            }
        }
        else if (lPriority != -1)
        {
            if (lPriority > priority)
            {
                Event temp = array[startIndex];
                array[startIndex] = array[lChild];
                array[lChild] = temp;
            }
        }
        
    }
}

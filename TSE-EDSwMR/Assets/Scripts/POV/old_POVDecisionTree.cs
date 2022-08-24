using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using System;

[System.Obsolete("Not used anymore")]
public class old_POVDecisionTree : MonoBehaviour
{

    public ToolTip rootPublicToolTip;
    public ToolTip innerNode;

    [SerializeField] dataPoint.categories rootAttribute;
    [SerializeField] dataPoint.categories layer1Attribute;
    

    [SerializeField] GameObject[] windIcons;
    [SerializeField] GameObject[] humidityIcons;




    public GameObject[] iconsDay1Prefab = new GameObject[StateScriptPOV.AMOUNT_ICONS];
    public GameObject[] iconsDay2Prefab = new GameObject[StateScriptPOV.AMOUNT_ICONS];
    public GameObject[] iconsDay3Prefab = new GameObject[StateScriptPOV.AMOUNT_ICONS];
    public GameObject[] iconsDay4Prefab = new GameObject[StateScriptPOV.AMOUNT_ICONS];
    //private GameObject mild;
    //private GameObject hot;
    //private GameObject cool;

    private List<GameObject[]> icons = new List<GameObject[]>();

    // The number of nodes is exactly the amount of day times the amount of icons
    private NodePOV[] nodes = new NodePOV[StateScriptPOV.AMOUNT_DAYS * StateScriptPOV.AMOUNT_ICONS];

    private ToolTip[] toolTips = new ToolTip[StateScriptPOV.AMOUNT_DAYS * StateScriptPOV.AMOUNT_ICONS];

    //private ToolTip node;
    private ToolTip rootPrivateToolTip;

    private List<ToolTip> firstLevel = new List<ToolTip> ();

    // Start is called before the first frame update
    void Start()
    {
        icons.Add(iconsDay1Prefab);
        icons.Add(iconsDay2Prefab);
        icons.Add(iconsDay3Prefab);
        icons.Add(iconsDay4Prefab);
        BuiltTree();
        DisplayRoot();
        IterativeDisplayChildren();
      

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    class NodePOV
    {
        NodePOV parent;
        private List<NodePOV> children = new List<NodePOV>();
        // contains indize of the contained days in this node
        private List<int> daysContainedIndex = new List<int>();
        int index;
        int layer;
        dataPoint.categories categoryName;
        GameObject icon;
        public bool isLeave = false;
        

        public NodePOV Parent { get => parent; set => parent = value; }
        public List<NodePOV> Children { get => children; set => children = value; }
        public List<int> DaysContainedIndex { get => daysContainedIndex; set => daysContainedIndex = value; }
        public int Layer { get => layer; set => layer = value; }
        public GameObject Icon { get => icon; set => icon = value; }
        public dataPoint.categories CategoryName { get => categoryName; set => categoryName = value; }
        public int Index { get => index; set => index = value; }

        public NodePOV(NodePOV parent,int index, int layer, GameObject icon)
        {
            this.Parent = parent;
            this.Layer = layer;
            this.Icon = icon;
        }

    }

    private void BuiltTree()
    {
        int indexChild = 1;
        // create root
        NodePOV rootNode = createRoot();
        nodes[0] = rootNode;
        // go through each node
        for (int index = 0; index < nodes.Length; index++)
        {
            NodePOV currentNode = nodes[index];
            int childLayer = currentNode.Layer + 1;
            List<GameObject> iconsChildren = ChildIcons(currentNode.DaysContainedIndex, currentNode.Layer);
            foreach (GameObject childIcon in iconsChildren)
            {
                NodePOV child = new NodePOV(currentNode, indexChild, childLayer, childIcon);
                if (childLayer >= StateScriptPOV.AMOUNT_ICONS + 1)
                {
                    child.isLeave = true;
                } else
                {
                    child.CategoryName = CorrectCategory(childLayer, childIcon);
                }
                child.DaysContainedIndex = addDaysToChildren(currentNode, iconsChildren, childIcon);
                rootNode.Children.Add(child);
                nodes[indexChild] = child;
                indexChild++;
            }
        }
    }
    /// <summary>
    /// Calculates the amount of children for the node and returns the icons of those in the List
    /// </summary>
    /// <param name="daysInNode"></param>
    /// <param name="layer"></param>
    /// <returns> icons of the children </returns>
    private List<GameObject> ChildIcons(List<int> daysInNode, int layer)
    {
        List<GameObject> daysContained = new List<GameObject>();
        foreach (int day in daysInNode)
        {
            Debug.Log("in numberOfChildren; day:"+day+", layer: " + layer +  "; icons[day][layer:" + icons[day][layer]);
            daysContained.Add(icons[day][layer]);
        }
        daysContained = daysContained.Distinct().ToList();
        Debug.Log("days contained count after distinct: " + daysContained.Count);
        return daysContained;
    }

    private dataPoint.categories CorrectCategory(int layer, GameObject icon)
    {
        Debug.Log("correct category ");
        if (layer == 1)
        {
            Debug.Log("correct category  layer 1");
            return layer1Attribute;
        } 

        if (layer == 2)
        {
            Debug.Log("correct category layer 2");
            foreach (GameObject wind in windIcons)
            {
                if (icon.Equals(wind))
                {
                    Debug.Log("wind ");
                    return dataPoint.categories.Wind;

                }

            }
            foreach (GameObject humidity in humidityIcons)
            {
                if (icon.Equals(humidity))
                {
                    Debug.Log("humitdíty ");
                    return dataPoint.categories.Humidity;

                }

            }

        }
        Debug.Log("correct category return nothing");
        return dataPoint.categories.Outlook;


    }

    private List<int> addDaysToChildren(NodePOV parent, List<GameObject> distinctIcons, GameObject icon)
    {
        List<int> daysInParent = parent.DaysContainedIndex;
        
        List<int> daysContained = new List<int>();
        
        if (!parent.isLeave)
        {
            foreach (int day in daysInParent)
            {
                Debug.Log("in addDaysToChildren; icon equals: " + icon.Equals(icons[day][parent.Layer + 1]) + " day: " + day);

                Debug.Log("in addDaysToChildren; icon equals: " + icon.Equals(icons[day][parent.Layer + 1]) + " day: " + day);
                if (icon.Equals(icons[day][parent.Layer + 1]))
                {

                    daysContained.Add(day);
                }
            }

        }

        return daysContained;

    }



    //private void IterativeDisplayChildren()
    //{
    //    int indexOfNextChild = 1;
    //    for (int curIndex = 0; curIndex < adjacencyList.Length; curIndex++)
    //    {
    //        foreach (GameObject childInAdList in adjacencyList[curIndex])
    //        {
    //            // if there are same add them at index of next child 
    //        }
    //    }

    //}



    //private void RootAddContainedDays()
    //{
    //    List<GameObject> firstIcons = new List<GameObject>();

    //    for (int i = 0; i < icons.Count; i++)
    //    {
    //        Debug.Log("icons Count at i:" + i + " count:" + icons.Count);
    //        firstIcons.Add(icons[i][0]);
    //        //Debug.Log("firsticon[" + i+"]: " + firstIcon[i]);
    //    }

    //    //adjacencyList[0] = firstIcons.Distinct().ToList();
 

    //}
    private NodePOV createRoot()
    {
        NodePOV rootNodePOV = new NodePOV(null, 0, 0, null);
        rootNodePOV.CategoryName = rootAttribute;
        for (int i = 0; i < icons.Count; i++)
        {
            rootNodePOV.DaysContainedIndex.Add(i);
        }

        return rootNodePOV;
    }
    
    /*
    private void rootChildren()
    {
        //iconsDay1Prefab[0] = mild;
        //iconsDay2Prefab[0] = hot;
        //iconsDay3Prefab[0] = cool;
        //iconsDay4Prefab[0] = mild;

        //
        List<GameObject> firstIcons = new List<GameObject>();

        for (int i = 0; i < icons.Count; i++)
        {
            Debug.Log("icons Count at i:"+ i + " count:" + icons.Count);
            firstIcons.Add(icons[i][0]);
            //Debug.Log("firsticon[" + i+"]: " + firstIcon[i]);
        }
        Debug.Log("line 63. firstIcons count: " + firstIcons.Count);
        GameObject[] distinctIcons = firstIcons.Distinct().ToArray(); // contains the needed icons
        Debug.Log("line 65. distinct icons: " + distinctIcons.Length);

        
        //foreach (GameObject icon in distinctIcons)
        //{
        //    Debug.Log("distinctIcons; " + icon);
        //    List<GameObject> same = firstIcon.FindAll(icon.GetHashCode);
        //}

        int amountChildren = distinctIcons.Length;



        Debug.Log("children #" + amountChildren);
        // So viele Kinder anordnen.
        // wissen welches wo Fall unterscheidung 

        recursiveDisplayNodes(rootPrivateToolTip, amountChildren);

        //for (int i = -1; i < amountChildren - 1; i++)
        //{
        //    Debug.Log("for i:" + i);

        //    ToolTip node = Instantiate(innerNode, gameObject.transform);
        //    node.transform.localPosition = new Vector3((float)(i * 0.5), 0, 0);
        //    // gameObject.GetComponent<ToolTip>();
        //    firstLevel.Add(node);
        //    Debug.Log("target root: " + rootNode.Anchor.GetComponent<Transform>().localPosition);
        //    ToolTipConnector con = node.GetComponent<ToolTipConnector>();
        //    con.Target = rootNode.Anchor;

           

        //}

        //foreach(ToolTip node in firstLevel)
        //{
        //    Debug.Log("first Level: "+node);
        //}



        //if (amountChildren != distinctIcons.Length)
        //{
        //    // gleiche zusammenfassen
        //    for (int i = 0; i < distinctIcons.Length; i++)
        //    {
        //        List<int> indexOfSame = new List<int>();
        //        indexOfSame.Add(i);

        //        for (int j = 0; j < distinctIcons.Length; i++)
        //        {
        //            if (firstIcons[i].Equals(firstIcons[j]))
        //            {
        //                indexOfSame.Add(j);
        //            }

        //        }
        //        // in index of same are all indize of days that have the same first icon
        //        foreach (int indexDay in indexOfSame)
        //        {
        //            // icons[indexDay][the current icon]; rekursiv or repeat
        //            // TODO write function iterative or rekursiv
        //            // TODO how can I do the placement? bottom up 

        //        }

        //    }
        //} else
        //{
        //    // weitermachen 
        //}

    }*/




    public void DisplayRoot()
    {
        rootPrivateToolTip = Instantiate(rootPublicToolTip, gameObject.transform);
        //rootNode.GetComponent<ToolTip>
        rootPrivateToolTip.ToolTipText = rootAttribute.ToString();
        rootPrivateToolTip.transform.localPosition = new Vector3(0, (float)0.2, 0);
        //rootNode.Anchor.transform.localPosition = root.transform.localPosition; 
        rootPrivateToolTip.Anchor.GetComponent<Transform>().localPosition = new Vector3(0, (float)0.2, 0);
        toolTips[0] = rootPrivateToolTip;
        Debug.Log("rootNode.Anchor.GetComponent<Transform>(): " + rootPrivateToolTip.Anchor.GetComponent<Transform>());
    }


    // what is missing the amount of children for the next layer 
    //private void recursiveDisplayNodes(ToolTip parent, NodePOV currentNode)
    //{
    //    Debug.Log("recursiv");

    //    if (currentNode.isLeave)
    //    {
    //        Debug.Log("recursiv case 0");
    //        return;
    //    }

    //    for (int i = -1; i < currentNode.Children.Count - 1; i++)
    //    {

    //        //TODO: match position to amount of children
    //        Debug.Log("for i:" + i);

    //        ToolTip node = Instantiate(innerNode, parent.transform);
    //        if (currentNode.Children.Count == 1)
    //        {
    //            node.transform.localPosition = new Vector3(0, (float)-0.2, 0);
    //        }
    //        else
    //        {
    //            node.transform.localPosition = new Vector3((float)(i * 0.5), (float)-0.2, 0);
    //        }

    //        // gameObject.GetComponent<ToolTip>();
    //        //firstLevel.Add(node);
    //        Debug.Log("target parent: " + parent.Anchor.GetComponent<Transform>().localPosition);
    //        ToolTipConnector con = node.GetComponent<ToolTipConnector>();
    //        con.Target = parent.Anchor;
    //        // get the amount of children out of the adjacency list
    //    }
    //}

    private void IterativeDisplayChildren()
    {
        Debug.Log(" in iterative ");
       for (int nodeIndex = 0; nodeIndex < nodes.Length; nodeIndex++)
        {
            NodePOV currentNode = nodes[nodeIndex];
            ToolTip parent = toolTips[currentNode.Parent.Index];
            for (int i = -1; i < currentNode.Children.Count - 1; i++)
            {

                //TODO: match position to amount of children
                Debug.Log("for i:" + i);

                ToolTip node = Instantiate(innerNode, parent.transform);
                if (currentNode.Children.Count == 1)
                {
                    node.transform.localPosition = new Vector3(0, (float)-0.2, 0);
                }
                else
                {
                    node.transform.localPosition = new Vector3((float)(i * 0.5), (float)-0.2, 0);
                }

                // gameObject.GetComponent<ToolTip>();
                //firstLevel.Add(node);
                Debug.Log("target parent: " + parent.Anchor.GetComponent<Transform>().localPosition);
                ToolTipConnector con = node.GetComponent<ToolTipConnector>();
                con.Target = parent.Anchor;
                // get the amount of children out of the adjacency list
            }
        }

    }
}

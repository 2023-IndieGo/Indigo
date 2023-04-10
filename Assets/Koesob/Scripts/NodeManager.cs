using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeManager : MonoBehaviour
{
    private static NodeManager instance = null;
    public static NodeManager Instance { get => instance; private set { instance = value; } }
    
    private enum Type
    {
        Success,
        Failure,
    }
    
    public class Node
    {
        private Card card;
        private Node successNode;
        private Node failureNode;

        public Card Card { get => card; set => card = value; }
        public Node SuccessNode { get => successNode; set => successNode = value; }
        public Node FailureNode { get => failureNode; set => failureNode = value; }
    }
    private List<List<Node>> nodes;

    [SerializeField] private CardNodeArea cardNodeArea;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            this.gameObject.name = typeof(NodeManager).ToString() + " (Singleton)";

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        nodes = new List<List<Node>>();

        List<Node> firstNodes = new List<Node>();

        Node successNode = new Node();
        Node failureNode = new Node();

        firstNodes.Add(successNode);
        firstNodes.Add(failureNode);

        nodes.Add(firstNodes);

        CardNodeArea success = Instantiate(cardNodeArea, ViewerManager.Instance.canvas.transform);
        success.GetComponent<RectTransform>().localPosition= new Vector3(0, 150, 0);
        success.node = successNode;
        success.OnCompleted += AddNextNode;
        
        CardNodeArea failure = Instantiate(cardNodeArea, ViewerManager.Instance.canvas.transform);
        failure.GetComponent<RectTransform>().localPosition = new Vector3(0, 75, 0);
        failure.node = failureNode; 
        failure.OnCompleted += AddNextNode;
    }

    private void AddNextNode()
    {
        Node successNode = new Node();
        Node failureNode = new Node();

        List<Node> nextNodes = new List<Node>() { successNode, failureNode };

        nodes.Add(nextNodes);

        nodes[nodes.Count - 1][0].SuccessNode = successNode;
        nodes[nodes.Count - 1][0].FailureNode = failureNode;
        nodes[nodes.Count - 1][1].SuccessNode = successNode;
        nodes[nodes.Count - 1][1].FailureNode = failureNode;

        CardNodeArea success = Instantiate(cardNodeArea, ViewerManager.Instance.canvas.transform);
        success.GetComponent<RectTransform>().localPosition = new Vector3((nodes.Count - 1)*75, 150, 0);
        success.node = successNode;
        success.OnCompleted += AddNextNode;

        CardNodeArea failure = Instantiate(cardNodeArea, ViewerManager.Instance.canvas.transform);
        failure.GetComponent<RectTransform>().localPosition = new Vector3((nodes.Count - 1) * 75, 75, 0);
        failure.node = failureNode;
        failure.OnCompleted += AddNextNode;
    }
}

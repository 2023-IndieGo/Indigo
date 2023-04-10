using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseViewer : MonoBehaviour
{
    private BaseModel model;

    public BaseViewer(BaseModel model)
    {
        this.model = model;
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public abstract void Initialize();

    public abstract void Refresh();
}

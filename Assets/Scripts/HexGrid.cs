using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    public int Width = 6;
    public int Height = 6;

    public HexCell hexPrefab;
    public Text cellLabelPrefab;

    Canvas gridCanvas;

    HexCell[] cells;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        cells = new HexCell[Width * Height];
        for (int x = 0,i=0;x<Width; x++)
        {
            for (int y = 0;y<Height;y++)
            {
                createCell(x, y, i++);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void createCell(int x,int z,int i)
    {
        Vector3 postion;
        postion.x = (x+z%2*0.5f) * (2f * HexMatrics.innerRadius);
        postion.y = 0f;
        postion.z = z * (1.5f * HexMatrics.outerRadius);
        HexCell cell = cells[i] = Instantiate<HexCell>(hexPrefab);
        cell.transform.SetParent(transform);
        cell.transform.localPosition = postion;
        //Text cellLabel = Instantiate<Text>(cellLabelPrefab);
        //cellLabel.rectTransform.SetParent(gridCanvas.transform,false);
        //cellLabel.rectTransform.anchoredPosition = new Vector2(postion.x, postion.z);
        //cellLabel.text = x.ToString() + "\n" + z.ToString();
    }
}

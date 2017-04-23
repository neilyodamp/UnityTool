using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UITableView : ScrollRect {

    GameObject cell;
    private int fristPadding = 2;
    private int lastPadding = 2;
    private int padding = 5;

    public delegate void OnShowCellDelegate(int idx,GameObject go);
    public delegate void OnHideCellDelegate (int idx,GameObject go);

    public delegate int NumOfCellDelegate();
    //一个cell 占用的像素
    public delegate int PixelCellDelegate(int idx, GameObject go);

    NumOfCellDelegate NumOfCell;
    PixelCellDelegate PixelCell;
    OnShowCellDelegate OnShowCell;
    OnHideCellDelegate OnHideCell;

    private Rect region;

    bool inited = false;
    //被弃用的cell
    //ObjectPool<GameObject> cellPool;
    //这里之所以没用通用的对象池,是因为不确定 显示最大数

    //没有被使用的cell
    BetterList<GameObject> unusedCells;
    //前面隐藏的cell数目
    private int numHeadHide = 0;
    //当前显示的cell,按顺序
    private BetterList<GameObject> showList = new BetterList<GameObject>();

    private float prevFramePos;
    public void Init()
    {
        RectTransform rectTs = this.transform as RectTransform;
        UITableView tableView = GetComponent<UITableView>();

        region = rectTs.rect;
        
        //修改cell的 Pivot 和 Anchors
    }

    public void Init(GameObject cell, NumOfCellDelegate NumOfCell,PixelCellDelegate PixelCell,int padding = 5,int fristPadding = 5,int lastPadding = 5)
    {
        
        if (cell == null)
            return;

        this.cell = cell;
        RectTransform rectTs = this.transform as RectTransform;
        UITableView tableView = GetComponent<UITableView>();
        region = rectTs.rect;

        this.NumOfCell = NumOfCell;
        this.PixelCell = PixelCell;

        RectTransform contentRt = content.transform as RectTransform;
        contentRt.pivot = new Vector2(0,1);
        contentRt.anchorMin = new Vector2(0,1);
        contentRt.anchorMax = new Vector2(0, 1);
        RectTransform viewportRt = viewport.transform as RectTransform;

        contentRt.sizeDelta = new Vector2(viewportRt.rect.width, viewportRt.rect.height);

        contentRt.localPosition = new Vector3(0, 0, 0);

        if (unusedCells == null)
            unusedCells = new BetterList<GameObject>();
        cell.SetActive(false);

        this.fristPadding = fristPadding;
        this.lastPadding = lastPadding;

        InitCellList();

        inited = true;
    }

    private void InitCellList()
    {
        RectTransform contentRt = content.transform as RectTransform;

        //contentRt
        float num = NumOfCell();

        float startPos = 0;

        for (int i =0;i<num;i++)
        {
            if (Mathf.Abs(startPos) > contentRt.rect.height)
                break;

            GameObject cellGo = CreateCell();
            
            //
            if(OnShowCell != null)
                OnShowCell(i,cellGo);
            
            int pad = i == 0 ? fristPadding : padding;

            startPos -= pad;

            
            Vector3 oldPos = cell.transform.localPosition;
            cellGo.transform.localPosition = vertical ? new Vector3(oldPos.x, startPos, oldPos.z) : new Vector3(oldPos.x, oldPos.y, oldPos.z);
            RectTransform cellRt = cellGo.transform as RectTransform;
            startPos -= cellRt.rect.height;

            showList.Add(cellGo);
        }

        float contentPixel = Mathf.Max(Mathf.Abs(startPos) + lastPadding,vertical? contentRt.rect.height : content.rect.width);
        contentRt.sizeDelta = vertical ? new Vector2(contentRt.sizeDelta.x, contentPixel) : new Vector2(contentPixel,contentRt.sizeDelta.y);

        
    }

    void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (!inited)
            return;
        //Debug.Log(velocity);

        UpdateTable();
    }
    
    private void OnScrolling()
    {

    }
    public override void OnScroll(PointerEventData data)
    {
        base.OnScroll(data);
        Debug.Log("Scroll");
    }

    private void UpdateTable()
    {


        //前面有
        NeedCreate();


        //判断是向上滑动还是向下滑动.
        /*
        if (numHeadHide + showList.size < NumOfCell() && (vertical ? velocity.y > 0 : velocity.x < 0))
            this.movementType = MovementType.Unrestricted; // 还有多余的  
        else if(vertical ? velocity.y < 0 : velocity.x > 0)
            this.movementType = MovementType.Elastic;
        else if(numHeadHide + showList.size == NumOfCell())
        */
        if ((vertical ? velocity.y > 0 : velocity.x < 0))
        {
            
        }
        else
        {

        }

    }
    private void NeedCreate()
    {
        RectTransform contentRt = content.transform as RectTransform;
        RectTransform viewPortRt = viewport.transform as RectTransform;
        if(numHeadHide + showList.size < NumOfCell())
        {
            
            if(showList.size > 0)
            {
                GameObject showLastGo = showList[showList.size - 1];
                RectTransform lastRect = showLastGo.transform as RectTransform;
                //相对 viewPort的position;
                Vector3 relPos = contentRt.localPosition + lastRect.localPosition;

                float endLine = vertical ? (relPos.y - lastRect.rect.height):
                    (relPos.x+lastRect.rect.width);

                if (vertical ? (endLine > 0 - viewPortRt.rect.height):
                    (endLine < viewPortRt.rect.width))
                {
                    GameObject cellGo = CreateCell();
                    if(OnShowCell != null)
                        OnShowCell(numHeadHide + showList.size,cellGo);
                    
                     

                    RectTransform cellRt = cellGo.transform as RectTransform;

                    cellGo.transform.localPosition =
                        vertical ? new Vector3(lastRect.localPosition.x, lastRect.localPosition.y - lastRect.rect.height - padding,0) :
                        new Vector3(lastRect.localPosition.x+lastRect.rect.width+padding,lastRect.localPosition.y,0);


                    //处理content 大小
                    Vector3 relCellPos = cellGo.transform.localPosition;
                    float endCellLine =vertical ? (relCellPos.y-cellRt.rect.height-lastPadding):(relCellPos.x+ cellRt.rect.width + lastPadding);
                    if (vertical)
                        content.sizeDelta =new Vector2(content.sizeDelta.x, Mathf.Max(Mathf.Abs(endCellLine),content.rect.height));
                    else
                        content.sizeDelta =new Vector2(Mathf.Max(Mathf.Abs(endCellLine),content.rect.width),content.sizeDelta.y);

                    showList.Add(cellGo);
                }else
                {
                    //不需要创建
                    
                    return;
                }

                    
            }
            //如果没有显示的存在,直接创建一个
            else
            {
                GameObject cellGo = CreateCell();
                if (OnShowCell != null)
                    OnShowCell(numHeadHide + showList.size, cellGo);

                RectTransform cellRt = cellGo.transform as RectTransform;
                cellRt.localPosition = vertical ? new Vector3(cellRt.localPosition.x, -fristPadding)
                    : new Vector3(fristPadding,cellRt.localPosition.y);

                //处理content 大小
                Vector3 relCellPos = cellGo.transform.localPosition;
                float endCellLine = vertical ? (relCellPos.y - cellRt.rect.height - lastPadding) : (relCellPos.x + cellRt.rect.width + lastPadding);

                if (vertical)
                    content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Max(Mathf.Abs(endCellLine), content.rect.height));
                else
                    content.sizeDelta = new Vector2(Mathf.Max(Mathf.Abs(endCellLine), content.rect.width), content.sizeDelta.y);

                showList.Add(cellGo);
            }
            NeedCreate();
        }
    }

    private void NeedRecly()
    {

    }

    private GameObject CreateCell()
    {

        GameObject newCell;
        if(unusedCells.size > 0)
        {
            newCell = unusedCells[0];
            unusedCells.RemoveAt(0);
        }
        else
        {
            newCell = Instantiate<GameObject>(cell);
        }
        newCell.transform.parent = content;
        newCell.SetActive(true);
        return newCell;
    }

    private void RecycleCell(GameObject cell)
    {
        cell.SetActive(false);
        unusedCells.Add(cell);
    }


    //只能够覆盖
    public void SetNumOfCell(NumOfCellDelegate NumOfCell)
    {
        this.NumOfCell = NumOfCell;
    }

    public void SetPixelCell(PixelCellDelegate PixelCell)
    {
        this.PixelCell = PixelCell;
    }

    public void SetOnShowCell(OnShowCellDelegate OnShowCell)
    {
        this.OnShowCell = OnShowCell;
    }

    public void SetOnHideCell(OnHideCellDelegate OnHideCell)
    {
        this.OnHideCell = OnHideCell;
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UITableView : ScrollRect
{
    GameObject cell;
    private int headPadding = 2;
    private int footPadding = 2;
    private int padding = 5;

    public delegate void OnShowCellDelegate(int idx, GameObject go);
    public delegate void OnHideCellDelegate(int idx, GameObject go);
    public delegate int NumOfCellDelegate();

    OnShowCellDelegate OnShowCell;
    OnHideCellDelegate OnHideCell;
    NumOfCellDelegate NumOfCell;

    private Rect region;
    bool inited = false;
    BetterList<GameObject> unusedCells;

    private int numHeadHide = 0;
    private BetterList<GameObject> showList = new BetterList<GameObject>();

    private float prevFramePos;
    public void Init()
    {

    }

    public void Init(GameObject cell,int padding ,int headPadding,int footPadding)
    {
        //没有设定cell
        if (cell == null)
            return;
        //没有设定content
        if (content == null)
            return;

        this.cell = cell;
        RectTransform rectTs = this.transform as RectTransform;
        region = rectTs.rect;

        content.pivot = new Vector2(0,1);
        content.anchorMin = new Vector2(0,1);
        content.anchorMax = new Vector2(0,1);

        content.sizeDelta = new Vector2(viewport.rect.width, viewport.rect.height);
        content.localPosition = new Vector3(0, 0, 0);

        if (unusedCells == null)
            unusedCells = new BetterList<GameObject>();

        cell.SetActive(false);

        this.headPadding = headPadding;
        this.footPadding = footPadding;

        InitCellList();

        inited = true;
    }
    private int GetNumCell()
    {
        return NumOfCell == null ? 0 : NumOfCell();
    }
    private void InitCellList()
    {
        float num = GetNumCell();
        float startPos = 0;
        for(int i = 0;i < num; i++)
        {
            if (Mathf.Abs(startPos) > content.rect.height)
                break;

            GameObject cellGo = CreateCell();

            if (OnShowCell != null) OnShowCell(i, cellGo);

            int pad = i == 0 ? headPadding : padding;
            startPos -= pad;

            Vector3 oldPos = cell.transform.localPosition;
            cellGo.transform.localPosition = vertical ? new Vector3(oldPos.x, startPos, oldPos.z) : new Vector3(oldPos.x, oldPos.y, oldPos.z);
            RectTransform cellRt = cellGo.transform as RectTransform;
            startPos -= cellRt.rect.height;

            showList.Add(cellGo);
        }
        float contentPixel = Mathf.Max(Mathf.Abs(startPos) + footPadding, vertical ? content.rect.height : content.rect.width);
        content.sizeDelta = vertical ? new Vector2(content.sizeDelta.x, contentPixel) : new Vector2(contentPixel, content.sizeDelta.y);
    }
    void Update()
    {
        if (!inited)
            return;

        UpdateTable();
    }
    private void UpdateTable()
    {
        NeedRecly();
        NeedCreate();

    }
    private void NeedCreate()
    {
        float showHeadLine = 0;
        float showFootLine = 0;
        
        //显示列表
        if(showList.size >0)
        {
            RectTransform showHeadTs = showList[0].transform as RectTransform;
            RectTransform showFootTs = showList[showList.size-1].transform as RectTransform;

            showHeadLine = vertical ? showHeadTs.localPosition.y : showHeadTs.localPosition.x;
            showFootLine = vertical ? showFootTs.localPosition.y-showFootTs.rect.height : showFootTs.localPosition.x+showFootTs.rect.width;
        } 
        else
        {
            //从当前位置显示位置开始算
            showHeadLine = vertical ? -(viewport.rect.height - footPadding) - content.position.y :
                viewport.rect.width - footPadding - content.position.x;

            showFootLine = vertical ? -headPadding -content.position.y : headPadding - content.position.x;
        }
        // 先创建前头需要创建的
        if (numHeadHide > 0 && 
            vertical ? (showHeadLine + content.localPosition.y < 0):(showHeadLine + content.localPosition.x > 0))
        {
            GameObject cell = CreateCell();
            if (OnShowCell != null) OnShowCell(numHeadHide-1,cell);
            RectTransform cellTs = cell.transform as RectTransform;
            cellTs.localPosition = vertical ? new Vector3(cellTs.localPosition.x, cellTs.rect.height + padding + showHeadLine, cellTs.localPosition.z) :
                new Vector3(showHeadLine - padding - cellTs.rect.width,cellTs.localPosition.y,cellTs.localPosition.z);

            showList.Insert(0, cell);
            NeedCreate();
        }
        // 再创建后头 ， 调整 rectTransform
        if (numHeadHide + showList.size <GetNumCell() && 
            vertical?(showFootLine + content.localPosition.y > -viewport.rect.height):(showFootLine +content.localPosition.x <viewport.rect.width))
        {
            GameObject cell = CreateCell();
            if (OnShowCell != null) OnShowCell(numHeadHide+showList.size, cell);
            RectTransform cellTs = cell.transform as RectTransform;
            cellTs.localPosition = vertical ? new Vector3(cellTs.localPosition.x, showFootLine - padding, cellTs.localPosition.z) :
                new Vector3(showFootLine+padding,cellTs.localPosition.y,cellTs.localPosition.z);

            showList.Add(cell);
            NeedCreate();
        }
            
    }

    private void NeedRecly()
    {
        //没有可回收的
        if (showList.size <= 0)
            return;

        float hideHeadLine;
        float hideFootLine;

        RectTransform hideHeadTs = showList[0].transform as RectTransform;
        RectTransform hideFootTs = showList[showList.size - 1].transform as RectTransform;

        hideHeadLine = vertical ? hideHeadTs.localPosition.y - hideHeadTs.rect.height :
            hideHeadTs.localPosition.x + hideHeadTs.rect.width;

        hideFootLine = vertical ? hideFootTs.localPosition.y :
            hideFootTs.localPosition.x;

        //先回收前头
        if(vertical?( hideHeadLine+content.localPosition.y > viewport.position.y):
            (hideHeadLine+content.localPosition.x < viewport.position.x))
        {
            GameObject reclyGo = showList[0];
            if (OnHideCell != null) OnHideCell(numHeadHide, reclyGo);

            unusedCells.Add(reclyGo);
            showList.RemoveAt(0);
            NeedRecly();
        }

        if(vertical?(hideFootLine+content.localPosition.y < viewport.position.y - viewport.rect.height):
            (hideFootLine + content.localPosition.x > viewport.position.x+viewport.rect.width ))
        {
            GameObject reclyGo = showList[showList.size - 1];
            if (OnHideCell != null) OnHideCell(numHeadHide + showList.size - 1, reclyGo);

            unusedCells.Add(reclyGo);
            showList.RemoveAt(showList.size -1);
            NeedRecly();
        }
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
    //增加 datasource数量 和 删除，会影响 content的大小
    private void AdjustContent()
    {
        
    }
    //是否约束列表
    private void NeedRestricted()
    {
        //判定为向上滑 或 向左滑处理
        

    }

}

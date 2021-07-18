using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsDraw : MonoBehaviour
{
    private Material lineMaterial;

    public static bool LifeRuning = false;

    public Vector2 PlaneSize = new Vector2(100, 100);
    public float CellSize = 1;
    public float CellGap = 0.1f;
    public float TickInterval = 0.25f;
    private float TickTimer = 0;
    private bool TickFlag = false;

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }


    void Start()
    {
        print("Init Game");
        GameCells.InitGame(PlaneSize, CellSize);
    }

    private void Update()
    {
        if (LifeRuning)
        {
            TickTimer -= Time.deltaTime;
            if (TickTimer < 0)
            {
                TickTimer = TickInterval;
                TickFlag = !TickFlag;
                if (TickFlag)
                    GameCells.CalcNextStates();
                else
                    GameCells.DoNextStates();
            }

        }




    }



    void OnPostRender()
    {
        GL.PushMatrix();

        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(Color.gray);

        Vector2 PlaneSize2 = PlaneSize / 2;


        for (float i = -PlaneSize2.y; i <= PlaneSize2.y; i += CellSize)
        {
            GL.Vertex3(-PlaneSize2.x, 0, i);
            GL.Vertex3(PlaneSize2.x, 0, i);
        }
        for (float i = -PlaneSize2.x; i <= PlaneSize2.x; i += CellSize)
        {
            GL.Vertex3(i, 0, -PlaneSize2.y);
            GL.Vertex3(i, 0, PlaneSize2.y);
        }

        GL.End();

        //Draw Cells
        GL.Begin(GL.QUADS);

        Vector2 gap = new Vector2(CellGap, CellGap);
        Vector2 d0 = new Vector2(-PlaneSize.x / 2, -PlaneSize.y / 2) + gap;
        Vector2 dgap = new Vector2(CellSize, CellSize) - gap - gap;
        foreach (var Cell in GameCells.Cells)
        {

            //Draw Box
            Vector2 p0 = (Vector2)Cell.Key * CellSize + d0;
            Vector2 p1 = p0 + dgap;
            GL.Color(GameCells.PlayerColors[Cell.Value]);
            GL.Vertex3(p0.x, 0, p0.y);
            GL.Vertex3(p1.x, 0, p0.y);
            GL.Vertex3(p1.x, 0, p1.y);
            GL.Vertex3(p0.x, 0, p1.y);

        }

        GL.End();

        GL.PopMatrix();
    }
}

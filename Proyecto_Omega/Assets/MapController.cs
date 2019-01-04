using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {
    public Vector3 mouse;
    public Camera Camara;
  
     
    public void Move()
    {
   
        this.transform.position = Input.mousePosition;
        //Vector3 NewPos =new Vector3(this.transform.position.x,
        //    transform.position.y, Camara.transform.localPosition.z);

        Vector3 NewPos = this.transform.localPosition;
        float x = NewPos.x;
        float y = NewPos.y;
        if (x<-450F)
        {
            x = -450F;
        }
        if (x>450F)
        {
            x = 450F;
        }
        if (y < -240F)
        {
            y = -240F;
        }
        if (y > 240F)
        {
            y = 240F;
        }
        
        NewPos = new Vector3(x,y, Camara.transform.localPosition.z);
        float step = 500* Time.deltaTime;
        Camara.transform.localPosition=Vector3.MoveTowards(Camara.transform.localPosition, NewPos, step);
    }
    public void EndMove()
    {
        this.transform.localPosition = Vector3.zero;
    }
    public void Jugar()
    {
        LevelLoader.instance.CargarEscena("VsIA");
    }
}

using UnityEngine;

public class BlockElement : MonoBehaviour 
{

    public CellType type = CellType.star;
    public int I;
    public int J;
    public int Id;

    public ParticleSystem psystem;
    public void Init(int i,int j)
    {
        I = i;
        J = j;
        if (psystem != null)
        {
            psystem.gameObject.transform.parent = transform.parent;
        }
    }

    public void Play()
    {
        if (psystem != null)
        {
            psystem.enableEmission = true;
         //   psystem.Stop();
            psystem.Play();
           // psystem.Simulate(1);
            Debug.Log("psystem.Play()");
        }
    }

    public override string ToString()
    {
        return "I:" + I + " J:" + J + " T:" + type + " Id:"+Id;
    }

}

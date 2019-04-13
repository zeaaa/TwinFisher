using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PondUI : MonoBehaviour
{

    [SerializeField]
    private Text fishname;

    [SerializeField]
    private Text maxLength;

    [SerializeField]
    private Text maxWeight;

    [SerializeField]
    private Text score;

    [SerializeField]
    private Image img;
    [SerializeField]
    private Image block; 

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetImage(Sprite i){
        img.sprite =i;
    }
    public void SetName(string s){
        fishname.text="Name:"+s;
    }
    public void SetWeight(float s){
        maxLength.text="MaxWeight:"+s;
    }
    public void SetLength(float s){
        maxWeight.text="MaxLength:"+s;
    }
    public void SetScore(float s){
        score.text="Score:"+s;
    }
    public void SetBlock(bool f){
        if(f)block.color=new Vector4(0.5f,0.5f,0.5f,1);
        else block.color=new Vector4(0,0,0,0);
    }
    public void SetButton(bool f){
        GetComponent<Button>().enabled=f;
        
    }
}

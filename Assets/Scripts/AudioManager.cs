using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource ClickSFX;
    public AudioSource CoinSFX;
    public AudioSource DieSFX;
    public AudioSource ExplodeSFX;
    public AudioSource SwitchSFX;   
    public AudioSource TurretFireSFX;
    public AudioSource WinSFX;


   
    public void BtnClick() //We have this one already
    {
        ClickSFX.Play(0);
    }

    public void CoinCollect() //We have this one already
    {
        CoinSFX.Play(0);
    }

    public void Death() //We have this one already
    {
        DieSFX.Play(0);
    }

    public void Explosion() //We have this one already
    {
        ExplodeSFX.Play(0);
    }
    
    public void Swap() //We have this one already
    {
        SwitchSFX.Play(0);
    }

    public void TurretFire() //We have this one already
    {
        TurretFireSFX.Play(0);
    }

    public void Win() //We have this one already
    {
        WinSFX.Play(0);
    }

}

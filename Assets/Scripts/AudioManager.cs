using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource BtnClickSFX;
    public AudioSource CoinSFX;
    public AudioSource DieSFX;
    public AudioSource MissileSFX;    
    public AudioSource TurretFireSFX;
    public AudioSource WinSFX;


   
    public void BtnClick()
    {
        BtnClickSFX.Play(0);
    }

    public void CoinCollect()
    {
        CoinSFX.Play(0);
    }

    public void Death()
    {
        DieSFX.Play(0);
    }

    public void Missile()
    {
        MissileSFX.Play(0);
    }
    
    public void TurretFire()
    {
        TurretFireSFX.Play(0);
    }

    public void Win()
    {
        WinSFX.Play(0);
    }

}

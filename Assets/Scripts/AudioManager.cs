using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource TurretFireSFX;
    public AudioSource WinSFX;
    public AudioSource MissileSFX;
    public AudioSource BtnClickSFX;

    
    public void TurretFire()
    {
        TurretFireSFX.Play(0);
    }

    public void Win()
    {
        WinSFX.Play(0);
    }

    public void Missile()
    {
        MissileSFX.Play(0);
    }

    public void BtnClick()
    {
        BtnClickSFX.Play(0);
    }

}

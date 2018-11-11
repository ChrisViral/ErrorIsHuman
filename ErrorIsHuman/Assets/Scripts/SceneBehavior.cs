using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace ErrorIsHuman.Base
{
    public class SceneBehavior : MonoBehaviour
    {

        #region Fields
        [SerializeField]
        private AudioClip[] sounds = new AudioClip[0];
        private AudioSource audioSource;

        [SerializeField]
        private AudioSource buzzLight;
        [SerializeField]
        private AudioSource mainSource;
        [SerializeField]
        private Image fade;
        #endregion

        #region Functions
        private void Start()
        {
            audioSource = this.GetComponent<AudioSource>();
            StartCoroutine(Fade());
        }




        #endregion

        private IEnumerator<YieldInstruction> Fade()
        {
            yield return this.fade.DOFade(1f, 0f).WaitForCompletion();
            audioSource.clip = sounds[0];
            audioSource.Play();
            yield return audioSource.DOFade(1f,8f).WaitForCompletion();
            audioSource.Stop();
            yield return new WaitForSeconds(2f);
            audioSource.clip = sounds[1];
            audioSource.volume = 0.5f;
            audioSource.Play();
            yield return new WaitForSeconds(2f);
            audioSource.Play();
            yield return audioSource.DOFade(1f, 2f).WaitForCompletion();
            //Add door here
            yield return new WaitForSeconds(1.5f);
            audioSource.clip = sounds[2];
            audioSource.Play();
            yield return new WaitForSeconds(3f);
            audioSource.clip = sounds[3];
            audioSource.Play();
            yield return new WaitForSeconds(1f);
            audioSource.clip = sounds[4];
            audioSource.Play();
            yield return new WaitForSeconds(5f);
            audioSource.clip = sounds[5];
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
            buzzLight.clip = sounds[6];
            buzzLight.Play();
            yield return this.fade.DOFade(0f, 0.5f).WaitForCompletion();
        }
    }
}

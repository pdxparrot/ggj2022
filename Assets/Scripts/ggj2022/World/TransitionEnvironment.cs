using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace pdxpartyparrot
{
    public class TransitionEnvironment : MonoBehaviour
    {
        public Material spriteMaterial;



        private GameObject[] slimes;
        private GameObject slime_container;


        private GameObject[] env_sprites;


        private MaterialPropertyBlock _prop_block;


        private float timeElapsed = 0f;
        public float transitionTime = 10f;

        private float startValue = 0f;
        private float endValue = 1f;
        private float valueToLerp;

        // Start is called before the first frame update
        void Start()
        {
            if(env_sprites == null) {
                env_sprites = GameObject.FindGameObjectsWithTag("EnvSprite");
            }

            spriteMaterial.SetFloat("_Fire_Ash_Blend", 0f);

        }






        // Update is called once per frame
        void Update()
        {
            //slimes = GameObject.FindGameObjectsWithTag("Slime");
            slime_container = GameObject.Find("Enemies");


            List<bool> slime_check = new List<bool>();

            foreach(Transform slime in slime_container.transform) {
                if(slime.gameObject.activeSelf) {
                    slime_check.Add(slime);
                }

            }

            if(slime_check.Count == 0) {

                if(timeElapsed < transitionTime) {
                    valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / transitionTime);
                    //Debug.Log(valueToLerp);
                    spriteMaterial.SetFloat("_Fire_Ash_Blend", valueToLerp);
                    timeElapsed += Time.deltaTime;
                }



                /*                foreach(GameObject sprite in env_sprites) {
                                    Renderer sprite_renderer = sprite.GetComponent<Renderer>();
                                    sprite_renderer.GetPropertyBlock(_prop_block);
                                    _prop_block.SetFloat("_Fire_Ash_Blend", 1f);
                                    sprite_renderer.SetPropertyBlock(_prop_block);
                                }*/


            }


        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentGuessClouds : MonoSingleton<InstrumentGuessClouds>
{
    [SerializeField] private GameObject cloudsParent;
    [SerializeField] private GameObject character;
    //public bool isClustered = false;
    
    private Sprite[] _cloudSprites;
    private readonly Dictionary<GameObject, Vector3> _originalPositions = new Dictionary<GameObject, Vector3>();

    private new void Awake()
    {
        // Load all sprites from the CloudsForInstruments folder
        _cloudSprites = Resources.LoadAll<Sprite>($"CloudsForInstruments/");
        
        // Select a random sprite from the _cloudSprites

        for (int i = 0; i < cloudsParent.transform.childCount; i++)
        {
            GameObject cloud = cloudsParent.transform.GetChild(i).gameObject;
            
            int randomIndex = Math.Abs(Guid.NewGuid().GetHashCode()) % _cloudSprites.Length;
            Sprite randomSprite = _cloudSprites[randomIndex];
            cloud.gameObject.GetComponent<Image>().sprite = randomSprite;
            
            _originalPositions[cloud] = cloud.transform.position;
        }
    }

    public void ClusterClouds()
    {
        for (int i = 0; i < cloudsParent.transform.childCount; i++)
        {
            StartCoroutine(ClusterClouds(cloudsParent.transform.GetChild(i).gameObject));   
        }
    }
    
    public void DisperseClouds()
    {
        for (int i = 0; i < cloudsParent.transform.childCount; i++)
        {
            StartCoroutine(DisperseClouds(cloudsParent.transform.GetChild(i).gameObject));
        }
    }
    
    #region Cluster&DisperseClouds
    
    private IEnumerator DisperseClouds(GameObject cloud)
    {
        float startTime = Time.time; // Record the start time

        Image image = cloud.GetComponent<Image>();
        Color targetColor = new Color(image.color.r, image.color.g, image.color.b, 0);

        Vector3 direction = (cloud.transform.position - character.transform.position).normalized;
        float speed = 10f;

        float lerpSpeed = 4f;

        while (image.color != targetColor)
        {
            image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * lerpSpeed);
            cloud.transform.position += direction * (speed * Time.deltaTime);

            yield return null;
        }

        float endTime = Time.time; // Record the end time

        // Calculate and print the duration of the coroutine
        float duration = endTime - startTime;
        //Debug.Log($"DisperseClouds took {duration} seconds to complete.");
    }

    private IEnumerator ClusterClouds(GameObject cloud)
    {
        float startTime = Time.time; // Record the start time
        
        Image image = cloud.GetComponent<Image>();
        float targetAlpha = 1f; // Set the target transparency to 1
        Color targetColor = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);

        float speed = 10f; // Increase the speed

        float lerpSpeed = 1.5f; // Decrease this value to make the color change slower

        float distanceToOriginalPosition = Vector3.Distance(cloud.transform.position, _originalPositions[cloud]);
        float alphaDifference = Mathf.Abs(image.color.a - targetAlpha);

        while (distanceToOriginalPosition > 0.01f || alphaDifference > 0.01f) // Check the distance and alpha difference
        {
            var position = cloud.transform.position;
            
            Vector3 direction = (_originalPositions[cloud] - position).normalized; // Move towards the original position

            // Gradually increase the alpha value of the cloud's sprite color to targetAlpha
            image.color = Color.Lerp(image.color, targetColor, Time.deltaTime * lerpSpeed);

            // Move the cloud towards the original position
            position += direction * speed * Time.deltaTime;
            cloud.transform.position = position;

            // Update the distance and alpha difference for the next iteration
            distanceToOriginalPosition = Vector3.Distance(position, _originalPositions[cloud]);
            alphaDifference = Mathf.Abs(image.color.a - targetAlpha);

            yield return null;
        }

        // Ensure the cloud arrives exactly at its original position and transparency
        cloud.transform.position = _originalPositions[cloud];
        image.color = targetColor;

        float endTime = Time.time; // Record the end time
        float duration = endTime - startTime;
        //Debug.Log($"ClusterClouds took {duration} seconds to complete.");
    }
    
    #endregion
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
   public Transform targetPoint;        // Точка притяжения
   public float attractionSpeed = 5f;   // Скорость притяжения
   public float minDistance = 0.1f;     // Дистанция, при которой объект уничтожается
   public AudioClip enterSound;         // Звук при входе
   public float shrinkSpeed = 1f;       // Скорость уменьшения
   public bool destroyOnArrival = true; // Уничтожать ли объект при достижении 
   public ScrollingEnvironment scrollingEnvironment;
   public TurnOnOffActivator TurnOnOffActivator;
   private Vector3 initialScale;


   public bool isAttracting = false;
   private Transform objectToAttract;
   private AudioSource audioSource;

   void Start()
   {
       // Настроим аудио
       audioSource = gameObject.AddComponent<AudioSource>();
       if (enterSound != null)
       {
           audioSource.clip = enterSound;
       }
   }

   void OnTriggerEnter(Collider other)
   {
       if (!isAttracting && other.CompareTag("Player"))
       {
           Debug.Log(other.name + " entered");
           initialScale = other.transform.localScale;

           isAttracting = true;
           objectToAttract = other.transform;
            
           if (enterSound != null)
           {
               //audioSource.Play();
           }
           
       }
   }

   void Update()
   {
       if (isAttracting && objectToAttract != null)
       {   
           TurnOnOffActivator._isActivated = false;
           scrollingEnvironment.enabled = false;
           
           // Притяжение к точке
           objectToAttract.position = Vector3.MoveTowards(
               objectToAttract.position,
               targetPoint.position,
               attractionSpeed * Time.deltaTime
           );
            
           // Уменьшение масштаба
           float distance = Vector3.Distance(objectToAttract.position, targetPoint.position);
           float scaleFactor = Mathf.Clamp01(distance);
           objectToAttract.localScale = initialScale * scaleFactor;


           // Уничтожение, если достаточно близко
           if (distance <= minDistance)
           {
               Destroy(objectToAttract.gameObject);
               objectToAttract = null;
               isAttracting = false;
               float time = Time.timeSinceLevelLoad;
               int stars = 0; //depending on the quality of going through level
               LevelManager.Instance.SetLastResult(time, stars);
               SceneManager.LoadScene("LevelTransitionMenu");
           }
       }
   }
}

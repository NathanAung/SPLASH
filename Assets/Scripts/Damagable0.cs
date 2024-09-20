// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.SceneManagement;

// public class Damagable : MonoBehaviour
// {
//     public bool isEnemy = false;
//     public Animator animator;
//     PlayerController controller;
//     EnemyMelee enemyMelee;
//     public UnityEvent<int, Vector2> OnHitEvents;    // events to invoke when received damage
//     //public UnityEvent<float, float> HealthChanged; // for UI
//     public bool wasHit {get{
//         if(isEnemy){
//             return enemyMelee.wasHit;
//         }
//         else{
//             return controller.wasHit;
//         }
//     }private set{
//         if(isEnemy){
//             enemyMelee.wasHit = value;
//         }
//         else{
//             controller.wasHit = value;
//         }
//     }}
//     [SerializeField] private bool _isAlive = true;
//     public bool isAlive {get{
//         return _isAlive;
//     }set{
//         _isAlive = value;
//     }}
//     [SerializeField] public int _maxHP = 100;
//     public int maxHP {get{
//         return _maxHP;
//     }private set{
//         _maxHP = value;
//     }}
//     [SerializeField] public int _HP = 100;  //current HP
//     public int HP {get{
//         return _HP;
//     }private set{
//         _HP = value;
//         //HealthChanged?.Invoke(_HP, _maxHP);  // UI update
//         if(_HP <= 0){
//             isAlive = false;
//             animator.SetBool("isAlive", false);
//             Debug.Log("dead");
//             // death animation
//             // destroy game object when the enemy is defeated (will be updated)
//             if(isEnemy){
//                 Destroy(gameObject);
//             }
//             else{
//                 // temporary way to restart level
//                 SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//             }
//         }
//     }}
//     [SerializeField] private bool isInvincible = false;
//     public float invTime = 0.5f; // invincibility time
//     private float invTimer = 0f; // timer


//     private void Awake(){
//         animator = GetComponent<Animator>();
//         if(isEnemy){
//             enemyMelee = GetComponent<EnemyMelee>();
//         }
//         else{
//             controller = GetComponent<PlayerController>();
//         }
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(isInvincible){
//             if(invTimer >= invTime){
//                 isInvincible = false; // reset invincibility
//                 invTimer = 0;
//                 if(wasHit){
//                     wasHit = false;
//                 }
//             }
//             else{
//                 invTimer += Time.deltaTime; // add time to timer
//             }
//         }
//         //Hit(10);
//     }

//     // deal damage
//     public void TakeDamage(int damage, Vector2 knockback){
//         if(isAlive && !isInvincible){
//             HP -= damage;
//             isInvincible = true; // temporary invincibility
//             OnHitEvents?.Invoke(damage, knockback); // ? = check null
//             wasHit = true;
//             Debug.Log(gameObject.name + " received " + damage + " damage");
//         }
//     }
// }

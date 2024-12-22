using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using TMPro;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// An event triggered every time health values change, for other classes to listen to
    /// </summary>
    public struct HealthChangeEvent
    {
        public Health AffectedHealth;
        public float NewHealth;

        public HealthChangeEvent(Health affectedHealth, float newHealth)
        {
            AffectedHealth = affectedHealth;
            NewHealth = newHealth;
        }

        static HealthChangeEvent e;

        public static void Trigger(Health affectedHealth, float newHealth)
        {
            e.AffectedHealth = affectedHealth;
            e.NewHealth = newHealth;
            MMEventManager.TriggerEvent(e);
        }
    }


    /// <summary>
    /// This class manages the health of an object, pilots its potential health bar, handles what happens when it takes damage,
    /// and what happens when it dies.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/Core/Health")]
    public class Health : TopDownMonoBehaviour
    {
        [MMInspectorGroup("Bindings", true, 3)]
        /// the model to disable (if set so)
        [Tooltip("the model to disable (if set so)")]
        public GameObject Model;

        [MMInspectorGroup("Status", true, 29)]
        /// the current health of the character
        [MMReadOnly]
        [Tooltip("the current health of the character")]
        public float CurrentHealth;

        /// If this is true, this object can't take damage at this time
        [MMReadOnly] [Tooltip("If this is true, this object can't take damage at this time")]
        public bool Invulnerable = false;

        [MMInspectorGroup("Health", true, 5)]
        [MMInformation(
            "Add this component to an object and it'll have health, will be able to get damaged and potentially die.",
            MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        /// the initial amount of health of the object
        [Tooltip("the initial amount of health of the object")]
        public float InitialHealth = 10;

        /// the maximum amount of health of the object
        [Tooltip("the maximum amount of health of the object")]
        public float MaximumHealth = 10;

        /// if this is true, health values will be reset everytime this character is enabled (usually at the start of a scene)
        [Tooltip(
            "if this is true, health values will be reset everytime this character is enabled (usually at the start of a scene)")]
        public bool ResetHealthOnEnable = true;

        [MMInspectorGroup("Damage", true, 6)]
        [MMInformation(
            "Here you can specify an effect and a sound FX to instantiate when the object gets damaged, and also how long the object should flicker when hit (only works for sprites).",
            MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        /// whether or not this Health object can be damaged 
        [Tooltip("whether or not this Health object can be damaged")]
        public bool ImmuneToDamage = false;

        /// the feedback to play when getting damage
        [Tooltip("the feedback to play when getting damage")]
        public MMFeedbacks DamageMMFeedbacks;
        [Tooltip("the feedback to play when getting critical damage")]
        public MMFeedbacks CriticalDamageMMFeedbacks;
        [Tooltip("the feedback to play when getting critical damage")]
        public MMFeedbacks HealthMMFeedbacks;


        /// if this is true, the damage value will be passed to the MMFeedbacks as its Intensity parameter, letting you trigger more intense feedbacks as damage increases
        [Tooltip(
            "if this is true, the damage value will be passed to the MMFeedbacks as its Intensity parameter, letting you trigger more intense feedbacks as damage increases")]
        public bool FeedbackIsProportionalToDamage = false;

        /// if you set this to true, other objects damaging this one won't take any self damage
        [Tooltip("if you set this to true, other objects damaging this one won't take any self damage")]
        public bool PreventTakeSelfDamage = false;

        [MMInspectorGroup("Knockback", true, 63)]
        /// whether or not this object is immune to damage knockback
        [Tooltip("whether or not this object is immune to damage knockback")]
        public bool ImmuneToKnockback = false;

        /// whether or not this object is immune to damage knockback if the damage received is zero
        [Tooltip("whether or not this object is immune to damage knockback if the damage received is zero")]
        public bool ImmuneToKnockbackIfZeroDamage = false;

        /// a multiplier applied to the incoming knockback forces. 0 will cancel all knockback, 0.5 will cut it in half, 1 will have no effect, 2 will double the knockback force, etc
        [Tooltip(
            "a multiplier applied to the incoming knockback forces. 0 will cancel all knockback, 0.5 will cut it in half, 1 will have no effect, 2 will double the knockback force, etc")]
        public float KnockbackForceMultiplier = 1f;

        [MMInspectorGroup("Death", true, 53)]
        [MMInformation(
            "Here you can set an effect to instantiate when the object dies, a force to apply to it (topdown controller required), how many points to add to the game score, and where the character should respawn (for non-player characters only).",
            MoreMountains.Tools.MMInformationAttribute.InformationType.Info, false)]
        /// whether or not this object should get destroyed on death
        [Tooltip("whether or not this object should get destroyed on death")]
        public bool DestroyOnDeath = true;

        /// the time (in seconds) before the character is destroyed or disabled
        [Tooltip("the time (in seconds) before the character is destroyed or disabled")]
        public float DelayBeforeDestruction = 0f;

        /// the points the player gets when the object's health reaches zero
        [Tooltip("the points the player gets when the object's health reaches zero")]
        public int PointsWhenDestroyed;

        /// if this is set to false, the character will respawn at the location of its death, otherwise it'll be moved to its initial position (when the scene started)
        [Tooltip(
            "if this is set to false, the character will respawn at the location of its death, otherwise it'll be moved to its initial position (when the scene started)")]
        public bool RespawnAtInitialLocation = false;

        /// if this is true, the controller will be disabled on death
        [Tooltip("if this is true, the controller will be disabled on death")]
        public bool DisableControllerOnDeath = true;

        /// if this is true, the model will be disabled instantly on death (if a model has been set)
        [Tooltip("if this is true, the model will be disabled instantly on death (if a model has been set)")]
        public bool DisableModelOnDeath = true;

        /// if this is true, collisions will be turned off when the character dies
        [Tooltip("if this is true, collisions will be turned off when the character dies")]
        public bool DisableCollisionsOnDeath = true;

        /// if this is true, collisions will also be turned off on child colliders when the character dies
        [Tooltip("if this is true, collisions will also be turned off on child colliders when the character dies")]
        public bool DisableChildCollisionsOnDeath = false;

        /// whether or not this object should change layer on death
        [Tooltip("whether or not this object should change layer on death")]
        public bool ChangeLayerOnDeath = false;

        /// whether or not this object should change layer on death
        [Tooltip("whether or not this object should change layer on death")]
        public bool ChangeLayersRecursivelyOnDeath = false;

        /// the layer we should move this character to on death
        [Tooltip("the layer we should move this character to on death")]
        public MMLayer LayerOnDeath;

        /// the feedback to play when dying
        [Tooltip("the feedback to play when dying")]
        public MMFeedbacks DeathMMFeedbacks;

        /// if this is true, color will be reset on revive
        [Tooltip("if this is true, color will be reset on revive")]
        public bool ResetColorOnRevive = true;

        /// the name of the property on your renderer's shader that defines its color 
        [Tooltip("the name of the property on your renderer's shader that defines its color")]
        [MMCondition("ResetColorOnRevive", true)]
        public string ColorMaterialPropertyName = "_Color";

        /// if this is true, this component will use material property blocks instead of working on an instance of the material.
        [Tooltip(
            "if this is true, this component will use material property blocks instead of working on an instance of the material.")]
        public bool UseMaterialPropertyBlocks = false;

        [MMInspectorGroup("Shared Health and Damage Resistance", true, 12)]
        /// another Health component (usually on another character) towards which all health will be redirected
        [Tooltip("another Health component (usually on another character) towards which all health will be redirected")]
        public Health MasterHealth;

        /// a DamageResistanceProcessor this Health will use to process damage when it's received
        [Tooltip("a DamageResistanceProcessor this Health will use to process damage when it's received")]
        public DamageResistanceProcessor TargetDamageResistanceProcessor;

        [MMInspectorGroup("Animator", true, 14)]
        /// the target animator to pass a Death animation parameter to. The Health component will try to auto bind this if left empty
        [Tooltip(
            "the target animator to pass a Death animation parameter to. The Health component will try to auto bind this if left empty")]
        public Animator TargetAnimator;

        /// if this is true, animator logs for the associated animator will be turned off to avoid potential spam
        [Tooltip(
            "if this is true, animator logs for the associated animator will be turned off to avoid potential spam")]
        public bool DisableAnimatorLogs = true;

        [MMInspectorGroup("Shield", true, 5)] [Tooltip("the initial amount of shield of the object")]
        public float InitialShield = 0;

        [Tooltip("the maximum amount of shield the object can have")]
        public float MaximumShield = 0;
        
        [Tooltip("HealthText")]
        public TextMeshProUGUI healthText;

        [MMReadOnly] [Tooltip("The current shield of the object")]
        public float CurrentShield;
        
        [MMInspectorGroup("Damage Reduction", true, 15)]
        /// <summary>
        /// The percentage of damage to reduce. A value from 0 (no reduction) to 100 (full immunity).
        /// </summary>
        [Range(0f, 100f)]
        [Tooltip("The percentage of damage to reduce. A value from 0 (no reduction) to 100 (full immunity).")]
        public float DamageReductionPercentage = 0f;


        public virtual float LastDamage { get; set; }
        public virtual Vector3 LastDamageDirection { get; set; }
        public virtual bool Initialized => _initialized;

        // hit delegate
        public delegate void OnHitDelegate();

        public OnHitDelegate OnHit;

        // respawn delegate
        public delegate void OnReviveDelegate();

        public OnReviveDelegate OnRevive;

        // death delegate
        public delegate void OnDeathDelegate();

        public OnDeathDelegate OnDeath;

        protected Vector3 _initialPosition;
        protected Renderer _renderer;
        protected Character _character;
        protected CharacterMovement _characterMovement;
        protected TopDownController _controller;

        protected MMHealthBar _healthBar;
        protected Collider2D _collider2D;
        protected Collider _collider3D;
        protected CharacterController _characterController;
        protected bool _initialized = false;
        protected Color _initialColor;
        protected AutoRespawn _autoRespawn;
        protected int _initialLayer;
        protected MaterialPropertyBlock _propertyBlock;
        protected bool _hasColorProperty = false;
        protected ProximityManaged _proximityManaged;

        protected const string _deathAnimatorParameterName = "Death";
        protected const string _healthAnimatorParameterName = "Health";
        protected const string _healthAsIntAnimatorParameterName = "HealthAsInt";
        protected int _deathAnimatorParameter;
        protected int _healthAnimatorParameter;
        protected int _healthAsIntAnimatorParameter;
        protected bool _hasDeathParameter = false;
        protected bool _hasHealthParameter = false;
        protected bool _hasHealthAsIntParameter = false;


        protected class InterruptiblesDamageOverTimeCoroutine
        {
            public Coroutine DamageOverTimeCoroutine;
            public DamageType DamageOverTimeType;
        }
        

        protected List<InterruptiblesDamageOverTimeCoroutine> _interruptiblesDamageOverTimeCoroutines;
        protected List<InterruptiblesDamageOverTimeCoroutine> _damageOverTimeCoroutines;

        #region Initialization

        /// <summary>
        /// On Awake, we initialize our health
        /// </summary>
        protected virtual void Awake()
        {
            Initialization();
            InitializeCurrentHealth();
        }

        /// <summary>
        /// On Start we grab our animator
        /// </summary>
        protected virtual void Start()
        {
            GrabAnimator();
        }

        /// <summary>
        /// Grabs useful components, enables damage and gets the inital color
        /// </summary>
        public virtual void Initialization()
        {
            _character = this.gameObject.GetComponentInParent<Character>();
            CurrentShield = InitialShield;

            if (Model != null)
            {
                Model.SetActive(true);
            }

            if (gameObject.GetComponentInParent<Renderer>() != null)
            {
                _renderer = GetComponentInParent<Renderer>();
            }

            if (_character != null)
            {
                _characterMovement = _character.FindAbility<CharacterMovement>();
                if (_character.CharacterModel != null)
                {
                    if (_character.CharacterModel.GetComponentInChildren<Renderer>() != null)
                    {
                        _renderer = _character.CharacterModel.GetComponentInChildren<Renderer>();
                    }
                }
            }

            if (_renderer != null)
            {
                if (UseMaterialPropertyBlocks && (_propertyBlock == null))
                {
                    _propertyBlock = new MaterialPropertyBlock();
                }

                if (ResetColorOnRevive)
                {
                    if (UseMaterialPropertyBlocks)
                    {
                        if (_renderer.sharedMaterial.HasProperty(ColorMaterialPropertyName))
                        {
                            _hasColorProperty = true;
                            _initialColor = _renderer.sharedMaterial.GetColor(ColorMaterialPropertyName);
                        }
                    }
                    else
                    {
                        if (_renderer.material.HasProperty(ColorMaterialPropertyName))
                        {
                            _hasColorProperty = true;
                            _initialColor = _renderer.material.GetColor(ColorMaterialPropertyName);
                        }
                    }
                }
            }

            _interruptiblesDamageOverTimeCoroutines = new List<InterruptiblesDamageOverTimeCoroutine>();
            _damageOverTimeCoroutines = new List<InterruptiblesDamageOverTimeCoroutine>();
            _initialLayer = gameObject.layer;

            if (TargetAnimator != null)
            {
                if (TargetAnimator.MMHasParameterOfType(_deathAnimatorParameterName,
                        AnimatorControllerParameterType.Trigger))
                {
                    _hasDeathParameter = true;
                }

                if (TargetAnimator.MMHasParameterOfType(_healthAnimatorParameterName,
                        AnimatorControllerParameterType.Float))
                {
                    _hasHealthParameter = true;
                }

                if (TargetAnimator.MMHasParameterOfType(_healthAsIntAnimatorParameterName,
                        AnimatorControllerParameterType.Int))
                {
                    _hasHealthAsIntParameter = true;
                }
            }

            _deathAnimatorParameter = Animator.StringToHash(_deathAnimatorParameterName);
            _healthAnimatorParameter = Animator.StringToHash(_healthAnimatorParameterName);
            _healthAsIntAnimatorParameter = Animator.StringToHash(_healthAsIntAnimatorParameterName);

            _proximityManaged = this.gameObject.GetComponentInParent<ProximityManaged>();
            _autoRespawn = this.gameObject.GetComponentInParent<AutoRespawn>();
            if (MasterHealth == null)
            {
                _healthBar = this.gameObject.GetComponentInParent<MMHealthBar>();
            }

            _controller = this.gameObject.GetComponentInParent<TopDownController>();
            _characterController = this.gameObject.GetComponentInParent<CharacterController>();
            _collider2D = this.gameObject.GetComponentInParent<Collider2D>();
            _collider3D = this.gameObject.GetComponentInParent<Collider>();

            DamageMMFeedbacks?.Initialization(this.gameObject);
            DeathMMFeedbacks?.Initialization(this.gameObject);

            StoreInitialPosition();
            _initialized = true;

            DamageEnabled();
        }

        /// <summary>
        /// Grabs the target animator
        /// </summary>
        protected virtual void GrabAnimator()
        {
            if (TargetAnimator == null)
            {
                BindAnimator();
            }

            if ((TargetAnimator != null) && DisableAnimatorLogs)
            {
                TargetAnimator.logWarnings = false;
            }

            UpdateHealthAnimationParameters();
        }

        /// <summary>
        /// Finds and binds an animator if possible
        /// </summary>
        protected virtual void BindAnimator()
        {
            if (_character != null)
            {
                if (_character.CharacterAnimator != null)
                {
                    TargetAnimator = _character.CharacterAnimator;
                }
                else
                {
                    TargetAnimator = GetComponent<Animator>();
                }
            }
            else
            {
                TargetAnimator = GetComponent<Animator>();
            }
        }

        /// <summary>
        /// Stores the initial position for further use
        /// </summary>
        public virtual void StoreInitialPosition()
        {
            _initialPosition = this.transform.position;
        }

        /// <summary>
        /// Initializes health to either initial or current values
        /// </summary>
        public virtual void InitializeCurrentHealth()
        {
            if (MasterHealth == null)
            {
                SetHealth(InitialHealth);
            }
            else
            {
                if (MasterHealth.Initialized)
                {
                    SetHealth(MasterHealth.CurrentHealth);
                }
                else
                {
                    SetHealth(MasterHealth.InitialHealth);
                }
            }
        }

        /// <summary>
        /// When the object is enabled (on respawn for example), we restore its initial health levels
        /// </summary>
        protected virtual void OnEnable()
        {
            if ((_proximityManaged != null) && _proximityManaged.StateChangedThisFrame)
            {
                return;
            }

            if (ResetHealthOnEnable)
            {
                InitializeCurrentHealth();
            }

            if (Model != null)
            {
                Model.SetActive(true);
            }

            DamageEnabled();
        }

        /// <summary>
        /// On Disable, we prevent any delayed destruction from running
        /// </summary>
        protected virtual void OnDisable()
        {
            CancelInvoke();
        }

        #endregion

        /// <summary>
        /// Returns true if this Health component can be damaged this frame, and false otherwise
        /// </summary>
        /// <returns></returns>
        public virtual bool CanTakeDamageThisFrame()
        {
            // if the object is invulnerable, we do nothing and exit
            if (Invulnerable || ImmuneToDamage)
            {
                return false;
            }

            if (!this.enabled)
            {
                return false;
            }

            // if we're already below zero, we do nothing and exit
            if ((CurrentHealth <= 0) && (InitialHealth != 0))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Called when the object takes damage
        /// </summary>
        /// <param name="damage">The amount of health points that will get lost.</param>
        /// <param name="instigator">The object that caused the damage.</param>
        /// <param name="flickerDuration">The time (in seconds) the object should flicker after taking the damage - not used anymore, kept to not break retrocompatibility</param>
        /// <param name="invincibilityDuration">The duration of the short invincibility following the hit.</param>
        public virtual void Damage(float damage, GameObject instigator, float flickerDuration, float invincibilityDuration, 
            Vector3 damageDirection, List<TypedDamage> typedDamages = null, bool isCritical = false)
        {
            if (!CanTakeDamageThisFrame())
            {
                return;
            }

            damage = ComputeDamageOutput(damage, typedDamages, true);
            
            
            // Aplica la reducción de daño basada en el porcentaje configurado
            float reductionMultiplier = 1f - (DamageReductionPercentage / 100f);
            damage *= reductionMultiplier;

            // Si el escudo tiene valores, lo reducimos primero
            if (CurrentShield > 0)
            {
                float absorbedDamage = Mathf.Min(CurrentShield, damage);
                CurrentShield -= absorbedDamage;
                damage -= absorbedDamage;
                
            }

            // Reducimos la salud con el daño restante
            float previousHealth = CurrentHealth;
            if (MasterHealth != null)
            {
                previousHealth = MasterHealth.CurrentHealth;
                MasterHealth.SetHealth(MasterHealth.CurrentHealth - damage);
            }
            else
            {
                SetHealth(CurrentHealth - damage);
            }

            LastDamage = damage;
            LastDamageDirection = damageDirection;
            OnHit?.Invoke();

            // Deshabilitamos daño temporal si es necesario
            if (invincibilityDuration > 0)
            {
                DamageDisabled();
                StartCoroutine(DamageEnabled(invincibilityDuration));
            }

            // Disparar eventos
            MMDamageTakenEvent.Trigger(this, instigator, CurrentHealth, damage, previousHealth, typedDamages);

            // Feedback y animaciones
            if (TargetAnimator != null)
            {
                TargetAnimator.SetTrigger("Damage");
            }

            if (FeedbackIsProportionalToDamage)
            {
                DamageMMFeedbacks?.PlayFeedbacks(this.transform.position, damage);
            }
            else
            {
                
                // Si el daño es crítico, puedes agregar lógica específica aquí
                if (isCritical)
                {
                    Debug.Log("¡Este daño fue un golpe crítico!");
                    CriticalDamageMMFeedbacks?.PlayFeedbacks(this.transform.position);
                }
                else
                {
                    DamageMMFeedbacks?.PlayFeedbacks(this.transform.position);
                }
     
            }

            // Actualizar barras gráficas
            UpdateHealthBar(true);

            // Verificar muerte
            if (MasterHealth != null && MasterHealth.CurrentHealth <= 0)
            {
                MasterHealth.CurrentHealth = 0;
                MasterHealth.Kill();
            }
            else if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Kill();
            }
        }

        /// <summary>
        /// Interrupts all damage over time, regardless of type
        /// </summary>
        public virtual void InterruptAllDamageOverTime()
        {
            foreach (InterruptiblesDamageOverTimeCoroutine coroutine in _interruptiblesDamageOverTimeCoroutines)
            {
                StopCoroutine(coroutine.DamageOverTimeCoroutine);
            }

            _interruptiblesDamageOverTimeCoroutines.Clear();
        }

        /// <summary>
        /// Interrupts all damage over time, even the non interruptible ones (usually on death)
        /// </summary>
        public virtual void StopAllDamageOverTime()
        {
            foreach (InterruptiblesDamageOverTimeCoroutine coroutine in _damageOverTimeCoroutines)
            {
                StopCoroutine(coroutine.DamageOverTimeCoroutine);
            }

            _damageOverTimeCoroutines.Clear();
        }

        /// <summary>
        /// Interrupts all damage over time of the specified type
        /// </summary>
        /// <param name="damageType"></param>
        public virtual void InterruptAllDamageOverTimeOfType(DamageType damageType)
        {
            foreach (InterruptiblesDamageOverTimeCoroutine coroutine in _interruptiblesDamageOverTimeCoroutines)
            {
                if (coroutine.DamageOverTimeType == damageType)
                {
                    StopCoroutine(coroutine.DamageOverTimeCoroutine);
                }
            }

            TargetDamageResistanceProcessor?.InterruptDamageOverTime(damageType);
        }

        /// <summary>
        /// Applies damage over time, for the specified amount of repeats (which includes the first application of damage, makes it easier to do quick maths in the inspector, and at the specified interval).
        /// Optionally you can decide that your damage is interruptible, in which case, calling InterruptAllDamageOverTime() will stop these from being applied, useful to cure poison for example.
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="instigator"></param>
        /// <param name="flickerDuration"></param>
        /// <param name="invincibilityDuration"></param>
        /// <param name="damageDirection"></param>
        /// <param name="typedDamages"></param>
        /// <param name="amountOfRepeats"></param>
        /// <param name="durationBetweenRepeats"></param>
        /// <param name="interruptible"></param>
        public virtual void DamageOverTime(float damage, GameObject instigator, float flickerDuration,
            float invincibilityDuration, Vector3 damageDirection, List<TypedDamage> typedDamages = null,
            int amountOfRepeats = 0, float durationBetweenRepeats = 1f, bool interruptible = true,
            DamageType damageType = null)
        {
            if (ComputeDamageOutput(damage, typedDamages, false) == 0)
            {
                return;
            }

            InterruptiblesDamageOverTimeCoroutine damageOverTime = new InterruptiblesDamageOverTimeCoroutine();
            damageOverTime.DamageOverTimeType = damageType;
            damageOverTime.DamageOverTimeCoroutine = StartCoroutine(DamageOverTimeCo(damage, instigator,
                flickerDuration,
                invincibilityDuration, damageDirection, typedDamages, amountOfRepeats, durationBetweenRepeats,
                interruptible));
            _damageOverTimeCoroutines.Add(damageOverTime);
            if (interruptible)
            {
                _interruptiblesDamageOverTimeCoroutines.Add(damageOverTime);
            }
        }

        /// <summary>
        /// A coroutine used to apply damage over time
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="instigator"></param>
        /// <param name="flickerDuration"></param>
        /// <param name="invincibilityDuration"></param>
        /// <param name="damageDirection"></param>
        /// <param name="typedDamages"></param>
        /// <param name="amountOfRepeats"></param>
        /// <param name="durationBetweenRepeats"></param>
        /// <param name="interruptible"></param>
        /// <param name="damageType"></param>
        /// <returns></returns>
        protected virtual IEnumerator DamageOverTimeCo(float damage, GameObject instigator, float flickerDuration,
            float invincibilityDuration, Vector3 damageDirection, List<TypedDamage> typedDamages = null,
            int amountOfRepeats = 0, float durationBetweenRepeats = 1f, bool interruptible = true,
            DamageType damageType = null)
        {
            for (int i = 0; i < amountOfRepeats; i++)
            {
                Damage(damage, instigator, flickerDuration, invincibilityDuration, damageDirection, typedDamages);
                yield return MMCoroutine.WaitFor(durationBetweenRepeats);
            }
        }

        /// <summary>
        /// Returns the damage this health should take after processing potential resistances
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public virtual float ComputeDamageOutput(float damage, List<TypedDamage> typedDamages = null,
            bool damageApplied = false)
        {
            if (Invulnerable || ImmuneToDamage)
            {
                return 0;
            }

            float totalDamage = 0f;
            // we process our damage through our potential resistances
            if (TargetDamageResistanceProcessor != null)
            {
                if (TargetDamageResistanceProcessor.isActiveAndEnabled)
                {
                    totalDamage = TargetDamageResistanceProcessor.ProcessDamage(damage, typedDamages, damageApplied);
                }
            }
            else
            {
                totalDamage = damage;
                if (typedDamages != null)
                {
                    foreach (TypedDamage typedDamage in typedDamages)
                    {
                        totalDamage += typedDamage.DamageCaused;
                    }
                }
            }

            return totalDamage;
        }

        /// <summary>
        /// Goes through resistances and applies condition state changes if needed
        /// </summary>
        /// <param name="typedDamages"></param>
        protected virtual void ComputeCharacterConditionStateChanges(List<TypedDamage> typedDamages)
        {
            if ((typedDamages == null) || (_character == null))
            {
                return;
            }

            foreach (TypedDamage typedDamage in typedDamages)
            {
                if (typedDamage.ForceCharacterCondition)
                {
                    if (TargetDamageResistanceProcessor != null)
                    {
                        if (TargetDamageResistanceProcessor.isActiveAndEnabled)
                        {
                            bool checkResistance =
                                TargetDamageResistanceProcessor.CheckPreventCharacterConditionChange(typedDamage
                                    .AssociatedDamageType);
                            if (checkResistance)
                            {
                                continue;
                            }
                        }
                    }

                    _character.ChangeCharacterConditionTemporarily(typedDamage.ForcedCondition,
                        typedDamage.ForcedConditionDuration, typedDamage.ResetControllerForces,
                        typedDamage.DisableGravity);
                }
            }
        }

        /// <summary>
        /// Goes through the resistance list and applies movement multipliers if needed
        /// </summary>
        /// <param name="typedDamages"></param>
        protected virtual void ComputeCharacterMovementMultipliers(List<TypedDamage> typedDamages)
        {
            if ((typedDamages == null) || (_character == null))
            {
                return;
            }

            foreach (TypedDamage typedDamage in typedDamages)
            {
                if (typedDamage.ApplyMovementMultiplier)
                {
                    if (TargetDamageResistanceProcessor != null)
                    {
                        if (TargetDamageResistanceProcessor.isActiveAndEnabled)
                        {
                            bool checkResistance =
                                TargetDamageResistanceProcessor.CheckPreventMovementModifier(typedDamage
                                    .AssociatedDamageType);
                            if (checkResistance)
                            {
                                continue;
                            }
                        }
                    }

                    _characterMovement?.ApplyMovementMultiplier(typedDamage.MovementMultiplier,
                        typedDamage.MovementMultiplierDuration);
                }
            }
        }

        /// <summary>
        /// Determines a new knockback force by processing it through resistances
        /// </summary>
        /// <param name="knockbackForce"></param>
        /// <param name="typedDamages"></param>
        /// <returns></returns>
        public virtual Vector3 ComputeKnockbackForce(Vector3 knockbackForce, List<TypedDamage> typedDamages = null)
        {
            return (TargetDamageResistanceProcessor == null)
                ? knockbackForce
                : TargetDamageResistanceProcessor.ProcessKnockbackForce(knockbackForce, typedDamages);
            ;
        }

        /// <summary>
        /// Returns true if this Health can get knockbacked, false otherwise
        /// </summary>
        /// <param name="typedDamages"></param>
        /// <returns></returns>
        public virtual bool CanGetKnockback(List<TypedDamage> typedDamages)
        {
            if (ImmuneToKnockback)
            {
                return false;
            }

            if (TargetDamageResistanceProcessor != null)
            {
                if (TargetDamageResistanceProcessor.isActiveAndEnabled)
                {
                    bool checkResistance = TargetDamageResistanceProcessor.CheckPreventKnockback(typedDamages);
                    if (checkResistance)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Kills the character, instantiates death effects, handles points, etc
        /// </summary>
        public virtual void Kill()
        {
            if (ImmuneToDamage)
            {
                return;
            }

            if (_character != null)
            {
                // we set its dead state to true
                _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Dead);
                _character.Reset();

                if (_character.CharacterType == Character.CharacterTypes.Player)
                {
                    TopDownEngineEvent.Trigger(TopDownEngineEventTypes.PlayerDeath, _character);
                }
            }

            SetHealth(0);

            // we prevent further damage
            StopAllDamageOverTime();
            DamageDisabled();

            DeathMMFeedbacks?.PlayFeedbacks(this.transform.position);

            // Adds points if needed.
            if (PointsWhenDestroyed != 0)
            {
                // we send a new points event for the GameManager to catch (and other classes that may listen to it too)
                TopDownEnginePointEvent.Trigger(PointsMethods.Add, PointsWhenDestroyed);
            }

            if (_hasDeathParameter)
            {
                TargetAnimator.SetTrigger(_deathAnimatorParameter);
            }

            // we make it ignore the collisions from now on
            if (DisableCollisionsOnDeath)
            {
                if (_collider2D != null)
                {
                    _collider2D.enabled = false;
                }

                if (_collider3D != null)
                {
                    _collider3D.enabled = false;
                }

                // if we have a controller, removes collisions, restores parameters for a potential respawn, and applies a death force
                if (_controller != null)
                {
                    _controller.CollisionsOff();
                }

                if (DisableChildCollisionsOnDeath)
                {
                    foreach (Collider2D collider in this.gameObject.GetComponentsInChildren<Collider2D>())
                    {
                        collider.enabled = false;
                    }

                    foreach (Collider collider in this.gameObject.GetComponentsInChildren<Collider>())
                    {
                        collider.enabled = false;
                    }
                }
            }

            if (ChangeLayerOnDeath)
            {
                gameObject.layer = LayerOnDeath.LayerIndex;
                if (ChangeLayersRecursivelyOnDeath)
                {
                    this.transform.ChangeLayersRecursively(LayerOnDeath.LayerIndex);
                }
            }

            OnDeath?.Invoke();
            MMLifeCycleEvent.Trigger(this, MMLifeCycleEventTypes.Death);

            if (DisableControllerOnDeath && (_controller != null))
            {
                _controller.enabled = false;
            }

            if (DisableControllerOnDeath && (_characterController != null))
            {
                _characterController.enabled = false;
            }

            if (DisableModelOnDeath && (Model != null))
            {
                Model.SetActive(false);
            }

            if (DelayBeforeDestruction > 0f)
            {
                Invoke("DestroyObject", DelayBeforeDestruction);
            }
            else
            {
                // finally we destroy the object
                DestroyObject();
            }
        }

        /// <summary>
        /// Revive this object.
        /// </summary>
        public virtual void Revive()
        {
            if (!_initialized)
            {
                return;
            }

            if (_collider2D != null)
            {
                _collider2D.enabled = true;
            }

            if (_collider3D != null)
            {
                _collider3D.enabled = true;
            }

            if (DisableChildCollisionsOnDeath)
            {
                foreach (Collider2D collider in this.gameObject.GetComponentsInChildren<Collider2D>())
                {
                    collider.enabled = true;
                }

                foreach (Collider collider in this.gameObject.GetComponentsInChildren<Collider>())
                {
                    collider.enabled = true;
                }
            }

            if (ChangeLayerOnDeath)
            {
                gameObject.layer = _initialLayer;
                if (ChangeLayersRecursivelyOnDeath)
                {
                    this.transform.ChangeLayersRecursively(_initialLayer);
                }
            }

            if (_characterController != null)
            {
                _characterController.enabled = true;
            }

            if (_controller != null)
            {
                _controller.enabled = true;
                _controller.CollisionsOn();
                _controller.Reset();
            }

            if (_character != null)
            {
                _character.ConditionState.ChangeState(CharacterStates.CharacterConditions.Normal);
            }

            if (ResetColorOnRevive && (_renderer != null))
            {
                if (UseMaterialPropertyBlocks)
                {
                    _renderer.GetPropertyBlock(_propertyBlock);
                    _propertyBlock.SetColor(ColorMaterialPropertyName, _initialColor);
                    _renderer.SetPropertyBlock(_propertyBlock);
                }
                else
                {
                    _renderer.material.SetColor(ColorMaterialPropertyName, _initialColor);
                }
            }

            if (RespawnAtInitialLocation)
            {
                transform.position = _initialPosition;
            }

            if (_healthBar != null)
            {
                _healthBar.Initialization();
            }

            Initialization();
            InitializeCurrentHealth();
            OnRevive?.Invoke();
            MMLifeCycleEvent.Trigger(this, MMLifeCycleEventTypes.Revive);
        }
        
        public void IncreaseMaximumHealthByPercentage(float percentage)
        {
            // Verificar que el porcentaje es válido (debe ser mayor que 0)
            if (percentage <= 0)
            {
                return;
            }

            // Calcular la cantidad que se incrementará en la vida máxima
            float additionalHealth = MaximumHealth * (percentage / 100f);

            // Calcular el porcentaje de vida actual
            float healthPercentage = CurrentHealth / MaximumHealth;

            // Incrementar la salud máxima
            MaximumHealth += additionalHealth;

            // Ajustar la salud actual para mantener el mismo porcentaje
            CurrentHealth = MaximumHealth * healthPercentage;

            // Actualizar la barra de salud visual
            UpdateHealthBar(true);
            UpdateHealthText();
            HealthMMFeedbacks?.PlayFeedbacks(this.transform.position);
        }
        
        /// <summary>
        /// Curar un porcentaje de la salud máxima del personaje.
        /// </summary>
        /// <param name="percentage">El porcentaje de vida máxima a curar.</param>
        public virtual void HealPercentage(float percentage)
        {
            // Validamos que el porcentaje sea mayor a 0
            if (percentage <= 0)
            {
                return;
            }

            // Calculamos cuánto curará en base al porcentaje de la salud máxima
            float healingAmount = MaximumHealth * (percentage / 100f);

            // Se asegura de no exceder la salud máxima al curar
            CurrentHealth = Mathf.Min(CurrentHealth + healingAmount, MaximumHealth);

            // Actualizamos la barra de salud visual
            UpdateHealthBar(true);

            // Disparar un evento de cambio de salud
            HealthChangeEvent.Trigger(this, CurrentHealth);
            UpdateHealthText();
            HealthMMFeedbacks?.PlayFeedbacks(this.transform.position);
        }
        
        /// <summary>
        /// Cura al personaje hasta un porcentaje específico de su vida máxima.
        /// </summary>
        /// <param name="targetPercentage">El porcentaje de la vida máxima al que se debe curar.</param>
        public virtual void HealToPercentage(float targetPercentage)
        {
            // Validación: El porcentaje debe estar entre 0 y 100
            if (targetPercentage < 0 || targetPercentage > 100)
            {
                return;
            }

            // Calculamos la cantidad de vida correspondiente al porcentaje dado
            float targetHealth = MaximumHealth * (targetPercentage / 100f);

            // Nos aseguramos de no reducir la vida actual si ya es mayor al límite deseado
            if (CurrentHealth < targetHealth)
            {
                CurrentHealth = targetHealth;

                // Actualizamos la barra de salud visual
                UpdateHealthBar(true);

                // Disparar un evento de cambio de salud
                HealthChangeEvent.Trigger(this, CurrentHealth);
            }
            
            HealthMMFeedbacks?.PlayFeedbacks(this.transform.position);
            UpdateHealthText();
        }

        /// <summary>
        /// Destroys the object, or tries to, depending on the character's settings
        /// </summary>
        protected virtual void DestroyObject()
        {
            if (_autoRespawn == null)
            {
                if (DestroyOnDeath)
                {
                    if (_character != null)
                    {
                        _character.gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                _autoRespawn.Kill();
            }
        }
        
        /// <summary>
        /// Verifica si la salud actual está por debajo del 50% de la salud máxima.
        /// </summary>
        /// <returns>Devuelve true si la salud actual es menor al 50% de la salud máxima, de lo contrario false.</returns>
        public virtual bool CheckLowHealth()
        {
            return CurrentHealth < (MaximumHealth * 0.5f);
        }

        #region HealthManipulationAPIs

        /// <summary>
        /// Sets the current health to the specified new value, and updates the health bar
        /// </summary>
        /// <param name="newValue"></param>
        public virtual void SetHealth(float newValue)
        {
            CurrentHealth = newValue;
            CheckLowHealth();
            UpdateHealthBar(false);
            UpdateHealthText();
            HealthChangeEvent.Trigger(this, newValue);
        }

        public void UpdateHealthText()
        {
            if(healthText is not null)
                healthText.text = ((int) CurrentHealth).ToString();
        }

        /// <summary>
        /// Called when the character gets health (from a stimpack for example)
        /// </summary>
        /// <param name="health">The health the character gets.</param>
        /// <param name="instigator">The thing that gives the character health.</param>
        public virtual void ReceiveHealth(float health, GameObject instigator)
        {
            // this function adds health to the character's Health and prevents it to go above MaxHealth.
            if (MasterHealth != null)
            {
                MasterHealth.SetHealth(Mathf.Min(CurrentHealth + health, MaximumHealth));
            }
            else
            {
                SetHealth(Mathf.Min(CurrentHealth + health, MaximumHealth));
            }

            if (health>0)
                HealthMMFeedbacks?.PlayFeedbacks(this.transform.position);
            UpdateHealthBar(true);
            UpdateHealthText();
        }

        /// <summary>
        /// Resets the character's health to its max value
        /// </summary>
        public virtual void ResetHealthToMaxHealth()
        {
            SetHealth(MaximumHealth);
        }

        /// <summary>
        /// Forces a refresh of the character's health and shield bar.
        /// </summary>
        /// <param name="show">Whether the bar should be shown or not</param>
        public virtual void UpdateHealthBar(bool show)
        {
            UpdateHealthAnimationParameters();

            // Verifica que la barra de salud está configurada
            if (_healthBar != null)
            {
                // Llama a la nueva versión de UpdateBar que ahora incluye escudo
                _healthBar.UpdateBar(CurrentHealth, 0f, MaximumHealth, CurrentShield, MaximumShield, show);
            }

            // Si no hay MasterHealth, actualiza la barra de la GUI si es un jugador
            if (MasterHealth == null)
            {
                if (_character != null)
                {
                    if (_character.CharacterType == Character.CharacterTypes.Player)
                    {
                        // Actualiza la barra de salud global en GUI (opcional)
                        if (GUIManager.HasInstance)
                        {
                            GUIManager.Instance.UpdateHealthBar(CurrentHealth, 0f, MaximumHealth, _character.PlayerID);
                        }
                    }
                }
            }
        }

        protected virtual void UpdateHealthAnimationParameters()
        {
            if (_hasHealthParameter)
            {
                TargetAnimator.SetFloat(_healthAnimatorParameter, CurrentHealth);
            }

            if (_hasHealthAsIntParameter)
            {
                TargetAnimator.SetInteger(_healthAsIntAnimatorParameter, (int)CurrentHealth);
            }
        }

        #endregion

        #region DamageDisablingAPIs

        /// <summary>
        /// Prevents the character from taking any damage
        /// </summary>
        public virtual void DamageDisabled()
        {
            Invulnerable = true;
        }

        /// <summary>
        /// Allows the character to take damage
        /// </summary>
        public virtual void DamageEnabled()
        {
            Invulnerable = false;
        }

        /// <summary>
        /// makes the character able to take damage again after the specified delay
        /// </summary>
        /// <returns>The layer collision.</returns>
        public virtual IEnumerator DamageEnabled(float delay)
        {
            yield return new WaitForSeconds(delay);
            Invulnerable = false;
        }

        #endregion
    }
}